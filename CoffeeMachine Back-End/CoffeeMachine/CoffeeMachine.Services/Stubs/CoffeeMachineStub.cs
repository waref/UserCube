using CoffeeMachine.Core.ConstantStrings;
using CoffeeMachine.Core.Enums;
using CoffeeMachine.Core.Interfaces;
using CoffeeMachine.Core.Models.Dto;



namespace CoffeeMachine.Services.Stubs
{
    public class CoffeeMachineStub : ICoffeeMachine
    {
        public bool IsOn { get; private set; }
        public bool IsMakingCoffee { get; private set; }
        public State WaterLevelState { get; private set; }
        public State BeanFeedState { get; private set; }
        public State WasteCoffeeState { get; private set; }
        public State WaterTrayState { get; private set; }

        private bool IsInAlertState => WaterLevelState == State.Alert
                                      || BeanFeedState == State.Alert
                                      || WasteCoffeeState == State.Alert
                                      || WaterTrayState == State.Alert;

        private readonly Random _randomStateGenerator;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CoffeeMachineStub()
        {
            _randomStateGenerator = new Random();
        }

        public async Task TurnOnAsync()
        {
            if (IsOn)
                throw new InvalidOperationException(ConsStringLogMessages.InvalidState);

            // Generate sample state for testing
            WaterLevelState = GetRandomState();
            BeanFeedState = GetRandomState();
            WasteCoffeeState = GetRandomState();
            WaterTrayState = GetRandomState();
            IsOn = true;
        }

        public async Task TurnOffAsync()
        {
            if (!IsOn || IsMakingCoffee)
                throw new InvalidOperationException(ConsStringLogMessages.InvalidState);
            IsOn = false;
        }

        public async Task MakeCoffeeAsync(CoffeeCreationOptionsDto options)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!IsOn || IsMakingCoffee || IsInAlertState)
                    throw new InvalidOperationException(ConsStringLogMessages.InvalidState);

                IsMakingCoffee = true;

                await Task.Delay(10000);
            }
            finally
            {
                IsMakingCoffee = false;
                _semaphore.Release();
            }
        }

        // Randomly create a state for testing.
        // This can be replaced as required.
        private State GetRandomState() => _randomStateGenerator.Next(1, 10) < 3 ? State.Alert : State.Okay;
    }
}
