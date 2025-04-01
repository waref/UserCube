using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models.Entities
{
    public class CoffeeCreationOptions {

        [Key]
        public int Id { get; set; }
        public int? NumEspressoShots { get; set; }
        public bool AddMilk { get; set; } 
    }
}
