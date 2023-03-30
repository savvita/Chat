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
    [Route("api/shoppings")]
    public class ShoppingController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public ShoppingController(ChatDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<List<Shopping>>> Get()
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

            var values = await _context.Shoppings.GetAsync();

            return new Result<List<Shopping>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<List<Shopping>>> Get(string id)
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

            var values = await _context.Shoppings.GetUserShoppingsAsync(id);

            return new Result<List<Shopping>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize]
        public async Task<Result<bool>> Create([FromBody] Subscription entity)
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

            var res = await _context.Shoppings.CreateAsync(user.Id, entity);

            var u = await _context.Users.GetAsync(user.Id);

            if(u == null)
            {
                throw new InternalServerException();
            }

            var claims = await _context.Users.GetClaimsAsync((UserModel)u);


            return new Result<bool>
            {
                Value = res,
                Hits = res == true ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration))
            };
        }
    }
}
