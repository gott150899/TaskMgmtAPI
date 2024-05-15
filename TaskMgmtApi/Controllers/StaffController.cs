using Microsoft.AspNetCore.Mvc;
using TaskMgmtApi.Context;
using TaskMgmtApi.Model;
using TaskMgmtApi.Utility;

namespace TaskMgmtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly TaskMgmtContext _dbContext;

        public StaffController(
            TaskMgmtContext dbContext
        )
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string term = "", int status = 9999)
        {
            var _query = this._dbContext.Staff.AsQueryable();
            switch (status)
            {
                case 9999:
                    break;
                case (int)StatusEntity.Active:
                    _query = _query.Where(x => x.Status == (int)StatusEntity.Active);
                    break;
                case (int)StatusEntity.Deactive:
                    _query = _query.Where(x => x.Status == (int)StatusEntity.Deactive);
                    break;
                case (int)StatusEntity.Deleted:
                    _query = _query.Where(x => x.Status == (int)StatusEntity.Deleted);
                    break;
            }
            if (!string.IsNullOrEmpty(term))
            {
                var _termNonUnicode = GlobalFunction.RemoveDiacritics(term);
                _query = _query.Where(x => x.NameNonUnicode.Contains(_termNonUnicode) || _termNonUnicode.Contains(x.NameNonUnicode));
            }
            var _result = _query.ToList();
            return Ok(_result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] StaffCreateBody Body)
        {
            var _staffCreate = this._dbContext.Staff.Add(new Staff
            {
                FullName = Body.FullName,
                ShortName = Body.ShortName,
                NameNonUnicode = GlobalFunction.RemoveDiacritics(Body.FullName),
                Status = (int)StatusEntity.Active
            }).Entity;
            this._dbContext.SaveChanges();

            return Ok(_staffCreate);
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            var _staffDetail = this._dbContext.Staff.Where(x => x.Id == Id && x.Status == (int)StatusEntity.Active).FirstOrDefault();
            return Ok(_staffDetail);
        }

        [HttpPut("{Id}")]
        public IActionResult Put(int Id, [FromBody] StaffUpdateBody Body)
        {
            var _staffUpdate = this._dbContext.Staff.Where(x => x.Id == Id && x.Status == (int)StatusEntity.Active).FirstOrDefault();
            if (_staffUpdate == null) return BadRequest("Staff does not existed");

            _staffUpdate.FullName = Body.FullName;
            _staffUpdate.ShortName = Body.ShortName;
            _staffUpdate.NameNonUnicode = GlobalFunction.RemoveDiacritics(Body.FullName);

            this._dbContext.Staff.Update(_staffUpdate);
            this._dbContext.SaveChanges();

            return Ok(_staffUpdate);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var _staffUpdate = this._dbContext.Staff.Where(x => x.Id == Id && x.Status == (int)StatusEntity.Active).FirstOrDefault();
            if (_staffUpdate == null) return BadRequest("Staff does not existed");

            _staffUpdate.Status = (int)StatusEntity.Deactive;

            this._dbContext.Staff.Update(_staffUpdate);
            this._dbContext.SaveChanges();

            return Ok(_staffUpdate);
        }
    }
}
