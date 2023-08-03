using Microsoft.IdentityModel.Tokens;
using RestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestApi.Helper;

public static class JwtHelpers
{
    public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, Guid id)
    {
        IEnumerable<Claim> claims = new Claim[]
                {
            new Claim(ClaimTypes.Name, userAccounts.UserName),
            new Claim(ClaimTypes.Email, userAccounts.EmailId),
            new Claim(ClaimTypes.NameIdentifier,id.ToString()),
            new Claim(ClaimTypes.Expiration,userAccounts.ExpiredTime.ToString("MMM ddd dd yyyy HH:mm:ss tt") )
                };
        return claims;
    }
    public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, out Guid id)
    {
        id = Guid.NewGuid();
        return userAccounts.GetClaims(id);
    }
    public static UserTokens GenTokenkey(UserTokens model, JwtSettings jwtSettings)
    {
        try
        {
            var userToken = new UserTokens();
            if (model == null) throw new ArgumentException(nameof(model));

            var expirationTime = jwtSettings.ExpirationTime ?? 10;

            DateTime expireTime = DateTime.Now.AddMinutes(expirationTime);
            var idRefreshToken = Guid.NewGuid();
            Guid Id = Guid.Empty;

            // Get secret key
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey));

            userToken.Validaty = expireTime.TimeOfDay;
            var JWToken = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: model.GetClaims(out Id),
                //   notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: DateTime.Now.AddMinutes(expirationTime),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

            );
            userToken.Token = "Bearer " + new JwtSecurityTokenHandler().WriteToken(JWToken);

            userToken.UserName = model.UserName;
            userToken.Id = model.Id;
            userToken.EmailId = model.EmailId;
            userToken.GuidId = Id;
            userToken.ExpiredTime = expireTime;
            return userToken;
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
