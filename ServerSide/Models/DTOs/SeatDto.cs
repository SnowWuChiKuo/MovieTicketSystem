namespace ServerSide.Models.DTOs
{
    public class SeatDto
    {
        public int Id { get; set; }

        public int TheaterId { get; set; }

        public string Row { get; set; }

        public string Number { get; set; }

        public bool IsDisabled { get; set; }
    }
}
