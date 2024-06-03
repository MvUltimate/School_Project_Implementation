using Microsoft.AspNetCore.Mvc;

namespace MVC_SchoolProject.Models
{
    public class CreateUserModel
    {
        public Guid Uuid { get; set; }
        public string Password { get; set; }
       
    }
}
