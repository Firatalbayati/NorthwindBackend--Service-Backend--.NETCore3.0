using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);

            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var LoginResult = _authService.CreateAccessToken(userToLogin.Data);

            if (LoginResult.Success) 
            {
                return Ok(LoginResult.Data);
            }

            return BadRequest(LoginResult.Message);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {

            var userExists = _authService.UserExists(userForRegisterDto.Email);

            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var RegisterResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(RegisterResult.Data);

            if (result.Success) 
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
