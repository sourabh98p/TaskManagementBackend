using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagementAPI.Common;
using TaskManagementAPI.Common.Request;
using TaskManagementAPI.Manager.Abstract;


namespace TaskManagementAPI.Controllers
{
    /// <summary>
    /// Authenticate Controller
    /// </summary>
    [Produces("application/json")]
    [Route("kiapi/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        /// <summary>
        /// The application settings
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Authenticate Manager
        /// </summary>
        private readonly IAuthenticateManager _authenticateManager;
        private readonly HttpContext _httpContext;
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Constructor Authenticate Controller
        /// </summary>
        public AuthenticateController(IAuthenticateManager authenticateManager, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _authenticateManager = authenticateManager;
            _appSettings = appSettings.Value;
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ValidateUser")]
        public async Task<IActionResult> ValidateUser([FromBody] LoginRequest request)
        {
            var response = await _authenticateManager.ValidateUser(request);
            _httpContext.Request.Headers.Add("PMName", "");
            return Ok(response);
        }
        
        /// <summary>
        /// This Method refresh the token based on refresh token provided earlier.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        [Authorize("Bearer")]
        public async Task<IActionResult> RefreshToken()
        {
            string tokentype = Utility.ClaimDetail(_httpContext, JwtRegisteredClaimNames.Typ);
            if (string.IsNullOrWhiteSpace(tokentype) || tokentype != "refresh_token")
            {
                string message = "An unexpected error has occurred. Please try again !";
                return BadRequest(message);
            }
            string userName = Utility.ClaimDetail(_httpContext, ClaimTypes.Name);
            var response = await _authenticateManager.RefreshToken(userName);
            return Ok(response);
        }
    }
}
