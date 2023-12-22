using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.Data;
using TaskService.Entities;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskServiceContext _context;

        public TasksController(TaskServiceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<IEnumerable<Tasks>> Get()
        {
            return await _context.Tasks
                .Select(task => TaskToTasks(task))
                .ToListAsync();
        }

        // GET api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> Get(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return TaskToTasks(task);
        }

        // POST api/Tasks
        [HttpPost]
        public async Task<ActionResult<Tasks>> CreateTask(TaskCreate task)
        {
            var newTask = new Tasks
            {
                Text = task.Text,
                IsDone = task.IsDone
            };

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the newly created task
            return CreatedAtAction(nameof(Get), new { id = newTask.Id }, TaskToTasks(newTask));
        }

        // PUT api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TaskCreate taskUpdate)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Text = taskUpdate.Text;
            task.IsDone = taskUpdate.IsDone;

            await _context.SaveChangesAsync();

            return Ok(TaskToTasks(task));
        }

        // DELETE api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        private static Tasks TaskToTasks(Tasks task)
        {
            return new Tasks
            {
                Id = task.Id,
                Text = task.Text,
                IsDone = task.IsDone
            };
        }
    }
}
