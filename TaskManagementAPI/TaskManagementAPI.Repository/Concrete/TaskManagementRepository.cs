using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Npgsql;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TaskManagementAPI.Common;
using TaskManagementAPI.Common.Response;
using TaskManagementAPI.Repository.Abstract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace TaskManagementAPI.Repository.Concrete
{
    public class TaskManagementRepository : ITaskManagementRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TaskManagementRepository> _logger;
        private readonly ConnectionStrings _connectionStrings;
        public TaskManagementRepository(ILogger<TaskManagementRepository> logger, AppDbContext dbContext, IOptionsSnapshot<ConnectionStrings> connectionString)
        {
            _dbContext = dbContext;
            _logger = logger;
            _connectionStrings = connectionString.Value;
        }

        public async Task<bool> CreateTaskAsync(TaskDetails task)
        {
            bool res = true;
            _logger.LogInformation("CreateTaskAsync Repo -> Start");
            try
            {
                _dbContext.tasks.Add(task);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                res = false;
                _logger.LogError($"Error inserting task: {ex.Message}");  
            }
            _logger.LogInformation("CreateTaskAsync Repo -> End");
            return res;
        }

        public async Task<bool> UpdateTaskAsync(TaskDetails updatedTask)
        {
            bool res = true;
            _logger.LogInformation("UpdateTaskAsync Repo -> Start");
            try
            {
                var existingTask = await _dbContext.tasks.FindAsync(updatedTask.id);

                if (existingTask == null)
                {
                    return false;
                }

                // Update properties of existingTask with updatedTask data
                existingTask.title = updatedTask.title;
                existingTask.description = updatedTask.description;
                existingTask.status = updatedTask.status;
                existingTask.dueDate = updatedTask.dueDate;
                existingTask.createdBy = updatedTask.createdBy;
                existingTask.assignedTo = updatedTask.assignedTo;
                existingTask.completedAt = updatedTask.completedAt;
                existingTask.updatedAt = DateTime.UtcNow;

                _dbContext.Entry(existingTask).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                res = false;
                _logger.LogError($"Error inserting task: {ex.Message}");
            }
            _logger.LogInformation("UpdateTaskAsync Repo -> End");
            return res;
        }
        public async Task<bool> deleteTaskAsync(int id)
        {
            bool res = true;
            _logger.LogInformation("deleteTaskAsync Repo -> Start");
            try
            {
                var task = await _dbContext.tasks.FindAsync(id);

                if (task == null)
                {
                    return false;
                }

                _dbContext.tasks.Remove(task);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                res = false;
                _logger.LogError($"Error inserting task: {ex.Message}");
            }
            _logger.LogInformation("deleteTaskAsync Repo -> End");
            return res;
        }

        public async Task<TaskDetails> GetTaskAsync(int id)
        {
            _logger.LogInformation("GetTaskAsync Repo -> Start");
            try
            {
                var res = await _dbContext.tasks.FindAsync(id);
                _logger.LogInformation("GetTaskAsync Repo -> End");
                return res;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting task: {ex.Message}");
                return null;
            }
            
        }

        public async Task<List<ReportResponse>> GetTaskReportAsync(ReportRequest request)
        {    
            List<ReportResponse> result = new List<ReportResponse>();
            _logger.LogInformation("deleteTaskAsync Repo -> Start");
            try
            {
                string dateCondition = "";

                switch (request.interval.ToLower())
                {
                    case "week":
                        dateCondition = "AND ta.createdat >= current_date - interval '7 days'";
                        break;
                    case "month":
                        dateCondition = "AND ta.createdat >= current_date - interval '30 days'";
                        break;
                    case "custom":
                        if (request.startDate.HasValue && request.endDate.HasValue)
                        {
                            dateCondition = $"AND ta.createdat >= '{request.startDate.Value.ToString("yyyy-MM-dd")}' " +
                                            $"AND ta.createdat <= '{request.endDate.Value.ToString("yyyy-MM-dd")}'";
                        }
                        break;
                    default:
                        // Default to no additional condition
                        break;
                }


                using (var conn = new NpgsqlConnection(_connectionStrings.DBConnectionString))
                    {
                        conn.Open();
                    string sqlQuery = $@"
                    WITH TotalTasks AS (
                        SELECT
                            t.id AS team_id,
                            t.teamname,
                            COUNT(*) AS total_tasks
                        FROM
                            Teams t
                        JOIN
                            TeamMembers tm ON t.id = tm.teamid
                        JOIN
                            Tasks ta ON tm.userid = ta.assignedto
                        WHERE
                            1=1 {dateCondition}
                        GROUP BY
                            t.id, t.teamname
                    ), ClosedTasks AS (
                        SELECT
                            t.id AS team_id,
                            COUNT(*) AS closed_tasks
                        FROM
                            Teams t
                        JOIN
                            TeamMembers tm ON t.id = tm.teamid
                        JOIN
                            Tasks ta ON tm.userid = ta.assignedto
                        WHERE
                            ta.status = 'closed' {dateCondition}
                        GROUP BY
                            t.id
                    )
                    SELECT
                        tt.teamname,
                        COALESCE(ct.closed_tasks, 0) AS closed_tasks,
                        COALESCE(tt.total_tasks, 0) AS total_tasks
                    FROM
                        TotalTasks tt
                    LEFT JOIN
                        ClosedTasks ct ON tt.team_id = ct.team_id";

                    using (var cmd = new NpgsqlCommand(
                      sqlQuery, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new ReportResponse
                                {
                                    teamName = reader["teamname"].ToString(),
                                    totalTasks = Convert.ToInt32(reader["total_tasks"]),
                                    closedTasks = Convert.ToInt32(reader["closed_tasks"])
                                });
                            }
                        }
                    }

                    conn.Close();
                    }

                   return result;
                }
                catch (Exception ex)
                {

                    _logger.LogError($"Error inserting task: {ex.Message}");
                    return null;
                }
            
        }
        public async Task<List<TaskDetails>> GetEmployeeTaskListAsync(int userid)
        {
            _logger.LogInformation("GetEmployeeTaskListAsync Repo -> Start");
            try
            {
                var tasks = await _dbContext.tasks.Where(t => t.assignedTo == userid).ToListAsync();

                _logger.LogInformation("GetEmployeeTaskListAsync Repo -> End");
                return tasks;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting task: {ex.Message}");
                return null;
            }

        }

        public async Task<managerDashboard> GetTasksAndEmployeesListAsync(int teamid)
        {
            var res = new managerDashboard() { employeeList = new List<Employee>(), taskList = new List<TaskDetails>() };
            _logger.LogInformation("GetTasksAndEmployeesListAsync Repo -> Start");
            try
            {
                List<TaskDetails> tasks = new List<TaskDetails>();
                List<Employee> employees = new List<Employee>();

                using (var conn = new NpgsqlConnection(_connectionStrings.DBConnectionString))
                {
                    conn.Open();

                    // Query to get tasks for the specified team
                    using (var cmdTasks = new NpgsqlCommand(
                        "SELECT t.id, t.title, t.description, t.status, t.duedate, t.createdby, t.assignedto, t.completedat, t.createdat, t.updatedat " +
                        "FROM tasks t " +
                        "JOIN teammembers tm ON t.assignedto = tm.userid " +
                        "WHERE tm.teamid = @teamId", conn))
                    {
                        cmdTasks.Parameters.AddWithValue("teamId", teamid);

                        using (var reader = cmdTasks.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new TaskDetails
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    title = reader["title"].ToString(),
                                    description = reader["description"].ToString(),
                                    status = reader["status"].ToString(),
                                    dueDate = reader["duedate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["duedate"]),
                                    createdBy = Convert.ToInt32(reader["createdby"]),
                                    assignedTo = Convert.ToInt32(reader["assignedto"]),
                                    completedAt = reader["completedat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["completedat"]),
                                    createdAt = Convert.ToDateTime(reader["createdat"]),
                                    updatedAt = Convert.ToDateTime(reader["updatedat"])
                                });
                            }
                        }
                    }
                    using (var cmdEmployees = new NpgsqlCommand(
                    "SELECT u.userid, u.email, u.fullname, u.role " +
                    "FROM users u " +
                    "JOIN teammembers tm ON u.userid = tm.userid " +
                    "WHERE tm.teamid = @teamId", conn))
                    {
                        cmdEmployees.Parameters.AddWithValue("teamId", teamid);

                        using (var reader = cmdEmployees.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(new Employee
                                {
                                    id = Convert.ToInt32(reader["userid"]),
                                    fullName = reader["fullname"].ToString()
                                });
                            }
                        }
                    }


                    conn.Close();
                }

                res.employeeList = employees;
                res.taskList = tasks;
                _logger.LogInformation("GetTasksAndEmployeesListAsync Repo -> End");
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting task: {ex.Message}");
                res.taskList = null;
                res.employeeList = null;
            }
            return res;

        }

        public async Task<TaskDetailsEmployee> GetTaskDetailsEmployeeAsync(int taskid)
        {
            var res = new TaskDetailsEmployee() { };
            _logger.LogInformation("GetTaskDetailsEmployee Repo -> Start");
            try
            {

                using (var conn = new NpgsqlConnection(_connectionStrings.DBConnectionString))
                {
                    conn.Open();

                    // Query to fetch task details
                    using (var cmdTask = new NpgsqlCommand(
                        "SELECT t.id, t.title, t.description, t.status, t.duedate, t.createdby, t.assignedto, t.completedat, t.createdat, t.updatedat " +
                        "FROM tasks t " +
                        "WHERE t.id = @taskId", conn))
                    {
                        cmdTask.Parameters.AddWithValue("taskId", taskid);

                        using (var reader = cmdTask.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                res = new TaskDetailsEmployee
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    title = reader["title"].ToString(),
                                    description = reader["description"].ToString(),
                                    status = reader["status"].ToString(),
                                    dueDate = reader["duedate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["duedate"]),
                                    createdBy = Convert.ToInt32(reader["createdby"]),
                                    assignedTo = Convert.ToInt32(reader["assignedto"]),
                                    completedAt = reader["completedat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["completedat"]),
                                    createdAt = Convert.ToDateTime(reader["createdat"]),
                                    updatedAt = Convert.ToDateTime(reader["updatedat"]),
                                    notes = new List<Note>(),
                                    documents = new List<Attachment>()
                                };
                            }
                        }
                    }

                    // Query to fetch notes for the task
                    using (var cmdNotes = new NpgsqlCommand(
                        "SELECT id, taskid, content, createdby, createdat " +
                        "FROM notes " +
                        "WHERE taskid = @taskId", conn))
                    {
                        cmdNotes.Parameters.AddWithValue("taskId", taskid);

                        using (var reader = cmdNotes.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.notes.Add(new Note
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    content = reader["content"].ToString(),
                                    createdDate = Convert.ToDateTime(reader["createdat"])
                                });
                            }
                        }
                    }

                    // Query to fetch attachments (documents) for the task
                    using (var cmdAttachments = new NpgsqlCommand(
                        "SELECT id, taskid, filename, filepath, uploadedby, uploadedat " +
                        "FROM attachments " +
                        "WHERE taskid = @taskId", conn))
                    {
                        cmdAttachments.Parameters.AddWithValue("taskId", taskid);

                        using (var reader = cmdAttachments.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.documents.Add(new Attachment
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    filename = reader["filename"].ToString(),
                                    filepath = reader["filepath"].ToString()
                                });
                            }
                        }
                    }

                    conn.Close();
                }
                 
                _logger.LogInformation("GetTasksAndEmployeesListAsync Repo -> End");


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting task: {ex.Message}");
                res = null;
            }
            return res;

        }
        public async Task<int> AddTaskNoteAsync(Note newNote)
        {
            int generatedId = 0;
            _logger.LogInformation("AddTaskNoteAsync Repo -> Start");
            try
            {
                

                using (var conn = new NpgsqlConnection(_connectionStrings.DBConnectionString))
                {
                    conn.Open();

                    // Insert command to add a new note and return the generated id
                    using (var cmdInsert = new NpgsqlCommand(
                        "INSERT INTO notes (taskid, content, createdby, createdat) " +
                        "VALUES (@taskid, @content, @createdby, @createdat) " +
                        "RETURNING id", conn))
                    {
                        cmdInsert.Parameters.AddWithValue("taskid", newNote.taskId);
                        cmdInsert.Parameters.AddWithValue("content", newNote.content);
                        cmdInsert.Parameters.AddWithValue("createdby", newNote.createdBy);
                        cmdInsert.Parameters.AddWithValue("createdat", newNote.createdDate);

                        // Execute the command and retrieve the generated id
                        generatedId = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    }

                    conn.Close();
                }

            }
            catch (Exception ex)
            {
          
                _logger.LogError($"Error inserting task: {ex.Message}");
            }
            _logger.LogInformation("AddTaskNoteAsync Repo -> End");
            return generatedId;
        }
        public async Task<int> AddTaskAttachmentAsync(Attachment newAttachment)
        {
            int generatedId = 0;
            _logger.LogInformation("AddTaskNoteAsync Repo -> Start");
            try
            {


                using (var conn = new NpgsqlConnection(_connectionStrings.DBConnectionString))
                {
                    conn.Open();

                    // Insert command to add a new attachment and return the generated id
                    using (var cmdInsert = new NpgsqlCommand(
                        "INSERT INTO attachments (taskid, filename, filepath, uploadedby, uploadedat) " +
                        "VALUES (@taskid, @filename, @filepath, @uploadedby, @uploadedat) " +
                        "RETURNING id", conn))
                    {
                        cmdInsert.Parameters.AddWithValue("taskid", newAttachment.taskid);
                        cmdInsert.Parameters.AddWithValue("filename", newAttachment.filename);
                        cmdInsert.Parameters.AddWithValue("filepath", newAttachment.filepath);
                        cmdInsert.Parameters.AddWithValue("uploadedby", newAttachment.uploadedby);
                        cmdInsert.Parameters.AddWithValue("uploadedat", newAttachment.uploadedat);

                        // Execute the command and retrieve the generated id
                        generatedId = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    }

                    conn.Close();
                }


            }
            catch (Exception ex)
            {

                _logger.LogError($"Error inserting task: {ex.Message}");
            }
            _logger.LogInformation("AddTaskNoteAsync Repo -> End");
            return generatedId;
        }



    }
}
