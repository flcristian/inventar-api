namespace inventar_api.System.Exceptions;

public class InvalidValue : Exception
{
    public InvalidValue(string? message) : base(message)
    {
    }
}