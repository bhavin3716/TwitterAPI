using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterWebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public UserCredentials UserCredential { get; set; }
        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCredential userCredential)
        {
            var respose = await _userService.Register(
                new Model.User { UserName = userCredential.UserName, Email = userCredential.Email }, userCredential.Password);

            if (respose == null)
            {
                return BadRequest(respose);
            }
            return Ok(respose);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredential userCredential)
        {
            var response = await _userService.Login(userCredential.UserName, userCredential.Password);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("user/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpGet("user/search/{username}")]
        public async Task<IActionResult> SearchByUserName(string userName)
        {
            var response = await _userService.SearchByUserName(userName);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
