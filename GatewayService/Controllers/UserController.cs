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

        
        // POST : api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin model)
        {
            
            
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/login/", model);

                    // Check if the response status code is 200 (OK)
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // You can deserialize the response content here if needed
                        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                        if(user != null) {
                            string token_u = GenerateJwtToken(user.Id , user.role);
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
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Login failed");
                }
            }
        }

        
        // POST : api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateModel model)
        {
            var user = new { Name = model.Name, Email =model.Email , Password = model.Password ,  role = "basic"};

            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                try{
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
                catch(Exception ex)
                {
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Register failed");

                }
            }
        }


        // GET : api/User/Users
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
                try{
                    var response = await client.GetAsync($"api/Users");


                    // Check if the response status code is 200 (OK)
                    if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                            return Ok(result);

                        }
                    else
                    {
                        return BadRequest("Failed to retrieve Users from UsersService.");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Failed to retrieve Users from UsersService.");
                    
                }
            }
            
        }




        // DELETE : api/User/1
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsyncAdmin(int id){
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                try{
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
                catch(Exception ex){
                    Console.WriteLine($"ErreurException : {ex.Message}");
                    return BadRequest("Erreur dans la suppression");

                }
            }
        }



        // PUT : api/User/1
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsyncAdmin(int id, UserDTO user){

            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                try{
                    // Send a POST request to the login endpoint
                    HttpResponseMessage response = await client.PutAsJsonAsync($"api/Users/{id}" , user);


                    // Check if the response status code is 200 
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


        //generation de token 
        private string GenerateJwtToken(int userId , string role)
            {
                var claims = new List<Claim>
                {
                    // On ajoute un champ UserId dans le token avec comme valeur userId en string
                    new Claim("UserId", userId.ToString()),
                    new Claim(ClaimTypes.Role, role),
                };

                // On créer la clé de chiffrement
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretC'esTqUe:EthmaNou_MohaMeDeNe_C'EsT_Le_MeiLLeUre_QuE_vOus_AllEz_rEnCoNtReZ_DanS_VoTrE_ViE_Je_RiGole!_MinCe!_nOn_C'esT_VraI"));
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
