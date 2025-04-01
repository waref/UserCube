namespace CoffeeMachine.Core.Models.Dto
{
    public class ActionTypeDto
    {
        public int Id { get; set; }
        public string? ActionName { get; set; }
        public bool IsDisabledAction { get; set; }
        public string? Description { get; set; }
    }
}
