using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.DTOs
{
    public class PriceDto
    {

        public int Id { get; set; }
        public int MovieId { get; set; }

        public string SalesType { get; set; }

        public string TicketType { get; set; }

        public int ReservedSeats { get; set; }

        public int Price1 { get; set; }
    }
}
