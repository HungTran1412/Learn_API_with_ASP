using BaiTest.DTOs;
using BaiTest.Models;
using BaiTest.Services;
using BaiTest.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace BaiTest.Controllers
{
    [ApiController]
    [Route("/room")]
    public class ExamRoomsController : ControllerBase
    {
        IExamRoomsService examRoomsService;

        public ExamRoomsController()
        {
            examRoomsService = new ExamRoomsServiceImpl();
        }

        [HttpGet]
        public ActionResult<List<ExamRooms>> GetAll() => examRoomsService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<ExamRooms> Get(int id)
        {
            var room = examRoomsService.Get(id);
            if(room == null) 
                return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public ActionResult<ExamRooms> Add([FromBody] ExamRoomsRequest request)
        {
            if(request == null)
                return BadRequest("Thông tin không hợp lệ!");
            var r = examRoomsService.Add(request);

            return Ok(r);
        }

        [HttpPut("{id}")]
        public ActionResult<ExamRooms> Update(int id, [FromBody] ExamRoomsRequest request)
        {
            if(request==null)
            {
                return BadRequest("Thông tin không hợp lệ!");
            }

            var update = examRoomsService.Update(id, request);

            if(update == null)
                return NotFound($"Không tìm thấy phòng có id: {id}");

            return Ok(update);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var e = examRoomsService.Get(id);
            if(e == null)
                return NotFound() ;
            examRoomsService.Delete(id);
            return Ok("Xóa phòng thành công!");
        }
    }
}
