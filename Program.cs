using api.Data;
using api.Models;
using api.Repositories;
using api.Services;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Use your React app's URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Allow cookies
    });
});
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("OpenPolicy", policy =>
//     {
//         policy.WithOrigins("http://localhost:3000")
//               .AllowAnyMethod()
//               .AllowAnyHeader()
//               .AllowCredentials();
//     });

//     // options.AddPolicy("OpenPolicy",
//     // builder =>
//     // {
//     //     builder.AllowAnyOrigin()
//     //            .AllowAnyMethod()
//     //            .AllowAnyHeader();
//     // });
// });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new Database(connectionString));

// Register each repository as scoped
builder.Services.AddScoped<SessionRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<TrainerRepository>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<AuthRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ReactAppPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
