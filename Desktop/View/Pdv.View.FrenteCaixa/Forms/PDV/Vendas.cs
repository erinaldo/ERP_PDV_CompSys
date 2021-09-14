using DevExpress.Office.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using PDV.CONTROLER.Funcoes;
using PDV.CONTROLER.FuncoesFaturamento;
using PDV.DAO.Custom;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.PDV;
using PDV.DAO.Enum;
using PDV.REPORTS.Reports.PedidoVendaTermica;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Relatorios;
using PDV.VIEW.Forms.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Contexto = PDV.VIEW.FRENTECAIXA.App_Context.Contexto;

namespace PDV.VIEW.FRENTECAIXA.Forms
{
   public partial class FormVendas : XtraForm
    {
        public string NomeTela { get; set; } = "Vendas";

        public decimal IdFluxoCaixa { get; set; }

        private string ConverterStatus(int numStatus)
        {
            if (numStatus == Status.Faturado)
                return "FATURADO";
            
            if (numStatus == Status.Cancelado)
                return "CANCELADO";

            return "ABERTO";
        }

        public List<decimal> IdsSelecionados 
        {
            get
            {
                var ids = new List<decimal>();
                foreach (var linha in gridView1.GetSelectedRows())
                    ids.Add(Grids.GetValorDec(gridView1, "idvenda", linha));
                return ids;
            }
        }

        public FormVendas(decimal idFluxoCaixa)
        {
            InitializeComponent();
            IdFluxoCaixa = idFluxoCaixa;
            Atualizar();
        }

        private void Atualizar()
        {           
            gridControl1.DataSource = FuncoesVenda.GetListaVendasPorFluxoDeCaixa(IdFluxoCaixa);

            Grids.FormatGrid(ref gridView1);

            Grids.FormatColumnType(ref gridView1, new List<string>
            { 
                "quantidadeitens",
                "idcomanda",
                "idcliente",
                "idusuario",
                "vendedor",
                "tipodevenda"
            }, GridFormats.VisibleFalse);

            gridView1.Columns["status"].VisibleIndex = gridView1.Columns.Count - 1;
        }

        private void buttonCancelar_Click(object sender, System.EventArgs e)
        {
            if (Confirm("Deseja efetuar o cancelamento?") == DialogResult.Yes)
                foreach (var id in IdsSelecionados)
                {
                    try
                    {
                        PDVControlador.BeginTransaction();
                        var venda = FuncoesVenda.GetVenda(id);
                        if (venda.Status != Status.Faturado)
                        {
                            PDVControlador.Commit();
                            continue;
                        }
                        var vendaFaturamento = new VendaFaturamento(venda, Contexto.USUARIOLOGADO);
                        var motivoCancelamento = XtraInputBox.Show($"Insira um motivo para o cancelamento da venda {id}", NomeTela, "");
                        vendaFaturamento.CancelarVenda(motivoCancelamento);
                        PDVControlador.Commit();
                        Atualizar();
                    }
                    catch (Exception ex)
                    {
                        PDVControlador.Rollback();
                        Alert(ex.Message);
                    }
                }
        }

        private DialogResult Confirm(string msg)
        {
            return MessageBox.Show(msg, NomeTela, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg, NomeTela, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
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
                    case "FATURADO":
                        e.Appearance.ForeColor = System.Drawing.Color.Green;
                        break;
                    case "CANCELADO":
                        e.Appearance.ForeColor = System.Drawing.Color.Red;
                        break;
                    case "ABERTO":
                        e.Appearance.ForeColor = System.Drawing.Color.Blue;
                        break;
                    case "DESFEITO":
                        e.Appearance.ForeColor = System.Drawing.Color.Purple;
                        break;
                    case "APP":
                        e.Appearance.ForeColor = System.Drawing.Color.Blue;
                        e.Appearance.BackColor = System.Drawing.Color.Yellow;
                        break;
                }
            }
        }
        private void EmitirCupomGerencial(decimal idVenda)
        {
            try
            {
                Configuracao Config_NomeImpressora = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGURACAOPEDIDOVENDA_NOMEIMPRESSORA);
                Configuracao Config_ExibirCaixaDialogo = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGURACAOPEDIDOVENDA_EXIBIRCAIXADIALOGO);

                ReciboPedidoVenda _ReciboPedidoVenda = new ReciboPedidoVenda(idVenda);
                if (Config_ExibirCaixaDialogo != null && "1".Equals(Encoding.UTF8.GetString(Config_ExibirCaixaDialogo.Valor)))
                {
                    using (ReportPrintTool printTool = new ReportPrintTool(_ReciboPedidoVenda))
                    {
                        _ReciboPedidoVenda.PrintingSystem.ShowMarginsWarning = false;
                        printTool.Print();
                    }
                }
                else
                {
                    Stream STRel = new MemoryStream();
                    _ReciboPedidoVenda.ExportToPdf(STRel);
                    new FREL_Preview(STRel).ShowDialog(this);

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Não foi possivel imprimir o cupom Detalhes:" + Ex.Message, "Pedido de Venda");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            EmitirCupomGerencial(Grids.GetValorDec(gridView1, "idvenda"));
        }
    }
}




