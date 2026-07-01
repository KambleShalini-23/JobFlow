using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using JobFlowApi.Models;

namespace JobFlowApi.Services;

public class JwtService
{
    private readonly string _secretKey;

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:Key"]!;
    }

    public string GenerateToken(User user)
    {
         var claims = new[]
         {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.Role, user.Role)
         };
         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
         
         var token = new JwtSecurityToken(
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(5),
             signingCredentials: creds);

         return new JwtSecurityTokenHandler().WriteToken(token);
    }
}