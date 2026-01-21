using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumOwnedRestaurants;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements.MinimumOwnedRestaurants;

[TestSubject(typeof(MinimumOwnedRestaurantsRequirementHandler))]
public class MinimumOwnedRestaurantsRequirementHandlerTest
{

    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock
            .Setup(m => m.GetCurrentUser())
            .Returns(currentUser);

        var ownedRestaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantsRepositoryMock
            .Setup(r => r.GetAllByOwnerIdAsync(currentUser.Id))
            .ReturnsAsync(ownedRestaurants);
        
        var loggerMock = new Mock<ILogger<MinimumOwnedRestaurantsRequirementHandler>>();

        var requirement = new MinimumOwnedRestaurantsRequirement(3);
        var handler = new MinimumOwnedRestaurantsRequirementHandler(loggerMock.Object,userContextMock.Object,restaurantsRepositoryMock.Object);

        var authorizationContext = new AuthorizationHandlerContext([requirement],null,null);

        // Act
        
        await handler.HandleAsync(authorizationContext);
        
        // Assert

        authorizationContext.HasSucceeded.Should().BeTrue();
    }
    
    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock
            .Setup(m => m.GetCurrentUser())
            .Returns(currentUser);

        var ownedRestaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantsRepositoryMock
            .Setup(r => r.GetAllByOwnerIdAsync(currentUser.Id))
            .ReturnsAsync(ownedRestaurants);
        
        var loggerMock = new Mock<ILogger<MinimumOwnedRestaurantsRequirementHandler>>();

        var requirement = new MinimumOwnedRestaurantsRequirement(5);
        var handler = new MinimumOwnedRestaurantsRequirementHandler(loggerMock.Object,userContextMock.Object,restaurantsRepositoryMock.Object);

        var authorizationContext = new AuthorizationHandlerContext([requirement],null,null);

        // Act
        
        await handler.HandleAsync(authorizationContext);
        
        // Assert

        authorizationContext.HasSucceeded.Should().BeFalse();
    }
}