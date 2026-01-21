namespace Restaurants.Infrastructure.Authorization;

public static class PolicyNames
{
    public const string HasNationality = "HasNatioanlity";
    public const string AtLeast20= "AtLeast20";
    public const string AtLeast2OwnedRestaurants = "AtLeast2OwnedRestaurants";

}

public static class AppClaimTypes
{
    public const string Nationality = "HasNationality";
    public const string DateOfBirth = "DateOfBirth";
    
}
