using AutoMapper;
using CoffeeMachine.Core.Models;
using CoffeeMachine.Core.Models.Dto;
using CoffeeMachine.Models.Entities;
using Action = CoffeeMachine.Models.Entities.Action;

namespace CoffeeMachine.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CoffeeCreationOptionsDto,CoffeeCreationOptions>().ReverseMap();
            CreateMap<ActionTypeDto, ActionType>().ReverseMap();
            CreateMap<ActionType, ActionTypeDto>().ReverseMap();
            CreateMap<CoffeeCreationOptions, CoffeeCreationOptionsDto>().ReverseMap();
            CreateMap<Action, ActionDto>().ReverseMap();
            CreateMap<CoffeeActionLogDto, CoffeeActionLog>().ReverseMap();
        }
    }
}
