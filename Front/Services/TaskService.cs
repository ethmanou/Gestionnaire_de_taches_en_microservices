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



public class TaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TaskModel>>("http://localhost:5000/api/User/tasks");
    }

    public async Task<string> CreateTask(string text , bool Bool){
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = "api/User/task"; 
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            // Construisez les données JSON pour la requête POST
            var postData = new { Text = text , IsDone = Bool  };
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
}
