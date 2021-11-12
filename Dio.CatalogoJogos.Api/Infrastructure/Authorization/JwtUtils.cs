using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dio.CatalogoJogos.Api.Infrastructure.Authorization
{
    public interface IJwtUtils
    {
        public string GerarJwtToken(Usuario usuario);
        public Guid? ValidarJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;

        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarJwtToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", usuario.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidarJwtToken(string token)
        {
            if (token == null)
                return null;

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "id").Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
