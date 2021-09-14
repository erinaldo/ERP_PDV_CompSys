using DFe.Utils;
using MetroFramework;
using MetroFramework.Forms;
using NFe.Danfe.Base.NFe;
using NFe.Danfe.Fast.NFe;
using PDV.CONTROLER.Funcoes;
using PDV.UTIL;
using PDV.VIEW.App_Context;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Estoque.Gerenciamento
{
    public partial class FEST_GerenciarNFeEntrada : DevExpress.XtraEditors.XtraForm
    {
        private string NOME_TELA = "GERENCIAR NF-E DE IMPORTAÇÃO";
        private DataTable DADOS = null;

        public FEST_GerenciarNFeEntrada()
        {
            InitializeComponent();
            ovTXT_EmissaoInicio.Value = ovTXT_EmissaoInicio.Value.AddMonths(-1);
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FEST_GerenciarNFeEntrada_Load(object sender, EventArgs e)
        {
            Carregar();
        }

        private void Carregar()
        {
            DADOS = FuncoesNFeEntrada.GetImportacoes(ovTXT_Chave.Text, ovTXT_EmissaoInicio.Value.Date, ovTXT_EmissaoFim.Value.Date);
            gridControl1.DataSource = DADOS;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            gridView1.BestFitColumns();
            AjustaHeaderTextGrid();
        }

        private void AjustaHeaderTextGrid()
        {
            gridView1.Columns[0].Caption = "NÚMERO";
            gridView1.Columns[1].Caption = "USUÁRIO";
            gridView1.Columns[2].Caption = "FORNECEDOR";
            gridView1.Columns[3].Caption = "CHAVE NF-E";
            gridView1.Columns[4].Caption = "IMPORTAÇÃO";
            gridView1.Columns[5].Caption = "EMISSÃO";
            gridView1.Columns[6].Caption = "TOTAL";
        }

        private void ovGRD_Importacoes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (gridView1.Columns[e.ColumnIndex].Name)
            {
                case "vnf":
                    e.Value = Convert.ToDecimal(e.Value).ToString("c2");
                    break;
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            decimal IDNFeEntrada = decimal.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfeentrada").ToString());
            if (!FuncoesNFeEntrada.PodeRemoverNFe(IDNFeEntrada))
            {
                MessageBox.Show(this, "NF-E Possui Títulos a Pagar com Baixa de Pagamento e não pode ser removido.", NOME_TELA);
                return;
            }

            if (MessageBox.Show(this, "Deseja remover a NF-E de Importação Selecionada?", NOME_TELA, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    PDVControlador.BeginTransaction();

                    if (!FuncoesNFeEntrada.Remover(IDNFeEntrada))
                        throw new Exception("Não foi possível remover a NF-E de Importação.");

                    PDVControlador.Commit();
                    Carregar();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(this, Ex.Message, NOME_TELA);
                    PDVControlador.Rollback();
                }
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            // Visualizar Danfe
            string XML = FuncoesNFeEntrada.GetXML (decimal.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfeentrada").ToString()));
            if (string.IsNullOrEmpty(XML))
            {
                MessageBox.Show(this, "XML Não encontrado.", NOME_TELA);
                return;
            }

            ConfiguracaoDanfeNfe Config = new ConfiguracaoDanfeNfe();
            Contexto.CONFIG_NFe.ConfiguracaoDanfeNFe.CopiarPropriedades(Config);
            Config.Logomarca = null;

            new DanfeFrNfe(FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(XML), Config, string.Empty).Visualizar(true);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string XML = FuncoesNFeEntrada.GetXML(decimal.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfeentrada").ToString()));
            if (string.IsNullOrEmpty(XML))
            {
                MessageBox.Show(this, "XML Não encontrado.", NOME_TELA);
                return;
            }

            try
            {
                SaveFileDialog SaveFile = new SaveFileDialog();
                SaveFile.Filter = "XML|*.xml";
                SaveFile.Title = "Exportação do XML";
                SaveFile.ShowDialog(this);
                SaveFile.ShowHelp = false;
                if (string.IsNullOrEmpty(SaveFile.FileName))
                    throw new Exception("Nome do arquivo não pode ser vazio.");

                File.WriteAllBytes(SaveFile.FileName, Encoding.Default.GetBytes(XML));
                MessageBox.Show(this, "Arquivo Exportado com Sucesso.", NOME_TELA);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, NOME_TELA);
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            ovTXT_Chave.Text = string.Empty;
            ovTXT_EmissaoInicio.Value = DateTime.Now.Date.AddMonths(-1);
            ovTXT_EmissaoFim.Value = DateTime.Now.Date;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Carregar();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
