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
        
        public string Authenticate(string username)
        {
            // Si le username est null, rejeter
            if (username == null)
            {
                return null;
            }

            // Si l'utilisateur n'existe pas, rejeter
            var user = getUserSAP(username);
            if (user == null)
            {
                return null;
            }

            // Le département pour savoir le type d'accès que vous aurez
            var departementName = user.Departement.DepartementName;

            // Retourner une confirmation que l'utilisateur est authentifié
            return $"User {username} authenticated with department {departementName}.";
        }



        //Method to retrieve the user by his name
        private SAP getUserSAP(string username)
        {
            using var context = new SchoolContext();
            var user = context.SAPs
            .Include(u => u.Departement)  // Include departement to be not null
            .FirstOrDefault(u => u.UserName == username);
            return user;
        }

        private bool login (string username)
        {
            using var context = new SchoolContext();
            //Verify the login, if the user exist, try to login
            var user = context.SAPs.FirstOrDefault(u => u.UserName == username);
            if(user != null)
            {
                var account = context.Accounts.FirstOrDefault(a => a.UUID == user.UUID);
            }
            return false;
            
        }
    }




}
