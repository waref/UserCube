using System.ComponentModel.DataAnnotations;
namespace CoffeeMachine.Models.Entities
{ 
    public class ActionType
    {
        [Key]
        public int Id { get; set; }
        public string? ActionName { get; set; }
        public bool IsDisabledAction { get; set; }
        public string? Description { get; set; }
    }
}
