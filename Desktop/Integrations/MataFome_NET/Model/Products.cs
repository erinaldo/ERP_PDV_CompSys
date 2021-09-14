namespace MataFome_NET.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            Categories = new HashSet<Categories>();
        }

        public int ID { get; set; }

        [StringLength(20)]
        public string ExternalID { get; set; }

        [StringLength(60)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        [StringLength(300)]
        public string ImageLink { get; set; }

        public bool Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categories> Categories { get; set; }
    }
}
