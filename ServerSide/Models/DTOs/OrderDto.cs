namespace ServerSide.Models.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public string? MemberAccount { get; set; }

        public int? CouponId { get; set; }

        public int TotalAmount { get; set; }

        public bool Status { get; set; }

        public int? DiscountPrice { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
