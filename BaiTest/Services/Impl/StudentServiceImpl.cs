using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services.Impl
{
    public class StudentServiceImpl: IStudentService
    {
        static List<Student> StudentList { get; set; }
        int nextId = 3;
        public StudentServiceImpl()
        {
            StudentList = new List<Student>
            {
                new Student(1, "001", "A", "OOP"),
                new Student(2, "002", "A", "OOP")
            };
        }

        public List<Student> GetAll() => StudentList;

        public Student Get(int id) => StudentList.FirstOrDefault(s => s.Id == id);

        public Student Add(StudentRequest s)
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

        public Student Update(int id, StudentRequest s)
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

        public void Remove(int id)
        {
            var student = Get(id);
            if (student is null)
                return;
            StudentList.Remove(student);
        }

        List<Student> IStudentService.GetAll()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
