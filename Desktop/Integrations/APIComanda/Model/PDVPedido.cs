namespace APIComanda.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDVPedido")]
    public partial class PDVPedido
    {
        public int ID { get; set; }

        public DateTime? Data { get; set; }

        public int? IDVendedor { get; set; }

        public string VendedorNome { get; set; }

        public string ClienteNome { get; set; }

        public int? IDMesa { get; set; }

        public decimal? TotalProduto { get; set; }

        public decimal? TotalPedido { get; set; }

        public string Observacao { get; set; }

        public int? IDPedido { get; set; }
    }
}
