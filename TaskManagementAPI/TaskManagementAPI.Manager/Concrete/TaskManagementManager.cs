
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using TaskManagementAPI.Common;
using TaskManagementAPI.Common.Response;
using TaskManagementAPI.Manager.Abstract;
using TaskManagementAPI.Repository.Abstract;
using TaskManagementAPI.Repository.Concrete;

namespace TaskManagementAPI.Manager.Concrete
{
    public class TaskManagementManager : ITaskManagementManager
    {
        private ILogger<TaskManagementManager> _logger;
        private ITaskManagementRepository _taskManagementRepository;
        public TaskManagementManager( ITaskManagementRepository taskManagementRepository, ILogger<TaskManagementManager> logger )
        {
            _logger = logger;
            _taskManagementRepository = taskManagementRepository;
        }
        public async Task<ServiceResponse<bool>> CreateTaskAsync(TaskDetails request)
        {
            _logger.LogInformation("Manager CreateTaskAsync -> Start");
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                response.IsSuccess = await _taskManagementRepository.CreateTaskAsync(request);
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager CreateTaskAsync -> End");

            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateTaskAsync(TaskDetails request)
        {
            _logger.LogInformation("Manager UpdateTaskAsync -> Start");
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                response.IsSuccess = await _taskManagementRepository.UpdateTaskAsync(request);
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager UpdateTaskAsync -> End");

            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteTaskAsync(int id)
        {
            _logger.LogInformation("Manager DeleteTaskAsync -> Start");
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                response.IsSuccess = await _taskManagementRepository.deleteTaskAsync(id);
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager DeleteTaskAsync -> End");

            return response;
        }

        public async Task<ServiceResponse<List<ReportResponse>>> GetTaskReportAsync(ReportRequest request)
        {
            _logger.LogInformation("Manager GetTaskReportAsync -> Start");
            ServiceResponse<List<ReportResponse>> response = new ServiceResponse<List<ReportResponse>>();
            try
            {
                var dalRes = await _taskManagementRepository.GetTaskReportAsync(request);
                if(dalRes != null)
                {
                    response.IsSuccess = true;
                    response.Data = dalRes;
                }
                else
                {   
                    response.Data = new List<ReportResponse>();
                }
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
                response.Data = new List<ReportResponse>();
            }

            _logger.LogInformation("Manager GetTaskReportAsync  -> End");

            return response;
        }

        public async Task<ServiceResponse<TaskDetails>> GetTaskAsync(int id)
        {
            _logger.LogInformation("Manager GetTaskAsync -> Start");
            ServiceResponse<TaskDetails> response = new ServiceResponse<TaskDetails>();
            try
            {
                var res = await _taskManagementRepository.GetTaskAsync(id);
                if(res != null)
                {
                    response.Data = res;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = new TaskDetails();
                }
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
                response.Data = new TaskDetails();
            }

            _logger.LogInformation("Manager GetTaskAsync -> End");

            return response;
        }
        public async Task<ServiceResponse<List<TaskDetails>>> GetEmployeeTaskListAsync(int userid)
        {
            _logger.LogInformation("Manager GetEmployeeTaskListAsync -> Start");
            ServiceResponse<List<TaskDetails>> response = new ServiceResponse<List<TaskDetails>>();
            try
            {
                var res = await _taskManagementRepository.GetEmployeeTaskListAsync(userid);
                if (res != null)
                {
                    response.Data = res;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = new List<TaskDetails>();
                }
                
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {   
                response.Data = new List<TaskDetails>();
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager GetEmployeeTaskListAsync -> End");

            return response;
        }
        public async Task<ServiceResponse<managerDashboard>> GetTasksAndEmployeesListAsync(int userid)
        {
            _logger.LogInformation("Manager GetTasksAndEmployeesListAsync -> Start");
            ServiceResponse<managerDashboard> response = new ServiceResponse<managerDashboard>();
            try
            {   
               
                var res = await _taskManagementRepository.GetTasksAndEmployeesListAsync(userid);
                if (res != null && res.employeeList !=null)
                {
                    response.IsSuccess = true;
                    response.Data = res;
                }
                else
                {
                    response.Data = new managerDashboard() { employeeList = new List<Employee>(), taskList = new List<TaskDetails>() };
                }
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                response.Data = new managerDashboard() { employeeList = new List<Employee>(), taskList = new List<TaskDetails>() };
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager GetTasksAndEmployeesListAsync -> End");

            return response;
        }

        public async Task<ServiceResponse<TaskDetailsEmployee>> GetTaskDetailsEmployeeAsync(int taskid)
        {
            _logger.LogInformation("Manager  GetTaskDetailsEmployeeAsync -> Start");
            ServiceResponse<TaskDetailsEmployee> response = new ServiceResponse<TaskDetailsEmployee>();
            try
            {

                var res = await _taskManagementRepository.GetTaskDetailsEmployeeAsync(taskid);
                if (res != null )
                {
                    response.IsSuccess = true;
                    response.Data = res;
                }
                else
                {
                    response.Data = new TaskDetailsEmployee() { notes = new List<Note>(), documents = new List<Attachment>() };
                }
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                response.Data = new TaskDetailsEmployee() { notes = new List<Note>(), documents = new List<Attachment>() };
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
            }

            _logger.LogInformation("Manager  GetTaskDetailsEmployeeAsync -> End");

            return response;
        }

        public async Task<ServiceResponse<Note>> AddTaskNoteAsync(Note request)
        {
            _logger.LogInformation("Manager AddTaskNoteAsync -> Start");
            ServiceResponse<Note> response = new ServiceResponse<Note>();
            try
            {   
                request.createdDate = DateTime.Now;
                int Id = await _taskManagementRepository.AddTaskNoteAsync(request);
                request.id = Id;
                if (request.id > 0)
                {
                    response.Data = request;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = new Note();
                }
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
                response.Data = new Note();
            }

            _logger.LogInformation("Manager AddTaskNoteAsync -> End");

            return response;
        }

        public async Task<ServiceResponse<Attachment>> AddTaskAttachmentAsync(RequestDocumentModel request , IWebHostEnvironment environment)
        {
            _logger.LogInformation("Manager AddTaskNoteAsync -> Start");
            ServiceResponse<Attachment> response = new ServiceResponse<Attachment>();
            try
            {
                if (request.fileData == null || request.fileData.Length == 0)
                {
                    return response;
                }
                var fileName = Guid.NewGuid().ToString() + "_" + request.fileData.FileName;
                var filePath = SaveFile(request.fileData,fileName , environment);
                var req = new Attachment()
                {
                    taskid = request.taskId,
                    filename = fileName,
                    uploadedby = request.createBy,
                    uploadedat = DateTime.Now
                };
                if (!string.IsNullOrEmpty(filePath.Result)) {
                    
                    int Id = await _taskManagementRepository.AddTaskAttachmentAsync(req);
                    req.id = Id;
                }
                if(req.id> 0)
                {
                    response.Data = req;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = new Attachment();
                }
                response.Message = response.IsSuccess ? Constants.SuccessMessage : Constants.ErrorMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                response.IsSuccess = false;
                response.Message = Constants.ErrorMessage;
                response.Data = new Attachment();
            }

            _logger.LogInformation("Manager AddTaskNoteAsync -> End");

            return response;
        }
        private async Task<string> SaveFile(IFormFile File, string fileName , IWebHostEnvironment environment)
        {
            if (File == null || File.Length == 0)
            {
                return "";
            }

            try
            {
                
                var uploadDir = Path.Combine(environment.ContentRootPath, "Uploads");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine(uploadDir, fileName);

                // Save the file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }

                // Return the relative file path
                var relativePath = Path.Combine("Uploads", fileName).Replace("\\", "/");
                return relativePath;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message:{ex.Message}{Environment.NewLine} STACK TRACE:{ex.StackTrace}");
                return "";
            }
        }

    }
}
