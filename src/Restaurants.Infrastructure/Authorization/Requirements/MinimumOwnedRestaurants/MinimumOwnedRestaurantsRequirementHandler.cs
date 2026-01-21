using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumOwnedRestaurants;

internal class MinimumOwnedRestaurantsRequirementHandler(
    ILogger<MinimumOwnedRestaurantsRequirementHandler> logger,
    IUserContext userContext,
    IRestaurantRepository restaurantRepository) : AuthorizationHandler<MinimumOwnedRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOwnedRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email} - Handling MinimumOwnedRestaurants", currentUser.Email);

        var ownedRestaurants = await restaurantRepository.GetAllByOwnerIdAsync(currentUser.Id);
        int countOfOwnedRestaurants = ownedRestaurants.Count();

        if(countOfOwnedRestaurants >= requirement.MinimumRestaurants)
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        
    }
}
