using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandValidator))]
public class CreateRestaurantCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Category = "Italian",
            Description = "Test",
            ContactEmail = "test@test.com",
            PostalCode = "12-345"
        };
        
        var validator = new CreateRestaurantCommandValidator();
        // Act

        var result = validator.TestValidate(command);
        // Assert
        
        result.ShouldNotHaveAnyValidationErrors();

    }
    
    [Fact]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Ita",
            ContactEmail = "@test.com",
            PostalCode = "12345"
        };
        
        var validator = new CreateRestaurantCommandValidator();
        // Act

        var result = validator.TestValidate(command);
        // Assert
        
        result.ShouldHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.Name);
        result.ShouldHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.Description);
        result.ShouldHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.Category);
        result.ShouldHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.ContactEmail);
        result.ShouldHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.PostalCode);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("Colombian")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProp(string category)
    {
        // Arrange
        
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand{ Category = category };
        
        // Act

        var result = validator.TestValidate(command);
        
        // Assert
        
        result.ShouldNotHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.Category);
    }

    [Theory]
    [InlineData("10220")]
    [InlineData("102-20")]
    [InlineData("10 220")]
    [InlineData("10-2 20")]
    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrors(string postalCode)
    {
        // Arrange
        
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand{ PostalCode = postalCode};
        
        // Act

        var result = validator.TestValidate(command);
        
        // Assert
        
        result.ShouldHaveValidationErrorFor(createRestaurantCommand => createRestaurantCommand.PostalCode);
    }
}