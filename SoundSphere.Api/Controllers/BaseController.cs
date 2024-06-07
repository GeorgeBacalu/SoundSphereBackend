using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace SoundSphere.Api.Controllers
{
    [ApiController] public class BaseController : Controller
    {
        public BaseController() { }

        protected int GetUserId()
        {
            string? rawToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string? token = rawToken?.Substring("Bearer ".Length).Trim();
            var parserToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string? rawUserId = parserToken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            int userId = int.Parse(rawUserId);
            return userId;
        }
    }
}