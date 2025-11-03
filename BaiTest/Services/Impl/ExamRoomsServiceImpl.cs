using BaiTest.DTOs;
using BaiTest.DTOs.Request;
using BaiTest.DTOs.Response;
using BaiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BaiTest.Services.Impl
{
    public class ExamRoomsServiceImpl : IExamRoomsService
    {
        private DatabaseContext db;

        public ExamRoomsServiceImpl(DatabaseContext db)
        {
            this.db = db;
        }

        public async Task<ExamRoom?> AddAsync(ExamRoomRequest request)
        {
            //Kiem tra xem phong da ton tai chua
            var room = await db.ExamRooms.SingleOrDefaultAsync(r => r.RoomCode == request.RoomCode);
            if (room != null) return null;
            if (request.Capacity <= 0) return null;
            //Tao moi doi tuong 
            var newRoom = new ExamRoom
            {
                RoomCode = request.RoomCode,
                Capacity = request.Capacity,
            };

            //luu vao db
            await db.ExamRooms.AddAsync(newRoom);
            await db.SaveChangesAsync();

            return room;
        }

        public async Task DeleteAsync(string roomCode)
        {
            //tim phong 
            var delete = await db.ExamRooms.SingleOrDefaultAsync(r => r.RoomCode == roomCode);
            
            //tra ve null neu khong tim thay
            if (delete == null) return;

            //luu vao db
            db.ExamRooms.Remove(delete);
            await db.SaveChangesAsync();
        }

        public async Task<List<ExamRoomResponse>> GetAllAsync()
        {
            //lay danh sach phong thi
            var roomList = await db.ExamRooms.ToListAsync();

            //tao moi doi tuong tra ve
            var response = roomList.Select(r => new ExamRoomResponse
            {
                Id = r.Id,
                RoomCode = r.RoomCode,
                Capacity = r.Capacity,
            }).ToList();

            return response;
        }

        public async Task<ExamRoomResponse> GetByCodeAsync(string roomCode)
        {
            //tim phong bang ma
            var room = await db.ExamRooms.SingleOrDefaultAsync(r => r.RoomCode == roomCode);
            if (room == null) return null;//tra ve null neu khong tim thay
            //tra ve doi tuong chua thong tin
            return new ExamRoomResponse
            {
                Id= room.Id,
                RoomCode = room.RoomCode,
                Capacity = room.Capacity,
            };
        }

        public async Task<ExamRoomResponse?> UpdateAsync(string roomCode, ExamRoomUpdateRequest request)
        {
            //Kiểm tra xem phòng đã tồn tại chưa
            var room = await db.ExamRooms.SingleOrDefaultAsync(r => r.RoomCode == roomCode);
            if(room == null) return null;

            //gan du lieu moi vao
            if (request.Capacity <= 0) return null;
            room.Capacity = request.Capacity;

            //luu vao db
            await db.SaveChangesAsync();
            //tra ve du lieu
            return new ExamRoomResponse
            {
                Id = room.Id,
                RoomCode = room.RoomCode,
                Capacity = room.Capacity,
            };
        }
    }
}
