namespace inventar_api.System.Constants;

public static class ExceptionMessages
{
    public const string ARTICLE_DOES_NOT_EXIST = "Article does not exist.";
    public const string ARTICLES_DO_NOT_EXIST = "Articles do not exist.";
    public const string ARTICLE_ALREADY_EXISTS = "Article code is already in use.";
    public const string INVALID_ARTICLE_CODE = "Article code is not allowed.";
    
    public const string LOCATION_DOES_NOT_EXIST = "Location does not exist.";
    public const string LOCATIONS_DO_NOT_EXIST = "Locations do not exist.";
    public const string LOCATION_ALREADY_EXISTS = "Location code is already in use.";
    
    public const string ARTICLE_LOCATION_DOES_NOT_EXIST = "Article location does not exist.";
    public const string ARTICLE_LOCATIONS_DO_NOT_EXIST = "Article locations do not exist.";
    public const string NO_STOCK_HISTORY = "There is no stock history.";
    public const string INVALID_ARTICLE_COUNT = "Article count is negative or not allowed.";
    public const string INVALID_STOCK_COUNT = "Stock value is invalid or not allowed.";
}