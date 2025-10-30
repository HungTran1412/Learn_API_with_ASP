using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IStudentService
    {
        List<Student> GetAll();
        Student Get(int id);
        Student Add(StudentRequest request);
        Student Update(int id, StudentRequest request);
        void Delete(int id);
    }
}
