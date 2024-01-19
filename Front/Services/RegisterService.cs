
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
    public class RegisterService
    {
        private readonly HttpClient _httpClient;

        // Utilisez un constructeur avec une injection de dépendances pour initialiser HttpClient
        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        
        public async Task<string> RegisterUser(string username, string password , string email)
        {
            
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = "api/User/register"; 
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            // Construisez les données JSON pour la requête POST
            var postData = new { Name = username, Password = password , Email =email };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");
            try{
            
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

                    if (response.IsSuccessStatusCode)
                    {
                            return "Bien fait";
                    }
                    else
                    {
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error Response Body: {responseBody}");
                            return "not fait";
                        }
                        else{
                            return "inregoinable";
                        }
                        
                    }
            }
            catch (HttpRequestException ex)
            {
                //Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return "inregoinable";
            }
        
                

        }
    }
}


