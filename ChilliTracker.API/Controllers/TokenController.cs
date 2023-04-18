using ChilliTracker.Business.Interfaces;
using ChilliTracker.Data.DTO;
using ChilliTracker.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(UserLoginDTO userLogin) 
        {
            var user = _userRepository.GetUserByCredentials(userLogin);
            if(user == null)
            {
                return BadRequest("No user matches those credentials");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("Id", user._id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtOptions.ValidIssuer,
                audience: _jwtOptions.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signingCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new TokenResponse
            {
                Token = tokenString
            });
        }


    }
}
