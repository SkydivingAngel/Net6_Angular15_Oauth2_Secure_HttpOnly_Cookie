namespace Net6AngularOauth2
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public string Url { get; set; }
    }
}