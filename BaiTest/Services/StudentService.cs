using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services
{
    public static class StudentService
    {
        static List<Student> StudentList { get; set; }
        static int nextId = 3;
        static StudentService()
        {
            StudentList = new List<Student>
            {
                new Student(1, "001", "A", "OOP"),
                new Student(2, "002", "A", "OOP")
            };
        }

        public static List<Student> GetAll => StudentList;

        public static Student Get(int id) => StudentList.FirstOrDefault(s => s.Id == id);

        public static Student Add(StudentRequest s)
        {
            var newStudent = new Student
            {
                StudentCode = s.StudentCode,
                Class = s.Class,
                Subject = s.Subject,
            };

            newStudent.Id = nextId++;
            StudentList.Add(newStudent);

            return newStudent;
        }

        public static Student Update(int id, StudentRequest s)
        {
            var index = StudentList.FindIndex(s1 => s1.Id == id);
            if (index == -1)
                return null;

            var updatedStudent = StudentList[index];
            updatedStudent.StudentCode = s.StudentCode;
            updatedStudent.Class = s.Class;
            updatedStudent.Subject = s.Subject;

            return updatedStudent;
        }

        public static void Remove(int id)
        {
            var student = Get(id);
            if (student is null)
                return;
            StudentList.Remove(student);
        }
    }
}
