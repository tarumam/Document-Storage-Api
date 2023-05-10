using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocStorageApi.Identity
{
    public class DocStorageJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiresOn;

        public DocStorageJwtService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expiresOn = configuration["Jwt:ExpiresOn"];
        }

        public (string tokenId, string token) GenerateJwtFor(Guid userId, string role)
        {
            var tokenId = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var expirationDate = DateTime.UtcNow.AddDays(30);

            if(int.TryParse(_expiresOn, out int expiresOn))
            {
                expirationDate = DateTime.UtcNow.AddHours(expiresOn);
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience,
            };

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return (tokenId, tokenHandler.WriteToken(token));
            }
            catch (SecurityTokenEncryptionFailedException ex)
            {
                //TODO: Log EX
                return (string.Empty, string.Empty);
            }
            catch(Exception ex) {
                //TODO: Log EX
                return (string.Empty, string.Empty);
            }
        }
    }
}
