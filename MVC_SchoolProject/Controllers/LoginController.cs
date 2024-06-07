using Microsoft.AspNetCore.Mvc;
using MVC_SchoolProject.Models;
using MVC_SchoolProject.Services;
using System.Net.Http.Headers;

namespace MVC_SchoolProject.Controllers
{
    public class LoginController : Controller
    {

        private readonly AuthService _authService;
        private readonly StudentService _studentService;

        public LoginController(AuthService authService, StudentService studentService)
        {
            _authService = authService;
            _studentService = studentService;
        }

        //Login page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var loginResult = await _authService.LoginAsync(loginModel.Username);

            if (loginResult.Success)
            {
                var userInfo = await _studentService.CheckInfo(loginModel.Username); // Pass the username to get user info

                if (userInfo.DepartmentId == 1)
                {
                    return RedirectToAction("Info", "Student");
                }
                else
                {
                    return RedirectToAction("AdminView", "Admin");
                }
            }

            // Stocker le message d'erreur dans ViewBag
            ViewBag.ErrorMessage = loginResult.ErrorMessage;

            return View(loginModel);
        }



    }
}
