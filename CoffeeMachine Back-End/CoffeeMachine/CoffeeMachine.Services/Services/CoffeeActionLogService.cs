using CoffeeMachine.Core.Interfaces;
using CoffeeMachine.Core.Models;
using CoffeeMachine.Models.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Action = CoffeeMachine.Models.Entities.Action;
using CoffeeMachine.Core.Models.Dto;
using CoffeeMachine.Core.ConstantStrings;

namespace CoffeeMachine.Services
{
    /// <summary>
    /// Implementation service for loging coffe machine activities
    /// </summary>
    public class CoffeeActionLogService : ICoffeeActionLogService
    {
        private readonly IRepository<CoffeeActionLog> _logRepository;
        private readonly IRepository<Action> _actionRepository;
        private readonly IRepository<ActionType> _actionTypeRepository;
        private readonly IRepository<CoffeeCreationOptions>  _creationOptionsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CoffeeActionLogService> _logger;

        public CoffeeActionLogService(
            IRepository<CoffeeActionLog> logRepository,
            IRepository<Action> actionRepository,
            IRepository<ActionType> actionTypeRepository,
            IRepository<CoffeeCreationOptions>  creationOptionsRepository,
            IMapper mapper,
            ILogger<CoffeeActionLogService> logger)
        {
            _logRepository = logRepository;
            _actionRepository = actionRepository;
            _actionTypeRepository = actionTypeRepository;
            _creationOptionsRepository = creationOptionsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task LogActionAsync(CoffeeActionLogDto logDto)
        {
            try
            {
                // Map ActionDto to Action entity
                var actionEntity = _actionRepository.GetAllAsync().Result.FirstOrDefault(a => a.Name == logDto.Action.Name);

                if (actionEntity == null)
                {
                    _logger.LogWarning($"{ConsStringLogMessages.UnexpectedAction}: {logDto.Action!.Name}.{ConsStringLogMessages.CreatingNewAction}");
                    actionEntity = _mapper.Map<Action>(logDto.Action);
                    await _actionRepository.AddAsync(actionEntity);
                }

                // Map CoffeeActionLogDto to CoffeeActionLog entity
                var logEntity = _mapper.Map<CoffeeActionLog>(logDto);
                logEntity.ActionId = actionEntity.Id; // Set the ActionId explicitly

                await _logRepository.AddAsync(logEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConsStringLogMessages.ErrorLoggingAction);
                throw;
            }
        }
        public async Task LogCoffeeCreationOption(CoffeeCreationOptionsDto optionDto)
        {
            var coffeeCreationOption = _mapper.Map<CoffeeCreationOptions>(optionDto);
             await _creationOptionsRepository.AddAsync(coffeeCreationOption);
        }

        public async Task<IEnumerable<CoffeeActionLogDto>> GetCoffeeActionLogsAsync()
        {
            try
            {
                var logEntities = await _logRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CoffeeActionLogDto>>(logEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConsStringLogMessages.ErrorRetrievingActionLogs);
                throw;
            }
        }
        public async Task<IEnumerable<ActionTypeDto>> GetCoffeeActionTypesAsync()
        {
            try
            {
                var actionTypes = await _actionTypeRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ActionTypeDto>>(actionTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ConsStringLogMessages.ErrorRetrievingActionTypes);
                throw;
            }
        }

    }
}