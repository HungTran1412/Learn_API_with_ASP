using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IStudentService
    {
        Task<List<StudentResponse>> GetAllAsync();
        Task<StudentResponse?> GetByStudentCodeAsync(string studentCode);
        Task<Student?> AddAsync(StudentRequest request);
        Task<StudentResponse?> UpdateAsync(string studentCode, StudentUpdateRequest request);
        Task DeleteAsync(string studentCode);
    }
}
