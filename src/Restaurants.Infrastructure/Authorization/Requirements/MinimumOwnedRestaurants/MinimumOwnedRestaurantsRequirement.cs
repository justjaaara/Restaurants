using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumOwnedRestaurants;

public class MinimumOwnedRestaurantsRequirement(int minimumRestaurants) : IAuthorizationRequirement
{
    public int MinimumRestaurants { get; } = minimumRestaurants;
}
