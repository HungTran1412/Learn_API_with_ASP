using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IExamRoomsService
    {
        Task<List<ExamRoomResponse>> GetAllAsync();
        Task<ExamRoomResponse> GetByCodeAsync(String roomCode);
        Task<ExamRoom?> AddAsync(ExamRoomRequest request);
        Task<ExamRoomResponse?> UpdateAsync(String roomCode, ExamRoomUpdateRequest request);
        Task DeleteAsync(String roomCode);
    }
}
