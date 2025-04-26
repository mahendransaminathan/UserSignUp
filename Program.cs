using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using Repositories;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddSingleton<CosmosClient>(sp =>
{

    var configuration = sp.GetRequiredService<IConfiguration>();
    var cosmosEndPoint = Environment.GetEnvironmentVariable("CosmosDB:EndpointUri") ?? configuration["CosmosDB:EndpointUri"];
    var cosmosKey = Environment.GetEnvironmentVariable("CosmosDB:PrimaryKey") ?? configuration["CosmosDB:PrimaryKey"];

    var connectionString = builder.Configuration.GetConnectionString("CosmosDB:ConnectionString");
    return new CosmosClient(cosmosEndPoint, cosmosKey);
});
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(Models.User).Assembly,
        typeof(Repositories.UserRepository).Assembly,
        typeof(Controllers.UserController).Assembly,
        typeof(CQRS.Commands.CreateUserCommand).Assembly,
        typeof(CQRS.Queries.GetUsersQuery).Assembly
    )
);
// builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CQRS.Commands.UpdateUserCommand>());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins("http://localhost:3000")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());    
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();


app.Run();
