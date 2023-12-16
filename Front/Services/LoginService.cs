using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;

@inject LoginService loginService;
using UserService.Entities.UserLogin;


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
            // Set the base address of the API you want to call
                _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");

                UserLogin model =  new { Username = username, Password = password };

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/User/login", model);

                Console.WriteLine("je suis la " + response.Content );

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode)
                {
                    // Désérialisez la réponse en un objet UserDTO
                    UserDTO user = await response.Content.ReadFromJsonAsync<UserDTO>();
                
                    return user;
                }
                else
                {
                
                    return null;
                }
            
            /*string gatewayUrl = "http://localhost:5000"; 
            string loginRoute = "/api/User/login"; 
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            // Construisez les données JSON pour la requête POST
            var postData = new { Username = username, Password = password };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

             // Envoyez la requête POST à la passerelle (gateway) en utilisant PostAsJsonAsync
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, jsonContent);
            
            //Console.WriteLine("je suis la : " + response.Content);
            

            // Assurez-vous que la réponse est réussie avant de continuer
            if (response.IsSuccessStatusCode)
            {
                // Désérialisez la réponse en un objet UserDTO
                UserDTO user = await response.Content.ReadFromJsonAsync<UserDTO>();
                
                return user;
            }
            else
            {
                //Console.WriteLine("je suis la ");
                return null;
            }*/
        }
        
    }
}




        
    



 
