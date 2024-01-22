
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace UserService.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string? Name { get; set;}
        public required string? Email { get; set;}
        public required string? role  {get; set; }
        public required string? PasswordHash { get; set; }

        public override string ToString()
        {
            return $"Id: ${Id} Name: ${Name} Email : ${Email} role: ${role} Pass: ${PasswordHash}";
        }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public required string? Name { get; set; }
        public required string? Email { get; set; }
        public required string? role { get; set; }
    }

    public class UserCreateModel
    {
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string role  { get; set; }
    }

    public class UserUpdateModel
    {
        public int Id { get; set; }
        public required string? Password { get; set; }
        public required string? Name { get; set; }
        public required string? Email { get; set; }
        public required string? role { get; set; }
    }
    public class UserLogin
    {
        public required string Name { get; set; }
        public required string Pass { get; set; }
    }
}
