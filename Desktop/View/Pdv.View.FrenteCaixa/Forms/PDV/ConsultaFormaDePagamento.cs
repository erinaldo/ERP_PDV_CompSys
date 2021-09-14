using PDV.CONTROLER.Funcoes;
using PDV.VIEW.Forms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDV.VIEW.FRENTECAIXA.Forms.PDV
{
    public partial class ConsultaFormaDePagamento : Form
    {
        public ConsultaFormaDePagamento()
        {
            InitializeComponent();

            var formaPagamento = FuncoesFormaDePagamento.GetFormasPagamentoPDV().Select(s => new { Cod = s.Codigo, Nome = s.Descricao }).OrderBy(x => x.Cod).ToList();
            gridControl1.DataSource = formaPagamento;
            Grids.FormatColumnType(ref gridView1, "codigo", GridFormats.VisibleFalse);
            Grids.FormatGrid(ref gridView1);
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
