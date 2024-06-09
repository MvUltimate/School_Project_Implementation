using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;


/*Class for the Authentication, genere the token and check the login 
 */
namespace WebApi_SchoolProject.Services
{
    public class AuthService
    {
        private readonly SchoolContext _context;
        private readonly string _secretKey;
        public AuthService (SchoolContext schoolContext, IConfiguration configuration)
        {
            _context = schoolContext;
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        public string Authenticate(string username, string password)
        {
         
            

            //If the user doesn't prompt good credentials, reject
            if (username == null || password == null || login(username, password)==false)
            {
                return null;
            }
            //If the user doesn't exist, reject
            var user = getUserSAP(username);
            if (user == null)
            {
                return null;
            }
            //The departement to know the type of token that you will have (token with privilege or not)
            var departementName = user.Departement.DepartementName;

            //create the token gestionnaire
            var tokenHandler = new JwtSecurityTokenHandler();
            //Signature key, must be enough long
            var key = Encoding.UTF8.GetBytes(_secretKey);
            //Description of the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Will be used to determine the right of the user on different requests
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UUID", user.UUID.ToString()),
                new Claim("Departement", departementName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "https://webapischoolproject-marc-yannick.azurewebsites.net/",
                Audience = "https://webapischoolproject-marc-yannick.azurewebsites.net/",
                //Sign the token with a symmetric key
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    


        //Method to retrieve the user by his name
        private SAP getUserSAP(string username)
        {
          
            var user = _context.SAPs
            .Include(u => u.Departement)  // Include departement to be not null
            .FirstOrDefault(u => u.UserName == username);
            return user;
        }

        private bool login (string username, string password)
        {
         
            //Verify the login, if the user exist, try to login and verify the password
            var user = _context.SAPs.FirstOrDefault(u => u.UserName == username);
            if(user != null)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.UUID == user.UUID);
                //Bcrypt is used to verify the password
                return BCrypt.Net.BCrypt.Verify(password, account.password);
            }
            return false;
            
        }
    }




}
