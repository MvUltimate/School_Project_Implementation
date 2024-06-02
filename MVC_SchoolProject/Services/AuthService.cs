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
        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            var loginModel = new { username, password };// Login model containing username and password
            // Sends a POST request to the API to authenticate user
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, loginModel);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                //If the answer contains success == true, retrieve the token and transfer
                if (result.TryGetProperty("success", out var successElement) && successElement.GetBoolean())
                {
                    result.TryGetProperty("token", out var tokenElement);
                    return new LoginResult
                    {
                        Success = true,
                        Token = tokenElement.GetString()
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = "Username or password incorrect !"
                    };
                }
            }    

        }
        }

}
