using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUsersRepo _users;

        public LoginController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<LoginResponse> Auth()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) && authHeader.ToString().StartsWith("Basic"))
            {
                var usernamePassword = AuthHelper.GetUsernamePasswordFromAuthHeader(authHeader);
                var user = _users.GetByUsername(usernamePassword[0]);
                var isLoggedIn = _users.HasAccess(usernamePassword[0], usernamePassword[1]);
                return new LoginResponse(user, isLoggedIn);
            }
            return new NotFoundResult();
        }
    }
}