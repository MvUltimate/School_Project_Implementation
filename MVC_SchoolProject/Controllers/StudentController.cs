using Microsoft.AspNetCore.Mvc;
using MVC_SchoolProject.Services;
using NuGet.Common;

namespace MVC_SchoolProject.Controllers
{
    public class StudentController : Controller
    {

        private readonly StudentService _studentService;
       

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<IActionResult> Info()
        {

            var studentInfo = await _studentService.checkInfo();
            return View(studentInfo);
        }

        //Redirect on the chargeAmount Page
        //Possible to put the student Info + ChargeAmount on the same page but we need to create a new DTO and modify the code
        //because we can only use 1 model on Razor Pages
        public async Task<IActionResult> ChargeAmount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>ChargeAmount(double amount)
         {
            var result = await _studentService.chargeAmount(amount);
            if (result)
            {
                return RedirectToAction("Info");
            }
            else
            {
                return View();
            }
        }
    }
}
