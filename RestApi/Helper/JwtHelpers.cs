using Microsoft.IdentityModel.Tokens;
using RestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestApi.Helper;

public static class JwtHelpers
{
    public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, Guid Id)
    {
        IEnumerable<Claim> claims = new Claim[]
                {
            new Claim(ClaimTypes.Name, userAccounts.UserName),
            new Claim(ClaimTypes.Email, userAccounts.EmailId),
            new Claim(ClaimTypes.NameIdentifier,Id.ToString()),
            new Claim(ClaimTypes.Expiration,DateTime.UtcNow.AddMinutes(10).ToString("MMM ddd dd yyyy HH:mm:ss tt") )
                };
        return claims;
    }
    public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, out Guid Id)
    {
        Id = Guid.NewGuid();
        return userAccounts.GetClaims(Id);
    }
    public static UserTokens GenTokenkey(UserTokens model, JwtSettings jwtSettings)
    {
        try
        {
            var UserToken = new UserTokens();
            if (model == null) throw new ArgumentException(nameof(model));

            // Get secret key
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey));
            Guid Id = Guid.Empty;
            DateTime expireTime = DateTime.Now.AddMinutes(10);
            UserToken.Validaty = expireTime.TimeOfDay;
            var JWToken = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: model.GetClaims(out Id),
                //   notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

            );
            UserToken.Token = "Bearer " + new JwtSecurityTokenHandler().WriteToken(JWToken);
            var idRefreshToken = Guid.NewGuid();

            UserToken.UserName = model.UserName;
            UserToken.Id = model.Id;
            UserToken.EmailId = model.EmailId;
            UserToken.GuidId = Id;
            UserToken.ExpiredTime = expireTime;
            return UserToken;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static UserTokens CreateTokenkey(UserTokens model, JwtSettings jwtSettings)
    {
        try
        {
            var UserToken = new UserTokens();
            if (model == null) throw new ArgumentException(nameof(model));
            var jwtSecretKey = jwtSettings.IssuerSigningKey;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, model.EmailId),
                    new Claim(JwtRegisteredClaimNames.NameId, model.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(10).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
                }),
                Expires = DateTime.UtcNow.AddMinutes(10), // Set the token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            UserToken.Token = tokenString;
            UserToken.UserName = model.UserName;
            UserToken.Id = model.Id;
            UserToken.EmailId = model.EmailId;
            UserToken.GuidId = Guid.NewGuid();
            UserToken.ExpiredTime = DateTime.Now.AddMinutes(10);
            return UserToken;
        }
        catch (Exception)
        {
            throw;
        }
    }

}
