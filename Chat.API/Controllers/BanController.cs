using Chat.API.Helpers;
using Chat.DataAccess;
using Chat.DataAccess.UI;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/bans")]
    public class BanController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        public BanController(ChatDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<Result<List<Ban>>> Get()
        {
            var values = await _context.Bans.GetAsync();

            return new Result<List<Ban>>
            {
                Value = values.ToList(),
                Hits = values.Count(),
                Token = User.Claims.Count() > 0 ? new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration)) : null
            };
        }
    }
}
