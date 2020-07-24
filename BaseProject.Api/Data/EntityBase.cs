using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Api.Data
{
    /// <summary>
    /// Add common Properties here that will be used for all your entities
    /// </summary>
    public class EntityBase
    {
        public long CreatedById { get; set; }
        public long ModifiedById { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
    }
}
