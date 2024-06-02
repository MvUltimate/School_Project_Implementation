using Microsoft.AspNetCore.Mvc;

namespace MVC_SchoolProject.Models
{
    public class CreateUserModel
    {
        public Guid Uuid { get; set; }
        public string password { get; set; }
       
    }
}
