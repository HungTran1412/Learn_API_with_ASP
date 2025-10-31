using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IExamAssignmentService
    {
        Task<List<StudentInRoomResponse>> GetListStudentByRoomAsync();
        Task<ExamAssignment?> ExamScheduleAsync(ExamScheduleRequest request);
        Task<QuantityRemainingResponse> StatisticQuantityRemaining();
       
    }

}
