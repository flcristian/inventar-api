namespace inventar_api.System.Exceptions;

public class ItemsDoNotExist : Exception
{
    public ItemsDoNotExist(string? message) : base(message)
    {
    }
}