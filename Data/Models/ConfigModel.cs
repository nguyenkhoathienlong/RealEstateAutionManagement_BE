namespace Data.Models
{
    public class DbSetupModel
    {
        public string ConnectionStrings { get; set; } = string.Empty;
    }

    public class JwtModel
    {
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
        public string? Secret { get; set; }
    }

    public class JWTToken
    {
        public string? TokenString { get; set; }
        public long ExpiresInMilliseconds { get; set; }
    }

    public class FirebaseModel
    {
        public string? FirebaseSDKFile { get; set; }
        public string? ProjectId { get; set; }
        public string? Bucket { get; set; }
    }
}
