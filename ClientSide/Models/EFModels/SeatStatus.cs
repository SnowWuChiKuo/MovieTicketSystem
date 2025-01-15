namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SeatStatus
    {
        public int Id { get; set; }

        public int ScreeningId { get; set; }

        public int SeatId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Screening Screening { get; set; }

        public virtual Seat Seat { get; set; }
    }
}
