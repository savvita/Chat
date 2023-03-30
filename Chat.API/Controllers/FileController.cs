using Chat.API.Helpers;
using Chat.DataAccess;
using Chat.DataAccess.UI;
using Chat.DataAccess.UI.Exceptions;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using Chat.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly S3Access _access;
        private readonly Rekoginition _rekognition;
        private readonly string _bucketName = "rekognitionimagesbucket";
        private readonly IConfiguration _configuration;
        private readonly DbContext _context;

        public FileController(ChatDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _access = new S3Access(configuration["S3:AccessKey"], configuration["S3:SecretKey"], Amazon.RegionEndpoint.EUWest2);
            _rekognition = new Rekoginition(configuration["S3:AccessKey"], configuration["S3:SecretKey"], Amazon.RegionEndpoint.EUWest2);
            _configuration = configuration;
            _context = new DbContext(context, userManager, roleManager);
        }

        [HttpPost("")]
        [Authorize]
        public async Task<Result<string>> Rekognize(IFormFileCollection upload)
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

            if (Request.Form.Files.Count == 0)
            {
                throw new ArgumentNullException();
            }

            var fileName = Guid.NewGuid().ToString();

            var res = await _access.UploadToBucketAsync(_bucketName, fileName, Request.Form.Files[0].OpenReadStream());

            string result = "";

            if(res)
            {
                if (Request.Form.Files[0].ContentType.StartsWith("image"))
                {
                    result = _rekognition.RekognizeFromImage(_bucketName, fileName);
                }
                else if (Request.Form.Files[0].ContentType.StartsWith("audio"))
                {
                    result = await _rekognition.TranscribeMediaFile(_bucketName, fileName, "en-US");
                }
            }

            return new Result<string>
            {
                Value = result,
                Hits = result != "" ? 1 : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(claims, _configuration))
            };
        }

        [HttpDelete("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Remove([FromQuery] string objectName)
        {
            var res = await _access.DeleteFromBucketAsync(_bucketName, objectName);
            return Ok(res);
        }
    }
}
