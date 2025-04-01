namespace CoffeeMachine.Core.Interfaces
{
    public interface IUtilizationService
    {
        Task<Dictionary<string, string>> GetFirstAndLastCupTimesPerDayOfWeekAsync();
        Task<Dictionary<string, double>> GetAverageCupsPerHourAsync();
    }
}