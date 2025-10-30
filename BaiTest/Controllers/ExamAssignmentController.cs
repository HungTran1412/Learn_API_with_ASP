using BaiTest.DTOs;
using BaiTest.Models;
using BaiTest.Services;
using BaiTest.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace BaiTest.Controllers
{
    [ApiController]
    [Route("/exam")]
    public class ExamAssignmentController : ControllerBase
    {
        IExamAssignmentService examAssignmentService;
        public ExamAssignmentController()
        {
            examAssignmentService = new ExamAssignmentServiceImpl();
        }


        [HttpGet]
        public ActionResult<List<ExamAssignments>> GetAll() => examAssignmentService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<ExamAssignments> Get(int id)
        {
            var room = examAssignmentService.GetExamByRoomId(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }


        [HttpGet("student/{roomId}")]
        public ActionResult<List<Student>> GetStudentInRoom(int roomId)
        {
            var assignment = examAssignmentService.GetListStudentInRoom(roomId);
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
            var newAssignment = examAssignmentService.Add(request);

            // 3. Xử lý kết quả từ Service
            if (newAssignment == null)
            {
               
                return BadRequest("Lỗi phân công: Kiểm tra ID sinh viên, ID phòng thi hoặc sức chứa phòng.");
            }

            return CreatedAtAction(nameof(Get), new { id = newAssignment.Id }, newAssignment);
        }

        [HttpDelete("/delete-student/")]
        public IActionResult Delete([FromBody] ExamAssignmentRequest request)
        {
            if(request == null || request.RoomId <= 0 || request.StudentId <= 0)
            {
                return BadRequest("Dữ liệu bị thiếu!");
            }

            bool deleted = examAssignmentService.DeleteStudentFromRoom(request);

            if (!deleted)
            {
                return BadRequest($"Không tìm thấy sinh viên có id là {request.StudentId} trong danh sách!"); 
            }

            return Ok("Xóa sinh viên khỏi phòng thi thành công!");
        }
    }
}
