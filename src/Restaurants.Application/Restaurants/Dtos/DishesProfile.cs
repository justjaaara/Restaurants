using Restaurants.Domain.Entities;
using AutoMapper;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Commands.CreateDish;

namespace Restaurants.Application.Restaurants.Dtos;

public class DishesProfile : Profile
{

    public DishesProfile()
    {
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<Dish, DishDto>();
    }
}
