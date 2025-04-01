using Microsoft.Extensions.Configuration;

namespace CoffeeMachine.Data.Services
{
    public class DevelopmentDbConnectionStringProvider : Interfaces.IDbConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public DevelopmentDbConnectionStringProvider()
        {

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CoffeeMachine.Data");

            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath) 
                .AddJsonFile("dbsettings.Development.json")
                .Build();
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }
    }
}