namespace BaiTest.Models
{
    public class ExamAssignments
    {
        public int Id { get; set; }
        public int studentId { get; set; }
        public int examRoomsId { get; set; }
        public DateTime AssignedDate { get; set; }

        public ExamAssignments()
        {
        }

        public ExamAssignments(int id, int studentId, int examRoomsId, DateTime assignedDate)
        {
            Id = id;
            this.studentId = studentId;
            this.examRoomsId = examRoomsId;
            AssignedDate = assignedDate;
        }
    }
}
