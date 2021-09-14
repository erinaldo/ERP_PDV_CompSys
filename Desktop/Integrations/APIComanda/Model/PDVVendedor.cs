namespace APIComanda.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDVVendedor")]
    public partial class PDVVendedor
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public int? IDVendedor { get; set; }
    }
}
