using Microsoft.AspNetCore.Mvc;
using TaskMgmtApi.Context;
using TaskMgmtApi.Model;
using TaskMgmtApi.Utility;

namespace TaskMgmtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskMgmtContext _dbContext;

        public TaskController(
            TaskMgmtContext dbContext
        ) {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string term = "")
        {
            //var _test = this._dbContext.StaffInTasks.ToList();
            var _query = (from task in this._dbContext.Tasks
                          join sit in this._dbContext.StaffInTasks on task.Id equals sit.TaskId into taskGroup
                          where task.Status == (int)StatusEntity.Active
                          select new
                          {
                              Task = task,
                              Staffs = taskGroup
                          }).ToList();
            //var _result = this._dbContext.Tasks.Where(x => x.Status == (int)StatusEntity.Active).ToList();
            return Ok(_query);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TaskCreateBody Body)
        {
            Context.Task _newTask = new Context.Task
            {
                ParentId = Body.ParentId,
                Label = Body.Label,
                Type = Body.Type,
                Name = Body.Name,
                StartDate = Convert.ToDateTime(Body.StartDate),
                EndDate = Convert.ToDateTime(Body.EndDate),
                Duration = Body.Duration,
                Progress = Body.Progress,
                IsUnscheduled = Body.IsUnscheduled,
                Status = (int)StatusEntity.Active
            };
            var _taskCreate = this._dbContext.Tasks.Add(_newTask).Entity;
            this._dbContext.SaveChanges();

            for (int i = 0; i < Body.StaffIds.Count; i++)
            {
                this._dbContext.StaffInTasks.Add(new StaffInTask
                {
                    StaffId = Body.StaffIds[i],
                    Status = (int)StatusEntity.Active,
                    TaskId = _taskCreate.Id
                });
            }

            this._dbContext.SaveChanges();

            return Ok(_taskCreate);
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            var _staffDetail = this._dbContext.Tasks.Where(x => x.Id == Id && x.Status == (int)StatusEntity.Active).FirstOrDefault();
            return Ok(_staffDetail);
        }

        [HttpPut("{Id}")]
        public IActionResult Put(int Id, [FromBody] TaskUpdateBody Body)
        {
            var _taskUpdate = this._dbContext.Tasks.Where(x => x.Id == Id && x.Status == (int)StatusEntity.Active).FirstOrDefault();
            if (_taskUpdate == null) return BadRequest("Task does not existed");

            _taskUpdate.ParentId = Body.ParentId;
            _taskUpdate.Label = Body.Label;
            _taskUpdate.Type = Body.Type;
            _taskUpdate.Name = Body.Name;
            _taskUpdate.StartDate = Convert.ToDateTime(Body.StartDate);
            _taskUpdate.EndDate = Convert.ToDateTime(Body.EndDate);
            _taskUpdate.Duration = Body.Duration;
            _taskUpdate.Progress = Body.Progress;
            _taskUpdate.IsUnscheduled = Body.IsUnscheduled;
            //_taskUpdate.Status = Body.Status;

            this._dbContext.Tasks.Update(_taskUpdate);

            var _staffInTask = this._dbContext.StaffInTasks.Where(x => x.TaskId == Body.Id).ToList();

            for (int i = 0; i < _staffInTask.Count; i++)
            {
                this._dbContext.StaffInTasks.Remove(_staffInTask[i]);
            }

            for (int i = 0; i < Body.StaffIds.Count; i++)
            {
                this._dbContext.StaffInTasks.Add(new StaffInTask
                {
                    StaffId = Body.StaffIds[i],
                    Status = (int)StatusEntity.Active,
                    TaskId = Body.Id
                });
            }

            this._dbContext.SaveChanges();

            return Ok(_taskUpdate);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var _taskUpdate = this._dbContext.Tasks.Where(x => x.Id == Id && x.Status == (int)StatusEntity.Active).FirstOrDefault();
            if (_taskUpdate == null) return BadRequest("Task does not existed");

            _taskUpdate.Status = (int)StatusEntity.Deactive;

            this._dbContext.Tasks.Update(_taskUpdate);
            this._dbContext.SaveChanges();

            return Ok(_taskUpdate);
        }
    }
}
