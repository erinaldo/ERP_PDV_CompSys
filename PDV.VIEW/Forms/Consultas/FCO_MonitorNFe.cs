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
    public partial class FCO_MonitorNFe : Form
    {
        public FCO_MonitorNFe()
        {
            InitializeComponent();
            AtualizaTiposdeOperacao();
        }
        private void AtualizaTiposdeOperacao()
        {
            gridControl1.DataSource = FuncoesDownloadNFe.GetDownloadsNFe();
            AjustaHeaderTextGrid();
        }
        private void AjustaHeaderTextGrid()
        {
            Grids.FormatColumnType(ref gridView1, new List<string>()
            {
                "cstat",
                "tpamb"
            }, GridFormats.VisibleFalse);

            Grids.FormatGrid(ref gridView1);
        }
    }
}
