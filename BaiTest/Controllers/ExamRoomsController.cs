using BaiTest.DTOs;
using BaiTest.Models;
using BaiTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaiTest.Controllers
{
    [ApiController]
    [Route("/room")]
    public class ExamRoomsController : ControllerBase
    {
        public ExamRoomsController()
        {
        }
        [HttpGet]
        public ActionResult<List<ExamRooms>> GetAll() => ExamRoomsService.GetAll;

        [HttpGet("{id}")]
        public ActionResult<ExamRooms> Get(int id)
        {
            var room = ExamRoomsService.Get(id);
            if(room == null) 
                return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public ActionResult<ExamRooms> Add([FromBody] ExamRoomsRequest request)
        {
            if(request == null)
                return BadRequest("Thông tin không hợp lệ!");
            var r = ExamRoomsService.Add(request);

            return Ok(r);
        }

        [HttpPut("{id}")]
        public ActionResult<ExamRooms> Update(int id, [FromBody] ExamRoomsRequest request)
        {
            if(request==null)
            {
                return BadRequest("Thông tin không hợp lệ!");
            }

            var update = ExamRoomsService.Update(id, request);

            if(update == null)
                return NotFound($"Không tìm thấy phòng có id: {id}");

            return Ok(update);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var e = ExamRoomsService.Get(id);
            if(e == null)
                return NotFound() ;
            ExamRoomsService.Remove(id);
            return Ok("Xóa phòng thành công!");
        }
    }
}
