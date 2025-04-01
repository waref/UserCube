using CoffeeMachine.Core.Interfaces;
using CoffeeMachine.Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly ICoffeeMachineService _coffeeMachineService;
        private readonly ILogger<CoffeeMachineController> _logger;

        public CoffeeMachineController(ICoffeeMachineService coffeeMachineService, ILogger<CoffeeMachineController> logger)
        {
            _coffeeMachineService = coffeeMachineService;
            _logger = logger;
        }

        [HttpGet("state")]
        public IActionResult GetState()
        {
            try
            {
                var state = new
                {
                    _coffeeMachineService.IsOn,
                    _coffeeMachineService.IsMakingCoffee,
                    _coffeeMachineService.WaterLevelState,
                    _coffeeMachineService.BeanFeedState,
                    _coffeeMachineService.WasteCoffeeState,
                    _coffeeMachineService.WaterTrayState
                };
                return Ok(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting coffee machine state.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("turnon")]
        public async Task<IActionResult> TurnOn()
        {
            try
            {
                await _coffeeMachineService.TurnOnAsync();
                return Ok(new { message = "Machine turned on." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error turning on coffee machine.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("turnoff")]
        public async Task<IActionResult> TurnOff()
        {
            try
            {
                await _coffeeMachineService.TurnOffAsync();
                return Ok(new { message = "Machine turned off." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error turning off coffee machine.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("makecoffee")]
        public async Task<IActionResult> MakeCoffee(CoffeeCreationOptionsDto options)
        {
            try
            {
                await _coffeeMachineService.MakeCoffeeAsync(options);
                return Ok(new { mewwage = "Coffee making started." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error making coffee.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}