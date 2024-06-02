using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_SchoolProject.Services
{
    //Used to create a User with the cryptage
    public class AccountService
    {

        public void CreateUser( Guid uuid, string password)
        {
            using var context = new SchoolContext();

            //If the User doesn't exist in SAP, it's not possible to create his Account
            var existingUser = context.SAPs.FirstOrDefault(a => a.UUID == uuid);
            if (existingUser == null)
            {
                throw new InvalidOperationException("The user with this UUID doesn't exist.");
            }
           
            //Generate a random salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            // Register the new Account of the user
            Account newUser = new Account
            {
                UUID = uuid,
                password = hashedPassword,
                salt = salt 
            };

            context.Add(newUser);
            
            context.SaveChanges();

        }
    }
}
