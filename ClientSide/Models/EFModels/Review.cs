namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Review
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int MovieId { get; set; }

        public int Rating { get; set; }

        [StringLength(2000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Member Member { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
