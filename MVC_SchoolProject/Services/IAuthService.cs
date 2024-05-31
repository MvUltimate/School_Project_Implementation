
namespace MVC_SchoolProject.Services
{
    public interface IAuthService
    {
        void AddTokenToHeader(string token);
        Task<string> LoginAsync(string username, string password);
    }
}