using Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ClaimsPrincipal GetUser(JwtService jwt)
        {
            var auth = Request.Headers["Authorization"].ToString();
            if (!auth.StartsWith("Bearer ")) throw new UnauthorizedAccessException();

            var token = auth.Replace("Bearer ", "");
            var user = jwt.Validate(token);

            if (user == null) throw new UnauthorizedAccessException();
            return user;
        }
    }
}
