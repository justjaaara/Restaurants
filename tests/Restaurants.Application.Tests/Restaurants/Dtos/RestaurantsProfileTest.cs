using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

        [TestSubject(typeof(RestaurantsProfile))]
public class RestaurantsProfileTest
{
    
    private IMapper _mapper;

    public RestaurantsProfileTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        }, NullLoggerFactory.Instance);
        
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange

        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            Address = new Address()
            {
                City = "Test city",
                Street = "Test street",
                PostalCode = "12-345"
            }
        };

        // Act

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // Assert
        
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);

    }
    
    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        

        var command = new CreateRestaurantCommand()
        {
            Name = "Test restaurant",
            Description = "Test description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "12345678",
            City = "Test city",
            Street = "Test street",
            PostalCode = "12-345"
        };

        // Act

        var restaurant = _mapper.Map<Restaurant>(command);

        // Assert
        
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);

    }
    
    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        

        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Updated restaurant",
            Description = "Updated description",
            HasDelivery = false,

        };

        // Act

        var restaurant = _mapper.Map<Restaurant>(command);

        // Assert
        
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);

    }
}