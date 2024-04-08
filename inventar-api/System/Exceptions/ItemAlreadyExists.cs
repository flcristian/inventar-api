namespace inventar_api.System.Exceptions;

public class ItemAlreadyExists : Exception
{
    public ItemAlreadyExists(string? message) : base(message)
    {
    }
}