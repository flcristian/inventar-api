using System.IdentityModel.Tokens.Jwt;
using System.Text;
using inventar_api.Users.Controllers.Interfaces;
using inventar_api.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RegisterRequest = inventar_api.Users.Models.RegisterRequest;

namespace inventar_api.Users.Controllers;

public class UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    : UsersApiController
{

    public override async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        throw new NotImplementedException();
    }

    public override async Task<ActionResult<User>> GetUser(int id)
    {
        throw new NotImplementedException();
    }

    public override async Task<ActionResult<User>> Register(RegisterRequest request)
    {
        User user = new User
        {
            UserName = request.Name,
            Email = request.Email,
            Name = request.Name,
            Age = request.Age,
            Gender = request.Gender,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var token = GenerateJwtToken();

            return Ok(new { Token = token });
        }

        return BadRequest(result.Errors);
    }

    public override async Task<ActionResult<User>> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
        {
            var token = GenerateJwtToken();
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }
    
    private string GenerateJwtToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}