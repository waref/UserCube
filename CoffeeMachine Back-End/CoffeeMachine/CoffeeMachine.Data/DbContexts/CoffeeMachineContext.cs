
using CoffeeMachine.Core.ConstantStrings;
using CoffeeMachine.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Action = CoffeeMachine.Models.Entities.Action;

namespace CoffeeMachine.Data
{
    public class CoffeeMachineContext : DbContext
    {
        private readonly ILogger<CoffeeMachineContext> _logger;
        private readonly Interfaces.IDbConnectionStringProvider _connectionStringProvider;


        public CoffeeMachineContext(DbContextOptions<CoffeeMachineContext> options, ILogger<CoffeeMachineContext> logger, Interfaces.IDbConnectionStringProvider connectionStringProvider) : base(options)
        {
            _logger = logger;
            _connectionStringProvider = connectionStringProvider;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _connectionStringProvider.GetConnectionString();

                _logger.LogInformation($"Attempting to use connection string: '{connectionString}'");

                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogError("Connection string is null or empty!");
                    throw new InvalidOperationException("Connection string is null or empty.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<CoffeeActionLog> CoffeeActionLogs { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<CoffeeMachineState> CoffeeMachineStates { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<CoffeeCreationOptions> CoffeeCreationOptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoffeeActionLog>()
                .HasOne(log => log.Action)
                .WithMany(a => a.CoffeeActionLogs)
                .HasForeignKey(log => log.ActionId)
                .IsRequired(true);

            modelBuilder.Entity<CoffeeActionLog>()
                .HasOne(actType => actType.ActionType)
                .WithMany()
                .HasForeignKey(logac => logac.ActionTypeId)
                .IsRequired(true);

            modelBuilder.Entity<CoffeeActionLog>()
                .HasOne(opt => opt.CoffeeCreationOptions)
                .WithMany()
                .HasForeignKey(opt => opt.CoffeeCreationOptionsId)
                .IsRequired(false);
          
            modelBuilder.Entity<ActionType>()
            .HasData(
                 new ActionType
                 {
                     Id = 1,
                     ActionName = ConsStringCoffeeMachineContext.ActionTypes.TurnOffName,
                     IsDisabledAction = false,
                     Description = ConsStringCoffeeMachineContext.ActionTypes.TurnOffDescription
                 },
                new ActionType
                {
                    Id = 2,
                    ActionName = ConsStringCoffeeMachineContext.ActionTypes.TurnOnName,
                    IsDisabledAction = false,
                    Description = ConsStringCoffeeMachineContext.ActionTypes.TurnOnDescription
                },
                new ActionType
                {
                    Id = 3,
                    ActionName = ConsStringCoffeeMachineContext.ActionTypes.MakeCoffeeName,
                    IsDisabledAction = false,
                    Description = ConsStringCoffeeMachineContext.ActionTypes.MakeCoffeeDescription
                }
            );


        }
        public void InitDemoData()
        {
            if (CoffeeActionLogs.Any()) return; // Skip if data already exists

            var random = new Random();
            var startDate = new DateTime(2025, 3, 25);
            var endDate = new DateTime(2025, 3, 28);
            var logs = new List<CoffeeActionLog>();

            for (int i = 0; i < 100; i++)
            {

                var logDate = startDate.AddDays(random.Next(0, (endDate - startDate).Days + 1));
                var hours = random.Next(8, 16);
                var minutes = hours == 8 ? random.Next(30, 60) : random.Next(0, 60);
                var timestamp = logDate.AddHours(hours).AddMinutes(minutes);

                // Create coffee options
                var coffeeOptions = new CoffeeCreationOptions
                {
                    NumEspressoShots = random.Next(1, 4),
                    AddMilk = random.Next(2) == 1
                };
                CoffeeCreationOptions.Add(coffeeOptions);


                var log = new CoffeeActionLog
                {
                    Timestamp = timestamp,
                    Action = new Action { Name = ConsStringCoffeeMachineContext.ActionTypes.MakeCoffeeName },
                    ActionTypeId = 3,
                    CoffeeCreationOptions = coffeeOptions,
                    Details = $"{ConsStringLogMessages.EspressoText} {coffeeOptions.NumEspressoShots} " +
                             (coffeeOptions.AddMilk ? $" and {ConsStringLogMessages.MilkText}" : "")
                };

                logs.Add(log);
            }

            CoffeeActionLogs.AddRange(logs);
            SaveChanges();
            _logger.LogInformation(ConsStringCoffeeMachineContext.LogAddedDemoRecords);
        }
    }
}