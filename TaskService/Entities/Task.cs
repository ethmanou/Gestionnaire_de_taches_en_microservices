using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace TaskService.Entities
{
    public class Tasks
    {
        public  int Id { get; set; }

        public required string Text { get; set; }

        public required bool IsDone { get; set; }

        public required int IdUser  {get; set; }

    }
    public class TaskCreate
    {
        public required string Text { get; set; }
        public required bool IsDone { get; set; }
        public required int IdUser  {get; set;}

    }
}
