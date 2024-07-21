using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementAPI.Controllers.Public
{
    /// <summary>
    /// Secure Controller
    /// </summary>
    [Produces("application/json")]
    [Authorize("Bearer")]
    public class SecureController : ControllerBase
    {
    }
}
