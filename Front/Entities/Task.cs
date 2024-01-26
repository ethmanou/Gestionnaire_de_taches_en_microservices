using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Front.Entities
{


    public class TaskModel
    {
        public int Id { get; set; }

        public  required string? Text { get; set; }

        public required bool IsDone { get; set; }

        public int IdUser  { get; set; }

        public required DateTime DeadLine { get; set; }

        public  DateTime? DoneDate { get; set; }

    }

    public class TaskCreate
    {
        public required string? Text { get; set; }

        public required bool IsDone { get; set; }

        public int IdUser  { get; set; }
        
        public required DateTime DeadLine { get; set; }

        public  DateTime? DoneDate { get; set; }


    }
    
    

}
