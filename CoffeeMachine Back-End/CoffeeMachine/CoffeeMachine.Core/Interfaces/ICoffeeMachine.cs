using CoffeeMachine.Core.Enums;
using CoffeeMachine.Core.Models.Dto;

namespace CoffeeMachine.Core.Interfaces
{
    public interface ICoffeeMachine
    {
        bool IsOn { get; }
        bool IsMakingCoffee { get; }
        State WaterLevelState { get; }
        State BeanFeedState { get; }
        State WasteCoffeeState { get; }
        State WaterTrayState { get; }
        Task TurnOnAsync();
        Task TurnOffAsync();
        Task MakeCoffeeAsync(CoffeeCreationOptionsDto options);
        
    }
}
