namespace BaiTest.DTOs.Response
{
    public class ExamAssigmentResponse
    {
        public int Id { get; set; }

        public int? StudentId { get; set; }

        public int? RoomId { get; set; }

        public DateTime? AssignedDate { get; set; }
    }
}
