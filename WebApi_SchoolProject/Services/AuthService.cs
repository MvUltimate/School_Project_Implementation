using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


/*Class for the Authentication, genere the token and check the login 
 */
namespace WebApi_SchoolProject.Services
{
    public class AuthService
    {
        
        public string Authenticate(string username, string password)
        {
            //Grab the secret key in the JSON File, it's possible that it has a better method
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();
            var secretKey = configuration["JwtSettings:SecretKey"];

            //If the user doesn't prompt good credentials, reject
            if (username == null || password == null || login(username, password)==false)
            {
                return "Informations incorrect";
            }
            //If the user doesn't exist, reject
            var user = getUserSAP(username);
            if (user == null)
            {
                return null;
            }
            //The departement to know the type of token that you will give (token with privilege or not)
            var departementName = user.Departement.DepartementName;

            //create the token gestionnaire
            var tokenHandler = new JwtSecurityTokenHandler();
            //Signature key, must be enough long
            var key = Encoding.UTF8.GetBytes(secretKey);
            //Description of the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Will be used to determine the right of the user on different request
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UUID", user.UUID.ToString()),
                new Claim("Departement", departementName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "http://localhost",
                Audience = "http://localhost",
                //Sign the token with a symmetric key
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    



        private SAP getUserSAP(string username)
        {
            using var context = new SchoolContext();
            var user = context.SAPs
            .Include(u => u.Departement)  // Include departement to be not null
            .FirstOrDefault(u => u.UserName == username);
            return user;
        }

        private bool login (string username, string password)
        {
            using var context = new SchoolContext();
            //Verify the login, if the user exist, try to login and verify the password
            var user = context.SAPs.FirstOrDefault(u => u.UserName == username);
            if(user != null)
            {
                var account = context.Accounts.FirstOrDefault(a => a.UUID == user.UUID);
                return BCrypt.Net.BCrypt.Verify(password, account.password);
            }
            return false;
            
        }
    }




}
