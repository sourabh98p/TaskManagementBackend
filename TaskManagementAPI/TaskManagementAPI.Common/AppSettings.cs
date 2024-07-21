using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementAPI.Common
{
    public class AppSettings
    {   
        public string AccessTokenExpireMinutes { get; set; }
        public string RefreshTokenExpireHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string TokenEncryptionKey { get; set; }

    }
    public class ConnectionStrings
    {
        public string DBConnectionString { get; set; }
    }
}
