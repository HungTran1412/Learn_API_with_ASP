using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IExamAssignmentService
    {
        //Lay danh sach sinh vien theo phong thi
        Task<List<StudentInRoomResponse>> GetListStudentByRoomAsync();
        //Them sinh vien vao phong thi
        Task<ExamAssignment?> ExamScheduleAsync(ExamScheduleRequest request);
        //thong ke so luon con lai cua phong thi
        Task<List<QuantityStatisticResponse>> StatisticQuantityRemainingAsync();
        //lay danh sach sinh vien chua duoc xep phong
        Task<List<StudentResponse>> GetStudentUnassignmentAsync();
        //Lay cac phong co so luong vuot qua so luong cua phong
        Task<List<OverCapacityResponse>> GetOverCapacityRoomAsync();
        //Kiem tra sinh vien bị trùng phòng
        Task<List<StudentResponse>> CheckDuplicateStudentAsync();
        //Kiem tra phong nao dang thi mon nao
        Task<List<RoomSubjectResponse>> GetExamNameInRoomAsync();
    }

}
