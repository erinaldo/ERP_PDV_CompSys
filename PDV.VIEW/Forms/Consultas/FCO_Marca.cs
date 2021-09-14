using MetroFramework;
using MetroFramework.Forms;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades;
using PDV.UTIL;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Cadastro;
using PDV.VIEW.Forms.Util;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Consultas
{
    public partial class FCO_Marca : DevExpress.XtraEditors.XtraForm
    {
        private string NOME_TELA = "CONSULTA DE MARCAS";

        public FCO_Marca()
        {
            InitializeComponent();
            AtualizarMarcas("", "");
        }

        private void AtualizarMarcas(string Codigo, string Descricao)
        {
            gridControl1.DataSource = FuncoesMarca.GetMarcas(Codigo, Descricao);
            FormatarGrid();
        }

        private void FormatarGrid()
        {
            Grids.FormatGrid(ref gridView1);
        }

        private void ovBTN_Novo_Click(object sender, EventArgs e)
        {
            FCA_Marca t = new FCA_Marca(new Marca());
            t.ShowDialog(this);
            AtualizarMarcas("","");
            DesabilitarBotoes();
        }

        private void ovBTN_Editar_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void ovBTN_Excluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Deseja remover a Marca selecionada?", NOME_TELA, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                var id = Grids.GetValorDec(gridView1, "idmarca");
                try
                {
                    if (!FuncoesMarca.Remover(id))
                        throw new Exception("Não foi possível remover a Marca.");
                }
                catch (Exception Ex)
                {
                   MessageBox.Show(this, Ex.Message, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AtualizarMarcas("", "");
            }
            DesabilitarBotoes();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            btnEditar.Enabled = true;
            btnRemover.Enabled = true;
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            Editar();
        }

        private void imprimriMetroButton_Click(object sender, EventArgs e)
        {
            gridView1.ShowPrintPreview();
            DesabilitarBotoes();
        }

        private void atualizarMetroButton_Click_1(object sender, EventArgs e)
        {
            AtualizarMarcas("","");
            DesabilitarBotoes();
        }

        private void DesabilitarBotoes()
        {
            btnEditar.Enabled = btnRemover.Enabled = false;
        }

        private void Editar()
        {
            try
            {
                var id = Grids.GetValorDec(gridView1, "idmarca");
                FCA_Marca t = new FCA_Marca(FuncoesMarca.GetMarca(id));
                t.ShowDialog(this);
                AtualizarMarcas("", "");
            }
            catch (NullReferenceException)
            {
            }
            finally
            {
                DesabilitarBotoes();
            }
        }
    }
}
