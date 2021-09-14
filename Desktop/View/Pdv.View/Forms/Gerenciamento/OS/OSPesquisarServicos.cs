using PDV.CONTROLER.Funcoes;
using PDV.VIEW.Forms.Util;
using System;

namespace PDV.VIEW.FRENTECAIXA.Forms.PDV.DAV
{
    public partial class OSPesquisarServicos : DevExpress.XtraEditors.XtraForm
    {
        public decimal IDServico { get; set; }
        public OSPesquisarServicos()
        {
            InitializeComponent();
            CarregarProdutos();
           
        }

        private void CarregarProdutos()
        {
            gridControl1.DataSource = FuncoesServico.GetServicosGridView();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Escolher();
        }

        private void Escolher()
        {
            IDServico = Grids.GetValorDec(gridView1, "IDServico");
            Close();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            Escolher();
        }
    }
}
