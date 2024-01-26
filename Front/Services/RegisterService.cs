
using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Services ; 
using System.Text.Json.Serialization;
using System.Text.Json;



namespace Front.Services
{
    public class RegisterService
    {
        private readonly HttpClient _httpClient;

        // Utilisez un constructeur avec une injection de dépendances pour initialiser HttpClient
        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public class RegistrationResult
        {
            public string Message { get; set; }
            public Dictionary<string, string> Errors { get; set; }
        }

        
        public async Task<RegistrationResult> RegisterUser(string username, string password , string email )
        {
            
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = "api/User/register"; 
            string apiUrl = $"{gatewayUrl}{loginRoute}";
            Dictionary<string, string> errors = new Dictionary<string, string>();
            string message = "";

            // Construisez les données JSON pour la requête POST
            var postData = new { Name = username, Password = password , Email =email };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");
            try{
            
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

                    if (response.IsSuccessStatusCode)
                    {
                            message = "Bien fait";
                    }
                    else
                    {
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error Response Body: {responseBody}");

                            if (responseBody.StartsWith("{")){
                                JsonDocument jsonDocument = JsonDocument.Parse(responseBody);

                                // Accédez aux propriétés du document JSON
                                JsonElement root = jsonDocument.RootElement;
                                if (root.TryGetProperty("errors", out JsonElement errorsElement)){
                                    // Enumérez les propriétés sous la propriété "errors"
                                    foreach (JsonProperty property in errorsElement.EnumerateObject())
                                    {
                                        string propertyName = property.Name;
                                        JsonElement propertyValue = property.Value;

                                        string nameErrorsString = propertyValue.ToString();

                                        errors[propertyName] = nameErrorsString;

                                    }
                                    message = "Erreur";
                                }
                                else{
                                    message = "not fait";
                                }
                            }
                            else{
                                message = "not fait";
                            }
                        }
                        else{
                            message = "inregoinable";
                        }
                        
                    }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                message = "inregoinable";
            }
        
            return new RegistrationResult
            {
                Message = message, 
                Errors = errors
            };   

        }
        
    }
}


