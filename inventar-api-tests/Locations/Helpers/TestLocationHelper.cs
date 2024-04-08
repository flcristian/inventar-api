using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;

namespace inventar_api_tests.Locations.Helpers;

public static class TestLocationHelper
{
    public static List<Location> CreateLocations(int count)
    {
        List<Location> list = new List<Location>();

        for (int i = 1; i <= count; i++)
        {
            list.Add(CreateLocation(i));
        }

        return list;
    }
    
    public static Location CreateLocation(int id)
    {
        return new Location
        {
            Id = id,
            Code = $"{id}"
        };
    }

    public static CreateLocationRequest CreateCreateLocationRequest(int id)
    {
        return new CreateLocationRequest
        {
            Code = $"{id}"
        };
    }
}