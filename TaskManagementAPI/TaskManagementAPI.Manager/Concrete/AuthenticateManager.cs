using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManagementAPI.Common;
using TaskManagementAPI.Repository.Abstract;
using TaskManagementAPI.Common.Response;
using TaskManagementAPI.Common.Request;
using TaskManagementAPI.Manager.Abstract;

namespace TaskManagementAPI.Manager.Concrete
{
    public class AuthenticateManager : IAuthenticateManager
    {

        /// <summary>
        /// The application settings
        /// </summary>
        private readonly AppSettings _appSettings;
       
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IAuthenticateRepository _authenticateRepository;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly  ILogger<AuthenticateManager> _logger;

        public AuthenticateManager(IAuthenticateRepository authenticateRepository, IOptions<AppSettings> appSettings , ILogger<AuthenticateManager> logger)
        {
            _appSettings = appSettings.Value;
            _authenticateRepository = authenticateRepository;
            _logger = logger;
        }
        /// <summary>
        /// Logs the <paramref name="request" /> and passes it to Repository to validate the user Id and Password.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// User's Details, User's Authorized companies and Access Token
        /// </returns>
        public async Task<ServiceResponse<loginResponse>> ValidateUser(LoginRequest request)
        {
            _logger.LogInformation("Manager ValidateUser -> Start");
            ServiceResponse<loginResponse> response = new ServiceResponse<loginResponse>();
            var res = new loginResponse() { TokenDetails = new TokenDetails(), UserDetails = new UserDetails()};
            try
            {
                (UserDetails dalResponse, bool IsSuccess) = await _authenticateRepository.ValidateUser(request);
                if (dalResponse != null && IsSuccess)
                {
                    var accessToken = Utility.GetSecurityToken(request.email,  DateTime.Now, _appSettings, TokenType.AccessToken);
                    var refreshToken = Utility.GetSecurityToken(request.email,  DateTime.Now, _appSettings, TokenType.RefreshToken);

                    res.UserDetails = dalResponse;
                    res.TokenDetails = new TokenDetails
                    {
                        AccessToken = accessToken.token,
                        Expires_In = accessToken.expiresIn,
                        TokenExpriationDate = accessToken.tokenExpireDate,
                        RefreshToken = refreshToken.token,
                        RefreshTokenExpriationDate = refreshToken.tokenExpireDate
                    };
                    response.Data = res;
                    response.IsSuccess = true;
                    response.Message = Constants.SuccessMessage;
                }
                else
                {
                    response.Data = new loginResponse() { TokenDetails = new TokenDetails(), UserDetails = new UserDetails() };
                    response.IsSuccess = false;
                    response.Message = Constants.ErrorMessage;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.Data = new loginResponse() { TokenDetails = new TokenDetails(), UserDetails = new UserDetails() };
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;


            }
            _logger.LogInformation("Manager ValidateUser ->  End");
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<TokenDetails>> RefreshToken(string userName)
        {
            _logger.LogInformation("Manager RefreshToken -> Start");
            DateTime now = DateTime.UtcNow;
            ServiceResponse<TokenDetails> response = new ServiceResponse<TokenDetails>();
            try
            {
                var refreshToken = Utility.GetSecurityToken(userName,  now, _appSettings, TokenType.RefreshToken);
                var accessToken = Utility.GetSecurityToken(userName, now, _appSettings, TokenType.AccessToken);
                response.Data = new TokenDetails
                { 
                        RefreshToken = refreshToken.token,
                        Expires_In = accessToken.expiresIn,
                        AccessToken = accessToken.token,
                        RefreshTokenExpriationDate = refreshToken.tokenExpireDate,
                        TokenExpriationDate = accessToken.tokenExpireDate
                };
                response.IsSuccess = true;
                response.Message = Constants.SuccessMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.Data = new TokenDetails();
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager RefreshToken -> End");

            return response;
        }
    }
}
