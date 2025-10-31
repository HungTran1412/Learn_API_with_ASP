using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BaiTest.Services.Impl
{
    public class StudentServiceImpl : IStudentService
    {
        private DatabaseContext db;

        public StudentServiceImpl(DatabaseContext db)
        {
            this.db = db;
        }

        public async Task<List<StudentResponse>> GetAllAsync()
        {
            var studentList = await db.Students.ToListAsync();

            var response = studentList.Select(s => new StudentResponse
            {
                Id = s.Id,
                StudentCode = s.StudentCode,
                Name = s.Name,
                Class = s.Class,
                Subject = s.Subject,
            }).ToList();

            return response;
        }

        public async Task<StudentResponse?> GetByStudentCodeAsync(string studentCode)
        {
            var student = await db.Students.SingleOrDefaultAsync(s => s.StudentCode == studentCode);

            if (student == null) return null;

            return new StudentResponse
            {
                Id = student.Id,
                StudentCode = student.StudentCode,
                Name = student.Name,
                Class = student.Class,
                Subject = student.Subject,
            };
        }

        public async Task<Student?> AddAsync(StudentRequest request)
        {
            //kiem tra xem ma sinh vien co ton tai khong
            var student = await db.Students.SingleOrDefaultAsync(s => s.StudentCode == request.StudentCode);
            if (student != null)
            {
                return null;
            }
            //tao moi doi tuong
            var newStudent = new Student
            {
                StudentCode = request.StudentCode,
                Name = request.Name,
                Class = request.Class,
                Subject = request.Subject
            };

            await db.Students.AddAsync(newStudent);
            //luu vao db
            await db.SaveChangesAsync();

            return newStudent;
        }

        public async Task<StudentResponse?> UpdateAsync(String studentCode, StudentUpdateRequest request)
        {
            //Kiểm tra xem sinh viên có tồn tại không
            var student = await db.Students.SingleOrDefaultAsync(s => s.StudentCode == studentCode);

            //Trả về null nếu không tìm thấy
            if (student == null) return null;

            student.Name = request.Name;
            student.Class = request.Class;
            student.Subject = request.Subject;


            await db.SaveChangesAsync();
            return new StudentResponse
            {
                Id = student.Id,
                StudentCode = student.StudentCode,
                Name = student.Name,
                Class = student.Class,
                Subject = student.Subject,
            };
        }

        public async Task DeleteAsync(string studentCode)
        {
            var student = await db.Students.SingleOrDefaultAsync(s => s.StudentCode == studentCode);
            if(student == null) return;
            db.Students.Remove(student);
            await db.SaveChangesAsync();
        }
    }
}
