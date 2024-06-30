using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using TodosApi.Container;
using TodosApi.Data;
using TodosApi.DTO;
using TodosApi.Middleware;
using TodosApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure Sql Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString"))
);

//Register DI container
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IValidator<TodoDTO>, TodoDTOValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//add custom middle
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
