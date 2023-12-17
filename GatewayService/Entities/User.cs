
namespace UserService.Entities
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
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

        public required string Text { get; set; }

        public bool IsDone { get; set; }

    }

    public class TaskCreate
    {
        public required string Text { get; set; }
        public string IsDone { get; set; }
    }


}
