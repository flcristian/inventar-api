namespace inventar_api.Articles.Models.Comparers;

public class ArticleEqualityComparer : IEqualityComparer<Article>
{
    public bool Equals(Article? x, Article? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.Code == y.Code && x.Name == y.Name && x.Consumption == y.Consumption && x.Machinery == y.Machinery && x.ArticleLocations.Equals(y.ArticleLocations);
    }

    public int GetHashCode(Article obj)
    {
        return HashCode.Combine(obj.Id, obj.Code, obj.Name, obj.Consumption, obj.Machinery, obj.ArticleLocations);
    }
}