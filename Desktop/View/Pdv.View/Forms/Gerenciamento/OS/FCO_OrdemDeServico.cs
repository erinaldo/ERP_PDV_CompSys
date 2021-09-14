using DevExpress.Office.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraReports.UI;
using PDV.CONTROLER.Funcoes;
using PDV.CONTROLER.FuncoesFaturamento;
using PDV.DAO.DB.Utils;
using PDV.DAO.Enum;
using PDV.REPORTS.Reports.Modelo_2;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sequence = PDV.DAO.DB.Utils.Sequence;

namespace PDV.VIEW.Forms.Gerenciamento.OS
{
    public partial class FCO_OrdemDeServico : XtraForm
    {
        public decimal IDOrdemServico 
        {
            get => Grids.GetValorDec(gridView1, "IDOrdemDeServico");
        }

        public List<decimal> IdsSelecionados
        {
            get
            {
                var ids = new List<decimal>();
                foreach (var row in gridView1.GetSelectedRows())
                    ids.Add(Grids.GetValorDec(gridView1, "IDOrdemDeServico", row));
                return ids;
            }
        }
        public FCO_OrdemDeServico()
        {
            InitializeComponent();
            DataDe = DateTime.Today;
            DataAte = DataDe.AddDays(1);
            Atualizar();
            FormatarGrid();
        }

        private void FormatarGrid()
        {
            Grids.FormatGrid(ref gridView1);
            Grids.FormatColumnType(ref gridView1, new List<string>() { }, GridFormats.Count);
        }

        public DateTime DataDe
        {
            get => dateEdit1.DateTime.Date;
            set => dateEdit1.DateTime = value;
        }

        public DateTime DataAte
        {
            get => dateEdit2.DateTime.Date;
            set => dateEdit2.DateTime = value;
        }


        private void atualizarMetroButton_Click(object sender, System.EventArgs e)
        {
            Atualizar();
        }

        private void Atualizar()
        {
            gridControl1.DataSource = FuncoesOrdemServico.GetOrdensDeServico(DataDe, DataAte);
        }
        
        private void novoMetroButton_Click(object sender, EventArgs e)
        {
            Novo();
        }

        private void Novo()
        {
            new FCA_OrdemDeServico().ShowDialog();
            Atualizar();
        }

        private void editarMetroButton_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void Editar()
        {
            new FCA_OrdemDeServico(IDOrdemServico).ShowDialog();
            Atualizar();
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (DataDe >= DataAte)
                DataAte = DataDe.AddDays(1);
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (DataDe >= DataAte)
                DataDe = DataAte.AddDays(-1);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            Editar();
        }

        private void buttonFaturar_Click(object sender, EventArgs e)
        {
            Faturar();
        }

        private void Faturar()
        {
            if (Confirm("Deseja faturar a(s) ordem(ns) selecionada(s)") == DialogResult.Yes)
            {
                foreach (var id in IdsSelecionados)
                {
                    try
                    {
                        PDVControlador.BeginTransaction();

                        var osFaturamento = new OSFaturamento(id, Contexto.USUARIOLOGADO);
                        osFaturamento.FaturarOS();
                        PDVControlador.Commit();

                    }
                    catch (Exception exception)
                    {
                        PDVControlador.Rollback();
                        Alert(exception.Message);
                    }
                }
                Atualizar();
            }
        }

        private void Alert(string message)
        {
            MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status")
            {
                string valor;
                try
                {
                    valor = Grids.GetValorStr(gridView1, "Status", e.RowHandle);
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

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Cancelar();
        }

        private void Cancelar()
        {
            if (Confirm("Deseja cancelar a(s) ordem(ns) selecionada(s)") == DialogResult.Yes)
            {
                try
                {
                    PDVControlador.BeginTransaction();
                    foreach (var id in IdsSelecionados)
                    {
                        var motivo = XtraInputBox
                      .Show($"Informe o motivo de cancelamento para a ordem de servico {id}",
                              "Cancelar Ordem de Serviço",
                              "");
                        var osFaturamento = new OSFaturamento(id, Contexto.USUARIOLOGADO);
                        osFaturamento.CancelarOs(motivo);
                    }
                  
                    PDVControlador.Commit();
                }
                catch (Exception exception)
                {
                    PDVControlador.Rollback();
                    Alert(exception.Message);
                }
                Atualizar();
            }
        }

        private DialogResult Confirm(string msg)
        {
            return MessageBox.Show(msg, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void simpleButtonDuplicar_Click(object sender, EventArgs e)
        {
            Duplicar();
        }

        private void Duplicar()
        {
            if (Confirm("Deseja duplicar a(s) ordem(ns) de serviço selecionada(s)") == DialogResult.Yes)
            {
                foreach (var id in IdsSelecionados)
                {
                    try
                    {
                        PDVControlador.BeginTransaction();

                        var ordemServico = FuncoesOrdemServico.GetOrdemDeServico(id);
                        ordemServico.DataCadastro = DateTime.Now;
                        ordemServico.DataFaturamento = null;
                        ordemServico.Status = Status.Aberto;
                        ordemServico.IDOrdemDeServico = Sequence.GetNextID("ORDEMDESERVICO", "IDORDEMDESERVICO");
                        FuncoesOrdemServico.Salvar(ordemServico, TipoOperacao.INSERT);

                        var itensOS = FuncoesItemOrdemDeServico.GetItensOrdemServico(id);
                        foreach (var item in itensOS)
                        {
                            item.IDOrdemDeServico = ordemServico.IDOrdemDeServico;
                            item.IDItemOrdemDeServico = Sequence.GetNextID("ITEMORDEMDESERVICO", "IDITEMORDEMDESERVICO");
                            FuncoesItemOrdemDeServico.Salvar(item);
                        }

                        var duplicatas = FuncoesDuplicataServico.GetDuplicatasServicos(id);
                        foreach (var duplicata in duplicatas)
                        {
                            duplicata.IDDuplicataServico = Sequence.GetNextID("DUPLICATASERVICO", "IDDUPLICATASERVICO");
                            duplicata.IDOrdemDeServico = ordemServico.IDOrdemDeServico;
                            FuncoesDuplicataServico.Salvar(duplicata);

                        }

                        PDVControlador.Commit();
                    }
                    catch (Exception ex)
                    {
                        PDVControlador.Rollback();

                        Alert(ex.Message);
                    }
                }
                Atualizar();
            }
        }

        private void metroButtonRemover_Click(object sender, EventArgs e)
        {
            Remover();
        }

        private void Remover()
        {
            if (Confirm("Deseja remover a(s) ordem(ns) de serviço selecionada(s)") == DialogResult.Yes)
            {
                foreach (var id in IdsSelecionados)
                {
                    try
                    {
                        PDVControlador.BeginTransaction();

                        var ordemDeServico = FuncoesOrdemServico.GetOrdemDeServico(id);
                        if (ordemDeServico.Status != Status.Aberto)
                            continue;
                        FuncoesDuplicataServico.Remover(id);
                        FuncoesItemOrdemDeServico.Remover(id);
                        if (!FuncoesOrdemServico.Remover(id))
                            throw new Exception($"Não foi possível remover a ordem {id}");

                        PDVControlador.Commit();
                    }
                    catch (Exception exception)
                    {
                        PDVControlador.Rollback();
                        Alert(exception.Message);
                    }
                }
                Atualizar();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            IdsSelecionados.ForEach(id => Imprimir(id));
        }
        private void Imprimir(decimal idOS, bool visualizar = false)
        {
            var report = new Modelo2Servico(idOS);
            using (var printTool = new ReportPrintTool(report))
            {
                report.ShowPrintMarginsWarning = report.PrintingSystem.ShowMarginsWarning = false;
                if (visualizar)
                {
                    printTool.ShowPreviewDialog();
                    return;
                }

                if (Confirm("Confirmar impressão?") == DialogResult.Yes)
                    printTool.Print();

            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Imprimir(IdsSelecionados.FirstOrDefault(), true);
        }
    }        
}




