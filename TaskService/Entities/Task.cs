using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace TaskService.Entities
{
    public class Tasks
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsDone { get; set; }

        public int IdUser  {get; set; }

    }
    public class TaskCreate
    {
        public string Text { get; set; }
        public bool IsDone { get; set; }
        //public int IdUser  {get; set;}

    }
}
