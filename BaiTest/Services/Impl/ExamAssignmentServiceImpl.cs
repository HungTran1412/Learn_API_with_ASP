using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services.Impl
{
    public class ExamAssignmentServiceImpl : IExamAssignmentService
    {
        StudentServiceImpl studentService;
        ExamRoomsServiceImpl examRoomsService;
        static List<ExamAssignments> AssignmentList { get; set; }
        int nextId = 3;

        public ExamAssignmentServiceImpl()
        {
            studentService = new StudentServiceImpl();
            examRoomsService = new ExamRoomsServiceImpl();
            AssignmentList = new List<ExamAssignments>
            {
                new ExamAssignments(1, 1, 1, DateTime.Now),
                new ExamAssignments(2, 2, 1, DateTime.Now),
            };
        }

        public List<ExamAssignments> GetAll() => AssignmentList;

        public ExamAssignments GetExamByRoomId(int id)
            => AssignmentList.FirstOrDefault(a => a.examRoomsId == id);

        //public Student GetStudentFromRoom(int studentId, int roomId)
        //{
        //    var student = StudentService.Get(studentId)
        //}

        private ExamAssignments GetExamByStudentAndRoomIds(int studentId, int roomId)
            => AssignmentList.FirstOrDefault(s => s.studentId == studentId && s.examRoomsId == roomId);

        public List<Student> GetListStudentInRoom(int roomId)
        {
            var studentIdInRoom = AssignmentList
                .Where(a => a.examRoomsId == roomId)
                .Select(a => a.studentId)
                .ToList();

            List<Student> studentList = studentService.GetAll()
                .Where(s => studentIdInRoom.Contains(s.Id))
                .ToList();

            return studentList;
        }

        public ExamAssignments Add(ExamAssignmentRequest request)
        {
            var s = studentService.Get(request.StudentId);
            var r = examRoomsService.Get(request.RoomId);

            if (s == null || r == null)
            {
                return null;
            }

            if (AssignmentList.Any(a => a.studentId == s.Id) == true)
            {
                return null;
            }

            int studentInRoom = AssignmentList.Count(a => a.examRoomsId == request.RoomId);
            if (studentInRoom > r.Capacity)
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

        public bool DeleteStudentFromRoom(ExamAssignmentRequest request)
        {
            var delete = GetExamByStudentAndRoomIds(request.StudentId, request.RoomId);

            if (delete == null) return false;

            AssignmentList.Remove(delete);
            return true;
        }
    }
}
