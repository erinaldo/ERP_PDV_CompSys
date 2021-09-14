namespace MataFome_NET.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("District")]
    public partial class District
    {
        public int ID { get; set; }

        public decimal Rate { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
    }
}
