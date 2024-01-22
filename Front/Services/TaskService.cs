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
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");
        var idUserResult = await _sessionStorage.GetAsync<int>("IdUser");

        if (jwtResult.Success && jwtResult.Value != null && idUserResult.Success)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);
            
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5000/api/User/tasks/{idUserResult.Value}");

                if (response.IsSuccessStatusCode)
                {
                    
                    var tasks = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return tasks ;
                    
                }
                else
                {
                    if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  new List<TaskModel>();
                        }
                        else{
                            Console.WriteLine($"Erreur HTTP : {response.StatusCode} - {response.ReasonPhrase}");
                            return  null;
                        }
                    
                    return null;
                }
            }
            catch(HttpRequestException ex){
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return null;
            }

        }
        else
        {
            return new List<TaskModel>();
        }
    }

    public async Task<List<TaskModel>> GetAllTasksAsync()
    {
        var jwtResult = await _sessionStorage.GetAsync<string>("jwt");

        if (jwtResult.Success && jwtResult.Value != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResult.Value);
             try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5000/api/User/tasks");

                if (response.IsSuccessStatusCode)
                {
                    
                    var tasks = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return tasks;
                    
                }
                else
                {
                    if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  new List<TaskModel>();
                        }
                        else{
                            Console.WriteLine($"Erreur HTTP : {response.StatusCode} - {response.ReasonPhrase}");
                            return  null;
                        }
                    
                    return null;
                }
            }
            catch(HttpRequestException ex){
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return null;
            }
        }
        else
        {
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

            try{
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

                if(response.IsSuccessStatusCode){
                        return   "Création Bien Fait";
                    }else{
                        
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  "Erreur dans la Création";
                        }
                        else{
                            return  "Erreur de Connexion";
                        }

                    }
            }
            catch(Exception ex){
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return  "Erreur de Connexion";
            }
            

            
        }
        else
        {
            return   "Erreur de Connexion";
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

            try{
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                if(response.IsSuccessStatusCode){
                        return   "suppression Bien Fait";
                    }else{
                        
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  "Erreur dans la suppression";
                        }
                        else{
                            return  "Erreur de Connexion";
                        }

                    }
            }
            catch(Exception ex){
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return "Erreur de Connexion";
            }
            

           
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
            try{
                    HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                    if(response.IsSuccessStatusCode){
                        return   "suppression Bien Fait";
                    }else{
                        
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  "Erreur dans la suppression";
                        }
                        else{
                            return  "Erreur de Connexion";
                        }

                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                    return "Erreur de Connexion";
                }
            
        }
        else
        {
            return "Erreur de Connexion";
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

            try{
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, putData);

                if(response.IsSuccessStatusCode){
                        return   "mis-à-jour Bien Fait";
                    }else{
                        
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  "Erreur dans la mis-à-jour";
                        }
                        else{
                            return  "Erreur de Connexion";
                        }

                    }
            }
            catch(Exception ex){
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return  "Erreur de Connexion";
            }
        }
        else
        {
            return "Erreur de Connexion";
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

            try{
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, putData);

                if(response.IsSuccessStatusCode){
                        return   "mis-à-jour Bien Fait";
                    }else{
                        
                        if ((int) response.StatusCode >= 400 && (int)response.StatusCode < 500){
                            return  "Erreur dans la mis-à-jour";
                        }
                        else{
                            return  "Erreur de Connexion";
                        }

                    }
            }
            catch(Exception ex){
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return "Erreur de Connexion";
            }
        }
        else
        {
            
            return "Erreur de Connexion";
        }
    }
}
