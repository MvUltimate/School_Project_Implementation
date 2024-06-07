using Microsoft.AspNetCore.Mvc;
using MVC_SchoolProject.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVC_SchoolProject.Services
{
    public class StudentService : IStudentService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7252/api/studentsaccount";
        private readonly IHttpContextAccessor _httpContextAccessor;


        public StudentService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> chargeAmount(double amount)
        {

            // Create the content of the request with a JSON format
            var content = JsonContent.Create(new { amount });

            // send the request 
            var response = await _httpClient.PostAsync(_baseUrl + "/chargeaccount", content);

            return response.IsSuccessStatusCode; // Returns true if the request was successful, false otherwise

        }

        public async Task<StudentsInfoM> checkInfo()
        {
            // Sends a HTTP GET request to the API to retrieve student information
            return await _httpClient.GetFromJsonAsync<StudentsInfoM>(_baseUrl + "/infos");
        }

        public async Task<List<TransactionModel>> checkTransaction()
        {


            var response = await _httpClient.GetAsync($"{_baseUrl}/transactions");
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
