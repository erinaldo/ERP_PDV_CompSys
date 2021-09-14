using DevExpress.Office.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DFe.Classes.Flags;
using FastReport.Data;
using Ionic.Zip;
using PDV.CONTROLER.Funcoes;
using PDV.CONTROLLER.NFE.Impressao;
using PDV.DAO.Entidades;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Util;
using PDV.VIEW.Forms.Vendas.NFe;
using PDV.VIEW.FRENTECAIXA.Tasks;
using PDV.VIEW.Task;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Gerenciamento
{
    public partial class GER_NFe : DevExpress.XtraEditors.XtraForm
    {
        private DataTable nfes = null;
        private string NOME_TELA = "GERENCIAMENTO DE NF-E";
        public GER_NFe()
        {
            InitializeComponent();


            Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.85);
            Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.85);

            AjustaCamposData();

            AtualizaGrid();

        }

        private void AjustaCamposData()
        {
            dateEdit1.DateTime = DateTime.Today;
            dateEdit2.DateTime = dateEdit1.DateTime.AddDays(1);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            AtualizaGrid();
        }

        public string idmovimentoFiscal { get; set; }

        private void ntnTransmitir_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                decimal? Autorizada = null;
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat") != DBNull.Value)
                    Autorizada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat").ToString());

                if (Autorizada != null && Autorizada == 100)
                {
                    MessageBox.Show(this, "NF-e está AUTORIZADA e não pode ser transmitida novamente.", NOME_TELA);
                    return;
                }
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal") != DBNull.Value)
                {
                    decimal IDMovimentoFiscal = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString());
                    if (Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada")) == 1)
                    {
                        MessageBox.Show(this, "NF-e já está CANCELADA.", NOME_TELA);
                        return;
                    }
                }
                string chave = (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "chave").ToString()).Replace("NFe", "");
                decimal idNFe = decimal.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfe").ToString());
                decimal idmovimentofiscal = 0;
                decimal.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString(), out idmovimentofiscal);
                EmitirNFe emitirNFe = new EmitirNFe(idNFe, idmovimentofiscal, false);
                AtualizaGrid();
                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
            catch (Exception EX)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                AtualizaGrid();
                MessageBox.Show(this, EX.Message, NOME_TELA);
            }
        }

        private void EmitirNFeTask_OnCompleted(TaskResult result)
        {
            if (result.GetBool("Result"))
            {
                AtualizaGrid();

                var idmov = result.GetString("idmovimentofiscal");
                //RetornoImpressaoNFe Retornod = new ImpressaoNFe()
                //{ CaminhoSolution = Contexto.CaminhoSolution }.ImprimirNFe(Retorno.IDMovimentoFiscal);
                //if (Retorno.isVisualizar)
                //    Retorno.danfe.Visualizar();
                //else
                //    Retorno.danfe.Imprimir(Retorno.isCaixaDialogo, Retorno.NomeImpressora);

            }
            else
            {
                MessageBox.Show(result.GetString("Msg"));
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            try
            {
                decimal Cancelada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada").ToString());
                if (Cancelada == 1)
                {
                    MessageBox.Show(this, "NF-e está CANCELADA e não pode ser impressa.", NOME_TELA);
                    return;
                }

                RetornoImpressaoNFe Retorno = new ImpressaoNFe()
                { CaminhoSolution = Contexto.CaminhoSolution }.ImprimirNFe(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString()));
                if (Retorno.isVisualizar)
                    Retorno.danfe.Visualizar();
                else
                    Retorno.danfe.Imprimir(Retorno.isCaixaDialogo, Retorno.NomeImpressora);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void metroButton8_Click(object sender, EventArgs e)
        {

            try
            {
                decimal Cancelada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada").ToString());
                decimal? Autorizada = null;
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat") != DBNull.Value)
                    Autorizada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat"));

                decimal? IDMovimentoFiscal = null;
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal") != DBNull.Value)
                    IDMovimentoFiscal = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal"));

                if (Cancelada == 1)
                {
                    MessageBox.Show(this, "NF-e está CANCELADA e não pode ser editada.", NOME_TELA);
                    return;
                }

                if (Autorizada != null && Autorizada == 100)
                {
                    MessageBox.Show(this, "NF-e está AUTORIZADA e não pode ser editada.", NOME_TELA);
                    return;
                }

                DAO.Entidades.NFe.NFe Nota = FuncoesNFe.GetNFe(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfe").ToString()));
                new GVEN_NFe(Nota, true, IDMovimentoFiscal).ShowDialog(this);
                AtualizaGrid();
            }
            catch (Exception)
            {

            }

        }

        private void GER_NFe_Load(object sender, EventArgs e)
        {
            AtualizaGrid();
            
        }

        private void AtualizaGrid()
        {            
            nfes = FuncoesMovimentoFiscal.GetNFe
            (
                Convert.ToDecimal(Contexto.CONFIG_NFe.CfgServico.tpAmb),
                dateEdit1.DateTime.Date,
                dateEdit2.DateTime.Date
            );


            gridControl1.DataSource = nfes;
            FormatarGrid();
        }
        private void FormatarGrid()
        {
            Grids.FormatGrid(ref gridView1);
            Grids.FormatColumnType(ref gridView1, "totalnfe", GridFormats.Finance);
            Grids.FormatColumnType(ref gridView1, "cancelada", GridFormats.VisibleFalse);
            Grids.FormatColumnType(ref gridView1, "totalnfe", GridFormats.SumFinance);
        }


        private void metroButton2_Click(object sender, EventArgs e)
        {
            decimal IDMovimentoFiscal = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString());
            Emitente Emit = FuncoesEmitente.GetEmitente();
            EmailEmitente EmailEmit = FuncoesEmitente.GetEmailEmitente(Emit.IDEmitente);

            if (EmailEmit.AutorizarEnviarEmail != 1)
            {
                MessageBox.Show(this, "O Envio de E-mail para notas autorizadas está desabilitado.", NOME_TELA);
                return;
            }
            if (Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada")) == 1)
            {
                MessageBox.Show(this, "NF-e está CANCELADA e não pode ser enviada por E-mail.", NOME_TELA);
                return;
            }
            new GER_EnviarPorEmail(FuncoesMovimentoFiscal.GetXMLEnvio(IDMovimentoFiscal), false, Emit, EmailEmit).ShowDialog(this);
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            try
            {
                decimal IDMovimentoFiscal = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString());
                if (Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada")) == 1)
                {
                    MessageBox.Show(this, "NF-e já está CANCELADA.", NOME_TELA);
                    return;
                }
                new GER_CancelarNfce(IDMovimentoFiscal, 55).ShowDialog(this);
                AtualizaGrid();
            }
            catch (Exception EX)
            {
                MessageBox.Show(this, "Detelhe : " + EX.Message, "");
            }

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            new GER_InutilizarNumeracao((int)ModeloDocumento.NFe).ShowDialog(this);
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            try
            {
                decimal Cancelada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada").ToString());
                decimal Autorizada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat").ToString());
                if (Cancelada == 1)
                {
                    MessageBox.Show(this, "NF-e está CANCELADA e não pode ser editada.", NOME_TELA);
                    return;
                }

                if (Autorizada != 100)
                {
                    MessageBox.Show(this, "NF-e Não está AUTORIZADA para enviar carta de correção.", NOME_TELA);
                    return;
                }

                new GER_CartaCorrecao(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString())).ShowDialog(this);
                AtualizaGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, NOME_TELA);
            }

        }


        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                decimal Cancelada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cancelada").ToString());
                decimal? Autorizada = null;
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat") != DBNull.Value)
                    Autorizada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat").ToString());

                if (Cancelada == 1)
                {
                    MessageBox.Show(this, "NF-e está CANCELADA e não pode ser editada.", NOME_TELA);
                    return;
                }

                if (Autorizada != null && Autorizada == 100)
                {
                    MessageBox.Show(this, "NF-e está AUTORIZADA e não pode ser editada.", NOME_TELA);
                    return;
                }
                DAO.Entidades.NFe.NFe Nota = FuncoesNFe.GetNFe(Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfe").ToString()));

                new GVEN_NFe(Nota, true).ShowDialog(this);
                AtualizaGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, NOME_TELA);
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var chave = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "chave");
                var movimento = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal");

                if (string.IsNullOrEmpty(chave.ToString()) || string.IsNullOrEmpty(movimento.ToString()))
                {
                    return;
                }
                GER_ConsultarNFe gER_ConsultarNFe = new GER_ConsultarNFe(chave.ToString(), decimal.Parse(movimento.ToString()));
                gER_ConsultarNFe.ShowDialog();
                AtualizaGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, NOME_TELA);
            }

        }

        private void btnEmitirNFe_Click(object sender, EventArgs e)
        {
            new GVEN_NFe(new DAO.Entidades.NFe.NFe()).ShowDialog(this);
            AtualizaGrid();

        }

        private void simpleButtonAtualizar_Click(object sender, EventArgs e)
        {
            AtualizaGrid();
        }

        private async void atualizarMetroButton_Click(object sender, EventArgs e)
        {
            AtualizaGrid();
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView currentView = sender as GridView;
                if (e.Column.FieldName == "status")
                {
                    string valor;
                    try
                    {
                        var cellValue = gridView1.GetRowCellValue(e.RowHandle, "status");
                        if (cellValue != null)
                            valor = cellValue.ToString();
                        else throw new Exception();
                    }
                    catch (Exception)
                    {
                        valor = "";
                    }
                    switch (valor)
                    {
                        case "AUTORIZADA":
                            e.Appearance.ForeColor = System.Drawing.Color.Green;
                            break;
                        case "CANCELADA":
                            e.Appearance.ForeColor = System.Drawing.Color.Red;
                            break;
                        case "NFE DIGITADA":
                            e.Appearance.ForeColor = System.Drawing.Color.Blue;
                            e.Appearance.BackColor = e.Appearance.BackColor2 = System.Drawing.Color.Yellow;
                            break;
                        case "REJEITADA":
                            e.Appearance.ForeColor = System.Drawing.Color.Red;
                            e.Appearance.BackColor = e.Appearance.BackColor2 = System.Drawing.Color.Orange;
                            break;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                decimal? Autorizada = null;
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat") != DBNull.Value)
                    Autorizada = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cstat").ToString());

                if (Autorizada != null && Autorizada == 100)
                {
                    //MessageBox.Show(this, "NF-e está AUTORIZADA e não pode ser transmitida novamente.", NOME_TELA);
                    //return;
                }

                decimal idNFe = decimal.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idnfe").ToString());
                decimal idmovimentofiscal = 0;
                EmitirNFe emitirNFe = new EmitirNFe(idNFe, idmovimentofiscal, true);

                AtualizaGrid();
            }
            catch (Exception EX)
            {
                AtualizaGrid();
                MessageBox.Show(this, EX.Message, NOME_TELA);
            }


        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (dateEdit1.DateTime > dateEdit2.DateTime)
                dateEdit2.DateTime = dateEdit1.DateTime.AddDays(1);
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (dateEdit1.DateTime > dateEdit2.DateTime)
            {
                dateEdit1.DateTime = dateEdit2.DateTime;
                dateEdit2.DateTime = dateEdit1.DateTime.AddDays(1);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl1.ShowRibbonPrintPreview();
        }

        private void gridView1_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            PrintingSystemBase pb = e.PrintingSystem as PrintingSystemBase;
            pb.PageSettings.Landscape = true;
            pb.PageSettings.LeftMargin = 5;
            pb.PageSettings.RightMargin = 5;
            pb.PageSettings.BottomMargin = 5;
            pb.PageSettings.TopMargin = 5;
        }

        private void btnBaixarXML_Click(object sender, EventArgs e)
        {
            decimal idmovimentofiscal = 0;
            string diretorio = "c:\\XMLDonwload\\";
            string arquivo;
            try
            {
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal") != DBNull.Value)
                {
                    idmovimentofiscal = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "idmovimentofiscal").ToString());
                }

                ModeloDocumento Modelo = ModeloDocumento.NFe;
                DataTable NFCes = FuncoesMovimentoFiscal.GetMovimentoDataTable(idmovimentofiscal);
                if (NFCes.Rows.Count == 0)
                {
                    MessageBox.Show(this, "Nenhuma " + Modelo.ToString() + " encontrada para exportar.", NOME_TELA);
                    return;
                }

                foreach (DataRow dr in NFCes.Rows)
                {
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    //var nfe = new NFe.Classes.NFe().CarregarDeXmlString(iso.GetString(dr["XMLENVIO"] as byte[]));

                    arquivo = dr["CHAVE"].ToString() + ".xml";

                    PDV.DAO.Entidades.MovimentoFiscal movimentoFiscal = FuncoesMovimentoFiscal.GetMovimento(idmovimentofiscal);

                    var xml = FuncoesMovimentoFiscal.GetXMLRetorno(int.Parse(dr["IDMOVIMENTOFISCAL"].ToString()));
                    var xmla = FuncoesMovimentoFiscal.GetXMLEnvio(int.Parse(dr["IDMOVIMENTOFISCAL"].ToString()));

                    var proc = new NFe.Classes.nfeProc().CarregarDeXmlString(FuncoesMovimentoFiscal.GetXMLRetorno(int.Parse(dr["IDMOVIMENTOFISCAL"].ToString())));

                    var nfeProc = new NFe.Classes.nfeProc()
                    {
                        NFe = proc.NFe,
                        protNFe = proc.protNFe,
                        versao = proc.versao
                    };
                    
                    if (!Directory.Exists(diretorio))
                        Directory.CreateDirectory(diretorio);
                    nfeProc.SalvarArquivoXml(diretorio + arquivo);
                    if (MessageBox.Show(this, "Deseja abrir o arquivo?", "XML NFe", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(diretorio + arquivo);
                    }
                    MessageBox.Show("Arquivo foi salvo em c:\\XMLDonwload\\","Sucesso");
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(this, EX.Message, NOME_TELA);
            }
        }
    }
}
