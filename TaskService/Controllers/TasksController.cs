﻿using Microsoft.AspNetCore.Mvc;
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
        

        //GET : api/tasks
        [HttpGet()]
        public async Task<IEnumerable<Tasks>> GetALL()
        {
             return await _context.Tasks
                        .ToListAsync();
        }

        // GET: api/Tasks/iduser
        [HttpGet("{iduser}")]
        public async Task<IEnumerable<Tasks>> Get(int iduser)
        {
             return await _context.Tasks
                        .Where(task => task.IdUser == iduser)
                        .ToListAsync();
        }

        // GET api/Tasks/5
        [HttpGet("{iduser}/{id}")]
        public async Task<ActionResult<Tasks>> GetT(int id , int iduser)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST api/Tasks
        [HttpPost("{iduser}")]
        public async Task<ActionResult<Tasks>> CreateTask(TaskCreate task , int iduser)
        {
            var newTask = new Tasks
            {
                Text = task.Text,
                IsDone = task.IsDone,
                IdUser = iduser ,
                DeadLine = task.DeadLine ,
            };

            if(task.IsDone){
                newTask.DoneDate = DateTime.Now ;
            }
            else{
                newTask.DoneDate = null ;
            }

            

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            // Return a 200 Created response with the newly created task
            return Ok(newTask);
        }

        // PUT api/Tasks/5
        [HttpPut("{iduser}/{id}")]
        public async Task<IActionResult> Put(int iduser, int id , TaskCreate taskUpdate)
        {
            if(iduser != 1){
                var task = await _context.Tasks
                            .FirstOrDefaultAsync(t => t.Id == id && t.IdUser == iduser);
                if (task == null)
                {
                    return NotFound();
                }

                if((!task.IsDone) && (taskUpdate.IsDone)){
                    task.DoneDate = DateTime.Now ;
                }

                task.Text = taskUpdate.Text;
                task.IsDone = taskUpdate.IsDone;
                task.IdUser = iduser;
                task.DeadLine = taskUpdate.DeadLine;


               

                await _context.SaveChangesAsync();

                return Ok(task);
            }
            else{
                var task = await _context.Tasks
                            .FirstOrDefaultAsync(t => t.Id == id);
                if (task == null)
                {
                    return NotFound();
                }

                if((!task.IsDone) && (taskUpdate.IsDone)){
                    task.DoneDate = DateTime.Now ;
                }

                task.Text = taskUpdate.Text;
                task.IsDone = taskUpdate.IsDone;
                task.IdUser = taskUpdate.IdUser;
                task.DeadLine = taskUpdate.DeadLine;
                


                await _context.SaveChangesAsync();

                return Ok(task);
            }
        }

        // DELETE api/Tasks/5
        [HttpDelete("{idUser}/{id}")]
        public async Task<IActionResult> Delete(int idUser , int id)
        {
            if(idUser != 1){
                
                var task = await _context.Tasks
                                .FirstOrDefaultAsync(t => t.Id == id && t.IdUser == idUser);
                if (task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else{
                var task = await _context.Tasks
                                .FirstOrDefaultAsync(t => t.Id == id);
                if (task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
        }

        
    }
}
