namespace APIComanda.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDVProdutoes")]
    public partial class PDVProduto
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public decimal? Preco { get; set; }

        public string UnidadeNome { get; set; }

        public int? UnidadeID { get; set; }

        public decimal? Estoque { get; set; }

        public int? ProdutoID { get; set; }

        public string Image { get; set; }
    }
}
