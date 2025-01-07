namespace ServerSide.Models.DTOs
{
    public class SeatStatusDto
    {
        public int Id { get; set; }

        public int ScreeningId { get; set; }

        public int SeatId { get; set; }

        public string Status { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
