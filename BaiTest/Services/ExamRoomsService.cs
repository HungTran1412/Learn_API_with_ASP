using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services
{
    public class ExamRoomsService
    {
        static List<ExamRooms> ListRoom { get; set; }
        static int nextId = 3;
        static ExamRoomsService()
        {
            ListRoom = new List<ExamRooms>
            {
                new ExamRooms(1, "P1", 5),
                new ExamRooms(2, "P2", 10),
            };
        }

        public static List<ExamRooms> GetAll => ListRoom;

        public static ExamRooms Get(int id) => ListRoom.FirstOrDefault(r => r.Id == id);

        public static ExamRooms Add(ExamRoomsRequest e)
        {
            var newRoom = new ExamRooms
            {
                RoomCode = e.RoomCode,
                Capacity = e.Capacity
            };

            newRoom.Id = nextId++;
            ListRoom.Add(newRoom);
            return newRoom;
        }

        public static ExamRooms Update(int id, ExamRoomsRequest request)
        {
            var index = ListRoom.FindIndex(r => r.Id == id);
            if (index < 0)
                return null;

            var updated = ListRoom[index];
            updated.Capacity = request.Capacity;
            updated.RoomCode = request.RoomCode;

            return updated;
        }

        public static void Remove(int id)
        {
            var del = Get(id);
            if (del is null)
                return;
            ListRoom.Remove(del);
        }
    }
}
