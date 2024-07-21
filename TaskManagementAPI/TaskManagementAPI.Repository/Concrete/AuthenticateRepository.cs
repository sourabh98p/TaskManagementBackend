using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TaskManagementAPI.Common;
using TaskManagementAPI.Common.Request;
using TaskManagementAPI.Common.Response;
using TaskManagementAPI.Repository.Abstract;

namespace TaskManagementAPI.Repository.Concrete
{
    public class AuthenticateRepository: IAuthenticateRepository
    {

        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthenticateRepository> _logger;

        public ConnectionStrings _connectionStrings { get; set; }

        [ExcludeFromCodeCoverage]
        public AuthenticateRepository(IOptionsSnapshot<AppSettings> appSettings, ILogger<AuthenticateRepository> logger,
             IOptions<ConnectionStrings> connectionStrings)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _connectionStrings = connectionStrings.Value;
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<(UserDetails,bool)> ValidateUser(LoginRequest request)
        {
            _logger.LogInformation("Repo ValidateUser -> Start");
            UserDetails response = new UserDetails() { };
            bool IsSuccess = false;

            try
            {
                // For retrieving password stored in db so that it can be verified/matched with input password.
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("An Error Occur {Ex}", ex.Message);
            }
            _logger.LogInformation("Repo ValidateUser -> End");
            return (response, IsSuccess);
        }
    }
}
