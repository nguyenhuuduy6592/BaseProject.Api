using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BaseProject.Api.Database
{
    class Program
    {
        private const string delimeter = " - ";
        private const string directory = "Scripts";
        private const string initMetadata = @"
            IF OBJECT_ID(N'[dbo].[DeploymentMetadata]') IS NULL
            BEGIN
	            CREATE TABLE [dbo].[DeploymentMetadata](
		            [Code] VARCHAR(128) NOT NULL,
		            [CreatedDate] DATETIME NOT NULL,
		            [By] NVARCHAR(128) NOT NULL DEFAULT ORIGINAL_LOGIN(), 
		            [As] NVARCHAR(128) NOT NULL DEFAULT SUSER_SNAME(), 
		            [CompletedDate] DATETIME NOT NULL DEFAULT GETDATE(), 
		            [With] NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),
		            PRIMARY KEY ([Code])
	            )
            END
        ";
        static void Main(string[] args)
        {
            Console.WriteLine("Reading appsettings.");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            string connectionString = config.GetConnectionString("TargetDatabase");

            Console.WriteLine("Starting deployment.");

            var files = GetFileList(directory);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Creating [dbo].[DeploymentMetadata] for migration script history if needed.");
                RunScriptAgainstDatabase(initMetadata, connection);

                var insertedList = GetInsertedList(connection);
                var fileList = files.Select(x => x.Name);
                var toInsertList = fileList.Except(insertedList)
                    .Select(x => new ScriptMetadata
                    {
                        Code = x
                    })
                    .OrderBy(x => x.Code)
                    .ToList();

                if (toInsertList.Count == 0)
                {
                    Console.WriteLine("Database is update to date.");
                }
                else
                {
                    foreach (var file in toInsertList)
                    {
                        var result = ValidateScript(file, connection);
                        if (result == CountEnum.NotExist)
                        {
                            Console.WriteLine($"Begin {file.Code}");
                            ExecuteScript(file, connection);
                            Console.WriteLine($"Finish {file.Code}");
                        }
                        else if (result == CountEnum.Exception)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Skip  {file.Code}");
                        }
                    }
                }

            }

            Console.WriteLine("Done deployment.");
            Console.ReadLine();
        }

        static void ExecuteScript(ScriptMetadata file, SqlConnection connection)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(directory);
                var fileFullName = d.FullName + "\\\\" + file.Code;
                var script = RemoveUse(File.ReadAllText(fileFullName));
                RunScriptAgainstDatabase(script, connection);
                InsertMetaData(file, connection);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(string.Format(
                    "Please check the SqlServer script.\nLine: {1} \nError: {2} \nFile: \n{3}",
                    ex.LineNumber, ex.Message, file.Code));
            }
        }

        static void RunScriptAgainstDatabase(string script, SqlConnection connection)
        {
            var server = new Server(new ServerConnection(sqlConnection: connection));
            server.ConnectionContext.ExecuteNonQuery(script);
        }

        private static List<string> GetInsertedList(SqlConnection connection)
        {
            Console.WriteLine("GetInsertedList for deployment.");
            var data = new List<string>();
            var commandString = $"SELECT Code FROM dbo.DeploymentMetadata";

            using (var command = new SqlCommand(commandString, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Add(reader.GetString(0));
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(string.Format("Please check the SqlServer script.\nLine: {0} \nError: {1}", ex.LineNumber, ex.Message));
                }
            }

            return data;
        }

        private static CountEnum ValidateScript(ScriptMetadata file, SqlConnection connection)
        {
            CountEnum count = CountEnum.Exist;
            var commandString = $"SELECT CASE WHEN exists(select  NULL from dbo.DeploymentMetadata WHERE Code = '{file.Code}') THEN 1 ELSE 0 END";

            using (var command = new SqlCommand(commandString, connection))
            {
                try
                {
                    var result = (int)command.ExecuteScalar();
                    count = result == 0 ? CountEnum.NotExist : CountEnum.Exist;
                }
                catch (SqlException ex)
                {
                    count = CountEnum.Exception;
                    Console.WriteLine(string.Format("Please check the SqlServer script.\nLine: {0} \nError: {1}", ex.LineNumber, ex.Message));
                }
            }

            return count;
        }

        private static void InsertMetaData(ScriptMetadata file, SqlConnection connection)
        {
            var commandString = $"INSERT INTO [dbo].[DeploymentMetadata] (Code, [CreatedDate]) VALUES (@Code, @CreatedDate)";

            using (var command = new SqlCommand(commandString, connection))
            {
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Code",
                    Value = file.Code,
                    SqlDbType = SqlDbType.VarChar,
                    Size = 128
                });
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CreatedDate",
                    Value = DateTime.UtcNow,
                    SqlDbType = SqlDbType.DateTime
                });

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(string.Format("Inserting meta data failed.\nLine: {0} \nError: {1}", ex.LineNumber, ex.Message));
                }
            }
        }

        private static FileInfo[] GetFileList(string directory)
        {
            Console.WriteLine("GetFileList for deployment.");
            DirectoryInfo d = new DirectoryInfo(directory);
            FileInfo[] files = d.GetFiles("*.sql", SearchOption.AllDirectories).OrderBy(x => x.Name).ThenByDescending(x => x.Directory.Name).ToArray();
            return files;
        }

        private static string RemoveUse(string content)
        {
            return content.Replace("USE [Wize_Configuration]", "", StringComparison.OrdinalIgnoreCase).Replace("USE Wize_Configuration", "", StringComparison.OrdinalIgnoreCase);
        }
    }

    class ScriptMetadata
    {
        public string Code { get; set; }
    }
}
