using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BaiTest.Services.Impl
{
    public class ExamAssignmentServiceImpl : IExamAssignmentService
    {
        private DatabaseContext db;
        private IStudentService studentService;
        private IExamRoomsService examRoomsService;

        public ExamAssignmentServiceImpl(DatabaseContext db, IStudentService studentService, IExamRoomsService examRoomsService)
        {
            this.db = db;
            this.studentService = studentService;
            this.examRoomsService = examRoomsService;
        }

        public async Task<ExamAssignment?> ExamScheduleAsync(ExamScheduleRequest request)
        {
            //Check xem sinh vien co ton tai khoong
            var student = await studentService.GetByStudentCodeAsync(request.StudentCode);
            if (student == null) return null;
            Console.WriteLine("Student: " + student.Id);
            
            //check xem phong thi co ton tai khong
            var room = await examRoomsService.GetByCodeAsync(request.RoomCode);
            if (room == null) return null;
            Console.WriteLine("Room: " + room.Id);

            //check xem phong da co sinh vien chua
            var isStudentAlreadyAssigned = await db.ExamAssignments.AnyAsync(e => e.StudentId == student.Id);
            if(isStudentAlreadyAssigned == true) return null;
            Console.WriteLine("Sinh vien da co mat trong phong thi: " +  isStudentAlreadyAssigned);


            //check xem so luong trong phong con lai la bao nhieuv
            int count = await db.ExamAssignments.CountAsync(e => e.RoomId == room.Id);
            if (count >= room.Capacity) return null;
            Console.WriteLine("So luong da co trong phong: " + count);

            DateTime assignedDate;
            try
            {
                assignedDate = System.Text.Json.JsonSerializer.Deserialize<DateTime>(request.AssignedDate);
            }
            catch
            {
                // Xử lý lỗi nếu request.AssignedDate không phải là chuỗi ngày tháng JSON hợp lệ
                return null;
            }

            //Tao moi
            var addExam = new ExamAssignment
            {
                StudentId = student.Id,
                RoomId = room.Id,
                AssignedDate = assignedDate
            };
            Console.WriteLine("Them moi: " + addExam);
            await db.ExamAssignments.AddAsync(addExam);

            //luu vao database
            await db.SaveChangesAsync();

            return addExam;
        }

        public async Task<List<StudentInRoomResponse>> GetListStudentByRoomAsync()
        {
            var exam = await db.ExamAssignments
                .Include(e => e.Room)//lay thong tin phong thi
                .Include(e => e.Student) //lay thong tin sinh vien
                .ToListAsync();

            var listStudent = exam.Where(e => e.Room != null && e.Student != null)
                .GroupBy(e => e.Room)
                .Select(group => new StudentInRoomResponse
                {
                    //lay thong tin phong
                    room = new ExamRoomResponse
                    {
                        Id = group.Key.Id,
                        RoomCode = group.Key.RoomCode,
                        Capacity = group.Key.Capacity,
                    },
                    studentList = (List<StudentResponse>)group.Select(e => new StudentResponse
                    {
                        Id = e.Student.Id,
                        StudentCode = e.Student.StudentCode,
                        Name = e.Student.Name,
                        Class = e.Student.Class,
                        Subject = e.Student.Subject,
                    }).ToList()
                }).ToList();
            return listStudent;
        }

        public Task<QuantityRemainingResponse> StatisticQuantityRemaining()
        {
            throw new NotImplementedException();
        }
    }
}
