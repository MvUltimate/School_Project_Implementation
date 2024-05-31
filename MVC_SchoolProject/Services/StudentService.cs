using Microsoft.AspNetCore.Mvc;
using MVC_SchoolProject.Models;
using System.Net.Http.Headers;

namespace MVC_SchoolProject.Services
{
    public class StudentService : IUserService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7252/api/studentsaccount";
        private readonly IHttpContextAccessor _httpContextAccessor;



        public StudentService(HttpClient httpClient, AuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> chargeAmount(double amount)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // Créer le contenu de la requête avec le montant sous forme JSON
            var content = JsonContent.Create(new { amount });

            // Envoyer la requête HTTP POST à l'API
            var response = await _httpClient.PostAsync(_baseUrl + "/chargeaccount", content);

            return response.IsSuccessStatusCode; // Returns true if the request was successful, false otherwise

        }

        public async Task<StudentsInfoM> checkInfo()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // Sends a HTTP GET request to the API to retrieve student information
            return await _httpClient.GetFromJsonAsync<StudentsInfoM>(_baseUrl+"/infos");
        }

    }
}
