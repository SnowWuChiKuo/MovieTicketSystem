using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int TicketId { get; set; }

        public string TicketName { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
