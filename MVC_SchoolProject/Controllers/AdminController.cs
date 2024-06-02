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

        [HttpGet]
        public IActionResult AddAmountUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAmountUser(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.AddAmountUser(model.UserName, model.Amount);
                if (success)
                {
                    TempData["Message"] = "Amount successfully added!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add amount.";
                }
                return RedirectToAction("AdminView");
            }
            TempData["ErrorMessage"] = "Input data is not valid.";
            return View("AddAmountUser", model);
        }

        [HttpGet]
        public IActionResult AddAmountClass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAmountClass(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.AddAmountClass(model.Class, model.Amount);
                if (success)
                {
                    TempData["Message"] = "Amount successfully added!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add amount.";
                }
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Input data is not valid.";
            return View(model);
        }

        [HttpGet]
        public IActionResult AddAmountAll()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>AmountAll(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _adminService.AddAmountAll(model.Amount))
                    TempData["Message"] = "All accounts charged successfully!";
                else
                    TempData["ErrorMessage"] = "Failed to charge all accounts.";
            }
            return View("AdminView");
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
