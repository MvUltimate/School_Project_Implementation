using MVC_SchoolProject.Models;
using NuGet.Common;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVC_SchoolProject.Services
{
    public class AuthService : IAuthService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7252/api/login/authenticate";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        //To return to the view a good error message
        public async Task<LoginResult> LoginAsync(string username)
        {
            var loginModel = new { username }; // Login model contenant le username
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, loginModel);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                // Si la réponse contient success == true, retourner succès
                if (result.TryGetProperty("success", out var successElement) && successElement.GetBoolean())
                {
                    return new LoginResult
                    {
                        Success = true
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = "Username incorrect !"
                    };
                }
            }
            return new LoginResult
            {
                Success = false,
                ErrorMessage = "An error occurred"
            };
        }
    }

}
