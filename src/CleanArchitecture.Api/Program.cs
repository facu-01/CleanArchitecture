using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Api.OptionsSetup;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Infraestructure;
using CleanArchitecture.Infraestructure.Authentication;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, conf) =>
    conf.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<
    IAuthorizationPolicyProvider,
    PermissionAuthorizationPolicyProvider>();

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
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));
}

app.ApplyMigration();
app.SeedData();
app.SeedDataUsers();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();

