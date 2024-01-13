// TaskService.cs
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Services; 
using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;



public class TaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        string cheminFichier = "poken.txt";
        string line = File.ReadAllText(cheminFichier);
        string[] parts = line.Split(':');
        string token = parts[1].Trim();
        int idUser ;
        int.TryParse(parts[0].Trim(), out idUser);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.GetFromJsonAsync<List<TaskModel>>($"http://localhost:5000/api/User/tasks/{idUser}");
    }

    public async Task<List<TaskModel>> GetAllTasksAsync()
    {
        string cheminFichier = "poken.txt";
        string line = File.ReadAllText(cheminFichier);
        string[] parts = line.Split(':');
        string token = parts[1].Trim();
        int idUser ;
        int.TryParse(parts[0].Trim(), out idUser);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.GetFromJsonAsync<List<TaskModel>>($"http://localhost:5000/api/User/tasks");
    }

    public async Task<string> CreateTask(string text , bool valid ){
            string cheminFichier = "poken.txt";
            string line = File.ReadAllText(cheminFichier);
            string[] parts = line.Split(':');
            int idUser ;
            int.TryParse(parts[0].Trim(), out idUser);
            string token = parts[1].Trim();
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = $"api/User/task/{idUser}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            // Construisez les données JSON pour la requête POST
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var postData = new { Text = text , IsDone = valid , IdUser = idUser};
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

            if (response.IsSuccessStatusCode)
            {
                    return "Bien fait";
            }
            else
            {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Response Body: {responseBody}");
                    return null;
            }
    }

    public async Task<string> DeleteTaskAsync(int id){
            string cheminFichier = "poken.txt";
            string line = File.ReadAllText(cheminFichier);
            string[] parts = line.Split(':');
            int idUser ;
            int.TryParse(parts[0].Trim(), out idUser);
            string token = parts[1].Trim();
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = $"api/User/task/{idUser}/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
            HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);


            // Check if the response status code is 200 
            if (response.IsSuccessStatusCode)
                {
                    return "suppressin est bien fait";

                }
            else
                {
                    return "Erreur dans la suppression";
                }
        }


    public async Task<string> UpdateTaskAsync(int id , string text , bool valid){
            string cheminFichier = "poken.txt";
            string line = File.ReadAllText(cheminFichier);
            string[] parts = line.Split(':');
            int idUser ;
            int.TryParse(parts[0].Trim(), out idUser);
            string token = parts[1].Trim();
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = $"api/User/task/{idUser}/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var putData = new { Text = text , IsDone = valid , IdUser = idUser};
        
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl , putData);


            // Check if the response status code is 200 
            if (response.IsSuccessStatusCode)
                {
                    return "update";

                }
            else
                {
                    return null;
                }
        }
}
