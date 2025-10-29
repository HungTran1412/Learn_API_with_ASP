namespace BaiTest.Models
{
    public class ExamRooms
    {
        public int Id { get; set; }
        public string RoomCode { get; set; }
        public int Capacity { get; set; }

        public ExamRooms()
        {
        }

        public ExamRooms(int id, string roomCode, int capacity)
        {
            Id = id;
            RoomCode = roomCode;
            Capacity = capacity;
        }
    }
}
