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
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ChatDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpPost("")]
        public async Task<Result<User>> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.CheckCredentialsAsync(model);

            if (user != null)
            {
                var claims = await _context.Users.GetClaimsAsync(user);
                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<User>()
                {
                    Token = token,
                    Hits = 1,
                    Value = new User(user)
                };
            }

            throw new InvalidCredentialsException();
        }

        [HttpPost("user")]
        public async Task<Result<User?>> Register([FromBody] LoginModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User });

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim("left", user.Subscription != null && user.Subscription.MaxCount != null ? (user.Subscription.MaxCount - user.Requests).ToString()! : "")
                };

                if (user.Subscription != null)
                {
                    claims.Add(new Claim("subscription", user.Subscription.Id.ToString()));
                    user.Subscription.Abilities
                        .ToList()
                        .ForEach(entity => claims.Add(new Claim("ability", entity.Id.ToString())));
                }

                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<User?>()
                {
                    Token = token,
                    Hits = 1,
                    Value = user
                };
            }

            throw new AuthorizationException();
        }

        [HttpPost("admin")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<User?>> RegisterAdmin([FromBody] LoginModel model)
        {
            var user = await _context.Users.CreateAsync(model, new List<string>() { UserRoles.User, UserRoles.Admin });

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Admin),
                    new Claim("left", user.Subscription != null && user.Subscription.MaxCount != null ? (user.Subscription.MaxCount - user.Requests).ToString()! : "")
                };

                if (user.Subscription != null)
                {
                    claims.Add(new Claim("subscription", user.Subscription.Id.ToString()));
                    user.Subscription.Abilities
                        .ToList()
                        .ForEach(entity => claims.Add(new Claim("ability", entity.Id.ToString())));
                }

                var token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration));

                return new Result<User?>()
                {
                    Token = token,
                    Hits = 1,
                    Value = user
                };
            }

            throw new AuthorizationException();
        }
    }
}
