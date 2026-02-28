namespace Application.Features._auth.DTOs.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Rol { get; set; }
    }
}
