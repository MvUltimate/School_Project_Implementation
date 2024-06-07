using Microsoft.AspNetCore.Http.HttpResults;
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
            {
                var result = _authService.Authenticate(request.username);

                if (result == null)
                {
                    return Unauthorized("Invalid username.");
                }

                return Ok(result);
            }

        }

        public class LoginRequest()
        {
            public string username { get; set; }

        }

    }
}
