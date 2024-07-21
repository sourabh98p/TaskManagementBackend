using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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
        private readonly AppDbContext _dbContext;
        private readonly ConnectionStrings _connectionStrings;

        public AuthenticateRepository(IOptionsSnapshot<AppSettings> appSettings, ILogger<AuthenticateRepository> logger,
             IOptions<ConnectionStrings> connectionStrings, AppDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _connectionStrings = connectionStrings.Value;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<(UserDetails,bool)> ValidateUser(LoginRequest request)
        {
            _logger.LogInformation("Repo ValidateUser -> Start");
            UserDetails response = new UserDetails();
            bool IsSuccess = false;
            try
            {
                using (var conn = new NpgsqlConnection(_connectionStrings.DBConnectionString))
                {
                    conn.Open();

                    // Query to verify email and password
                    using (var cmd = new NpgsqlCommand(
                        InlineQuery.GetUserDetails, conn))
                    {
                        cmd.Parameters.AddWithValue("Email", request.email);
                        cmd.Parameters.AddWithValue("Password", request.password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                response = new UserDetails
                                {
                                    userId = Convert.ToInt32(reader["userid"]),
                                    email = reader["email"].ToString(),
                                    fullName = reader["fullname"].ToString(),
                                    role = reader["role"].ToString(),
                                    teamId = reader["teamid"] != DBNull.Value ? Convert.ToInt32(reader["teamid"]) : 0
                                };
                            }
                        }
                    }

                    conn.Close();
                }
                IsSuccess = true;
                _logger.LogInformation("Repo ValidateUser -> End");
            }
            catch (Exception ex)
            {   
                
                _logger.LogError("An Error Occur {Ex}", ex.Message);
            }

            return (response,IsSuccess);
        }
    }
}
