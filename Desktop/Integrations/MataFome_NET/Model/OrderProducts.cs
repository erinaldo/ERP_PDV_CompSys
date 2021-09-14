namespace MataFome_NET.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderProducts
    {
        public long ID { get; set; }

        public long OrderID { get; set; }

        public int? ProductID { get; set; }

        [StringLength(20)]
        public string ExternalID { get; set; }

        [StringLength(60)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        public int Amount { get; set; }

        public decimal TotalPrice { get; set; }

        [StringLength(240)]
        public string Observation { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
