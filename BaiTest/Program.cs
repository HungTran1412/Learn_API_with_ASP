using System.Text.Json.Serialization;
using BaiTest.Models;
using BaiTest.Services;
using BaiTest.Services.Impl;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Tùy ch?n: D? ??c h?n trong quá trình phát tri?n
         options.JsonSerializerOptions.WriteIndented = true; 
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connect to database
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies()
.UseSqlServer(connectionString));

builder.Services.AddScoped<IStudentService, StudentServiceImpl>();
builder.Services.AddScoped<IExamAssignmentService, ExamAssignmentServiceImpl>();
builder.Services.AddScoped<IExamRoomsService, ExamRoomsServiceImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
