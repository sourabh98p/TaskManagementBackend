
using System.Threading.Tasks;
using TaskManagementAPI.Common.Request;
using TaskManagementAPI.Common.Response;

namespace TaskManagementAPI.Repository.Abstract
{
    public interface IAuthenticateRepository
    {
        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<(UserDetails, bool)> ValidateUser(LoginRequest request);
    }
}
