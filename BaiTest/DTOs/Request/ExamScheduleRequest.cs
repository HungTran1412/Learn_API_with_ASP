namespace BaiTest.DTOs.Request
{
    public class ExamScheduleRequest
    {
        public string? StudentCode { get; set; }
        public string? RoomCode { get; set; }
        public DateTime? AssignedDate { get; set; }
    }
}
