
using CoffeeMachine.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models.Entities
{
    public class CoffeeMachineState
    {
        [Key]
        public int Id { get; set; }
        public State WaterLevel { get; set; }
        public State BeanFeed { get; set; }
        public State WasteCoffee { get; set; }
        public State WaterTray { get; set; }
        public bool IsOn { get; set; }
        public bool IsMakingCoffee { get; set; }
    }
}
