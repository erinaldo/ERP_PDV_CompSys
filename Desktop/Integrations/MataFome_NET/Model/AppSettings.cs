namespace MataFome_NET.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppSettings
    {
        public int ID { get; set; }

        [StringLength(30)]
        public string Company { get; set; }

        [StringLength(20)]
        public string Document { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(10)]
        public string Number { get; set; }

        [StringLength(50)]
        public string Complement { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(15)]
        public string Domain { get; set; }

        [StringLength(15)]
        public string Telephone { get; set; }

        [StringLength(15)]
        public string CellPhone { get; set; }

        [StringLength(60)]
        public string Email { get; set; }

        [StringLength(300)]
        public string LogoLink { get; set; }

        public int OrderLimit { get; set; }

        public decimal RateMin { get; set; }
    }
}
