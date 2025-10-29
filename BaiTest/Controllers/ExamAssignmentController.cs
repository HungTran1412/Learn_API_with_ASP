using BaiTest.DTOs;
using BaiTest.Models;
using BaiTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaiTest.Controllers
{
    [Controller]
    [Route("/exam")]
    public class ExamAssignmentController : ControllerBase
    {
        public ExamAssignmentController()
        {
        }


        [HttpGet]
        public ActionResult<List<ExamAssignments>> GetAll() => ExamAssignmentService.GetAll;

        [HttpGet("{id}")]
        public ActionResult<ExamAssignments> Get(int id)
        {
            var room = ExamAssignmentService.GetExamByRoomId(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }


        [HttpGet("student/{roomId}")]
        public ActionResult<List<Student>> GetStudentInRoom(int roomId)
        {
            var assignment = ExamAssignmentService.GetListStudentInRoom(roomId);
            if(assignment == null)
                return NotFound($"Không tìm thấy phân công nào trong phòng ID {roomId}.");
            return Ok(assignment);
        }


        [HttpPost]
        public ActionResult<ExamAssignments> Add([FromBody] ExamAssignmentRequest request)
        {
            // 1. Kiểm tra tính hợp lệ cơ bản của dữ liệu đầu vào
            if (request == null)
            {
                return BadRequest("Dữ liệu phân công không hợp lệ.");
            }

            // 2. Gọi hàm Service
            var newAssignment = ExamAssignmentService.Add(request);

            // 3. Xử lý kết quả từ Service
            if (newAssignment == null)
            {
               
                return BadRequest("Lỗi phân công: Kiểm tra ID sinh viên, ID phòng thi hoặc sức chứa phòng.");
            }

            return CreatedAtAction(nameof(Get), new { id = newAssignment.Id }, newAssignment);
        }

      
    }
}
