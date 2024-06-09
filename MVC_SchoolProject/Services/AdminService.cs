// AdminService.cs
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_SchoolProject.Models;

namespace MVC_SchoolProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public AdminService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = configuration["webAPI:BaseURL"];
        }

        public async Task<List<AdminModel>> GetAllUsers()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync(_baseUrl+"/adminaccounts");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<AdminModel>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return users;
            }
            else
            {
                // Handle HTTP error response
                return new List<AdminModel>();
            }

        }

        public async Task<bool> CreateUser(Guid uuid, string password)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var content = JsonContent.Create(new { uuid, password });
            var response = await _httpClient.PostAsync(_baseUrl+"/adminaccounts/createaccount", content);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {response.StatusCode}, Body: {responseBody}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddAmountUser(string username, double amount)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var content = JsonContent.Create(new { username, amount });
            var response = await _httpClient.PostAsync(_baseUrl+"/adminaccounts/chargeamount", content);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {response.StatusCode}, Body: {responseBody}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddAmountClass(string Class, double Amount)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var content = JsonContent.Create(new { Class, Amount });
            var response = await _httpClient.PostAsync(_baseUrl+"/adminaccounts/chargeClass", content);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {response.StatusCode}, Body: {responseBody}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddAmountAll(double amount)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsync(_baseUrl+"/adminaccounts/chargeAll?amount="+amount,null);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<TransactionModel>> GetTransactionsFromUser(string username)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync(_baseUrl+"/adminaccounts/getUserTransaction?username="+username);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TransactionModel>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                // Handle HTTP error response
                return new List<TransactionModel>();
            }

        }
    }
}
