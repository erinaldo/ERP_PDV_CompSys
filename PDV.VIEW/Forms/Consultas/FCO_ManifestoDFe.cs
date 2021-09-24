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

namespace PDV.VIEW.Forms.Consultas
{
    public partial class FCO_ManifestoDFe : Form
    {
        public FCO_ManifestoDFe()
        {
            InitializeComponent();
            AtualizaTiposdeOperacao();
        }
        private void AtualizaTiposdeOperacao()
        {
            gridControl1.DataSource = FuncoesManifestacaoDestinatario.GetManisfestacaoDestinatario();
            AjustaHeaderTextGrid();
        }
        private void AjustaHeaderTextGrid()
        {
            Grids.FormatColumnType(ref gridView1, new List<string>()
            {
                "tipoambiente"
            }, GridFormats.VisibleFalse);

            Grids.FormatGrid(ref gridView1);
        }

        private void btnSearchChaveNFe_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = FuncoesManifestacaoDestinatario.GetManifestoPorChaveDataTable(textSearchChaveNFe.Text);
            AjustaHeaderTextGrid();
        }
    }
}
