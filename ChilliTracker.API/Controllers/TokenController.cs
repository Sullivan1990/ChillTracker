using ChilliTracker.Business.Interfaces;
using ChilliTracker.Data.DataModels;
using ChilliTracker.Data.DTO;
using ChilliTracker.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChilliTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions _jwtOptions;
        public TokenController(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions) 
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenResponse tokenData)
        {
            if (tokenData == null) return BadRequest();

            var principal = GetPrincipalFromExpiredToken(tokenData.Token);

            if (principal == null) return BadRequest();

            var userIdClaim = principal.Claims.Where(c => c.Type == "Id").FirstOrDefault();

            if (userIdClaim == null) return BadRequest();

            var user = _userRepository.GetUserById(userIdClaim.Value);

            if (user == null || user.RefreshToken != tokenData.RefreshToken || user.RefreshTokenExpiry <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string refreshToken = GenerateRefreshToken();
            string accessToken = CreateToken(principal.Claims.ToList());

            _userRepository.UpdateUserRefreshToken(refreshToken, user._id.ToString());

            return new ObjectResult(new
            {
                Token = accessToken,
                RefreshToken = refreshToken,
            });

        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(UserLoginDTO userLogin) 
        {
            var user = _userRepository.GetUserByCredentials(userLogin);
            if(user == null)
            {
                return BadRequest("No user matches those credentials");
            }

            var claims = new List<Claim>
            {
                new Claim("Id", user._id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            string refreshToken = GenerateRefreshToken();

            _userRepository.SetUserRefreshTokenDetails(new RefreshTokenSetDTO
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiry = DateTime.Now.AddDays(7)
            }, user._id.ToString());

            return Ok(new TokenResponse
            {
                RefreshToken = refreshToken,
                Token = CreateToken(claims)
            });
        }

        private string CreateToken(List<Claim> claims)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtOptions.ValidIssuer,
                audience: _jwtOptions.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

    }
}
