using PDV.DAO.Atributos;
using System;

namespace PDV.DAO.Entidades
{
    public class OrdemDeServico
    {
        [CampoTabela(nameof(IDOrdemDeServico))]
        public decimal IDOrdemDeServico { get; set; }
    
        [CampoTabela(nameof(IDCliente))]
        public decimal IDCliente { get; set; }
    
        [CampoTabela(nameof(ValorTotal))]
        public decimal ValorTotal { get; set; }
    
        [CampoTabela(nameof(DataCadastro))]
        public DateTime DataCadastro { get; set; }
    
        [CampoTabela(nameof(Dinheiro))]
        public decimal Dinheiro { get; set; }
    
        [CampoTabela(nameof(Troco))]
        public decimal Troco { get; set; }
    
        [CampoTabela(nameof(Status))]
        public int Status { get; set; }
    
        [CampoTabela(nameof(Observacao))]
        public string Observacao { get; set; }
    
        [CampoTabela(nameof(IDVendedor))]
        public decimal IDVendedor { get; set; }
    
        [CampoTabela(nameof(IDTipoDeOperacao))]
        public decimal IDTipoDeOperacao { get; set; }
    
        [CampoTabela(nameof(MotivoDeCancelamento))]
        public string MotivoDeCancelamento { get; set; }
    
        [CampoTabela(nameof(DataFaturamento))]
        public DateTime? DataFaturamento { get; set; }
    
        [CampoTabela(nameof(IDRomaneio))]
        public decimal IDRomaneio { get; set; }
    
        [CampoTabela(nameof(Romaneio))]
        public bool Romaneio { get; set; }
    }
}
