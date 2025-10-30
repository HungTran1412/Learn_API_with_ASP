using BaiTest.DTOs;
using BaiTest.Models;

namespace BaiTest.Services.Impl
{
    public class ExamRoomsServiceImpl : IExamRoomsService
    {
        static List<ExamRooms> ListRoom { get; set; }
        int nextId = 3;
        public ExamRoomsServiceImpl()
        {
            ListRoom = new List<ExamRooms>
            {
                new ExamRooms(1, "P1", 5),
                new ExamRooms(2, "P2", 10),
            };
        }

        public List<ExamRooms> GetAll() => ListRoom;

        public ExamRooms Get(int id) => ListRoom.FirstOrDefault(r => r.Id == id);

        public ExamRooms Add(ExamRoomsRequest e)
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

        public ExamRooms Update(int id, ExamRoomsRequest request)
        {
            var index = ListRoom.FindIndex(r => r.Id == id);
            if (index < 0)
                return null;

            var updated = ListRoom[index];
            updated.Capacity = request.Capacity;
            updated.RoomCode = request.RoomCode;

            return updated;
        }

        public void Remove(int id)
        {
            var del = Get(id);
            if (del is null)
                return;
            ListRoom.Remove(del);
        }

        public ExamRooms Update(ExamRoomsRequest request)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
