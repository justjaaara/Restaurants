using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(CurrentUser))]
public class CurrentUserTest
{

    // TestMethod_Scenario_ExpectedResult
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ReturnsTrue(string roleName)
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act
        var isInRole = currentUser.IsInRole(roleName);

        // assert
        isInRole.Should().Be(true);
    }
    
    [Fact]
    public void IsInRole_WithNotMatchingRole_ReturnsFalse()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        // assert
        isInRole.Should().Be(false);
    }
    
    [Fact]
    public void IsInRole_WithNotMatchingRoleCase_ReturnsFalse()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        // assert
        isInRole.Should().Be(false);
    }
    
    
}