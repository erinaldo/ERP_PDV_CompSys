using PDV.DAO.Atributos;

namespace PDV.DAO.Entidades.Estoque.Suprimentos
{
    public class ConversaoUnidadeDeMedida
    {

        [CampoTabela("IDCONVERSAOUNIDADEDEMEDIDA")]
        public decimal IDConversaoUnidadeDeMedida { get; set; } = -1;

        [CampoTabela("IDPRODUTO")]
        public decimal IDProduto { get; set; } = -1;

        [CampoTabela("IDUNIDADEDEMEDIDAENTRADA")]
        public decimal IDUnidadeDeMedidaEntrada { get; set; } = -1;

        [CampoTabela("IDUNIDADEDEMEDIDASAIDA")]
        public decimal IDUnidadeDeMedidaSaida { get; set; } = -1;

        [CampoTabela("FATOR")]
        public decimal Fator { get; set; } = 1;

        [CampoTabela("UNENTRADA")]
        public string UNENTRADA { get; set; }

        [CampoTabela("UNSAIDA")]
        public string UNSAIDA { get; set; }

        public string Descricao { get { return $"Entrada: {UNENTRADA}, Saida: {UNSAIDA} - Fator: {Fator}"; } }

        public ConversaoUnidadeDeMedida() { }
    }
}
