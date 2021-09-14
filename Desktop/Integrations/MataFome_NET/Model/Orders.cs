namespace MataFome_NET.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            OrderProducts = new HashSet<OrderProducts>();
        }

        public long ID { get; set; }

        public long? UserID { get; set; }

        public decimal OrderTotal { get; set; }

        public int OrderStatus { get; set; }

        [StringLength(60)]
        public string AddressStreet { get; set; }

        [StringLength(12)]
        public string AddressNumber { get; set; }

        [StringLength(40)]
        public string AddressCity { get; set; }

        public int AddressState { get; set; }

        [StringLength(12)]
        public string AddressPostalCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Updated { get; set; }

        [StringLength(60)]
        public string AddressCoordinates { get; set; }

        [StringLength(120)]
        public string CustomerName { get; set; }

        [StringLength(120)]
        public string CustomerEmail { get; set; }

        [StringLength(20)]
        public string CustomerPhone { get; set; }

        public int PaymentType { get; set; }

        [StringLength(30)]
        public string AddressDistrict { get; set; }

        [StringLength(60)]
        public string AddressReference { get; set; }

        public decimal? DeliveryTax { get; set; }

        [StringLength(240)]
        public string CancellationReason { get; set; }

        [StringLength(240)]
        public string Observation { get; set; }

        public decimal Change { get; set; }

        [StringLength(500)]
        public string SystemObservation { get; set; }

        [StringLength(14)]
        public string CustomerCPF { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProducts> OrderProducts { get; set; }
    }
}
