using PDV.DAO.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDV.DAO.Entidades
{
    public class Caixa
    {
        [CampoTabela("IDCaixa")]
        [MaxLength(18)]
        public decimal IDCaixa { get; set; } = -1;

        [CampoTabela("NumeroCaixa")]
        [MaxLength(50)]
        public string NumeroCaixa { get; set; }

        [CampoTabela("SerialPOS")]
        [MaxLength(50)]
        public string SerialPOS { get; set; }

        [CampoTabela("NomePOS")]
        [MaxLength(50)]
        public string NomePOS { get; set; }


        [CampoTabela("Ativo")]
        public bool Ativo { get; set; }

        [CampoTabela("TipoDeVenda")]
        public String TipoDeVenda { get; set; }

        public int TipoPDV { get; set; }

        public static readonly int Mercado = 1;

        public static readonly int Restaurante = 2;
    }
}
