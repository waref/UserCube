namespace CoffeeMachine.Core.Models.Dto
{
    public class ActionDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<CoffeeActionLogDto>? CoffeeActionLogs { get; set; }
    }
}
