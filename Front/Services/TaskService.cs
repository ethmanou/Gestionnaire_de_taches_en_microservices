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
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;



public class TaskService
{
    private readonly HttpClient _httpClient;
    private ProtectedLocalStorage _sessionStorage;


    public TaskService(HttpClient httpClient , ProtectedLocalStorage sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        var jwt = await _sessionStorage.GetAsync<string>("jwt");
        var IdUser = await _sessionStorage.GetAsync<int>("IdUser");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
        return await _httpClient.GetFromJsonAsync<List<TaskModel>>($"http://localhost:5000/api/User/tasks/{IdUser.Value}");
    }

    public async Task<List<TaskModel>> GetAllTasksAsync()
    {
        var jwt = await _sessionStorage.GetAsync<string>("jwt");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
        return await _httpClient.GetFromJsonAsync<List<TaskModel>>($"http://localhost:5000/api/User/tasks");
    }

    public async Task<string> CreateTask(string text , bool valid ){
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            var IdUser = await _sessionStorage.GetAsync<int>("IdUser");
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = $"api/User/task/{IdUser.Value}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
            var postData = new { Text = text , IsDone = valid , IdUser = IdUser.Value};
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
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            var IdUser = await _sessionStorage.GetAsync<int>("IdUser");
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = $"api/User/task/{IdUser.Value}/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
        
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
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            var IdUser = await _sessionStorage.GetAsync<int>("IdUser");
            string gatewayUrl = "http://localhost:5000/"; 
            string loginRoute = $"api/User/task/{IdUser.Value}/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);

            var putData = new { Text = text , IsDone = valid , IdUser = IdUser.Value};
        
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
