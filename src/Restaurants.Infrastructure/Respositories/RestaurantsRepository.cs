using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Respositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantRepository
{
    public async Task<int> Create(Restaurant entity)
    {
        dbContext.Restaurants.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dbContext
            .Restaurants
            .Where(restaurant => searchPhrase == null ||
                    (restaurant.Name.ToLower().Contains(searchPhraseLower)
                    || restaurant.Description.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();

        if(sortBy != null)
        {

            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r=> r.Name },
                { nameof(Restaurant.Description), r=> r.Description },
                { nameof(Restaurant.Category), r=> r.Category },
            };

            var selectedColumn = columnsSelector[sortBy];
            baseQuery = sortDirection ==SortDirection.Ascending
                        ? baseQuery.OrderBy(selectedColumn)
                        : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        // PageSize =5, PageNumber = 2: Skip => PageSize * (PageNumber - 1) => 5* (3-1) => 10
        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(restaurant => restaurant.Dishes)
            .FirstOrDefaultAsync(restaurant => restaurant.Id == id);
        return restaurant;
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(Restaurant entity)
    {
        dbContext.Restaurants.Update(entity);
        await dbContext.SaveChangesAsync();

    }

    public async Task<IEnumerable<Restaurant>> GetAllByOwnerIdAsync(string ownerId)
    {
        var restaurants = await dbContext.Restaurants.Where(restaurant => restaurant.OwnerId == ownerId).ToListAsync();
        return restaurants;
    }

}
