namespace ServerSide.Models.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int RatingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public int? RunTime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Updated { get; set; }
    }
}
