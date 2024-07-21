using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementAPI.Common
{
    public static class Constants
    {
        public const string ErrorMessage = "An unexpected error has occurred. Please try again !";
        public const string SuccessMessage = "Completed successfully.";
    }
    public static class InlineQuery
    {
        public const string GetUserDetails = @"SELECT u.userid, u.email, u.fullname, u.role, tm.teamid FROM users u LEFT JOIN teammembers tm ON u.userid = tm.userid WHERE u.email = @Email AND u.password = @Password";
        public const string GetReportFirst = @"SELECT t.teamid, tm.teamname, COUNT(t.id) AS total_tasks FROM tasks t JOIN teams tm ON t.teamid = tm.id WHERE 1=1 {0} GROUP BY t.teamid, tm.teamname";
        public const string GetReportSecond = @"SELECT t.teamid, tm.teamname, COUNT(t.id) AS closed_tasks FROM tasks t JOIN teams tm ON t.teamid = tm.id WHERE t.status = 'Closed' {0} GROUP BY t.teamid, tm.teamname";

    }
}
