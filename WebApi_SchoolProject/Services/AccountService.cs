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

            var existingUser = context.SAPs.FirstOrDefault(a => a.UUID == uuid);
            if (existingUser == null)
            {
                throw new InvalidOperationException("L'utilisateur avec cet UUID n'existe pas.");
            }
           
            //Générer un sel aléatoire
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hacher le mot de passe avec le sel
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            // Enregistrer l'utilisateur avec le mot de passe haché
            Account newUser = new Account
            {
                UUID = uuid,
                password = hashedPassword,
                salt = salt // Vous pouvez également stocker le sel dans votre base de données
            };

            context.Add(newUser);
            
            context.SaveChanges();

        }
    }
}
