using Chat.API.Helpers;
using Chat.DataAccess;
using Chat.DataAccess.UI;
using Chat.DataAccess.UI.Exceptions;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using ChatGPT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ChatGPT.ChatGPT _chat;
        public RequestController(ChatDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
            _chat = new ChatGPT.ChatGPT(configuration["ChatGPT:ApiKey"]);
        }

        [HttpGet("")]
        [Authorize]
        public async Task<Result<List<Request>>> Get()
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

            var values = await _context.Requests.GetUserRequestsAsync(user.Id);

            return new Result<List<Request>>
            {
                Value = values,
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration)) : null
            };
        }

        [HttpPost("")]
        [Authorize]
        public async Task<Result<Request>> Create([FromBody] string request)
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

            var entity = new Request
            {
                Date = DateTime.Now,
                RequestMessage = request,
                UserId = user.Id
            };

            entity.ResponseMessage = await _chat.GetResponseAsync(entity.RequestMessage);


            var res = await _context.CreateRequestAsync(entity);

            var u = await _context.Users.GetAsync(user.Id);

            if (u == null)
            {
                throw new InternalServerException();
            }

            var claims = await _context.Users.GetClaimsAsync((UserModel)u);
            
            return new Result<Request>
            {
                Value = res,
                Hits = res != null ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration))
            };
        }
        
    }
}
