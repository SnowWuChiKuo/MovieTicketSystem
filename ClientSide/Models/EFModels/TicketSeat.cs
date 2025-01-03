namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketSeat
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public int SeatId { get; set; }

        public virtual Seat Seat { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
