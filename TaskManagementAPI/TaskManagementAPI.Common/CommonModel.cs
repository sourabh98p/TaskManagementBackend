using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementAPI.Common
{
    public enum TokenType
    {
        /// <summary>
        /// The access token
        /// </summary>
        [Display(Name = "access_token")]
        AccessToken = 0,
        /// <summary>
        /// The refresh token
        /// </summary>
        [Display(Name = "refresh_token")]
        RefreshToken = 1
    }

}
