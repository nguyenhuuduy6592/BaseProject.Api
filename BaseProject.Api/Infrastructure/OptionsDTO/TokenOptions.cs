namespace BaseProject.Api.Infrastructure.OptionsDTO
{
    public class TokenOptions
    {
        public string Secret { get; set; }
        public string RefreshSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; } = 60; // in minute
        public int RefreshExpiration { get; set; } = 43200; // 1 month in minute
        public int ResetPasswordExpirationTime { get; set; } = 1440; // 1 day in minute
    }
}
