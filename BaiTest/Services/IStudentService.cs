using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;

namespace BaiTest.Services
{
    public interface IStudentService
    {
        //Lay danh sach sinh vien
        Task<List<StudentResponse>> GetAllAsync();
        //lay thong tin sinh vien bang ma sinh vien
        Task<StudentResponse?> GetByStudentCodeAsync(string studentCode);
        //them sinh vien
        Task<Student?> AddAsync(StudentRequest request);
        //Cap nhat thong tin sinh vien
        Task<StudentResponse?> UpdateAsync(string studentCode, StudentUpdateRequest request);
        //Xoa sinh vien
        Task DeleteAsync(string studentCode);
    }
}
