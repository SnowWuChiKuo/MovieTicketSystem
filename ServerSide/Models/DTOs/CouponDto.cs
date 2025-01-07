namespace ServerSide.Models.DTOs
{
    public class CouponDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string DiscountType { get; set; }

        public int DiscountValue { get; set; }

        public DateOnly ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
