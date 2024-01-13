
namespace UserService.Entities
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? role  { get; set; }
    }
    public class UserLogin
    {
        public required string Name { get; set; }
        public required string Pass { get; set; }
    }
    public class UserCreateModel
    {
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class TaskModel
    {
        public int Id { get; set; }

        public  string Text { get; set; }

        public bool IsDone { get; set; }

        public int IdUser {get; set; }

    }

    public class TaskCreate
    {
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public int IdUser  { get; set; }
    }
    public class response_t
    {
        public UserDTO user {get; set;}
        public string token {get; set;}
    }


}
