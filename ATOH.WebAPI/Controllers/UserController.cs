using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ATOH.WebAPI.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("api/User")]
[Authorize]
public class UserController : Controller
{
    [HttpGet("Auth")]
    [AllowAnonymous]
    public async Task<ActionResult> Authenticate()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "id"),
            new Claim(JwtRegisteredClaimNames.Name, "some name")
        };

        var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
        var key = new SymmetricSecurityKey(secretBytes);
        var algorithm = SecurityAlgorithms.HmacSha256;
        var signingCredentials = new SigningCredentials(key, algorithm);

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            claims,
            DateTime.Now,
            DateTime.Now.AddHours(1),
            signingCredentials);

        var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { access_token = tokenJson });
    }

    [HttpGet("Temp")]
    public async Task<ActionResult> Temp()
    {
        return Ok("132231231231");
    }
}