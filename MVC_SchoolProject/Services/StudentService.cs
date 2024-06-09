using Microsoft.AspNetCore.Mvc;
using MVC_SchoolProject.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVC_SchoolProject.Services
{
    public class StudentService : IStudentService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public StudentService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = configuration["webAPI:BaseURL"];
        }

        public async Task<bool> chargeAmount(double amount)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            //Add the token to verify the authorization
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // Create the content of the request with a JSON format
            var content = JsonContent.Create(new { amount });

            // send the request 
            var response = await _httpClient.PostAsync(_baseUrl + "/studentsaccount/chargeaccount", content);

            return response.IsSuccessStatusCode; // Returns true if the request was successful, false otherwise

        }

        public async Task<StudentsInfoM> checkInfo()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            //Add the token to verify the authorization
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // Sends a HTTP GET request to the API to retrieve student information
            return await _httpClient.GetFromJsonAsync<StudentsInfoM>(_baseUrl + "/studentsaccount/infos");
        }

        public async Task<List<TransactionModel>> checkTransaction()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync(_baseUrl + "/studentsaccount/transactions");
            //Add the token to verify the authorization
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
