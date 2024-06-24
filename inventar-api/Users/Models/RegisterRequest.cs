namespace inventar_api.Users.Models;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}