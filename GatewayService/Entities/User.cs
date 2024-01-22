using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


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
        public required string? Name { get; set; }
        public required string? Pass { get; set; }
    }
    public class UserCreateModel
    {
        
        public required string Password { get; set; }

        [Alphanumeric(ErrorMessage = "username doit être alphanumérique.")]
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "E-mail n'est pas valide.")]
        public required string Email { get; set; }
    }

    public class TaskModel
    {
        public int Id { get; set; }

        public  string? Text { get; set; }

        public bool? IsDone { get; set; }

        public int? IdUser {get; set; }

    }

    public class TaskCreate
    {
        public string? Text { get; set; }
        public bool? IsDone { get; set; }
        public int? IdUser  { get; set; }
    }
    public class response_t
    {
        public UserDTO? user {get; set; } 
        public string? token {get; set;}
    }


    public class AlphanumericAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? inputValue = value.ToString();

                // Utilisez une expression régulière pour vérifier que la chaîne ne contient que des caractères alphanumériques
                if (!string.IsNullOrEmpty(inputValue) && !Regex.IsMatch(inputValue, "^[a-zA-Z0-9]+$"))
                {
                    return new ValidationResult(ErrorMessage ?? "La valeur doit être alphanumérique.");
                }
            }

            return ValidationResult.Success;
        }
    }


}
