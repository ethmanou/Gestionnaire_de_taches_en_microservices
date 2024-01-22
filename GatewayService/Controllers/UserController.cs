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
    public class UserController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        
        // api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin model)
        {
            
            
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/login/", model);

                // Check if the response status code is 200 (OK)
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // You can deserialize the response content here if needed
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    if(user != null) {
                    var role = "basic" ;
                    if(user.role != null){
                        role = user.role ;
                    }
                    string token_u = GenerateJwtToken(user.Id , role);
                    var result = new {user = user , token = token_u};
                    return Ok(result);
                    }
                    else{
                        return BadRequest("Login failed");
                    }
                }
                else
                {
                    return BadRequest("Login failed");
                }
            }
        }

        
        // api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateModel model)
        {
            var user = new { Name = model.Name, Email =model.Email , Password = model.Password ,  role = "basic"};
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/register", user);


                // Check if the response status code is 201 (Created)
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Register failed");
                }
            }
        }

        // api/User/tasks
        [Authorize(Roles = "admin")]
        [HttpGet("tasks")]
        public async Task<ActionResult> GetAllTasks()
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                // Send a POST request to the login endpoint
                var response = await client.GetFromJsonAsync<List<TaskModel>>($"api/tasks");


                // Check if the response status code is 201 (Created)
                if (response != null)
                {
                    
                    //var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return Ok(response);

                }
                else
                {
                    return BadRequest("Failed to retrieve tasks from TaskService.");
                }
            }
            
        }

        // GET : api/user/Users
        [Authorize(Roles = "admin")]
        [HttpGet("Users")]
        public async Task<ActionResult> GetAllUsers()
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                var response = await client.GetFromJsonAsync<List<UserDTO>>($"api/Users");


                // Check if the response status code is 201 (Created)
                if (response != null)
                {
                    // You can deserialize the response content here if needed
                    //var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return Ok(response);

                }
                else
                {
                    return BadRequest("Failed to retrieve Users from UsersService.");
                }
            }
            
        }

        


         // api/User/tasks
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

                // Send a POST request to the login endpoint
                var response = await client.GetFromJsonAsync<List<TaskModel>>($"api/tasks/{iduser}");


                // Check if the response status code is 201 (Created)
                if (response != null)
                {
                    // You can deserialize the response content here if needed
                    //var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return Ok(response);

                }
                else
                {
                    return BadRequest("Failed to retrieve tasks from TaskService.");
                }
            }
            
        }

         // api/User/task
        [Authorize]
        [HttpPost("task/{iduser}")]
        public async Task<ActionResult> CreateTasks(TaskCreate task , int iduser)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync($"api/tasks/{iduser}" , task);


                // Check if the response status code is 201 (Created)
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // You can deserialize the response content here if needed
                    //var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return Ok();

                }
                else
                {
                    return BadRequest("Failed to create task.");
                }
            }
            
        }

        [Authorize(Roles = "admin")]
        [HttpPost("task")]
        public async Task<ActionResult> CreateTasksAdmin(TaskCreate task )
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync($"api/tasks/{task.IdUser}" , task);


                // Check if the response status code is 201 (Created)
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // You can deserialize the response content here if needed
                    //var result = await response.Content.ReadFromJsonAsync<List<TaskModel>>();
                    return Ok();

                }
                else
                {
                    return BadRequest("Failed to create task.");
                }
            }
            
        }

        [Authorize]
        [HttpDelete("task/{iduser}/{id}")]
        public async Task<IActionResult> DeleteTaskAsync(int iduser , int id){
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


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
        }

        [Authorize( Roles = "admin")]
        [HttpDelete("task/{id}")]
        public async Task<IActionResult> DeleteTaskAsyncAdmin(int id){
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


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
        }

        [Authorize]
        [HttpPut("task/{iduser}/{id}")]
        public async Task<IActionResult> UpdateTaskAsync(int iduser , int id , TaskCreate task){

            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/tasks/{iduser}/{id}" , task);


                // Check if the response status code is 201 (Created)
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Updated ");

                }
                else
                {
                    return BadRequest("Erreur : Not Updated");
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("task/{id}")]
        public async Task<IActionResult> UpdateTaskAsyncAdmin(int Id , TaskCreate task){

            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/tasks/1/{Id}" , task);


                // Check if the response status code is 201 (Created)
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Updated ");

                }
                else
                {
                    return BadRequest("Erreur : Not Updated");
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsyncAdmin(int id){
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");


                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.DeleteAsync($"api/Users/{id}");


                // Check if the response status code is 200 
                if (response.IsSuccessStatusCode)
                {
                    return Ok("suppressin est bien fait");

                }
                else
                {
                    return BadRequest("Erreur dans la suppression");
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsyncAdmin(int id, UserDTO user){

            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Users/{id}" , user);


                // Check if the response status code is 200 
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Updated ");

                }
                else
                {
                    return BadRequest("Erreur : Not Updated");
                }
            }
        }


        ///ger
        private string GenerateJwtToken(int userId , string role)
            {
                var claims = new List<Claim>
                {
                    // On ajoute un champ UserId dans notre token avec comme valeur userId en string
                    new Claim("UserId", userId.ToString()),
                    new Claim(ClaimTypes.Role, role),
                };

                // On créer la clé de chiffrement
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretCesTMoiEthmaNouMohaMeDeNeLeMeiLLeUreQuEvOuSaLLeZreNcoNTrEZ"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // On paramètre notre token
                var token = new JwtSecurityToken(
                    issuer: "GatwayService", // Qui a émit le token
                    audience: "localhost:5000", // A qui est destiné ce token
                    claims: claims, // Les données que l'on veux encoder dans le token
                    expires: DateTime.Now.AddMinutes(3000), // Durée de validité
                    signingCredentials: creds); // La clé de chiffrement

                // On renvoie le token signé
                return new JwtSecurityTokenHandler().WriteToken(token);
            }

    }
}
