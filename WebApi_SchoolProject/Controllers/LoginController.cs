using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SchoolProject.Services;

 namespace WebApi_SchoolProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly AuthService _authService;
        

        public LoginController(AuthService authService)
        {

            _authService = authService;

        }

        [HttpPost("authenticate")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token  = _authService.Authenticate(request.username, request.password);

            if(token == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(new {token});

        }

        public class LoginRequest()
        {
            public string username { get; set; }
            public string password { get; set; }

        }

    }
}
