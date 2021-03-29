using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog_API.Modals;
using Blog_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Blog_API.Controllers
{       
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel authenticate)
        {
            var user = _userRepository.Authenticate(authenticate.username, authenticate.password);
        
            if (user.Result == null)
                return BadRequest(new { message = "Username or password is incorrect" }
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.privateKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Result.userID.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var username = user.Result.username;
            var fullname = user.Result.fullname;
            return Ok(new
            {
                user=new {
                username=username,
                fullname=fullname
                },
                Token = tokenString
            });
            
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            
            var user= await _userRepository.Get(id);
            if (user == null)
            {
                return BadRequest(
                    new
                    {
                        message = "not found user"
                    });
            }
            return Ok(new
            {
                user=new {
                    username=user.username,
                    fullname=user.fullname
                },
            });
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> updateUser(int id,[FromBody] UserUpdate user)
        {
            _userRepository.Update(id, user);
            
            return Ok(new
            {
                message="update success!"
            });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> deleteUser(int id)
        {
            _userRepository.Delete(id);
            return Ok(new
            {
                message="Delete success!"
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<User>> getallUser()
        {
            return await _userRepository.GetAll();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult  createUser([FromBody]CreateUserModal user)
        {

            try
            {
                _userRepository.Create(user);
                var userAuthen = _userRepository.Authenticate(user.username, user.password);
        
                if (userAuthen == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Settings.privateKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userAuthen.Result.userID.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                var username = userAuthen.Result.username;
                var fullname = userAuthen.Result.fullname;
                return Ok(new
                {
                    user=new {
                        username=username,
                        fullname=fullname
                    },
                    Token = tokenString
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}