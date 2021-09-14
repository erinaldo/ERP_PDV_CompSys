using MetroFramework;
using MetroFramework.Forms;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades.Estoque.Suprimentos;
using PDV.UTIL;
using PDV.VIEW.Forms.Cadastro.Suprimentos;
using System;
using System.Data;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Consultas.Suprimento
{
    public partial class FCO_ConversaoUM : DevExpress.XtraEditors.XtraForm
    {
        private string NOME_TELA = "CONSULTA CONVERSÕES DE UNIDADE DE MEDIDA";
        private DataTable CONVERSOES = null;
        public FCO_ConversaoUM()
        {
            InitializeComponent();
            AtualizaConversaoUM();
        }

        private void AtualizaConversaoUM()
        {
            CONVERSOES = FuncoesConversaoUnidadeMedida.GetConversoes("");
            gridControl1.DataSource = CONVERSOES;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            gridView1.BestFitColumns();
            AjustaHeaderTextGrid();
        }

        private void AjustaHeaderTextGrid()
        {

            gridView1.Columns[0].Caption = "PRODUTO";
            gridView1.Columns[1].Caption = "UN. DE ENTRADA";
            gridView1.Columns[2].Caption = "UN.DE SAIDA";
            gridView1.Columns[3].Caption = "FATOR";
            for (int i = 4; i < 8; i++)
            {
                gridView1.Columns[i].Visible = false;
            }
        }
    
        private void ovGRD_Conversoes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (gridView1.Columns[e.ColumnIndex].Name)
            {
                case "fator":
                    if (e.Value != null && e.Value != DBNull.Value)
                        e.Value = Convert.ToDecimal(e.Value).ToString("n4");
                    break;
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            
            try
            {
                FCA_ConversaoUM t = new FCA_ConversaoUM(FuncoesConversaoUnidadeMedida.GetConversao(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idconversaounidadedemedida").ToString())));
                t.ShowDialog(this);
                AtualizaConversaoUM();
                editarconvundmetroButton4.Enabled = false;
                removerMetroButton.Enabled = false;
            }
            catch (Exception)
            {

            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            editarconvundmetroButton4.Enabled = true;
            removerMetroButton.Enabled = true;
        }

        private void atualizarMetroButton_Click(object sender, EventArgs e)
        {
            AtualizaConversaoUM();
            editarconvundmetroButton4.Enabled = false;
            removerMetroButton.Enabled = false;
        }

        private void imprimirMetroButton_Click(object sender, EventArgs e)
        {
            gridView1.ShowPrintPreview();
            editarconvundmetroButton4.Enabled = false;
            removerMetroButton.Enabled = false;
        }


        private void editarconvundmetroButton4_Click(object sender, EventArgs e)
        {
            try
            {
                FCA_ConversaoUM t = new FCA_ConversaoUM(FuncoesConversaoUnidadeMedida.GetConversao(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idconversaounidadedemedida").ToString())));
                t.ShowDialog(this);
                AtualizaConversaoUM();
            }
            catch (Exception)
            {

            }
            finally
            {
                editarconvundmetroButton4.Enabled = false;
                removerMetroButton.Enabled = false;

            }
        }

        private void removerMetroButton_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(this, "Deseja remover a Conversão selecionada?", NOME_TELA, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    if (!FuncoesConversaoUnidadeMedida.Remover(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idconversaounidadedemedida").ToString())))
                        throw new Exception("Não foi possível remover a Conversão.");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(this, Ex.Message, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AtualizaConversaoUM();
            }
            editarconvundmetroButton4.Enabled = false;
            removerMetroButton.Enabled = false;
        }

        private void novoMetroButton_Click(object sender, EventArgs e)
        {
            FCA_ConversaoUM t = new FCA_ConversaoUM(new ConversaoUnidadeDeMedida());
            t.ShowDialog(this);
            AtualizaConversaoUM();
            editarconvundmetroButton4.Enabled = false;
            removerMetroButton.Enabled = false;
        }

        private void imprimirMetroButton_Clic(object sender, EventArgs e)
        {

        }
    }
}
