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
            var token  = _authService.Authenticate(request.username, request.password);

            //Used to the MVC, if we don't return OK, the View will crash
            //return ok but will be blocked later
            if (token == null)
                return Ok(new
                {
                    success = false,
                   
                });

            return Ok(new
            {
                success = true,
                token
            });

        }

        public class LoginRequest()
        {
            public string username { get; set; }
            public string password { get; set; }

        }

    }
}
