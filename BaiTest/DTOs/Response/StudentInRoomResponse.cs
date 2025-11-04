namespace BaiTest.DTOs.Response
{
    public class StudentInRoomResponse
    {
        public ExamRoomResponse room { get; set; }
        public List<StudentResponse> studentList { get; set; }
    }
}
