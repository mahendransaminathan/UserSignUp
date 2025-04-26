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

    return new CosmosClient(cosmosEndPoint, cosmosKey);
});
builder.Services.AddSingleton<IUserRepository>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>(); 
    var configuration = sp.GetRequiredService<IConfiguration>();   
    return new UserRepository(cosmosClient, configuration);
});

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

// Setup CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors("AllowLocalhost"); // <-- Added this!
app.UseHttpsRedirection();
app.MapControllers();


app.Run();
