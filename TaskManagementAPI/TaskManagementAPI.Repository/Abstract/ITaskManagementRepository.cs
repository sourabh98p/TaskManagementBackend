using TaskManagementAPI.Common;

namespace TaskManagementAPI.Repository.Abstract
{
    public interface ITaskManagementRepository
    {
        Task<bool> CreateTaskAsync(TaskDetails task);
        Task<bool> UpdateTaskAsync(TaskDetails updatedTask);
        Task<bool> deleteTaskAsync(int id);
        Task<List<ReportResponse>> GetTaskReportAsync(ReportRequest request);
        Task<TaskDetails> GetTaskAsync(int id);
        Task<List<TaskDetails>> GetEmployeeTaskListAsync(int userid);
        Task<managerDashboard> GetTasksAndEmployeesListAsync(int teamid);
        Task<TaskDetailsEmployee> GetTaskDetailsEmployeeAsync(int taskid);
        Task<int> AddTaskNoteAsync(Note newNote);
        Task<int> AddTaskAttachmentAsync(Attachment newAttachment);
    }
}
