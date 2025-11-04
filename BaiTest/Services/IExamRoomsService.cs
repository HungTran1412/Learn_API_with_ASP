using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IExamRoomsService
    {
        //Lay danh sach phong thi
        Task<List<ExamRoomResponse>> GetAllAsync();
        //Lay thong tin phong bang ma phong
        Task<ExamRoomResponse> GetByCodeAsync(String roomCode);
        //Them phong
        Task<ExamRoom?> AddAsync(ExamRoomRequest request);
        //Cap nhat phong
        Task<ExamRoomResponse?> UpdateAsync(String roomCode, ExamRoomUpdateRequest request);
        //Xoa phong
        Task DeleteAsync(String roomCode);
    }
}
