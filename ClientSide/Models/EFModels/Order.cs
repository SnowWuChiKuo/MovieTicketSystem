namespace ClientSide.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }

        public int MemberId { get; set; }

        public int? CouponId { get; set; }

        public int TotalAmount { get; set; }

        public bool Status { get; set; }

        public int? CouponAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Coupon Coupon { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
