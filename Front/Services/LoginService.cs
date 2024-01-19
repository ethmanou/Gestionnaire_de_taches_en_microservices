using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Front.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _sessionStorage;

        public LoginService(HttpClient httpClient, ProtectedLocalStorage sessionStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
        }

        public async Task<UserDTO> AuthenticateUser(string username, string password)
        {
            string gatewayUrl = "http://localhost:5000/";
            string loginRoute = "api/User/login";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            // Construisez les données JSON pour la requête POST
            var postData = new { Name = username, Pass = password };
            var jsonContent = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var resultat = await response.Content.ReadFromJsonAsync<response_t>();
                    if (resultat?.token != null && resultat?.user?.Id != null)
                    {
                        await _sessionStorage.SetAsync("IdUser", resultat.user.Id);
                        await _sessionStorage.SetAsync("jwt", resultat.token);
                    }
                    return resultat.user ;
                }
                else
                {
                    if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Response Body: {responseBody}");
                        return  new UserDTO { Id = 0, Name = "nobody", Email = "nobody", role = "nobody" };
                    }
                    else{

                        return  new UserDTO { Id = -1, Name = "nobody", Email = "nobody", role = "nobody" };
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return new UserDTO { Id = -1, Name = "nobody", Email = "nobody", role = "nobody" };
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            if (jwt.Success && jwt.Value != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
            }
            return await _httpClient.GetFromJsonAsync<List<UserDTO>>("http://localhost:5000/api/User/Users") ?? new List<UserDTO>();
        }

        public async Task<string> UpdateUserAsync(int IdUser, UserDTO user)
        {
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            if (jwt.Success && jwt.Value != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
            }
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/User/{IdUser}", user);

            return response.IsSuccessStatusCode ? "updated" : "Not Updated";
        }

        public async Task<string> DeleteUserAsync(int IdUser)
        {
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            if (jwt.Success && jwt.Value != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
            }
            HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5000/api/User/{IdUser}");

            return response.IsSuccessStatusCode ? "suppression Bien Fait" : "Erreur dans la suppression";
        }
    }
}
