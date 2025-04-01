using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models.Entities
{
    public class Action
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<CoffeeActionLog>? CoffeeActionLogs { get; set; }
    }
}
