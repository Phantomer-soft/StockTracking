using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StockTrackApi.Models.Entities;

namespace StockTrackApi.Helpers;

public class JwtTokenHelper
{
    public Token CreateAccessToken(IConfiguration config,Company company)
    {
        Token token = new();

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? string.Empty));
        
        SigningCredentials credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512);
        token.expiration = DateTime.UtcNow.AddMinutes(Convert.ToInt16(config["Jwt:ExpirationMins"])); 
        JwtSecurityToken securityToken = new(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            expires: token.expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: credentials,
            claims:
            [
                new Claim("companyId",company.Id.ToString()),
                new Claim("companyName",company.Name),
                new Claim("mail",company.Mail),
            ]
        );
        JwtSecurityTokenHandler tokenHandler = new();
        token.accessToken = tokenHandler.WriteToken(securityToken);

        return token;

    }
}