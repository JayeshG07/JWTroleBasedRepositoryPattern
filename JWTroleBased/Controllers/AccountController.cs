
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JWTroleBased.Dto;
using JWTroleBased.Models;
using JWTroleBased.Authorisation.Authorization;

namespace JWTroleBased.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAuthManager _authManager;
       
        public AccountController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        //[Route("api/Register")]
        [HttpPost("Register")]
        public async Task<ActionResult> Register( RegisterUserDto userDto)
        {
            var error = await _authManager.RegisterUser(userDto);
            if (!error)
            {


                return BadRequest(error);
            }
            return Ok();
        }

        //[/*Route("api/Login")]*/
           [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var Response = await _authManager.Login(loginDto);
            if (Response == null)
            {
                return Unauthorized();
            }
            else
                return Ok(Response);
        }
 
    }
}
