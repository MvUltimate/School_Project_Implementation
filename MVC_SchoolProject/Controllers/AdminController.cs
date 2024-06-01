// AdminController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_SchoolProject.Models;
using MVC_SchoolProject.Services;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using static WebApi_SchoolProject.Controllers.AdminAccountsController;

namespace MVC_SchoolProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> AdminView()
        {
            var users = await _adminService.GetAllUsers();
            Console.WriteLine($"Feched {users.Count} users");
            if (!users.Any())
            {
                // Vérifiez ici si la liste est vide et pourquoi
                Console.WriteLine("No users found.");
            }
            return View(users);
        }
        

        public async Task<IActionResult> ViewTransactions(string username)
        {
            List<TransactionModel> transactions = await _adminService.GetTransactionsFromUser(username);
            
            return View(transactions); // Renvoyer vers une vue qui affichera les transactions
        }

        public async Task<bool> ChargeAmount(string username, double amount)
        {
            return await _adminService.ChargeAmount(username, amount);
        }

        

        [HttpPost]
        public async Task<IActionResult> ProcessSelectedUsers(List<Guid> usernames, string selectedAction)
        {
            switch (selectedAction)
            {
                case "chargeAmount":
                    foreach (var username in usernames)
                    {
                        //ChargeAmount(username, amount);
                    }
                    break;
                case "otherAction":
                    // Logique pour une autre action
                    break;
            }

            

            return RedirectToAction("AdminView");
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.CreateUser(model);
                if (success)
                {
                    TempData["Message"] = "Utilisateur créé avec succès !";
                    return RedirectToAction("AdminView");
                }
                else
                {
                    TempData["ErrorMessage"] = "Échec de la création de l'utilisateur.";
                }
            }
            return View(model);
        }
        


    }



}
