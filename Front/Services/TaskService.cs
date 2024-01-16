// TaskService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

public class TaskService
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _sessionStorage;

    public TaskService(HttpClient httpClient, ProtectedLocalStorage sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");
        var idUserResult = await _sessionStorage.GetAsync<int>("IdUser");

        if (jwtResult.Success && jwtResult.Value != null && idUserResult.Success)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);
            var tasks = await _httpClient.GetFromJsonAsync<List<TaskModel>>($"http://localhost:5000/api/User/tasks/{idUserResult.Value}");

            return tasks ?? new List<TaskModel>();
        }
        else
        {
            // Gérer le cas où la récupération de jwt ou IdUser a échoué
            // Vous pouvez lancer une exception, enregistrer un message d'erreur, etc.
            return new List<TaskModel>();
        }
    }

    public async Task<List<TaskModel>> GetAllTasksAsync()
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");

        if (jwtResult.Success && jwtResult.Value != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);
            return await _httpClient.GetFromJsonAsync<List<TaskModel>>("http://localhost:5000/api/User/tasks") ?? new List<TaskModel>();
        }
        else
        {
            // Gérer le cas où la récupération de jwt a échoué
            // Vous pouvez lancer une exception, enregistrer un message d'erreur, etc.
            return new List<TaskModel>();
        }
    }

    public async Task<string> CreateTask(string text, bool valid)
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");
        var idUserResult = await _sessionStorage.GetAsync<int>("IdUser");

        if (jwtResult.Success && jwtResult.Value != null && idUserResult.Success)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);

            string gatewayUrl = "http://localhost:5000/";
            string loginRoute = $"api/User/task/{idUserResult.Value}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";
            var postData = new { Text = text, IsDone = valid, IdUser = idUserResult.Value };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

            return response.IsSuccessStatusCode ? "Bien fait" : "not Bien";
        }
        else
        {
            // Gérer le cas où la récupération de jwt ou IdUser a échoué
            // Vous pouvez lancer une exception, enregistrer un message d'erreur, etc.
            return "not Bien";
        }
    }

    public async Task<string> DeleteTaskAsync(int id)
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");
        var idUserResult = await _sessionStorage.GetAsync<int>("IdUser");

        if (jwtResult.Success && jwtResult.Value != null && idUserResult.Success )
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);

            string gatewayUrl = "http://localhost:5000/";
            string loginRoute = $"api/User/task/{idUserResult.Value}/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";
            HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

            return response.IsSuccessStatusCode ? "suppression Bien Fait" : "Erreur dans la suppression";
        }
        else
        {
            return "Erreur dans la suppression";
        }
    }

    public async Task<string> DeleteTaskAsyncAdmin(int id)
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");

        if (jwtResult.Success && jwtResult.Value != null )
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);

            string gatewayUrl = "http://localhost:5000/";
            string loginRoute = $"api/User/task/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";
            HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

            return response.IsSuccessStatusCode ? "suppression Bien Fait" : "Erreur dans la suppression";
        }
        else
        {
            return "Erreur dans la suppression";
        }
    }

    public async Task<string> UpdateTaskAsync(int id, string text, bool valid)
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");
        var idUserResult = await _sessionStorage.GetAsync<int>("IdUser");

        if (jwtResult.Success && jwtResult.Value != null && idUserResult.Success )
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);

            string gatewayUrl = "http://localhost:5000/";
            string loginRoute = $"api/User/task/{idUserResult.Value}/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            var putData = new { Text = text, IsDone = valid, IdUser = idUserResult.Value };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, putData);

            return response.IsSuccessStatusCode ? "update" : "not updated";
        }
        else
        {
            // Gérer le cas où la récupération de jwt ou IdUser a échoué
            // Vous pouvez lancer une exception, enregistrer un message d'erreur, etc.
            return "not updated";
        }
    }

    public async Task<string> UpdateTaskAsyncAdmin(int id, string text, bool valid , int IdUser)
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");

        if (jwtResult.Success && jwtResult.Value != null )
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);

            string gatewayUrl = "http://localhost:5000/";
            string loginRoute = $"api/User/task/{id}";
            string apiUrl = $"{gatewayUrl}{loginRoute}";

            var putData = new { Text = text, IsDone = valid, IdUser = IdUser };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, putData);

            return response.IsSuccessStatusCode ? "update" : "not updated";
        }
        else
        {
            // Gérer le cas où la récupération de jwt ou IdUser a échoué
            // Vous pouvez lancer une exception, enregistrer un message d'erreur, etc.
            return "not updated";
        }
    }
}
