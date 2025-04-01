using CoffeeMachine.Core.Enums;
using CoffeeMachine.Core.Interfaces;
using CoffeeMachine.Core.Models;
using CoffeeMachine.Models.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CoffeeMachine.Core.Models.Dto;
using CoffeeMachine.Services.Stubs;
using CoffeeMachine.Core.ConstantStrings;

namespace CoffeeMachine.Services
{
    /// <summary>
    /// Implementation Service for commanding coffees to hardware.
    /// We use stub class <see cref="CoffeeMachineStub"/>
    /// </summary>
    public class CoffeeMachineService : ICoffeeMachineService
    {
        private readonly IRepository<CoffeeMachineState> _stateRepository;
        private readonly ICoffeeActionLogService _logService;
        private readonly IMapper _mapper;
        private readonly ILogger<CoffeeMachineService> _logger;
        private readonly CoffeeMachineStub _coffeeMachine;

        public CoffeeMachineService(
            IRepository<CoffeeMachineState> stateRepository,
            ICoffeeActionLogService logService,
            IMapper mapper,
            ILogger<CoffeeMachineService> logger,
            CoffeeMachineStub coffeeMachine)
        {
            _stateRepository = stateRepository;
            _logService = logService;
            _mapper = mapper;
            _logger = logger;
            _coffeeMachine = coffeeMachine;
        }

        public bool IsOn => _coffeeMachine.IsOn;
        public bool IsMakingCoffee => _coffeeMachine.IsMakingCoffee;
        public State WaterLevelState => _coffeeMachine.WaterLevelState;
        public State BeanFeedState => _coffeeMachine.BeanFeedState;
        public State WasteCoffeeState => _coffeeMachine.WasteCoffeeState;
        public State WaterTrayState => _coffeeMachine.WaterTrayState;

        private bool IsInAlertState =>
            _coffeeMachine.WaterLevelState == State.Alert ||
            _coffeeMachine.BeanFeedState == State.Alert ||
            _coffeeMachine.WasteCoffeeState == State.Alert ||
            _coffeeMachine.WaterTrayState == State.Alert;

        private async Task UpdateMachineStateEntity(CoffeeMachineState newState)
        {
            var currentState = await _stateRepository.GetAsync(1); // Assuming ID is 1
            if (currentState == null)
            {
                await _stateRepository.AddAsync(newState);
            }
            else
            {
                _mapper.Map(newState, currentState); // Update existing entity
                await _stateRepository.UpdateAsync(currentState);
            }
        }

        private async Task LogAction(int actionId, string details, CoffeeCreationOptionsDto? coffeeCreationOptionsDto = null)
        {
            var actionTypes = await _logService.GetCoffeeActionTypesAsync();
            var actionName = actionTypes.First(x => x.Id == actionId && !x.IsDisabledAction).ActionName;
            if (actionName != null)
            {
                CoffeeActionLogDto? coffeeActionLogDto = null;
                //New Coffee
                if (actionId == 3 && coffeeCreationOptionsDto != null)
                {
                    coffeeActionLogDto = GetNewCoffeActionDto(actionName, details, coffeeCreationOptionsDto);
                }
                else
                {
                    coffeeActionLogDto = new CoffeeActionLogDto
                    {
                        Timestamp = DateTime.UtcNow,
                        Action = new ActionDto { Name = actionName },
                        Details = details,
                        ActionTypeId = actionId,
                        CoffeeCreationOptions = null

                    };
                }
              
                await _logService.LogActionAsync(coffeeActionLogDto);
            }
        }

        public async Task TurnOnAsync()
        {
            if (IsOn)
                throw new InvalidOperationException(ConsStringLogMessages.InvalidStateMachIsOn);

            try
            {
                await _coffeeMachine.TurnOnAsync();

                // Update the state in the database
                var newState = new CoffeeMachineState
                {
                    IsOn = _coffeeMachine.IsOn,
                    WaterLevel = _coffeeMachine.WaterLevelState,
                    BeanFeed = _coffeeMachine.BeanFeedState,
                    WasteCoffee = _coffeeMachine.WasteCoffeeState,
                    WaterTray = _coffeeMachine.WaterTrayState
                };
                await UpdateMachineStateEntity(newState);

                await LogAction(
                    2, // TurnOn
                    ConsStringLogMessages.MachTunedOn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConsStringLogMessages.ErrorTurningOnMach);
                throw;
            }
        }

        public async Task TurnOffAsync()
        {
            if (!IsOn || IsMakingCoffee)
                throw new InvalidOperationException(ConsStringLogMessages.InvalidStateMachStatus);

            try
            {
                await _coffeeMachine.TurnOffAsync();

                // Update the state in the database
                var newState = new CoffeeMachineState
                {
                    IsOn = _coffeeMachine.IsOn
                };
                await UpdateMachineStateEntity(newState);

                await LogAction(
                    1,//TurnOff
                    ConsStringLogMessages.MachTunedOff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ConsStringLogMessages.ErrorTurningOffMach);
                throw;
            }
        }

        public async Task MakeCoffeeAsync(CoffeeCreationOptionsDto options)
        {
            if (!IsOn || IsMakingCoffee || IsInAlertState)
                throw new InvalidOperationException(ConsStringLogMessages.InvalidStateMachStatus);

            if (!options.AddMilk && options.NumEspressoShots == 0)
                throw new InvalidOperationException(ConsStringLogMessages.InvalidRequest);

            try
            {
                await _coffeeMachine.MakeCoffeeAsync(options);

                // Update the state in the database
                var newState = new CoffeeMachineState
                {
                    IsMakingCoffee = _coffeeMachine.IsMakingCoffee
                };
                await UpdateMachineStateEntity(newState);

                await LogAction(
                    3, // MakeCoffee
                    $"{ConsStringLogMessages.EspressoText} {options.NumEspressoShots}, {ConsStringLogMessages.MilkText} - {options.AddMilk}", options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConsStringLogMessages.ErrorMakingCoffee);
                throw;
            }
        }
        private CoffeeActionLogDto GetNewCoffeActionDto(string actionName, string details, CoffeeCreationOptionsDto coffeeCreationOptionsDto)
        {
            var logDto = new CoffeeActionLogDto
            {
                Timestamp = DateTime.UtcNow,
                Action = new ActionDto { Name = actionName },
                Details = details,
                ActionTypeId = 3,
                CoffeeCreationOptions = new CoffeeCreationOptionsDto
                {
                    NumEspressoShots = coffeeCreationOptionsDto.NumEspressoShots,
                    AddMilk = coffeeCreationOptionsDto.AddMilk
                }
            };
            return logDto;
        }
    }
}