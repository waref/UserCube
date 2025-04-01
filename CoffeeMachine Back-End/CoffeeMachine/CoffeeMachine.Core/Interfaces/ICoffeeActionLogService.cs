using CoffeeMachine.Core.Models;
using CoffeeMachine.Core.Models.Dto;

namespace CoffeeMachine.Core.Interfaces
{
    public interface ICoffeeActionLogService
    {
        Task LogActionAsync(CoffeeActionLogDto logDto);
        Task LogCoffeeCreationOption(CoffeeCreationOptionsDto optionDto);
        Task<IEnumerable<CoffeeActionLogDto>> GetCoffeeActionLogsAsync();
        Task<IEnumerable<ActionTypeDto>> GetCoffeeActionTypesAsync();
    }
}
