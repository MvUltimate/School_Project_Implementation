// AdminService.cs
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_SchoolProject.Models;

namespace MVC_SchoolProject.Services
{
    public class AdminService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl = "https://localhost:7252/api/AdminAccounts";

        public AdminService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<AdminModel>> GetAllUsers()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync(_baseUrl);
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

        public async Task<bool> CreateUser(AdminModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl + "/createaccount", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChargeAmount(string username, double amount)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var chargeRequest = new { Username = username, Amount = amount };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl + "/chargeaccount", chargeRequest);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<TransactionModel>> GetTransactionsFromUser(string username)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"{_baseUrl}/getUserTransaction?username={username}");
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
