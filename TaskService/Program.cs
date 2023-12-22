using TaskService.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Data;
using TaskService.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskServiceContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TaskServiceContext") ?? throw new InvalidOperationException("Connection string 'TaskServiceContext' not found.")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
