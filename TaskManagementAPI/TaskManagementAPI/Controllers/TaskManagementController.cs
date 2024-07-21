using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Common;
using TaskManagementAPI.Controllers.Public;
using TaskManagementAPI.Manager.Abstract;

namespace TaskManagementAPI.Controllers
{
    /// <summary>
    /// Values Controller
    /// </summary>
    [Route("tapi/v1/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {

        private readonly ITaskManagementManager _taskManagementManager;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// Google Wallet Controller
        /// </summary>
        public TaskManagementController(ITaskManagementManager taskManagementManager , IWebHostEnvironment environment)
        {
            _taskManagementManager = taskManagementManager;
            _environment = environment;
        }

        /// <summary>
        /// Create Digital Card
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDetails request)
        {
            var response = await _taskManagementManager.CreateTaskAsync(request);
            return Ok(response);
        }
        /// <summary>
        /// Update Aliro CardStatus
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDetails request)
        {
            var response = await _taskManagementManager.UpdateTaskAsync(request);
            return Ok(response);
        }
        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromBody]  int id)
        {
            var response = await _taskManagementManager.DeleteTaskAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [Route("GetTaskReport")]
        public async Task<IActionResult> GetTaskReport([FromBody] ReportRequest request)
        {
            var response = await _taskManagementManager.GetTaskReportAsync(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("GetTask")]
        public async Task<IActionResult> GetTask([FromBody] int id)
        {
            var response = await _taskManagementManager.GetTaskAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [Route("GetEmployeeTaskList")]
        public async Task<IActionResult> GetEmployeeTaskList([FromBody] int userid)
        {
            var response = await _taskManagementManager.GetEmployeeTaskListAsync(userid);
            return Ok(response);
        }
        [HttpPost]
        [Route("GetTasksAndEmployeesList")]
        public async Task<IActionResult> GetTasksAndEmployeesList([FromBody] int teamid)
        {
            var response = await _taskManagementManager.GetTasksAndEmployeesListAsync(teamid);
            return Ok(response);
        }

        [HttpPost]
        [Route("GetTaskDetailsEmployee")]
        public async Task<IActionResult> GetTaskDetailsEmployee([FromBody] int teamid)
        {
            var response = await _taskManagementManager.GetTaskDetailsEmployeeAsync(teamid);
            return Ok(response);
        }
        [HttpPost]
        [Route("AddTaskNote")]
        public async Task<IActionResult> AddTaskNote([FromBody] Note request)
        {
            var response = await _taskManagementManager.AddTaskNoteAsync(request);
            return Ok(response);
        }
        [HttpPost]
        [Route("AddTaskAttachment")]
        public async Task<IActionResult> AddTaskAttachment([FromBody] RequestDocumentModel request)
        {
            var response = await _taskManagementManager.AddTaskAttachmentAsync(request , _environment);
            return Ok(response);
        }


    }
}
