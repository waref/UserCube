using CoffeeMachine.Core.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models.Entities
{
    public class CoffeeActionLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int ActionId { get; set; }
        public Action Action { get; set; }
        public int ActionTypeId { get; set; }
        public ActionType ActionType { get; set; }
        public int? CoffeeCreationOptionsId { get; set; }
        public CoffeeCreationOptions? CoffeeCreationOptions { get; set; }
        public string? Details { get; set; }
    }
}
