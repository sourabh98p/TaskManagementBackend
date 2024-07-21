using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagementAPI.Common.Response;

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
    public class TaskDetails 
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; } = "TO-DO";
        [Column("duedate")]
        public DateTime? dueDate { get; set; }
        [Column("createdby")]
        public int createdBy { get; set; }
        [Column("assignedto")]
        public int assignedTo { get; set; }
        [Column("completedat")]
        public DateTime? completedAt { get; set; }
        [Column("createdat")]
        public DateTime createdAt { get; set; }
        [Column("updatedat")]
        public DateTime updatedAt { get; set; }
        
    }
    public class TaskDetailsEmployee : TaskDetails
    {
        public List<Note> notes { get; set; }
        public List<Attachment> documents { get; set; }
    }

    public class Note
    {
        public int? id { get; set; }
        public string content { get; set; }
        public DateTime createdDate { get; set; }
        public int? taskId { get; set; }
        public int? createdBy { get; set; }
    }

    public class RequestDocumentModel
    {
        public IFormFile? fileData { get; set; }  // Using IFormFile for file uploads
        public int taskId { get; set; }
        public int createBy { get; set; }
    }

    public class Attachment
    {
        public int id { get; set; }
        public int taskid { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public int uploadedby { get; set; }
        public DateTime uploadedat { get; set; }
    }

    public class ReportRequest
    {
        public string? interval { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
    public class ReportResponse
    {   
        public int teamId { get; set; }
        public string teamName { get; set; }
        public int closedTasks { get; set; }
        public int totalTasks { get; set; }
    }

    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }

    }

    public class TeamMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public int teamid { get; set; }
        public int userid { get; set; }

    }
    public class Employee
    {
        public int id { get; set; }
        public string fullName { get; set; }
    }
    public class managerDashboard
    {
        public List<Employee> employeeList { get; set; }
        public List<TaskDetails> taskList { get; set; }
    }


}
