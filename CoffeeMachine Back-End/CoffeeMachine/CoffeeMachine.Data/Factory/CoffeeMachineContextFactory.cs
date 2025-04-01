using CoffeeMachine.Core.ConstantStrings;
using CoffeeMachine.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace CoffeeMachine.Data.Factory
{
    public class CoffeeMachineContextFactory : IDesignTimeDbContextFactory<CoffeeMachineContext>
    {
        private static IConfiguration Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(ConsStringConfig.DbDevConfig)
            .Build();

        public CoffeeMachineContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoffeeMachineContext>();
            var connectionStringProvider = new DevelopmentDbConnectionStringProvider();

            optionsBuilder.UseSqlServer(connectionStringProvider.GetConnectionString());

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<CoffeeMachineContext>();

            return new CoffeeMachineContext(optionsBuilder.Options, logger, connectionStringProvider);
        }
    }
}