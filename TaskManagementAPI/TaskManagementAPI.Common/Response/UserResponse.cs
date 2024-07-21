using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementAPI.Common.Response
{
    public class loginResponse
    {
        public UserDetails UserDetails { get; set; }
        public TokenDetails TokenDetails { get; set; }
    }

    public class User
    {
        public string fullName { get; set; }
        public int userId { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
    public class UserDetails : User
    {   
        public int? teamId { get; set; }
    }
    public class TokenDetails
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpriationDate { get; set; }
        public double? Expires_In { get; set; }
        public DateTime RefreshTokenExpriationDate { get; set; }
    }
}
