using PDV.DAO.Atributos;
using System;

namespace PDV.DAO.Entidades
{
    public class DuplicataServico
    {
        [CampoTabela(nameof(IDDuplicataServico))]
        public decimal IDDuplicataServico { get; set; }

        [CampoTabela(nameof(Descricao))]
        public string Descricao { get; set; }

        [CampoTabela(nameof(DataVencimento))]
        public DateTime DataVencimento { get; set; }

        [CampoTabela(nameof(Valor))]
        public decimal Valor { get; set; }

        [CampoTabela(nameof(IDFormaDePagamento))]
        public decimal IDFormaDePagamento { get; set; }

        [CampoTabela(nameof(IDOrdemDeServico))]
        public decimal IDOrdemDeServico { get; set; }
    }
}
