using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;
using Xunit.Sdk;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTest
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
    
    private readonly UpdateRestaurantCommandHandler _commandHandler;

    public UpdateRestaurantCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        
        _commandHandler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _restaurantRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // Arrange

        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "Test Restaurant",
            Description = "Test Restaurant",
            HasDelivery = true,
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test",
            HasDelivery = true,
        };
        
        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);
        
        // Act
        
        await _commandHandler.Handle(command, CancellationToken.None);
        
        // Assert
        
        _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command,restaurant), Times.Once);

    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ThrowsNotFoundException()
    {
        // Arrange

        var restaurantId = 2;
        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
        };
        
        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant)null);

        
        // Act
        
        Func<Task> action = async () => await _commandHandler.Handle(request, CancellationToken.None);
        
        // Assert

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");

    }
    
    [Fact]
    public async Task Handle_WithUnauthorizedUser_ThrowsForbidException()
    {
        // Arrange

        var restaurantId = 3;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
        };
        
        var existingRestaurant = new Restaurant()
        {
            Id = restaurantId,
        };
        
        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(existingRestaurant);
        
        _restaurantAuthorizationServiceMock
            .Setup(m => m.Authorize(existingRestaurant, ResourceOperation.Update))
            .Returns(false);

        
        // Act
        
        Func<Task> action = async () => await _commandHandler.Handle(command, CancellationToken.None);
        
        // Assert

        await action.Should().ThrowAsync<ForbidException>();

    }
}