namespace BaiTest.DTOs.Request
{
    public class ExamAssignmentRequest
    {
        public int? StudentId { get; set; }

        public int? RoomId { get; set; }

        public DateTime? AssignedDate { get; set; }
    }
}
