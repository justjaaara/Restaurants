using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();

            }

        }

        if (!dbContext.Roles.Any())
        {
            var roles = GetRoles();
            dbContext.Roles.AddRange(roles);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
            [
                new (UserRoles.User)
                {
                    NormalizedName = UserRoles.User.ToUpper()
                },
                new (UserRoles.Owner)
                {
                    NormalizedName = UserRoles.Owner.ToUpper()
                },
                new (UserRoles.Admin)
                {
                    NormalizedName = UserRoles.Admin.ToUpper() 
                },
            ];

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        User owner = new User()
        {
            Email = "seed-user@test.com"
        };
        List<Restaurant> restaurants = [
            new(){
                Owner = owner,
                Name = "KFC",
                Category=  "Fast Food",
                Description = "Kentucky Fried Chicken is a fast food restaurant chain that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes = [
                    new(){
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pieces)",
                        Price = 10.30M,
                    },

                    new(){
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pieces)",
                        Price = 5.30M,
                    }
                    ],
                Address = new(){
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "WC2N 5DU"
                }
            },
             new(){
                 Owner =  owner,
                Name = "McDonald Szewska",
                Category=  "Fast Food",
                Description = "McDonald's corporation (McDonald's), incorporated on December 21, 1964, operates and ",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new(){
                    City = "London",
                    Street = "Boots 193",
                    PostalCode = "W1F 8SR"
                }
            },
            ];

        return restaurants;
    }
}
