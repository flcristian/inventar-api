namespace inventar_api.ArticleLocations.Models.Comparers;

public class ArticleLocationEqualityComparer : IEqualityComparer<ArticleLocation>
{
    public bool Equals(ArticleLocation? x, ArticleLocation? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.ArticleCode == y.ArticleCode && x.LocationCode.Equals(y.LocationCode) && x.Count == y.Count;
    }

    public int GetHashCode(ArticleLocation obj)
    {
        return HashCode.Combine(obj.Id, obj.ArticleCode, obj.Article, obj.LocationCode, obj.Location, obj.Count);
    }
}