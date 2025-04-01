using CoffeeMachine.Core.ConstantStrings;
using CoffeeMachine.Core.Interfaces;

namespace CoffeeMachine.Services
{
    /// <summary>
    /// Implementation service for monitoring the utilization of coffe machine
    /// </summary>
    public class UtilizationService : IUtilizationService
    {
        private readonly ICoffeeActionLogService _logService;

        public UtilizationService(ICoffeeActionLogService logService)
        {
            _logService = logService;
        }

        public async Task<Dictionary<string, string>> GetFirstAndLastCupTimesPerDayOfWeekAsync()
        {
            var logs = await _logService.GetCoffeeActionLogsAsync();
            var coffeeLogs = logs.Where(log => log.ActionTypeId == 3).ToList();
            var result = new Dictionary<string, string>();

            // Group logs by week and day of the week
            var groupedLogs = coffeeLogs
                .GroupBy(log => new { WeekOfYear = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(log.Timestamp, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday), log.Timestamp.DayOfWeek })
                .OrderBy(group => group.Key.WeekOfYear)
                .ThenBy(group => group.Key.DayOfWeek);

            foreach (var group in groupedLogs)
            {
                var firstCup = group.Min(log => log.Timestamp).ToString("HH:mm:ss");
                var lastCup = group.Max(log => log.Timestamp).ToString("HH:mm:ss");
                result[group.Key.DayOfWeek.ToString()] = $"{ConsStringLogMessages.FirstCup}{firstCup}, {ConsStringLogMessages.LastCup}{lastCup}";
            }

            return result;
        }

        public async Task<Dictionary<string, double>> GetAverageCupsPerHourAsync()
        {
            var logs = await _logService.GetCoffeeActionLogsAsync();
            var coffeeLogs = logs.Where(log => log.ActionTypeId == 3).ToList();
            var result = new Dictionary<string, double>();

            var groupedLogs = coffeeLogs.GroupBy(log => log.Timestamp.Hour);

            foreach (var group in groupedLogs)
            {
                double averageCups = (double)group.Count() / coffeeLogs.Count;
                result[$"{group.Key:D2}:00"] = Math.Round(averageCups, 2);
            }

            return result;
        }
    }
}