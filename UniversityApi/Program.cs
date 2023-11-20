using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Repositories;
using UniversityApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UniversistyContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LecturerRepository>();
builder.Services.AddScoped<FacultyRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<LecturerServices>();
builder.Services.AddScoped<FacultyServices>();
builder.Services.AddScoped<CourseServices>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
