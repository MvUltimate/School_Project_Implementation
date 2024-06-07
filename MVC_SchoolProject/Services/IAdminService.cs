// AdminService.cs
using MVC_SchoolProject.Models;

namespace MVC_SchoolProject.Services
{
    public interface IAdminService
    {
        Task<bool> AddAmountAll(double amount);
        Task<bool> AddAmountClass(string Class, double Amount);
        Task<bool> AddAmountUser(string username, double amount);
        Task<bool> CreateUser(Guid uuid);
        Task<List<AdminModel>> GetAllUsers();
        Task<List<TransactionModel>> GetTransactionsFromUser(string username);
    }
}