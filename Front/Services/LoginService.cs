using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Services ; 


namespace Front.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       public async Task<UserDTO> AuthenticateUser(string username, string password)
        {
            
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = "api/User/login"; 
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            // Construisez les données JSON pour la requête POST
            var postData = new { Name = username, Pass = password };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            try
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

                    if (response.IsSuccessStatusCode)
                    {
                        UserDTO user = await response.Content.ReadFromJsonAsync<UserDTO>();
                        return user;
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Response Body: {responseBody}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                    return null;
                }

        }
        
    }
}




        
    



 
