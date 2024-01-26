using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Front.Entities
{

     
    public class UserDTO
    {
        public int Id { get; set; }
        public required string? Name { get; set; }
        public required string? Email { get; set; }
        public required string? role {get; set; }
    }
    public class UserCreateModel
    {
        public required string? Password { get; set; }
        public required string? Name { get; set; }
        public required string? Email { get; set; }
    }
    
    public class response_t
    {
        public required  UserDTO? user {get; set;}
        public required string? token {get ; set ;}
    }

    public class RegisterModel
    {
        [Alphanumeric(ErrorMessage = "username doit être alphanumérique.")]
        public string Name { get; set; } = "";

        
        public string Password { get; set; } = "";

        [EmailAddress(ErrorMessage = "E-mail n'est pas valide.")]
        public string Email { get; set; } = "";
    }

   



    //pour verifier username
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

    


    public class ErrorDetails
    {
        public Dictionary<string, List<string>> Name { get; set; }
        
    }
    
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public ErrorDetails Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class PasswordRule
    {
        public string Description { get; set; }
        public bool IsSatisfied { get; set; }
    }




        public interface IKonamiCodeHandler
        {
            event Action KonamiCodeCompleted;
            void HandleKeyPress(string key);
        }

}
