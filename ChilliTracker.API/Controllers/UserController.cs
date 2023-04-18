using ChilliTracker.Business.Interfaces;
using ChilliTracker.Data.DTO;
using ChilliTracker.Shared.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ChilliTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreateDTO newUser)
        {
            if (newUser == null) { return BadRequest(); }

            _userRepository.CreateUser(newUser);

            return CreatedAtAction("CreateUser", null);
        }


    }
}
