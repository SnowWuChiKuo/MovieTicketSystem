namespace ServerSide.Models.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int TicketId { get; set; }

        public string TicketName { get; set; }

        public int Price { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }
    }
}
