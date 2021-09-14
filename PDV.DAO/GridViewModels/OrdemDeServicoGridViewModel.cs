using PDV.DAO.Atributos;
using System;

namespace PDV.DAO.GridViewModels
{
    public class OrdemDeServicoGridViewModel
    {
        [CampoTabela(nameof(IDOrdemDeServico))]
        public decimal IDOrdemDeServico { get; set; }

        [CampoTabela(nameof(DataCadastro))]
        public DateTime DataCadastro { get; set; }

        [CampoTabela(nameof(DataFaturamento))]
        public DateTime? DataFaturamento { get; set; }

        [CampoTabela(nameof(ValorTotal))]
        public decimal ValorTotal { get; set; }

        [CampoTabela(nameof(Status))]
        public string Status { get; set; }

        [CampoTabela(nameof(Cliente))]
        public string Cliente { get; set; }
    }
}