namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int TicketId { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        [StringLength(20)]
        public string SeatsName { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
