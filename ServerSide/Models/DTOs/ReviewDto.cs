
namespace ServerSide.Models.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int MovieId { get; set; }

        public int OrderId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string MemberName { get; set; }
        public string MovieTitle { get; set; }
    }
}
