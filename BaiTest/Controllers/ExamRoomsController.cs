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
    [Route("/room")]
    public class ExamRoomsController : ControllerBase
    {
        private IExamRoomsService examRoomService;

        public ExamRoomsController(IExamRoomsService examRoomService)
        {
            this.examRoomService = examRoomService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<ExamRoomResponse>>> GetAllAsync()
        {
            try
            {
                var roomList = await examRoomService.GetAllAsync();
                return Ok(roomList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server khi truy vấn dữ liệu!");
            }
        }

        [HttpGet("get/{roomCode}")]
        public async Task<ActionResult> GetByCodeAsync(string roomCode)
        {
            try
            {
                var room = await examRoomService.GetByCodeAsync(roomCode);
                if (room == null) return NotFound($"Không tìm thấy phòng có mã: {roomCode}");
                return Ok(room);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Lỗi server khi truy vấn {roomCode}.");
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddAsync([FromBody] ExamRoomRequest request)
        {
            try
            {
                var newRoom = await examRoomService.AddAsync(request);
                if (newRoom == null) return NotFound("Dữ liệu không hợp lệ!");

                return Ok("Thêm phòng thi thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi " + ex.ToString());
                throw;
            }
        }

        [HttpPut("update/{roomCode}")]
        public async Task<ActionResult> UpdateAsync(string roomCode, [FromBody] ExamRoomUpdateRequest request)
        {
            var updated = await examRoomService.UpdateAsync(roomCode, request);
            if (updated == null) return NotFound($"Không tìm thấy phòng có mã:{roomCode}");
            return Ok(updated);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                await examRoomService.DeleteAsync(id);
                return Ok("Xóa phòng thi thành công!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error " + e.ToString());
            }
        }
    }
}
