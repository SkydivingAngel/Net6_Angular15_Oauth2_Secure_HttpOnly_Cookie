using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Net6AngularOauth2
{
    public class JwtHandler
    {
        private readonly IConfiguration configuration;

        public JwtHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> GetTokenAsync(LoginRequest loginRequest)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(await GetClaimsAsync(loginRequest)),
                Issuer = "MyApi",
                Audience = "MyApi",
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = GetSigningCredentials()
            };

            SecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            string? jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes("mysupersecretkey"); // configuration["JwtSettings:SecurityKey"]
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature);//SecurityAlgorithms.HmacSha256  HmacSha512Signature
        }

        private async Task<List<Claim>> GetClaimsAsync(LoginRequest loginRequest)
        {
            string epochTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginRequest.Email),
                new Claim("Client_Id", "Client_Id"),
                new Claim("Ente", "A944"),
                new Claim(ClaimTypes.NameIdentifier, "NameIdentifier"),
                new Claim(JwtRegisteredClaimNames.Sub, "Subject"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N").ToUpperInvariant()),
                new Claim(JwtRegisteredClaimNames.Iat, epochTime),
                new Claim(JwtRegisteredClaimNames.Nbf, epochTime),
            };

            return claims;
        }
    }
}
