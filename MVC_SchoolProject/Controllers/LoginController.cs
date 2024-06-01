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
        private readonly HttpClient _httpClient;

        public LoginController(AuthService authService, StudentService studentService, HttpClient httpClient)
        {
            _authService = authService;
            _studentService = studentService;
            _httpClient = httpClient;
        }

        //Login page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {       
                //Verify that the login is successful and the token is not null
                var token = await _authService.LoginAsync(loginModel.Username, loginModel.Password);
                if (token != null)
                {
                    HttpContext.Session.SetString("token", token);
                var userInfo = await _studentService.checkInfo();
                // Redirect to the student info page

                if (userInfo.DepartmentId == 1)
                {
                    return RedirectToAction("Info", "Student");
                }
                else
                {
                    return RedirectToAction("AdminView", "Admin");
                }



            }
                ModelState.AddModelError("", "Invalid username or password.");

            return View(loginModel);
        }
            
        
    }
}
