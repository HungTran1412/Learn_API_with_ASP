using Azure.Core;
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
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<StudentResponse>>> GetAllAsync()
        {
            try
            {
                var studentList = await studentService.GetAllAsync();
                return studentList;
            }
            catch(Exception ex) 
            {
                return StatusCode(500, "Lỗi server khi truy vấn dữ liệu!");
            }
        }

        [HttpGet("get/{studentCode}")]
        public async Task<ActionResult> GetByStudentCodeAsync(string studentCode)
        {
            try
            {
                var student = await studentService.GetByStudentCodeAsync(studentCode);
                if (student == null) return NotFound($"Không tìm thấy sinh viên có mã sinh viên {studentCode}");
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server khi truy vấn sinh viên ID {studentCode}.");
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddAsync([FromBody] StudentRequest request)
        {
            try
            {
                var newStudent = await studentService.AddAsync(request);
                if (newStudent == null) return NotFound("Dữ liệu không được trống!");

                return Ok("Thêm sinh viên thành công!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Lỗi Server nội bộ khi thêm sinh viên." + e.ToString());
            }
        }

        [HttpPut("update/{studentCode}")]
        public async Task<ActionResult> UpdateAsync(string studentCode,[FromBody] StudentUpdateRequest request)
        {
            try
            {
                var update = await studentService.UpdateAsync(studentCode, request);
                if (update == null) return NotFound($"Không tìm thấy sinh viên có mã: {studentCode}");
                return Ok(update);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Lỗi Server nội bộ khi cập nhật dữ liệu." + e.ToString());
            }
        }

        [HttpDelete("delete/{studentCode}")]
        public async Task<ActionResult> DeleteAsync(string studentCode)
        {
            try
            {
                await studentService.DeleteAsync(studentCode);
                return Ok("Xóa sinh viên thành công!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Lỗi Server nội bộ khi cập nhật dữ liệu." + e.ToString());
            }
        }
    }
}
