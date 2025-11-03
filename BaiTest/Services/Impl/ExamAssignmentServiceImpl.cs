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
            if (isStudentAlreadyAssigned == true)
            {
                Console.WriteLine("Sinh vien da co mat trong phong thi: " + isStudentAlreadyAssigned);
                return null;
            }

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

            //tao danh sach sinh vien
            var listStudent = exam.Where(e => e.Room != null && e.Student != null) //lay cac du lieu hop le
                .GroupBy(e => e.Room)//gom nhom theo phong thi
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

        public async Task<List<RoomSubjectResponse>> GetExamNameInRoomAsync()
        {
            //lay danh sach thi
            var exam = await db.ExamAssignments
                .Include(e => e.Room)
                .Include(e => e.Student)
                .ToListAsync();

            //lay danh sach mon hoc
            var listSubject = exam.Where(e => e.Room != null && e.Student != null)//lay du lieu hop le
                .GroupBy(e => e.Room) //gomm nhom theo phong
                .Select(group => new RoomSubjectResponse
                {
                    //Lay thong tin mon
                    RoomId = group.Key.Id,
                    RoomCode = group.Key.RoomCode,
                    Subject = group.Select(e => e.Student.Subject).Distinct().ToList()
                }).ToList();
            return listSubject;
        }

        public async Task<List<StudentResponse>> GetStudentUnassignmentAsync()
        {
            //lay sinh vien da duoc xem phong
            var assignedStudent = await db.ExamAssignments
                .Select(s => s.StudentId)
                .ToListAsync();

            //lay sinh vien chua duoc xep phong
            var unassignedStudent = await db.Students
                .Where(s => !assignedStudent.Contains(s.Id))
                .ToListAsync();

            var response = unassignedStudent.Select(s => new StudentResponse
            {
                Id = s.Id,
                StudentCode = s.StudentCode,
                Name = s.Name,
                Class = s.Class,
                Subject = s.Subject,
            }).ToList();

            return response;
        }

        public async Task<List<QuantityStatisticResponse>> StatisticQuantityRemainingAsync()
        {
            // Lay danh sach thong ke
            var statisticByAssignment = await db.ExamAssignments
                .Include(e => e.Room)                   //Lay du lieu tu bang khac
                .Where(e => e.Room != null)             //lay ca thi co gan phong thi
                .GroupBy(e => e.Room)                   //gom nhom theo phong
                .Select(group => new QuantityStatisticResponse
                {
                    RoomId = group.Key.Id,             
                    RoomCode = group.Key.RoomCode,       
                    Capacity = (int)group.Key.Capacity,   
                    CurrentStudent = group.Count(),       
                    QuantityRemaining = (int)(group.Key.Capacity - group.Count())
                })
                .ToListAsync();

            //Lay danh sach tat ca phong thi
            var allRooms = await db.ExamRooms.ToListAsync();

            var result = allRooms.Select(room =>
            {
                // Tim du lieu thong ke tuong ung
                var stat = statisticByAssignment.FirstOrDefault(s => s.RoomId == room.Id);

                // Gan so luong sinh vien co trong phong
                int currentStudents = stat != null ? stat.CurrentStudent : 0;

                // Lay ra suc chua cua phong
                int capacity = (int)room.Capacity;

                // Tao doi tuong ket qua thong ke
                return new QuantityStatisticResponse
                {
                    RoomId = room.Id,                     
                    RoomCode = room.RoomCode,             
                    Capacity = capacity,                  
                    CurrentStudent = currentStudents,    
                    QuantityRemaining = capacity - currentStudents 
                };
            }).ToList();

            return result;
        }


        public async Task<List<OverCapacityResponse>> GetOverCapacityRoomAsync()
        {
            //lay thong tin co mon thi
            var roomAssignments = await db.ExamAssignments
                .Include(e => e.Room) //kiem tra cac bang khac
                .Where(e => e.Room != null) //lay cac ban ghi co phong khong bi trong
                .GroupBy(e => e.RoomId) //gom nhom theo id phong
                .Select(group => new
                {
                    RoomId = group.Key,
                    CurrentStudents = group.Count() //dem so lan xuat hien de lay ra so luong sinh vien trong phong
                })
                .ToListAsync();

            //lay tat ca phong thi
            var allRooms = await examRoomsService.GetAllAsync();
            
            //lay ra phong co so luong bi vuot qua so luong cua phonog
            var overCapacityRooms = allRooms
                 .Select(room =>
                 {
                     //tim du lieu thong ke so sinh vien hien tai cua phong
                     var assignmentData = roomAssignments.FirstOrDefault(a => a.RoomId == room.Id);

                     //lay ra so luong hien tai cua phong 
                     int currentCount = assignmentData != null ? assignmentData.CurrentStudents : 0;
                     
                     //lay ra suc chua cua phong
                     int capacity = (int)room.Capacity;

                     //tao moi doi tuong tra ve
                     return new OverCapacityResponse
                     {
                         RoomId = room.Id,
                         RoomCode = room.RoomCode,
                         Capacity = capacity,
                         CurrentStudent = currentCount,
                         ExceedAmount = currentCount - capacity //tinh so luong vuot qua
                     };
                 })
                 .Where(response => response.ExceedAmount > 0) // chi lay nhung phong co so sinh vien > suc chua
                 .ToList();

            return overCapacityRooms;
        }

        public async Task<List<StudentResponse>> CheckDuplicateStudentAsync()
        {
            //lay ra cac ma sinh vien trung lap
            var duplicateStudentId = await db.ExamAssignments
                .Where(e => e.StudentId != null && e.RoomId != null) // lay cac du lieu co ma sinh vien va ma phong khong bi trung
                .GroupBy (e => e.StudentId) //gom nhom theo ma sinh vien
                .Where(gr =>  gr.Count() > 1) //chon du lieu co so luong > 1
                .Select(gr => gr.Key) // lay ra id cua cac sinh vien bi trung
                .ToListAsync();

            // lay thong tin  chi tiet cua cac sinh vien bi trung 
            var duplicateStudents = await db.Students
                .Where(s => duplicateStudentId.Contains(s.Id))
                .ToListAsync();
            
            //tao moi doi tuong tra ve
            var response = duplicateStudents.Select(s => new StudentResponse
            {
                Id = s.Id,
                StudentCode = s.StudentCode, 
                Name = s.Name,
                Class = s.Class,
                Subject = s.Subject,
            }).ToList();
            
            return response;
        }
    }
}
