using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Api.OptionsSetup;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Infraestructure;
using CleanArchitecture.Infraestructure.Authentication;

using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplyMigration();
// app.SeedData();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();

