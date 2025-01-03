namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Seat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seat()
        {
            SeatStatus = new HashSet<SeatStatu>();
            TicketSeats = new HashSet<TicketSeat>();
        }

        public int Id { get; set; }

        public int TheaterId { get; set; }

        [Required]
        [StringLength(1)]
        public string Row { get; set; }

        public int Number { get; set; }

        public bool IsDisabled { get; set; }

        public virtual Theater Theater { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SeatStatu> SeatStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TicketSeat> TicketSeats { get; set; }
    }
}
