using CoffeeMachine.Core.ConstantStrings;
using CoffeeMachine.Core.Interfaces;
using CoffeeMachine.Data;
using CoffeeMachine.Data.Factory;
using CoffeeMachine.Data.Interfaces;
using CoffeeMachine.Data.Repositories;
using CoffeeMachine.Data.Services;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Stubs;
using CoffeeMachine.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

//  Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeMachine.WebAPI", Version = "v1" });
});

// Get current Env mode & DB CNX String
var isDevelopment = builder.Configuration.GetValue<bool>("IsDevelopment");
builder.Environment.EnvironmentName = isDevelopment ? "Development" : "Production";

// Register DbContext with Dependency Injection, retrieving the connection string from CoffeeMachine.Data
builder.Services.AddScoped<IDbConnectionStringProvider, DevelopmentDbConnectionStringProvider>();

builder.Services.AddDbContext<CoffeeMachineContext>((serviceProvider, options) =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<CoffeeMachineContext>>();
    var connectionStringProvider = serviceProvider.GetRequiredService<IDbConnectionStringProvider>();
    options.UseSqlServer(connectionStringProvider.GetConnectionString());
}, ServiceLifetime.Scoped);



// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register Services
builder.Services.AddScoped<ICoffeeMachineService, CoffeeMachineService>();
builder.Services.AddScoped<ICoffeeActionLogService, CoffeeActionLogService>();
builder.Services.AddScoped<IUtilizationService, UtilizationService>();

//  Configure Stubs
builder.Services.AddSingleton<CoffeeMachineStub>();

builder.Services.AddAutoMapper(typeof(CoffeeMachine.Mappings.MappingProfile));

// Seq Logging 
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(ConsStringConfig.SeqLogURL)
    .CreateLogger();

builder.Host.UseSerilog();


// Add Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

//Add CORS services here
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeMachine.WebAPI v1"));
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CoffeeMachineContext>();
        context.Database.EnsureCreated();

        //Demo data
        context.InitDemoData();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, ConsStringLogMessages.ErrorInitDB);
    }
}
// Add the middleware ErrorHandlingMiddleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();