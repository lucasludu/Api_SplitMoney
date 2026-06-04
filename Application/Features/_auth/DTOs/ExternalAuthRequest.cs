namespace Application.Features._auth.DTOs
{
    public class ExternalAuthRequest
    {
        public string Provider { get; set; } = string.Empty;
        public string IdToken { get; set; } = string.Empty;
    }
}
