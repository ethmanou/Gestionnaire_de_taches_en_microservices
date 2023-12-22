using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskService.Entities;

namespace TaskService.Data
{

    public class TaskServiceContext : DbContext
    {
        public TaskServiceContext(DbContextOptions<TaskServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; } = default!;
    }


}
