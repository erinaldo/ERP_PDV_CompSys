namespace APIComanda.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDVMesas")]
    public partial class PDVMesa
    {
        public int ID { get; set; }

        public string Numero { get; set; }

        public string NomeCliente { get; set; }

        public int Status { get; set; }
    }
}
