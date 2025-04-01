
using CoffeeMachine.Core.Models.Dto;

namespace CoffeeMachine.Core.Models
{
    public class CoffeeActionLogDto
    {
        public int? Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int? ActionId { get; set; }
        public ActionDto? Action { get; set; }
        public int? ActionTypeId { get; set; }
        public ActionTypeDto? ActionType { get; set; }
        public int? CoffeeCreationOptionsId { get; set; }
        public CoffeeCreationOptionsDto? CoffeeCreationOptions{get;set;}
        public string? Details { get; set; }
    }
}
