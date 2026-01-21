
using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using System.Runtime.InteropServices.Marshalling;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        // Como tienen exactamente las mismas propiedades no se hace nada más
        CreateMap<UpdateRestaurantCommand, Restaurant>();

        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(destination => destination.Address, opt => opt.MapFrom(
                src => new Address
                {
                    City = src.City,
                    PostalCode = src.PostalCode,
                    Street = src.Street
                }));
                      
        //Mapea automáticamente toda la entidad Restaurant a RestaurantDto
        CreateMap<Restaurant, RestaurantDto>()
            //Se utiliza el ForMember para indicarle que como el Address es otro tipo pero hace parte de restaurante, entonces la incluya
            .ForMember(destination => destination.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(destination => destination.PostalCode, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(destination => destination.Street, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(destination => destination.Dishes, opt =>
                opt.MapFrom(src => src.Dishes));
    }
}
