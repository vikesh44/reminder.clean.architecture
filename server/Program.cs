using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReminderApp.Infrastructure.Security;
using ReminderApp.Application.Ports;
using ReminderApp.Application.UseCases;
using ReminderApp.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// URLs
builder.WebHost.UseUrls("http://localhost:5080");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for Vite default dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Ports implementations
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService>(sp =>
{
    var secret = builder.Configuration["Jwt:Secret"]!;
    var issuer = builder.Configuration["Jwt:Issuer"];
    var audience = builder.Configuration["Jwt:Audience"];
    return new JwtTokenService(secret, issuer, audience);
});

builder.Services.AddSingleton<IClock, SystemClock>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();

// Use cases
builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<LoginUser>();
builder.Services.AddScoped<ListReminders>();
builder.Services.AddScoped<AddReminder>();
builder.Services.AddScoped<DeleteReminder>();

// Auth
var secret = builder.Configuration["Jwt:Secret"]!;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

// Disable default claim type mapping to preserve JWT claim names
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowVite");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
