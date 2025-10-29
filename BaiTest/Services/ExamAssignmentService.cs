using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services
{
    public class ExamAssignmentService
    {
        static List<ExamAssignments>  AssignmentList { get; set; }
        static int nextId = 3;
        static ExamAssignmentService() 
        {
            AssignmentList = new List<ExamAssignments>
            {
                new ExamAssignments(1, 1, 1, DateTime.Now),
                new ExamAssignments(2, 2, 1, DateTime.Now),
            };
        }

        public static List<ExamAssignments> GetAll => AssignmentList;

        public static ExamAssignments GetExamByRoomId(int id)
            => AssignmentList.FirstOrDefault(a => a.examRoomsId == id);

        //public static Student GetStudentFromRoom(int studentId, int roomId)
        //{
        //    var student = StudentService.Get(studentId)
        //}

        private static ExamAssignments GetExamByStudentAndRoomIds(int studentId, int roomId)
            => AssignmentList.FirstOrDefault(s => s.studentId == studentId && s.examRoomsId == roomId);

        public static List<Student> GetListStudentInRoom(int roomId)
        {
            var studentIdInRoom = AssignmentList
                .Where(a => a.examRoomsId == roomId)
                .Select(a => a.studentId)
                .ToList();

            List<Student> studentList = StudentService.GetAll
                .Where(s => studentIdInRoom.Contains(s.Id))
                .ToList();

            return studentList;
        }

        public static ExamAssignments Add(ExamAssignmentRequest request)
        {
            var s = StudentService.Get(request.StudentId);
            var r = ExamRoomsService.Get(request.RoomId);

            if (s == null || r == null)
            {
                return null;
            } 

            if(AssignmentList.Any(a => a.studentId == s.Id) == true)
            {
                return null;
            }

            int studentInRoom = AssignmentList.Count(a => a.examRoomsId == request.RoomId);
            if(studentInRoom > r.Capacity)
            {
                return null;
            }
            var newAssignment = new ExamAssignments
            {
                Id = nextId++,
                studentId = request.StudentId,
                examRoomsId = request.RoomId,
                AssignedDate = DateTime.Now
            };

            AssignmentList.Add(newAssignment);
            return newAssignment;
        }

        public static void DeleteStudentFromRoom(ExamAssignmentRequest request)
        {
            var s = StudentService.Get(request.StudentId);
            var r = ExamRoomsService.Get(request.RoomId);

            if (s == null || r == null) return;
            //Kiem tra xem sinh vien co trong danh sach thi khong
            if (AssignmentList.Any(a => a.examRoomsId == request.RoomId) == false) return;
            else if (AssignmentList.Any(a => a.studentId == request.StudentId) == false
                && AssignmentList.Any(a => a.examRoomsId == request.RoomId) == true) return;
            else
            {
                var delete = GetExamByStudentAndRoomIds(request.StudentId, request.RoomId);
                if (delete == null) return;
                AssignmentList.Remove(delete);
            }

        }
    }
}
