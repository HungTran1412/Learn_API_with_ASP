using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IExamAssignmentService
    {
        List<ExamAssignments> GetAll();
        ExamAssignments GetExamByRoomId(int id);
        List<Student> GetListStudentInRoom(int roomId);
        ExamAssignments Add(ExamAssignmentRequest request);
        bool DeleteStudentFromRoom(ExamAssignmentRequest request);
    }
}
