using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Services ; 
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;



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
                        var resultat = await response.Content.ReadFromJsonAsync<response_t>();
                        string cheminFichier = "poken.txt";
                        string lineToAdd = $"{resultat.user.Id}:{resultat.token}";
                        File.WriteAllText(cheminFichier, $"{lineToAdd}\n");
                        return resultat.user;
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


        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            string cheminFichier = "poken.txt";
            string line = File.ReadAllText(cheminFichier);
            string[] parts = line.Split(':');
            string token = parts[1].Trim();
            int idUser ;
            int.TryParse(parts[0].Trim(), out idUser);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<List<UserDTO>>($"http://localhost:5000/api/User/Users");
        }

        

        public async Task<string>  UpdateUserAsync(int IdUser , UserDTO user) 
        {
            string cheminFichier = "poken.txt";
            string line = File.ReadAllText(cheminFichier);
            string[] parts = line.Split(':');
            string token = parts[1].Trim();
            int idUser ;
            int.TryParse(parts[0].Trim(), out idUser);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response =  await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/User/{IdUser}" , user);

            if(response.IsSuccessStatusCode){

                return "updated" ;
            }
            else{
                return "Not Updated" ;
            }

        }

        public async Task<string> DeleteUserAsync(int IdUser) 
        {
            string cheminFichier = "poken.txt";
            string line = File.ReadAllText(cheminFichier);
            string[] parts = line.Split(':');
            string token = parts[1].Trim();
            int idUser ;
            int.TryParse(parts[0].Trim(), out idUser);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response =  await _httpClient.DeleteAsync($"http://localhost:5000/api/User/{IdUser}");

            if(response.IsSuccessStatusCode){

                return "suppression Bien Fait";
            }
            else{
                return "Erreur dans la suppression" ;
            }
        }

            
        
    }
}




        
    



 
