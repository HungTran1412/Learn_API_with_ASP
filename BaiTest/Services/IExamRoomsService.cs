using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IExamRoomsService
    {
        List<ExamRooms> GetAll();
        ExamRooms Get(int id);
        ExamRooms Add(ExamRoomsRequest request);
        ExamRooms Update(int id,ExamRoomsRequest request);
        void Delete(int id);
    }
}
