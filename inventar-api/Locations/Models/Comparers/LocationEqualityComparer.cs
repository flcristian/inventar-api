namespace inventar_api.Locations.Models.Comparers;

public class LocationEqualityComparer : IEqualityComparer<Location>
{
    public bool Equals(Location? x, Location? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.Code == y.Code && x.ArticleLocations.Equals(y.ArticleLocations);
    }

    public int GetHashCode(Location obj)
    {
        return HashCode.Combine(obj.Id, obj.Code, obj.ArticleLocations);
    }
}