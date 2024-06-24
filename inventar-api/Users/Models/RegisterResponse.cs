namespace inventar_api.Users.Models;

public class RegisterResponse
{
    public string Token { get; set; }
    public User User { get; set; }
}