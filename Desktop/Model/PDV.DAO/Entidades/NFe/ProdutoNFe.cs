using PDV.DAO.Atributos;

namespace PDV.DAO.Entidades.NFe
{
    public class ProdutoNFe
    {
        [CampoTabela("IDPRODUTONFE")]
        public decimal IDProdutoNFe { get; set; } = -1;

        [CampoTabela("SEQUENCIA")]
        public int Sequencia { get; set; }

        [CampoTabela("FRETE")]
        public decimal Frete { get; set; }

        [CampoTabela("OUTRASDESPESAS")]
        public decimal OutrasDespesas { get; set; }

        [CampoTabela("QUANTIDADE")]
        public decimal Quantidade { get; set; }

        [CampoTabela("VALORUNITARIO")]
        public decimal ValorUnitario { get; set; }

        [CampoTabela("DESCONTO")]
        public decimal Desconto { get; set; }

        [CampoTabela("IDPRODUTO")]
        public decimal IDProduto { get; set; }

        [CampoTabela("IDCFOP")]
        public decimal IDCFOP { get; set; }

        [CampoTabela("IDNFE")]
        public decimal IDNFe { get; set; }

        [CampoTabela("SEGURO")]
        public decimal Seguro { get; set; }

        [CampoTabela("IDINTEGRACAOFISCAL")]
        public decimal IDIntegracaoFiscal { get; set; }

        public decimal ValorTotal { get; set; }

        public decimal TotalFinanceiro { get; set; }

        [CampoTabela("IDUNIDADEDEMEDIDA")]
        public decimal IDUnidadeDeMedida { get; set; }


        public ProdutoNFe() { }
    }
}
