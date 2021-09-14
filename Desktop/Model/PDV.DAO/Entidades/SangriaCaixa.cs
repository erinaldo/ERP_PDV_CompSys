using System;

namespace PDV.DAO.Entidades
{
    public class SuprimentoCaixa
    {
        public decimal IDSuprimentocaixa { get; set; }
        public decimal IDUsuario { get; set; }
        public decimal IDUsuarioCadastro { get; set; }
        public decimal IDFluxoCaixa { get; set; }
        public DateTime? DataSuprimentocaixa { get; set; }
        public decimal Valor { get; set; }

        public string Observacao { get; set; }

        public SuprimentoCaixa() { }
    }
}
