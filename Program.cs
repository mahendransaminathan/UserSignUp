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

// Setup CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();    
    var cosmosEndPoint = Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT");
    var cosmosKey = Environment.GetEnvironmentVariable("COSMOS_DB_KEY");


    if (string.IsNullOrEmpty(cosmosEndPoint) || string.IsNullOrEmpty(cosmosKey))
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogError("Cosmos DB configuration is missing.");
        throw new InvalidOperationException("Cosmos DB configuration is missing.");
        
    }

    // var cosmosEndPoint = configuration["ConnectionStrings:Endpoint"];
    // var cosmosKey = configuration["ConnectionStrings:PrimaryKey"];
     return new CosmosClient(cosmosEndPoint, cosmosKey);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,        
        typeof(CQRS.Commands.CreateUserCommand).Assembly,
        typeof(CQRS.Queries.GetUsersQuery).Assembly
    )
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost"); // <-- Added this!
app.UseRouting();
app.MapControllers();


app.Run();
