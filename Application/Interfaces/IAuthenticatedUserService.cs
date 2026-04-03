namespace Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        string? UserName { get; }
        IEnumerable<string> Roles { get; }
    }
}
