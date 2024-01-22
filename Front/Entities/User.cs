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
    // TaskModel
    public class TaskModel
    {
        public int Id { get; set; }

        public  required string? Text { get; set; }

        public required bool IsDone { get; set; }

        public int IdUser  { get; set; }

    }

    public class TaskCreate
    {
        public required string? Text { get; set; }
        public required bool IsDone { get; set; }
        public int IdUser  { get; set; }
    }
    
    public class response_t
    {
        public required  UserDTO? user {get; set;}
        public required string? token {get ; set ;}
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

    


    





}
