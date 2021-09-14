using DevExpress.Office.Utils;
using PDV.CONTROLER.Funcoes;
using PDV.VIEW.Forms.Util;
using System;
using System.Collections.Generic;

namespace PDV.VIEW.Forms.Gerenciamento.OS
{
    public partial class OSPesquisarVendedores : DevExpress.XtraEditors.XtraForm
    {
        public decimal Codigo { get; set; }

        public OSPesquisarVendedores()
        {
            InitializeComponent();
            gridControl1.DataSource = FuncoesUsuario.GetUsuariosVendedores();
            FormatarGrids();
        }

        private void FormatarGrids()
        {
            Grids.FormatGrid(ref gridView1);
            Grids.FormatColumnType(ref gridView1, new List<string>
            {
                "Login",
                "Senha",
                "Ativo",
                "Email",
                "IDPerfilAcesso",
                "PerfilAcesso",
                "IDUsuarioSupervisor",
                "Root",
                "Pin",
                "FormaDesconto",
                "TipoDesconto",
                "IsVendedor",
                "IsComprador",
                "DescontoMaximo"
            }, GridFormats.VisibleFalse);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            Codigo = Grids.GetValorDec(gridView1, "IDUsuario");
            Close();
        }
    }
}
