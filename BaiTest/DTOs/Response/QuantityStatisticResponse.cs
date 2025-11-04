namespace BaiTest.DTOs.Response
{
    public class QuantityStatisticResponse
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public int Capacity { get; set; }
        public int CurrentStudent {  get; set; }
        public int? QuantityRemaining { get; set; }
    }
}
