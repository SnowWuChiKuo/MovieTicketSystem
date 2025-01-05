namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Movie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movie()
        {
            Prices = new HashSet<Price>();
            Reviews = new HashSet<Review>();
            Screenings = new HashSet<Screening>();
        }

        public int Id { get; set; }

        public int GenreId { get; set; }

        public int RatingId { get; set; }

        [Required]
        [StringLength(70)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Director { get; set; }

        [Required]
        [StringLength(1000)]
        public string Cast { get; set; }

        public int? RunTime { get; set; }

        public DateTime ReleaseDate { get; set; }

        [StringLength(70)]
        public string PosterURL { get; set; }

        [StringLength(70)]
        public string TrailerURL { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? Updated { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Rating Rating { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Price> Prices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Screening> Screenings { get; set; }
    }
}
