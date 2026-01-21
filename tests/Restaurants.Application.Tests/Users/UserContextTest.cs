using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(UserContext))]
public class UserContextTest
{

    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ReturnsCurrentUser()
    {
        // arrange
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userContext = new UserContext(httpContextAccessorMock.Object);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "German"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
        };
        
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });

        // act
        var currentUser = userContext.GetCurrentUser();

        // assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }
    
    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        
        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        // Act
        
        Action action = () => userContext.GetCurrentUser();
        
        // Assert
        
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");


    }
}