using System;

namespace PDV.DAO.Entidades
{
    public class SangriaCaixa
    {
        public decimal IDSangriaCaixa { get; set; }
        public decimal IDUsuario { get; set; }
        public decimal IDUsuarioCadastro { get; set; }
        public decimal IDFluxoCaixa { get; set; }
        public DateTime? DataSangria { get; set; }
        public decimal Valor { get; set; }

        public string Observacao { get; set; }

        public SangriaCaixa() { }
    }
}
