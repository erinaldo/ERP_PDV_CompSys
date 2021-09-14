namespace APIComanda.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDVPedidoItem")]
    public partial class PDVPedidoItem
    {
        public int ID { get; set; }

        public int? IDPedido { get; set; }

        public int? IDProduto { get; set; }

        public string ProdutoNome { get; set; }

        public decimal? Quantidade { get; set; }

        public int? IDUnidade { get; set; }

        public string UnidadeNome { get; set; }

        public decimal? Valor { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? Total { get; set; }

        public bool? Importado { get; set; }

        public int? IDVendedor { get; set; }
    }
}
