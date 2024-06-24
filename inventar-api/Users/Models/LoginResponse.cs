namespace inventar_api.Users.Models;

public class LoginResponse
{
    public string Token { get; set; }
    public User User { get; set; }
}