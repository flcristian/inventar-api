namespace inventar_api.ArticleLocations.Models.Comparers;

public class ArticleLocationEqualityComparer : IEqualityComparer<ArticleLocation>
{
    public bool Equals(ArticleLocation? x, ArticleLocation? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.ArticleId == y.ArticleId && x.Article.Equals(y.Article) && x.LocationId == y.LocationId && x.Location.Equals(y.Location) && x.Count == y.Count;
    }

    public int GetHashCode(ArticleLocation obj)
    {
        return HashCode.Combine(obj.Id, obj.ArticleId, obj.Article, obj.LocationId, obj.Location, obj.Count);
    }
}