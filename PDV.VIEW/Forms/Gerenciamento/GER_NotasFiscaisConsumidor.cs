using DevExpress.XtraReports.UI;
using DFe.Classes.Flags;
using DFe.Utils;
using MetroFramework;
using MetroFramework.Forms;
using NFe.Classes;
using NFe.Classes.Servicos.Recepcao;
using NFe.Danfe.Fast.NFCe;
using NFe.Utils;
using NFe.Utils.NFe;
using PDV.CONTROLER.Funcoes;
using PDV.CONTROLER.FuncoesFaturamento;
using PDV.CONTROLLER.NFCE.Impressao;
using PDV.CONTROLLER.NFCE.Transmissao;
using PDV.CONTROLLER.NFCE.Util;
using PDV.DAO.Custom;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.PDV;
using PDV.DAO.Enum;
using PDV.REPORTS.Reports.PedidoVendaTermica;
using PDV.UTIL;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Relatorios;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Gerenciamento
{
    public partial class GER_NotasFiscaisConsumidor : DevExpress.XtraEditors.XtraForm
    {
        DataTable DATATABLE_NFCES = null;
        DataTable DATATABLE_NFCES_CONTINGENCIA = null;
        private string NOME_TELA = "Gerenciamento de NFC-e";

        public GER_NotasFiscaisConsumidor()
        {
            InitializeComponent();
            ovTXT_InicioContingencia.Value = DateTime.Now.AddMonths(-1);
            ovTXT_InicioVigencia.Value = DateTime.Now.AddMonths(-1);

            Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.85);
            Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.85);

            metroTabControl1.SelectedTab = metroTabPage1;
        }

        private void AjustarTextHeader()
        {

            vendasGridView.Columns[0].Visible = false;
            vendasGridView.Columns[1].Visible = false;
            vendasGridView.Columns[2].Caption = "NÚMERO";
            vendasGridView.Columns[3].Caption = "SÉRIE";
            vendasGridView.Columns[4].Caption = "CHAVE";
            vendasGridView.Columns[5].Caption = "VENDEDOR";
            vendasGridView.Columns[6].Visible = false;
            vendasGridView.Columns[7].Caption = "FORMA DE PAGAMENTO";
            vendasGridView.Columns[8].Caption = "VENDA";
            vendasGridView.Columns[9].Caption = "EMISSÃO";
            vendasGridView.Columns[10].Caption = "VALOR TOTAL";
            vendasGridView.Columns[11].Caption = "STATUS";
            vendasGridView.Columns[12].Visible = false;
            vendasGridView.Columns[13].Caption = "RETORNO SEFAZ";
            vendasGridView.Columns[14].Visible = false;
            vendasGridView.Columns[15].Visible = false;
        }

        private void AjustarTextHeaderContingencia()
        {
            contingenciaGridView.Columns[0].Visible = false;
            contingenciaGridView.Columns[1].Visible = false;
            contingenciaGridView.Columns[2].Visible = false;
            contingenciaGridView.Columns[3].Caption = "NÚMERO";
            contingenciaGridView.Columns[4].Caption = "SÉRIE";
            contingenciaGridView.Columns[5].Caption = "CHAVE";
            contingenciaGridView.Columns[6].Caption = "VENDEDOR";
            contingenciaGridView.Columns[7].Caption = "FORMA DE PAGAMENTO";
            contingenciaGridView.Columns[8].Caption = "VENDA";
            contingenciaGridView.Columns[9].Caption = "EMISSÃO";
            contingenciaGridView.Columns[10].Caption = "VALOR TOTAL";
            contingenciaGridView.Columns[11].Caption = "STATUS";
            contingenciaGridView.Columns[12].Caption = "RETORNO SEFAZ";
            contingenciaGridView.Columns[13].Visible = false;
            contingenciaGridView.Columns[14].Visible = false;
            contingenciaGridView.Columns[15].Visible = false;
        }

        private void AtualizarGrid()
        {
            DATATABLE_NFCES = FuncoesMovimentoFiscal.GetMovimentos(ovTXT_InicioVigencia.Value, ovTXT_DataFim.Value, ovCKB_Transmitida.Checked, ovCKB_Cancelada.Checked, ovCKB_VendaProcessada.Checked, ovCKB_Rejeitada.Checked, Convert.ToDecimal(Contexto.CONFIG_NFCe.CfgServico.tpAmb), Convert.ToInt32(Contexto.CONFIG_NFCe.CfgServico.ModeloDocumento));
            vendasGridControl.DataSource = DATATABLE_NFCES;
            vendasGridView.OptionsBehavior.Editable = false;
            vendasGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            vendasGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            vendasGridView.BestFitColumns();
            AjustarTextHeader();
        }

        private void AtualizarGridContingencia()
        {
            DATATABLE_NFCES_CONTINGENCIA = FuncoesMovimentoFiscal.GetNotasEmContingencia(ovTXT_InicioContingencia.Value, ovTXT_FimContingencia.Value, Convert.ToDecimal(Contexto.CONFIG_NFCe.CfgServico.tpAmb), Convert.ToInt32(Contexto.CONFIG_NFCe.CfgServico.ModeloDocumento));

            contingenciaGridControl.DataSource = DATATABLE_NFCES_CONTINGENCIA;
            contingenciaGridView.OptionsBehavior.Editable = false;
            contingenciaGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            contingenciaGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            contingenciaGridView.BestFitColumns();
            AjustarTextHeaderContingencia();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void GER_NotasFiscaisConsumidor_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
            AtualizarGridContingencia();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            EmitirCupomGerencial(Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idvenda").ToString()));
        }

        private void ImprimirNFCe()
        {
            decimal cStat = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "cstat").ToString());
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            if (cStat != 100)
            {
                MessageBox.Show(this, "NFC-e Não está AUTORIZADA para impressão. Verifique e tente novamente.", NOME_TELA);
                return;
            }

            try
            {
                ImpressaoNFCe Imp = new ImpressaoNFCe() { CaminhoSolution = Contexto.CaminhoSolution, };
                RetornoImpressaoNFCe RetImpressao = Imp.ImprimirNFCe(IDMovimentoFiscal);

                if (RetImpressao.isVisualizar)
                    RetImpressao.danfe.Visualizar();
                else
                    RetImpressao.danfe.Imprimir(RetImpressao.isCaixaDialogo, RetImpressao.NomeImpressora);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Não foi possível imprimir a NFC-e. Motivo: " + Ex.Message, NOME_TELA);
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            if (Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "cancelada").ToString()) == 1)
            {
                MessageBox.Show(this, "NFC-e já está CANCELADA.", NOME_TELA);
                return;
            }
            new GER_CancelarNfce(IDMovimentoFiscal, 65).ShowDialog(this);
           

            AtualizarGrid();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            decimal cStat = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "cstat").ToString());
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            decimal Cancelada = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "cancelada").ToString());
            if (cStat != 100 || Cancelada == 1)
            {
                MessageBox.Show(this, "NFC-e Não está AUTORIZADA ou CANCELADA para ser enviada por E-mail. Verifique e tente novamente.", NOME_TELA);
                return;
            }

            Emitente Emit = FuncoesEmitente.GetEmitente();
            EmailEmitente EmailEmit = FuncoesEmitente.GetEmailEmitente(Emit.IDEmitente);

            if (Cancelada == 1 && EmailEmit.CancelarEnviarEmail != 1)
            {
                MessageBox.Show(this, "O Envio de E-mail para notas canceladas está desabilitado.", NOME_TELA);
                return;
            }
            else if (Cancelada == 0 && EmailEmit.AutorizarEnviarEmail != 1)
            {
                MessageBox.Show(this, "O Envio de E-mail para notas autorizadas está desabilitado.", NOME_TELA);
                return;
            }

            new GER_EnviarPorEmail(FuncoesMovimentoFiscal.GetXMLEnvio(IDMovimentoFiscal), Cancelada == 1, Emit, EmailEmit).ShowDialog(this);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            EmitirNFCE();
        }

        private void EmitirNFCE()
        {
            decimal? IDMovimentoFiscal = null;
            if (vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idmovimentofiscal") != DBNull.Value)
                IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idmovimentofiscal").ToString());

            decimal? cStat = null;
            if (vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idmovimentofiscal") != DBNull.Value)
                cStat = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "cstat").ToString());

            decimal IDVenda = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "idvenda").ToString());
            decimal Cancelada = Convert.ToDecimal(vendasGridView.GetRowCellValue(vendasGridView.FocusedRowHandle, "cancelada").ToString());
            if (cStat.HasValue && cStat.Value == 100)
            {
                MessageBox.Show(this, "NFC-e está AUTORIZADA e não pode ser Transmitida.", NOME_TELA);
                return;
            }

            if (Cancelada == 1)
            {
                MessageBox.Show(this, "NFC-e está CANCELADA e não pode ser Transmitida.", NOME_TELA);
                return;
            }

            int NumeroNFCe;
            int Serie;
            /* Pegar Série e Número da NFC-e caso Existir */

            var emitente = FuncoesEmitente.GetEmitente();

            if (IDMovimentoFiscal.HasValue)
            {
                NFe.Classes.NFe nota = new NFe.Classes.NFe().CarregarDeXmlString(FuncoesMovimentoFiscal.GetXMLEnvio(IDMovimentoFiscal.Value));
                NumeroNFCe = Convert.ToInt32(nota.infNFe.ide.nNF);
                Serie = nota.infNFe.ide.serie;
            }
            else
            {
                NumeroNFCe = Convert.ToInt32(emitente.ProximoNumeroNFCe);
                Serie = Contexto.CONFIGURACAO_SERIE.SerieNFCe;
            }

            try
            {
                DAO.Entidades.PDV.Venda VENDA = FuncoesVenda.GetVenda(IDVenda);
                Cliente CLIENTE = VENDA.IDCliente.HasValue ? CLIENTE = FuncoesCliente.GetCliente(VENDA.IDCliente.Value) : null;
                EventosNFCe EventoNFCe = new EventosNFCe()
                {
                    VENDA = VENDA,
                    ITENS_VENDA = FuncoesItemVenda.GetItens(IDVenda),
                    PAGAMENTOS = FuncoesItemDuplicataNFCe.GetPagamentosPorVenda(IDVenda),
                    CaminhoSolution = Contexto.CaminhoSolution,
                    IDCLiente = VENDA.IDCliente,
                    IDMovimentoFiscal = IDMovimentoFiscal,
                    SERIE = Serie,
                    NUMERO = NumeroNFCe,
                    TipoCliente = CLIENTE != null ? (decimal?)CLIENTE.TipoDocumento : null,
                    VERSAO = "DUE" + Contexto.VERSAO,
                    CPF_CNPJ = CLIENTE != null ? CLIENTE.TipoDocumento == 0 ? CLIENTE.CNPJ : CLIENTE.CPF : null
                };

                //RetornoTransmissaoNFCe Retorno = EventoNFCe.TransmitirNFCeNormal();
                RetornoTransmissaoNFCe Retorno = EventoNFCe.TransmitirNFCe();

                if (EventoNFCe.NFCeIsValida)
                {
                    emitente.ProximoNumeroNFCe++;
                    FuncoesEmitente.SalvarEmitente(emitente, TipoOperacao.UPDATE);
                }

                if (!IDMovimentoFiscal.HasValue && Retorno.isAutorizada)
                    ZeusUtil.GerarFinanceiro(Retorno.IDMovimentoFiscal, ModeloDocumento.NFCe, Contexto.USUARIOLOGADO);

                if (!string.IsNullOrEmpty(Retorno.MotivoErro))
                    throw new Exception(Retorno.MotivoErro);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Não foi possível emitir a NFC-e, Motivo: " + Ex.Message, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            AtualizarGrid();
        }

        private void ImprimirNFCeContingencia()
        {
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            var nfe = new NFe.Classes.NFe().CarregarDeXmlString(FuncoesMovimentoFiscal.GetXMLEnvio(IDMovimentoFiscal));
            retEnviNFe retEnvi = null;

            if (!string.IsNullOrEmpty(FuncoesMovimentoFiscal.GetXMLRetorno(IDMovimentoFiscal)))
                retEnvi = FuncoesXml.XmlStringParaClasse<retEnviNFe>(FuncoesMovimentoFiscal.GetXMLRetorno(IDMovimentoFiscal));

            var nfeProc = new nfeProc()
            {
                NFe = nfe,
                protNFe = retEnvi.protNFe,
                versao = retEnvi.versao
            };

            var danfe = new DanfeFrNfce(nfe, Contexto.CONFIG_NFCe.ConfiguracaoDanfeNfce, Contexto.CONFIG_NFCe.ConfiguracaoCsc.CIdToken, Contexto.CONFIG_NFCe.ConfiguracaoCsc.Csc);
            if (retEnvi != null)
                danfe = new DanfeFrNfce(nfeProc, Contexto.CONFIG_NFCe.ConfiguracaoDanfeNfce, Contexto.CONFIG_NFCe.ConfiguracaoCsc.CIdToken, Contexto.CONFIG_NFCe.ConfiguracaoCsc.Csc);

            Configuracao configExibirDialogo = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGUACAODANFE_EXIBIRCAIXADIALOGO);
            if (configExibirDialogo != null && "1".Equals(Encoding.UTF8.GetString(configExibirDialogo.Valor)))
                danfe.Visualizar();
            else
            {
                Configuracao configNomeImpressora = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGUACAODANFE_NOMEIMPRESSORA);
                danfe.Imprimir(configExibirDialogo == null ? false : "1".Equals(Encoding.UTF8.GetString(configExibirDialogo.Valor)),
                               configNomeImpressora != null ? Encoding.UTF8.GetString(configNomeImpressora.Valor) : string.Empty);
            }
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            Emitente Emit = FuncoesEmitente.GetEmitente();
            EmailEmitente EmailEmit = FuncoesEmitente.GetEmailEmitente(Emit.IDEmitente);

            if (EmailEmit.AutorizarEnviarEmail != 1)
            {
                MessageBox.Show(this, "O Envio de E-mail para notas autorizadas está desabilitado.", NOME_TELA);
                return;
            }

            new GER_EnviarPorEmail(FuncoesMovimentoFiscal.GetXMLEnvio(IDMovimentoFiscal), false, Emit, EmailEmit).ShowDialog(this);
        }
        private void metroButton6_Click_1(object sender, EventArgs e)
        {
            AtualizarGridContingencia();
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            EmitirCupomGerencial(Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "idvenda").ToString()));
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            // Transmitir Nota.
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            try
            {
                decimal? cStat = null;
                if (vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "cstat") != DBNull.Value)
                    cStat = Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "cstat").ToString());

                decimal Cancelada = Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "cancelada").ToString());
                if (cStat.HasValue && cStat.Value == 100)
                {
                    MessageBox.Show(this, "NFC-e está AUTORIZADA e não pode ser Transmitida.", NOME_TELA);
                    return;
                }

                if (Cancelada == 1)
                {
                    MessageBox.Show(this, "NFC-e está CANCELADA e não pode ser Transmitida.", NOME_TELA);
                    return;
                }
                EventosNFCe Evento = new EventosNFCe()
                {
                    IDMovimentoFiscal = IDMovimentoFiscal,
                    CaminhoSolution = Contexto.CaminhoSolution
                };

                RetornoTransmissaoNFCe Retorno = Evento.TransmitirNFCeContingencia();
                //if (!string.IsNullOrEmpty(Retorno.MotivoErro))
                //    throw new Exception(Retorno.MotivoErro);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Não foi possível emitir a NFC-e, Motivo: " + Ex.Message, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            AtualizarGridContingencia();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            decimal IDMovimentoFiscal = Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "idmovimentofiscal").ToString());
            if (Convert.ToDecimal(vendasGridView.GetRowCellValue(contingenciaGridView.FocusedRowHandle, "cancelada").ToString()) == 1)
            {
                MessageBox.Show(this, "NFC-e já está CANCELADA.", NOME_TELA);
                return;
            }
            new GER_CancelarNfce(IDMovimentoFiscal, 65).ShowDialog(this);
            AtualizarGridContingencia();
        }

        public void EmitirCupomGerencial(decimal IDVenda)
        {
            try
            {
                Configuracao Config_NomeImpressora = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGURACAOPEDIDOVENDA_NOMEIMPRESSORA);
                Configuracao Config_ExibirCaixaDialogo = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGURACAOPEDIDOVENDA_EXIBIRCAIXADIALOGO);

                ReciboPedidoVenda _ReciboPedidoVenda = new ReciboPedidoVenda(IDVenda);
                if (Config_ExibirCaixaDialogo != null && "1".Equals(Encoding.UTF8.GetString(Config_ExibirCaixaDialogo.Valor)))
                {
                    using (ReportPrintTool printTool = new ReportPrintTool(_ReciboPedidoVenda))
                    {
                        if (Config_NomeImpressora != null && !string.IsNullOrEmpty(Encoding.UTF8.GetString(Config_NomeImpressora.Valor)))
                            printTool.PrinterSettings.PrinterName = Encoding.UTF8.GetString(Config_NomeImpressora.Valor);
                        printTool.PrintDialog();
                    }
                }
                else
                {
                    Stream STRel = new MemoryStream();
                    _ReciboPedidoVenda.ExportToPdf(STRel);
                    new FREL_Preview(STRel).ShowDialog(this);

                    //SaveFileDialog SaveFile = new SaveFileDialog();
                    //SaveFile.Filter = "RTF|*.rtf|PDF|*.pdf|XLS|*.xls|XLSX|*.xlsx";
                    //SaveFile.Title = "Salvar Relatório de Pedido de Venda";
                    //SaveFile.ShowDialog(this);
                    //SaveFile.ShowHelp = false;
                    //if (string.IsNullOrEmpty(SaveFile.FileName))
                    //    return;

                    //switch (SaveFile.FilterIndex)
                    //{
                    //    case 1:
                    //        _ReciboPedidoVenda.ExportToRtf(SaveFile.FileName);
                    //        break;
                    //    case 2:
                    //        _ReciboPedidoVenda.ExportToPdf(SaveFile.FileName);
                    //        break;
                    //    case 3:
                    //        _ReciboPedidoVenda.ExportToXls(SaveFile.FileName);
                    //        break;
                    //    case 4:
                    //        _ReciboPedidoVenda.ExportToXlsx(SaveFile.FileName);
                    //        break;
                    //}
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, NOME_TELA);
            }
        }

        private void metroButton2_Click_1(object sender, EventArgs e)
        {
            ImprimirNFCe();
        }

        private void metroButton8_Click_1(object sender, EventArgs e)
        {
            ImprimirNFCeContingencia();
        }

        private void metroButton13_Click(object sender, EventArgs e)
        {
            new GER_InutilizarNumeracao(65).ShowDialog(this);
        }

        private void ovGRD_NotasEmContingencia_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (contingenciaGridView.Columns[e.ColumnIndex].Name)
            {
                case "valornfce":
                    if (e.Value != null && e.Value != DBNull.Value)
                        e.Value = Convert.ToDecimal(e.Value).ToString("c2");
                    break;
            }
        }

        private void ovGRD_Notas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (vendasGridView.Columns[e.ColumnIndex].Name)
            {
                case "valornfce":
                    if (e.Value != null && e.Value != DBNull.Value)
                        e.Value = Convert.ToDecimal(e.Value).ToString("c2");
                    break;
            }
        }

    }
}
