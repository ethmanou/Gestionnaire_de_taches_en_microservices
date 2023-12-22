using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserService.Entities;


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
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/login", model);

                // Check if the response status code is 200 (OK)
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // You can deserialize the response content here if needed
                    var result = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return Ok(result);
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
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/register", model);


                // Check if the response status code is 201 (Created)
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // You can deserialize the response content here if needed
                    return Ok();
                }
                else
                {
                    return BadRequest("Register failed");
                }
            }
        }
         // api/User/tasks
        [HttpGet("tasks")]
        public async Task<ActionResult> GetTasks()
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                // Send a POST request to the login endpoint
                var response = await client.GetFromJsonAsync<List<TaskModel>>("api/tasks/");


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
        [HttpPost("task")]
        public async Task<ActionResult> GetTasks(TaskCreate task)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5002/");


                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/tasks/" , task);


                // Check if the response status code is 201 (Created)
                if (response.StatusCode == HttpStatusCode.Created)
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

    }
}
