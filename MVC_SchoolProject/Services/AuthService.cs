using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVC_SchoolProject.Services
{
    public class AuthService : IAuthService
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl = "https://localhost:7252/api/login/authenticate";

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginModel = new { username, password };// Login model containing username and password
            // Sends a POST request to the API to authenticate user
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, loginModel);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenObject = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                if (tokenObject.TryGetProperty("token", out var tokenValue))
                {
                    // Retrieves the token value
                    var token = tokenValue.GetString();
                    //AddTokenToHeader(token); // Adds the token to HTTP request header for future request
                    return token;
                }
            }
            return null;

        }

        // Method to add token to HTTP request header for authentication
        public void AddTokenToHeader(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }



    }
}
