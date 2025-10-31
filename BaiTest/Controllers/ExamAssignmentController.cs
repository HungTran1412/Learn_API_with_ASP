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
    }
}
