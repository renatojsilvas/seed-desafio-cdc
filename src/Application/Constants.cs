namespace Application;

public static class Constants
{
    public const int MaxDescriptionLength = 400;
    public const int MaxSummaryLength = 500;
    public const int MinPriceValue = 20;
    public const int MinNumberOfPages = 100;
    
    public static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Now);
}