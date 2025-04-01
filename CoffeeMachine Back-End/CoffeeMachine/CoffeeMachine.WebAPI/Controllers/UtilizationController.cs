using CoffeeMachine.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilizationController : ControllerBase
    {
        private readonly IUtilizationService _utilizationService;

        public UtilizationController(IUtilizationService utilizationService)
        {
            _utilizationService = utilizationService;
        }

        ///  <summary>
        ///   Gets the first and last cup times per day of the week.
        ///  </summary>
        ///  <returns>A dictionary containing the first and last cup times for each day of the week.</returns>
        [HttpGet("firstLastCupTimes")]
        public async Task<ActionResult<Dictionary<string, string>>> GetFirstAndLastCupTimesPerDayOfWeek()
        {
            var result = await _utilizationService.GetFirstAndLastCupTimesPerDayOfWeekAsync();
            return Ok(result);
        }

        ///  <summary>
        ///   Gets the average number of cups made per hour.
        ///  </summary>
        ///  <returns>A dictionary containing the average number of cups made per hour.</returns>
        [HttpGet("averageCupsPerHour")]
        public async Task<ActionResult<Dictionary<string, double>>> GetAverageCupsPerHour()
        {
            var result = await _utilizationService.GetAverageCupsPerHourAsync();
            return Ok(result);
        }
    }
}