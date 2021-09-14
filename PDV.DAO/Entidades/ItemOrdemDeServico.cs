using PDV.DAO.Atributos;

namespace PDV.DAO.Entidades
{
    public class ItemOrdemDeServico
    {
        [CampoTabela(nameof(IDItemOrdemDeServico))]
        public decimal IDItemOrdemDeServico { get; set; }

        [CampoTabela(nameof(IDServico))]
        public decimal IDServico { get; set; }

        [CampoTabela(nameof(IDOrdemDeServico))]
        public decimal IDOrdemDeServico { get; set; }

        [CampoTabela(nameof(Descricao))]
        public string Descricao { get; set; }

        [CampoTabela(nameof(Quantidade))]
        public decimal Quantidade { get; set; }

        [CampoTabela(nameof(DescontoPorcentagem))]
        public decimal DescontoPorcentagem { get; set; }

        [CampoTabela(nameof(DescontoValor))]
        public decimal DescontoValor { get; set; }

        [CampoTabela(nameof(ValorUnitarioItem))]
        public decimal ValorUnitarioItem { get; set; }        

        [CampoTabela(nameof(SubTotal))]
        public decimal SubTotal { get; set; }
    }
}
