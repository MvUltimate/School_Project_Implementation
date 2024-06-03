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
        public async Task<IActionResult> AddAmountUser(ChargeUserModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.AddAmountUser(model.Username, model.amount);
                if (success)
                {
                    TempData["Message"] = "Amount successfully added!";
                    return RedirectToAction("AdminView");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add amount.";
                    return View("AddAmountUser", model);
                }
                
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
        public async Task<IActionResult> AddAmountClass(ChargeClassModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.AddAmountClass(model.className, model.amount);
                if (success)
                {
                    TempData["Message"] = "Amount successfully added!";
                    return RedirectToAction("AdminView");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add amount.";
                    return View("AddAmountClass", model);
                }
            }
            TempData["ErrorMessage"] = "Input data is not valid.";
            return View("AddAmountClass", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddAmountAll()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>AddAmountAll(double amount)
        {
            var result = await _adminService.AddAmountAll(amount);
            if (result)
            {
                TempData["Message"] = "All accounts charged successfully!";
                return RedirectToAction("AdminView");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to charge all accounts.";
                return View();
            }           
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.CreateUser(model.Uuid, model.Password);
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
