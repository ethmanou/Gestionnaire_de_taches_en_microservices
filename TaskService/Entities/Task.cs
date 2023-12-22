using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace TaskService.Entities
{
    public class Tasks
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsDone { get; set; }
        public override string ToString()
        {
            return $"Id: ${Id} Text: ${Text}";
        }

    }
    public class TaskCreate
    {
        public string Text { get; set; }
        public bool IsDone { get; set; }
    }

    public class TaskItem
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }
}
