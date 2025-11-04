using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
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
        private IExamAssignmentService examAssignmentService;

        public ExamAssignmentController(IExamAssignmentService examAssignmentService)
        {
            this.examAssignmentService = examAssignmentService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<StudentInRoomResponse>>> GetListStudentInRoomAsync()
        {
            try
            {
                var result = await examAssignmentService.GetListStudentByRoomAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error : " + e.ToString());
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> ExamScheduleAsync([FromBody] ExamScheduleRequest request)
        {
            try
            {
                var add = await examAssignmentService.ExamScheduleAsync(request);
                if (add == null) return NotFound("Dữ liệu không hợp lệ!");
                return Ok("Thêm thành công!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "error: " + e.ToString());
            }
        }

        [HttpGet("/room-stat")]
        public async Task<ActionResult<List<QuantityStatisticResponse>>> GetQuantityStatisticAsync()
        {
            try
            {
                var result = await examAssignmentService.StatisticQuantityRemainingAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error: " + e.ToString());
            }
        }

        [HttpGet("/unassigned")]
        public async Task<ActionResult<List<StudentResponse>>> GetUnassignmentStudentAsync()
        {
            try
            {
                var result = await examAssignmentService.GetStudentUnassignmentAsync();

                if (result.Any()) return Ok(result);

                return NotFound("Tất cả sinh viên đã được xếp phòng");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error: " + e.ToString());
            }
        }

        [HttpGet("/over-capacity")]
        public async Task<ActionResult<List<OverCapacityResponse>>> GetOverCapacityRoomAsync()
        {
            try
            {
                var result = await examAssignmentService.GetOverCapacityRoomAsync();
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error: " + e.ToString());
            }
        }

        [HttpGet("/duplicate-student")]
        public async Task<ActionResult<List<StudentResponse>>> CheckDuplicateStudentAsync()
        {
            try
            {
                var result = await examAssignmentService.CheckDuplicateStudentAsync();
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error: " + e.ToString());
            }
        }

        [HttpGet("/subject")]
        public async Task<ActionResult<List<RoomSubjectResponse>>> GetExamNameInRoomAsync()
        {
            try
            {
                var result = await examAssignmentService.GetExamNameInRoomAsync();
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error: " + e.ToString());
            }
        }
    }
}
