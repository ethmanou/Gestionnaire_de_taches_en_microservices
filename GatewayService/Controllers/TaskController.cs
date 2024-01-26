using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserService.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;


namespace GatewayService.Controllers
{
    


    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public TaskController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }




        //Section Authorized pour un utilisateur basic


         // GET : api/User/tasks/1
        [Authorize]
        [HttpGet("tasks/{iduser}")]
        public async Task<ActionResult> GetTasks(int iduser)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            // on vérifie qu'elle existe bien
            if (UserId == null) return Unauthorized();
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                try{
                    // Send a POST request to the login endpoint
                    var response = await client.GetAsync($"api/tasks/{iduser}");


                    // Check if the response status code is 200 (OK)
                    if (response != null)
                    {
                        
                        var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                        return Ok(result);

                    }
                    else
                    {
                        return BadRequest("Failed to retrieve tasks from TaskService.");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Failed to retrieve tasks from TaskService.");


                }
            }
            
        }

         // POST : api/User/task/1
        [Authorize]
        [HttpPost("task/{iduser}")]
        public async Task<ActionResult> CreateTasks(TaskCreate task , int iduser)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");
                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.PostAsJsonAsync($"api/tasks/{iduser}" , task);


                    // Check if the response status code is 201 (Created)
                    if (response.IsSuccessStatusCode)
                    {
                        
                        var result = await response.Content.ReadFromJsonAsync<TaskModel>();
                        return Ok(result);

                    }
                    else
                    {
                        return BadRequest("Failed to create task.");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"Exception : {ex.Message}");
                    return BadRequest("Failed to create task.");

                }
            }
            
        }
        

        // DELETE : api/User/task/1/1
        [Authorize]
        [HttpDelete("task/{iduser}/{id}")]
        public async Task<IActionResult> DeleteTaskAsync(int iduser , int id){
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.DeleteAsync($"api/tasks/{iduser}/{id}");


                    // Check if the response status code is 201 (Created)
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("suppressin est bien fait");

                    }
                    else
                    {
                        return BadRequest("Erreur dans la suppression");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Erreur dans la suppression");

                }
            }
        }

        // PUT : api/User/task/1/1
        [Authorize]
        [HttpPut("task/{iduser}/{id}")]
        public async Task<IActionResult> UpdateTaskAsync(int iduser , int id , TaskCreate task){

            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.PutAsJsonAsync($"api/tasks/{iduser}/{id}" , task);


                    // Check if the response status code is 201 (Created)
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Updated");

                    }
                    else
                    {
                        return BadRequest("Not Updated");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Not Updated");

                }
            }
        }

        // section Authorized pour admin



        // GET : api/User/tasks
        [Authorize(Roles = "admin")]
        [HttpGet("tasks")]
        public async Task<ActionResult> GetAllTasks()
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                try{
                    // Send a POST request to the login endpoint
                    var response = await client.GetAsync($"api/tasks");


                    // Check if the response status code is 200 
                    if (response.IsSuccessStatusCode)
                    {
                        
                        var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                        return Ok(result);

                    }
                    else
                    {
                        return BadRequest("Failed to retrieve tasks from TaskService.");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Failed to retrieve tasks from TaskService.");
                }
            }
            
        }









        // POST : api/User/task
        [Authorize(Roles = "admin")]
        [HttpPost("task")]
        public async Task<ActionResult> CreateTasksAdmin(TaskCreate task )
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.PostAsJsonAsync($"api/tasks/{task.IdUser}" , task);


                    // Check if the response status code is 201 (Created)
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        
                        var result = await response.Content.ReadFromJsonAsync<TaskModel>();
                        return Ok(result);

                    }
                    else
                    {
                        return BadRequest("Failed to create task.");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ResponseBody : {ex.Message}");
                    return BadRequest("Failed to create task.");


                }
            }
            
        }


        // DELETE : api/User/task/1
        [Authorize( Roles = "admin")]
        [HttpDelete("task/{id}")]
        public async Task<IActionResult> DeleteTaskAsyncAdmin(int id){
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.DeleteAsync($"api/tasks/1/{id}");


                    // Check if the response status code is 201 (Created)
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("suppressin est bien fait");

                    }
                    else
                    {
                        return BadRequest("Erreur dans la suppression");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Erreur dans la suppression");
                }
            }
        }

        
        // PUT : api/User/task/1
        [Authorize(Roles = "admin")]
        [HttpPut("task/{id}")]
        public async Task<IActionResult> UpdateTaskAsyncAdmin(int Id , TaskCreate task){

            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                try{
                     // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.PutAsJsonAsync($"api/tasks/1/{Id}" , task);


                    // Check if the response status code is 201 (Created)
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Updated");

                    }
                    else
                    {
                        return BadRequest("Not Updated");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Not Updated");
                }
            }
        }
    }
}
