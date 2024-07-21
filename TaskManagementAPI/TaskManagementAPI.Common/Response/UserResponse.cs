using System;
using System.Collections.Generic;
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
    public class UserDetails
    { 
        public string fullname {  get; set; }
        public int UserId { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public int teamid { get; set; }
  
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
