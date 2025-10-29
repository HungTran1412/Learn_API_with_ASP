using BaiTest.Models;
namespace BaiTest.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }

        public Student()
        {
        }

        public Student(int Id, string StudentCode, string Class, string Subject)
        {
            this.Id = Id;
            this.StudentCode = StudentCode;
            this.Class = Class;
            this.Subject = Subject;
        }
    }
}
