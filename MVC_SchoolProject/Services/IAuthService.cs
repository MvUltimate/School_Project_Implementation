
using Microsoft.AspNetCore.Identity.Data;
using MVC_SchoolProject.Models;

namespace MVC_SchoolProject.Services
{
    public interface IAuthService
    {
        
        Task<LoginResult> LoginAsync(string username, string password);
    }
}