using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementAPI.Common;
using TaskManagementAPI.Common.Response;
namespace TaskManagementAPI.Manager.Abstract
{
    public interface ITaskManagementManager
    {
        Task<ServiceResponse<bool>> CreateTaskAsync(TaskDetails request);
        Task<ServiceResponse<bool>> UpdateTaskAsync(TaskDetails request);
        Task<ServiceResponse<bool>> DeleteTaskAsync(int id);
        Task<ServiceResponse<List<ReportResponse>>> GetTaskReportAsync(ReportRequest request);
        Task<ServiceResponse<TaskDetails>> GetTaskAsync(int id);
        Task<ServiceResponse<List<TaskDetails>>> GetEmployeeTaskListAsync(int userid);
        Task<ServiceResponse<managerDashboard>> GetTasksAndEmployeesListAsync(int userid);
        Task<ServiceResponse<TaskDetailsEmployee>> GetTaskDetailsEmployeeAsync(int taskid);
        Task<ServiceResponse<Note>> AddTaskNoteAsync(Note request);
        Task<ServiceResponse<Attachment>> AddTaskAttachmentAsync(RequestDocumentModel request, IWebHostEnvironment environment);

    }
}
