using TaskManagementAPI.Common.Request;
using TaskManagementAPI.Common.Response;

namespace TaskManagementAPI.Manager.Abstract
{
    public interface IAuthenticateManager
    {
        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ServiceResponse<loginResponse>> ValidateUser(LoginRequest request);

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<ServiceResponse<TokenDetails>> RefreshToken(string userName);
    }
}
