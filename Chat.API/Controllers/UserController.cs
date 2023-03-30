using Chat.API.Helpers;
using Chat.DataAccess;
using Chat.DataAccess.UI;
using Chat.DataAccess.UI.Exceptions;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ChatDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<List<User>>> Get()
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }
            var claims = await _context.Users.GetClaimsAsync((UserModel)user);

            var users = (await _context.Users.GetAsync()).ToList();
            return new Result<List<User>>
            {
                Value = users,
                Hits = users.Count,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<User?>> Get(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }
            var claims = await _context.Users.GetClaimsAsync((UserModel)user);

            var res = await _context.Users.GetAsync(id);
            return new Result<User?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<Result<User?>> GetMe()
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }
            var res = await _context.Users.GetAsync(user.Id);
            var claims = await _context.Users.GetClaimsAsync((UserModel)res);

            return new Result<User?>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Delete(string id, [FromBody]Ban ban)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }
            var claims = await _context.Users.GetClaimsAsync((UserModel)user);

            var res = await _context.Users.BanAsync(id, ban);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration))
            };
        }

        [HttpPut("unban/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Unban(string id)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }
            var claims = await _context.Users.GetClaimsAsync((UserModel)user);

            var res = await _context.Users.BanAsync(id, null);
            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration))
            };
        }


        [HttpPut("")]
        public async Task<Result<User>> Update([FromBody] User entity)
        {
            await _context.Users.CheckUserAsync(User.Identity);
            var username = User.FindFirst(c => c.Type == ClaimTypes.Name);

            if (username == null)
            {
                throw new InternalServerException();
            }

            var user = await _context.Users.GetByUserNameAsync(username.Value);

            if (user == null)
            {
                throw new UserNotFoundException(username.Value);
            }
            var res = await _context.Users.UpdateAsync(entity);

            var claims = await _context.Users.GetClaimsAsync((UserModel)user);

            return new Result<User>
            {
                Value = res,
                Hits = 1,
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }
    }
}
