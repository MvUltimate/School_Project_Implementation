using MVC_SchoolProject.Models;

namespace MVC_SchoolProject.Services
{
    public interface IStudentService
    {
        Task<bool> chargeAmount(double amount);
        Task<StudentsInfoM> checkInfo();
        Task<List<TransactionModel>> checkTransaction();
    }
}