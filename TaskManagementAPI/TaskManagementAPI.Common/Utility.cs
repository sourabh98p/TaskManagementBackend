using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TaskManagementAPI.Common
{
    public static class Utility
    {
        public static string ClaimDetail(this HttpContext httpContext, string type)
        {
            string result = string.Empty;
            try
            {
                result = (httpContext?.User)?.Claims.Single((Claim c) => c.Type == type).Value;
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public static (string token, double expiresIn, DateTime tokenExpireDate) GetSecurityToken(string userName, DateTime now,
          AppSettings appSettings, TokenType tokenType)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();

            SigningCredentials signingCredentials = new SigningCredentials
                   (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.TokenEncryptionKey)), SecurityAlgorithms.HmacSha256);
            var refreshTokenExpireHours = appSettings.RefreshTokenExpireHours;

            DateTime expireDate = now.Add(tokenType == TokenType.AccessToken
                              ? TimeSpan.FromMinutes(Convert.ToDouble(appSettings.AccessTokenExpireMinutes))
                              : TimeSpan.FromHours(Convert.ToDouble(refreshTokenExpireHours)));
            JwtSecurityToken jwtSecurityToken = securityTokenHandler.CreateJwtSecurityToken
            (
             issuer: appSettings.Issuer,
             audience: appSettings.Audience,
             notBefore: now,
             subject: new ClaimsIdentity(claims.Append(new Claim(JwtRegisteredClaimNames.Typ, tokenType.GetDisplayName()))),
             expires: expireDate,
             issuedAt: now,
             signingCredentials: signingCredentials
            );
            return (securityTokenHandler.WriteToken(jwtSecurityToken), expiresIn: (jwtSecurityToken.ValidTo - jwtSecurityToken.ValidFrom).TotalSeconds, expireDate);
        }
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

    }
    public static class JwtServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the JWT.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(cfg =>
                   {
                       cfg.SaveToken = true;
                       var issuer = config["AppSettings:Audience"];
                       var audience = config["AppSettings:Issuer"];
                       var securityKey = config["AppSettings:TokenEncryptionKey"];
                       cfg.TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidIssuer = issuer,
                           ValidAudience = audience,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                           ValidateAudience = true,
                           ValidateIssuer = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ClockSkew = TimeSpan.Zero
                       };
                   });

            return services;
        }
    }
}
