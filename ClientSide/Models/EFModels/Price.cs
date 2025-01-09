namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Price
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        [Required]
        [StringLength(50)]
        public string SalesType { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketType { get; set; }

        public int ReservedSeats { get; set; }

        [Column("Price")]
        public int Price1 { get; set; }

        public virtual Movy Movy { get; set; }
    }
}
