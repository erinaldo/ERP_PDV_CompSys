using DevExpress.XtraBars;
using DevExpress.XtraNavBar;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using One.Loja;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Custom;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using PDV.DAO.Enum;
using PDV.UTIL;
using PDV.UTIL.FORMS.Forms;
using PDV.UTIL.FORMS.Forms.Atualizador;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.BI;
using PDV.VIEW.Forms.Cadastro;
using PDV.VIEW.Forms.Cadastro.Financeiro.Modulo;
using PDV.VIEW.Forms.Configuracoes;
using PDV.VIEW.Forms.Consultas;
using PDV.VIEW.Forms.Consultas.Financeiro;
using PDV.VIEW.Forms.Consultas.Financeiro.Modulo;
using PDV.VIEW.Forms.Consultas.MDFe;
using PDV.VIEW.Forms.Consultas.Parametros;
using PDV.VIEW.Forms.Consultas.Suprimento;
using PDV.VIEW.Forms.Estoque.Gerenciamento;
using PDV.VIEW.Forms.Estoque.ImportacaoNFeEntrada;
using PDV.VIEW.Forms.Estoque.Inventario;
using PDV.VIEW.Forms.Estoque.PedidoDeCompra;
using PDV.VIEW.Forms.Estoque.Transferencia;
using PDV.VIEW.Forms.Financeiro;
using PDV.VIEW.Forms.Gerenciamento;
using PDV.VIEW.Forms.Gerenciamento.Comercial_Relatorios;
using PDV.VIEW.Forms.Gerenciamento.DAV;
using PDV.VIEW.Forms.Gerenciamento.OS;
using PDV.VIEW.Forms.Gerenciamento.Romaneio;
using PDV.VIEW.Forms.Relatorios;
using PDV.VIEW.Forms.Vendas.Manifesto;
using PDV.VIEW.Forms.Vendas.NFe;
using PDV.VIEW.Integração_Migrados;
using PDV.VIEW.SINTEGRA.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace PDV.VIEW
{
    public partial class Principal : DevExpress.XtraEditors.XtraForm
    {

        private string NOME_TELA = "";

        public Principal()
        {
            InitializeComponent();
            CarregaInformacoesContexto();
            CarregarInformacoesIBPT();
            CarregarAtualizacaoDisponivel();
            CarergarInformacoesCertificado();
            VerificaPermissoes();

            //System.Threading.Tasks.Task.Run((Action)(Tarefas.RunMonitoramento));
        }

        #region Carregamento das Informações

        private void CarregarInformacoesIBPT()
        {
            DAO.Entidades.Ncm Ncm = FuncoesNcm.GetVigenciaIBPT();
            if (Ncm == null)
            {
                ovTXT_ChaveIBPT.Text = "<Não Informado>";
                ovTXT_VigenciaIBPT.Text = "[Tabela não Importada]";
            }
            else
            {
                ovTXT_ChaveIBPT.Text = Ncm.Chave;
                ovTXT_VigenciaIBPT.Text = string.Format("{0} Até {1}", Ncm.VigenciaInicio.ToString("dd/MM/yyyy"), Ncm.VigenciaFim.ToString("dd/MM/yyyy"));
            }
        }

        private void CarregaInformacoesContexto()
        {
            var usuario = Contexto.USUARIOLOGADO.Login;
            var nome = Contexto.USUARIOLOGADO.Nome;
            //ovTXT_Footer.Caption = "DUE ERP " + Contexto.VERSAO + string.Format(" Copyright ©  {0} - Todos os Direitos Reservados Software", DateTime.Now.Year);
            ovTXT_Footer.Caption = "";
            if (Contexto.USUARIOLOGADO.Root == 0)
            {
                //VerificaPermissaoItensMenu();
                ovTXT_PerfilUsuario.Caption = Contexto.USUARIOLOGADO.PerfilAcesso;
            }
            else
            {
                Contexto.USUARIOLOGADO.PerfilAcesso = "ROOT";
                ovTXT_PerfilUsuario.Caption = "ROOT";
            }
          //  validadeToolStripMenuItem.Text = $"Sistema Licenciado para {FuncoesGlobal.Empresa}  CNPJ: {FuncoesGlobal.CNPJ}   Data Validade : {FuncoesGlobal.ValidadeSistema.ToString("dd/MM/yyyy")}";
        }

        private void CarregarInformacoesNFCe()
        {
            //ovTXT_NumeroNFCe.Text = Sequence.GetValorAtualSequence("ID" + Contexto.CONFIGURACAO_SERIE.NomeSequenceNFCe, Contexto.IDCONEXAO_SECUNDARIA_THREAD_NFE_NFCE).ToString();
            //ovTXT_SerieNFCe.Text = Contexto.CONFIGURACAO_SERIE.SerieNFCe.ToString();
        }

        private void CarregarInformacoesNFe()
        {
            //ovTXT_NumeroNFe.Text = Sequence.GetValorAtualSequence("ID" + Contexto.CONFIGURACAO_SERIE.NomeSequenceNFe, Contexto.IDCONEXAO_SECUNDARIA_THREAD_NFE_NFCE).ToString();
            //ovTXT_SerieNFe.Text = Contexto.CONFIGURACAO_SERIE.SerieNFe.ToString();
        }

        private void CarergarInformacoesCertificado()
        {
            try
            {
                X509Certificate2 x509 = new X509Certificate2();
                Emitente EMIT = FuncoesEmitente.GetEmitente();

                Text = string.Format("DUE ERP {0} - {1}", Contexto.VERSAO, NOME_TELA) + $"     Data de Validade { Convert.ToDateTime(DueLicence.Crypto.Decrypt(EMIT.chaveerp)).ToString("dd/MM/yyyy")}  " +
                     string.Format("Validade do certificado: {0} até {1}", x509.NotBefore.ToShortDateString(), x509.NotAfter.ToShortDateString());

                x509.Import(EMIT.Certificado, Criptografia.DecodificaSenha(EMIT.SenhaCertificado), X509KeyStorageFlags.DefaultKeySet);

                //ovTXT_DescricaoCertificado.Caption = EMIT.NomeCertificado;
                ovTXT_DescricaoCertificado.Caption = "";
                ovTXT_ValidadeCertificado.Caption = string.Format("Validade do certificado: {0} até {1}", x509.NotBefore.ToShortDateString(), x509.NotAfter.ToShortDateString());


            }
            catch (Exception)
            {
                ovTXT_DescricaoCertificado.Caption = "<Não Informado>";
                ovTXT_ValidadeCertificado.Caption = "<Não Informado>";
            }
        }

        private void ovTemp_Tick(object sender, EventArgs e)
        {
            int Hora = DateTime.Now.Hour;
            string Saudacao = string.Empty;
            if (Hora >= 0 && Hora <= 6)
                Saudacao = string.Format("Boa noite {0}, ", Contexto.USUARIOLOGADO.Nome);

            if (Hora > 6 && Hora <= 13)
                Saudacao = string.Format("Bom dia {0}, ", Contexto.USUARIOLOGADO.Nome);

            if (Hora > 13 && Hora <= 18)
                Saudacao = string.Format("Boa tarde {0}, ", Contexto.USUARIOLOGADO.Nome);

            if (Hora > 18 && Hora <= 23)
                Saudacao = string.Format("Boa noite {0}, ", Contexto.USUARIOLOGADO.Nome);

            //dataEHora.Caption = Saudacao + DateTime.Now.ToString();
            horaToolStripMenuItem.Text = Saudacao + DateTime.Now.ToString();
            CarregarInformacoesNFCe();
            CarregarInformacoesNFe();
        }

        private void CarregarAtualizacaoDisponivel()
        {
            try
            {
                ovBTN_VerificarAtualizacoes.Visible = true;
                ovTXT_VersaoAtual.Text = Contexto.VERSAO.ToString();
                ovTXT_VersaoDisponivel.Text = "Verificando Atualização...";

                VerificarAtualizacao();
            }
            catch
            {
                ovTXT_VersaoDisponivel.Text = "<DUE NFCE Atualizado>";
            }
        }

        private void VerificarAtualizacao()
        {
            VersaoModulo Versao = ZeusUtil.VerificarVersaoDisponivel(DAO.Enum.Modulo.ERP, Contexto.VERSAO);
            ovTXT_VersaoAtual.Text = Versao.VersaoAtual.ToString();

            if (Versao.Disponivel)
                ovTXT_VersaoDisponivel.Text = Versao.VersaoDisponivel.ToString();
            else
                ovTXT_VersaoDisponivel.Text = "Nenhuma Versão Disponível...";

            ovBTN_VerificarAtualizacoes.Visible = Versao.Disponivel;
        }

        #endregion

        #region ListarMenus
        private void ovBTN_VerificarAtualizacoes_Click(object sender, EventArgs e)
        {
            new FAT_AtualizarVersao(DAO.Enum.Modulo.ERP, new VersaoModulo()
            {
                VersaoAtual = Contexto.VERSAO,
                VersaoDisponivel = null
            }).ShowDialog(this);
        }


        #endregion

        #region Ações dos Menus

        public void ShowForm(Form form, int tipo)
        {
            try
            {
                bool IsOpen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == form.Text)
                    {
                        IsOpen = true;
                        f.Focus();
                        break;
                    }
                }

                if (IsOpen == false)
                {

                    if (tipo == 1)// Grid
                    {
                        form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                        form.MdiParent = this;
                        form.Show();
                    }
                    else if (tipo == 2) //Form
                    {
                        form.ShowDialog();
                    }
                }
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void transferênciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCA_Transferencia fCA_Transferencia = new FCA_Transferencia();
            ShowForm(fCA_Transferencia, 1);
        }

        private void ovBTN_EstoqueInventario_Click(object sender, EventArgs e)
        {
            FESTCO_Inventario fESTCO_Inventario = new FESTCO_Inventario();
            ShowForm(fESTCO_Inventario, 1);
        }
        private void gerenciarNFeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FEST_GerenciarNFeEntrada fEST_GerenciarNFeEntrada = new FEST_GerenciarNFeEntrada();
            ShowForm(fEST_GerenciarNFeEntrada, 1);
        }

        private void importaçãoDaNFeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new FEST_EscolhaFormatoImportacaoNFe().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_PedidoCompra_Click(object sender, EventArgs e)
        {
            FCO_PedidoCompra fCO_PedidoCompra = new FCO_PedidoCompra();
            ShowForm(fCO_PedidoCompra, 1);
        }
        private void motivoDeCancelamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new FCO_MotivoCancelamento().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }
        private void movimentaçãoBancáriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCOFIN_MovimentacaoBancaria fCOFIN_MovimentacaoBancaria = new FCOFIN_MovimentacaoBancaria();
            ShowForm(fCOFIN_MovimentacaoBancaria, 1);
        }

        private void conversãoDeUnidadeDeMedidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCO_ConversaoUM fCO_ConversaoUM = new FCO_ConversaoUM();
            ShowForm(fCO_ConversaoUM, 1);
        }

        private void requisitanteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FCO_Requisitante fCO_Requisitante = new FCO_Requisitante();
            ShowForm(fCO_Requisitante, 1);
        }

        private void almoxarifadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCO_Almoxarifado fCO_Almoxarifado = new FCO_Almoxarifado();
            ShowForm(fCO_Almoxarifado, 1);
        }

        private void ovBTN_Cadastro_Categoria_Click(object sender, EventArgs e)
        {
            FCO_Categorias fCO_Categorias = new FCO_Categorias();
            ShowForm(fCO_Categorias, 1);
        }

        private void ovBTN_Cadastro_Cliente_Click(object sender, EventArgs e)
        {
            // new FCO_Cliente().ShowDialog(this);
            FCO_Cliente form = new FCO_Cliente();
            ShowForm(form, 1);
        }

        private void ovBTN_Cadastro_Emitente_Click(object sender, EventArgs e)
        {
            try
            {
                new FCA_Emitente().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Cadastro_Fornecedor_Click(object sender, EventArgs e)
        {
            FCO_Fornecedor fCO_Fornecedor = new FCO_Fornecedor();
            ShowForm(fCO_Fornecedor, 1);
        }

        private void ovBTN_CadastroFinanceiro_FormaPagamento_Click(object sender, EventArgs e)
        {
            FCO_FormaDePagamento fCO_FormaDePagamento = new FCO_FormaDePagamento();
            ShowForm(fCO_FormaDePagamento, 1);
        }

        private void ovBTN_CadastroFinanceiro_TipoTitulo_Click(object sender, EventArgs e)
        {
            FCO_CentroCusto fCO_TipoTitulo = new FCO_CentroCusto();
            ShowForm(fCO_TipoTitulo, 1);

        }

        private void ovBTN_CadastroFinanceiro_StatusTitulo_Click(object sender, EventArgs e)
        {

        }

        private void ovBTN_CadastroFinanceiro_CentroCusto_Click(object sender, EventArgs e)
        {
            var form = new FCO_CentroCusto();
            ShowForm(form, 1);
        }

        private void ovBTN_CadastroFinanceiro_NaturezaFinanceira_Click(object sender, EventArgs e)
        {
            FCO_NaturezaFinanceira fCO_NaturezaFinanceira = new FCO_NaturezaFinanceira();
            ShowForm(fCO_NaturezaFinanceira, 1);
        }

        private void ovBTN_CadastroFinanceiro_HistoricoFinanceiro_Click(object sender, EventArgs e)
        {
            FCO_HistoricoFinanceiro fCO_HistoricoFinanceiro = new FCO_HistoricoFinanceiro();
            ShowForm(fCO_HistoricoFinanceiro, 1);
        }

        private void ovBTN_CadastroFinanceiro_ContaBancaria_Click(object sender, EventArgs e)
        {
            FCO_ContaBancaria fCO_ContaBancaria = new FCO_ContaBancaria();
            ShowForm(fCO_ContaBancaria, 1);
        }

        private void ovBTN_CadastroFinanceiro_Talonario_Click(object sender, EventArgs e)
        {
            FCO_Talonario fCO_Talonario = new FCO_Talonario();
            ShowForm(fCO_Talonario, 1);
        }

        private void ovBTN_CadastroFinanceiro_CarteiraCobranca_Click(object sender, EventArgs e)
        {
            FCO_ContaCobranca fCO_ContaCobranca = new FCO_ContaCobranca();
            ShowForm(fCO_ContaCobranca, 1);
        }

        private void ovBTN_Cadastro_Marca_Click(object sender, EventArgs e)
        {
            FCO_Marca fCO_Marca = new FCO_Marca();
            ShowForm(fCO_Marca, 1);
        }

        private void ovBTN_Cadastro_Produto_Click(object sender, EventArgs e)
        {
            FCO_Produto fCO_Produto = new FCO_Produto();
            ShowForm(fCO_Produto, 1);
        }

        private void ovBTN_Cadastro_Transportadora_Click(object sender, EventArgs e)
        {
            FCO_Transportadora fCO_Transportadora = new FCO_Transportadora();
            ShowForm(fCO_Transportadora, 1);
        }

        private void ovBTN_Cadastro_UnidadeDeMedida_Click(object sender, EventArgs e)
        {
            try
            {
                new FCO_UnidadeMedida().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_CadastroFiscal_Portaria_Click(object sender, EventArgs e)
        {
            FCO_Portaria fCO_Portaria = new FCO_Portaria();
            ShowForm(fCO_Portaria, 1);
        }

        private void ovBTN_CadastroFiscal_IntegracaoFiscal_Click(object sender, EventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                FCO_IntegracaoFiscal fCO_IntegracaoFiscal = new FCO_IntegracaoFiscal();
                ShowForm(fCO_IntegracaoFiscal, 1);
            }

        }

        private void ovBTN_CadastroFiscal_AliquotaICMSPorEstado_Click(object sender, EventArgs e)
        {
            FCA_AliquotaICMSPorEstado fCA_AliquotaICMSPorEstado = new FCA_AliquotaICMSPorEstado();
            ShowForm(fCA_AliquotaICMSPorEstado, 1);
        }

        private void ovBTN_CadastroManifesto_Condutor_Click(object sender, EventArgs e)
        {
            FCO_Condutor fCO_Condutor = new FCO_Condutor();
            ShowForm(fCO_Condutor, 1);
        }

        private void ovBTN_CadastroManifesto_Proprietario_Click(object sender, EventArgs e)
        {
            FCO_Proprietario fCO_Proprietario = new FCO_Proprietario();
            ShowForm(fCO_Proprietario, 1);
        }

        private void ovBTN_CadastroManifesto_Veiculo_Click(object sender, EventArgs e)
        {
            FCO_Veiculo fCO_Veiculo = new FCO_Veiculo();
            ShowForm(fCO_Veiculo, 1);
        }

        private void ovBTN_CadastroManifesto_Seguradora_Click(object sender, EventArgs e)
        {
            FCO_Seguradora fCO_Seguradora = new FCO_Seguradora();
            ShowForm(fCO_Seguradora, 1);
        }

        private void ovBTN_Fiscal_EmissaoNFe_Click(object sender, EventArgs e)
        {
            try
            {
                new GVEN_NFe(new DAO.Entidades.NFe.NFe()).ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Fiscal_EmissaoMDFe_Click(object sender, EventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                try
                {
                    new ManifestoDFE().ShowDialog(this);
                }
                catch (Exception exception)
                {
                    Alert(exception.Message);
                }
            }

        }

        private void ovBTN_Gerenciamento_NFCeVendasContingencia_Click(object sender, EventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                GER_NotasFiscaisConsumidor gER_NotasFiscaisConsumidor = new GER_NotasFiscaisConsumidor();
                ShowForm(gER_NotasFiscaisConsumidor, 1);
            }
        }

        private void ovBTN_Gerenciamento_NFeVendas_Click(object sender, EventArgs e)
        {
            try
            {
                Emitente emitente = FuncoesEmitente.GetEmitente();

                if (emitente == null)
                {
                    MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    GER_NFe gER_NFe = new GER_NFe();
                    ShowForm(gER_NFe, 1);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Tela Inicial", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void ovBTN_Gerenciamento_ExportarNFCeNFe_Click(object sender, EventArgs e)
        {
            try
            {
                new GER_ExportarNFCe().ShowDialog(this);

            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Gerenciamento_EventosRegistradosNFCeNFe_Click(object sender, EventArgs e)
        {
            GER_EventosRegistrados gER_EventosRegistrados = new GER_EventosRegistrados();
            ShowForm(gER_EventosRegistrados, 1);
        }

        private void ovBTN_Gerenciamento_ManifestoDocumentoFiscal_Click(object sender, EventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                GER_Manifesto gER_Manifesto = new GER_Manifesto();
                ShowForm(gER_Manifesto, 1);
            }
        }

        private void ovBTN_Configuracoes_Android_Click(object sender, EventArgs e)
        {

        }

        private void ovBTN_Configuracoes_Atualizador_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_Atualizador().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_Email_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_Email().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_NFCe_Contingencia_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_Contingencia().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }

        }

        private void ovBTN_Configuracoes_NFCe_Geral_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_NFCeGeral().ShowDialog(this);

            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_NFCe_ImpressaoDanfe_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_ImpressaoDanfe().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_NFe_Geral_Click(object sender, EventArgs e)
        {
            try
            {
                new GERAL().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_NFe_ImpressaoDanfe_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_DanfeNFe().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }


        private void ovBTN_Configuracoes_MDFe_Geral_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_MDFeGeral().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_MDFe_ImpressaoDamfe_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_DamfeMDFe().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }

        }

        private void ovBTN_Configuracoes_IBPT_ImportarTabela_Click(object sender, EventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                try
                {
                    new FCONFIG_ImportarTabelaIBPT().ShowDialog(this);
                }
                catch (Exception exception)
                {
                    Alert(exception.Message);
                }
            }
        }

        private void ovBTN_Configuracoes_IBPT_ConsultarTabela_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_ConsultarTabelaIBPT().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }

        }

        private void ovBTN_Configuracoes_Vendas_Geral_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_Vendas_Geral().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_Vendas_Impressao_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_ImpressaoPedidoVenda().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Configuracoes_Geral_StatusSefaz_Click(object sender, EventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (emitente.Certificado == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                try
                {
                    new FCONFIG_ConsultaStatusWS().ShowDialog(this);
                }
                catch (Exception exception)
                {
                    Alert(exception.Message);
                }
            }
        }

        private void ovBTN_Configuracoes_Geral_WebServices_Click(object sender, EventArgs e)
        {
            try
            {
                new FCONFIG_WebServicesNFCE().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Financeiro_ContasReceber_Click(object sender, EventArgs e)
        {
            FCOFIN_ContaReceber fCOFIN_ContaReceber = new FCOFIN_ContaReceber();
            ShowForm(fCOFIN_ContaReceber, 1);
        }

        private void ovBTN_Financeiro_ContasPagar_Click(object sender, EventArgs e)
        {

            FCOFIN_ContaPagar fCOFIN_ContaPagar = new FCOFIN_ContaPagar();
            ShowForm(fCOFIN_ContaPagar, 1);
        }

        private void ovBTN_Financeiro_ConciliacaoBancaria_Click(object sender, EventArgs e)
        {
            FCAFIN_ConciliacaoBancaria fCAFIN_ConciliacaoBancaria = new FCAFIN_ConciliacaoBancaria();
            ShowForm(fCAFIN_ConciliacaoBancaria, 1);
        }

        private void ovBTN_Financeiro_Duplicatas_Click(object sender, EventArgs e)
        {
            GFIN_Duplicata gFIN_Duplicata = new GFIN_Duplicata();
            ShowForm(gFIN_Duplicata, 1);
        }

        private void ovBTN_Financeiro_RenegociacaoDeContas_Click(object sender, EventArgs e)
        {
            GFIN_RenegociacaoTitulo gFIN_RenegociacaoTitulo = new GFIN_RenegociacaoTitulo();
            ShowForm(gFIN_RenegociacaoTitulo, 1);
        }

        private void ovBTN_Relatorios_ComandasAbertoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                new FREL_ComandasEmAberto().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Relatorios_ImpressaoComandas_Click(object sender, EventArgs e)
        {
            try
            {
                new FREL_ImpressaoComandas().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Relatorios_ProdutosFornecedor_Click(object sender, EventArgs e)
        {
            try
            {
                new FREL_ProdutosPorFornecedor().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Ajuda_VerificarAtualizacoes_Click(object sender, EventArgs e)
        {
            try
            {
                PDVControlador.BeginTransaction();

                FAT_AtualizarVersao Form = new FAT_AtualizarVersao(DAO.Enum.Modulo.ERP, new VersaoModulo()
                {
                    VersaoAtual = Contexto.VERSAO,
                    VersaoDisponivel = null,
                });

                Form.ShowDialog(this);
                PDVControlador.Commit();
            }
            catch (Exception Ex)
            {
                PDVControlador.Rollback();
            }
        }

        private void ovBTN_Ajuda_Sobre_Click(object sender, EventArgs e)
        {
            try
            {
                new Sobre().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Ajuda_ConsultaCNPJ_Click(object sender, EventArgs e)
        {
            try
            {
                new FormConsultaSintegra(string.Empty).ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void ovBTN_Ajuda_Sair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ovBTN_CadastroPermissoes_PerfilAcesso_Click(object sender, EventArgs e)
        {
            FCO_PerfilAcesso fCO_PerfilAcesso = new FCO_PerfilAcesso();
            ShowForm(fCO_PerfilAcesso, 1);
        }

        private void ovBTN_CadastroPermissoes_Usuario_Click(object sender, EventArgs e)
        {
            FCO_Usuario fCO_Usuario = new FCO_Usuario();
            ShowForm(fCO_Usuario, 1);
        }

        private void ovBTN_CadastroVendas_Click(object sender, EventArgs e)
        {
            FCO_Comanda fCO_Comanda = new FCO_Comanda();
            ShowForm(fCO_Comanda, 1);
        }

        private void fluxoDeCaixaPorUsuárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new FREL_FluxoDeCaixaDiarioPorUsuario().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        #endregion

        #region Permissões
        private void VerificaPermissoes()
        {
            if (Contexto.USUARIOLOGADO.Root == 1)
                return;

            ovBTN_VerificarAtualizacoes.Visible = Contexto.ITENSMENU.Where(o => o.IDItemMenu == (int)ItemMenuERP.AJUDA_VERIFICAR_ATUALIZACOES).Count() == 1;
            foreach (Component comp in EnumerateComponents())
            {
                if (comp.GetType() == typeof(UTIL.Components.Custom.ToolStripMenuItem))
                {
                    UTIL.Components.Custom.ToolStripMenuItem _comp = ((UTIL.Components.Custom.ToolStripMenuItem)comp);
                    _comp.Visible = Contexto.ITENSMENU.Where(o => o.IDItemMenu == _comp.IDItemMenu).Count() == 1 || _comp.IDItemMenu == -1;
                }

                if (comp.GetType() == typeof(UTIL.Components.Custom.PictureBox))
                {
                    UTIL.Components.Custom.PictureBox _comp = ((UTIL.Components.Custom.PictureBox)comp);
                    _comp.Visible = Contexto.ITENSMENU.Where(o => o.IDItemMenu == _comp.IDItemMenu).Count() == 1;
                }
            }

            PermissoesNavElements();
            if (Contexto.USUARIOLOGADO.IDPerfilAcesso == 1)
                navBarGroup8.Visible = navBarItem34.Visible = true;
        }


        private void PermissoesNavElements()
        {
            IEnumerable<NavElement> EnumerateNavElement()
            {
                return from field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       where typeof(NavElement).IsAssignableFrom(field.FieldType)
                       let component = (NavElement)field.GetValue(this)
                       where component != null
                       select component;

            }

            foreach (var item in EnumerateNavElement())
            {
                if (item.Tag != null)
                {
                    var tag = item.Tag.ToString().ToLower();
                    item.Visible = Contexto.ITENSMENU.Where(i => i.IDItemMenu.ToString() == tag).Count() == 1;
                }
                else
                {
                    item.Visible = false;
                }
            }
        }

        private IEnumerable<Component> EnumerateComponents()
        {
            var lQuery = from field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         where typeof(Component).IsAssignableFrom(field.FieldType)
                         let component = (Component)field.GetValue(this)
                         where component != null
                         select component;

            return lQuery.Where(o => o.GetType().Equals(typeof(UTIL.Components.Custom.ToolStripMenuItem)) || o.GetType().Equals(typeof(UTIL.Components.Custom.PictureBox)));
        }




        #endregion

        private void Principal_Load(object sender, EventArgs e)
        {
            //Consultar();



            //            Emitente emitente = FuncoesEmitente.GetEmitente();
            //
            //            if (emitente == null)
            //            {
            //                return;
            //            }
            //            else
            //            {
            //                try
            //                {
            //
            //                    Sysadmin sysadmin = new Sysadmin();
            //                    sysadmin.CheckLicense(emitente.CNPJ, 3);
            //
            //                }
            //                catch (Exception ex)
            //                {
            //                    string cpuid = SysAdm.Sysadmin.GetCPUID();
            //                    MessageBox.Show($@"{ex.Message}
            //
            //CNPJ: {emitente.CNPJ}
            //Terminal: {cpuid}", "Licença", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                    Clipboard.SetText(cpuid);
            //                    Environment.Exit(1);
            //                }
            //            }
            frmBiPDV frmBiPDV = new frmBiPDV();
            ShowForm(frmBiPDV, 1);

        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saldoEstoqueInicialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCO_SaldoEstoqueInicial fCO_SaldoEstoqueInicial = new FCO_SaldoEstoqueInicial();
            ShowForm(fCO_SaldoEstoqueInicial, 1);
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                FCO_Emitente fCO_Emitente = new FCO_Emitente();
                fCO_Emitente.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                VendasGeral vendasGeral = new VendasGeral();
                vendasGeral.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }
        private void TrataRetorno(RetornoBasico retornoBasico)
        {
            RetornoDados(retornoBasico.Retorno, statusSefazLabel1);
        }

        internal void RetornoDados<T>(T objeto, BarButtonItem TextBox) where T : class
        {
            TextBox.Caption = string.Empty;

            foreach (var atributos in LerPropriedades(objeto))
            {
                if (atributos.Key == "xMotivo")
                    TextBox.Caption += (atributos.Key + " = " + atributos.Value);
            }

        }
        public static Dictionary<string, object> LerPropriedades<T>(T objeto) where T : class
        {
            //A função pode ser melhorada para trazer recursivamente as proprieades dos objetos filhos
            var dicionario = new Dictionary<string, object>();

            foreach (var attributo in objeto.GetType().GetProperties())
            {
                var value = attributo.GetValue(objeto, null);
                dicionario.Add(attributo.Name, value);
            }

            return dicionario;
        }
        private void Consultar()
        {
            try
            {
                #region Status do serviço
                var servicoNFe = new ServicosNFe(Contexto.CONFIG_NFe.CfgServico);
                var retornoStatus = servicoNFe.NfeStatusServico();
                TrataRetorno(retornoStatus);
                statusSefazLabel1.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Green;
                statusSefazLabel1.Caption = statusSefazLabel1.Caption.Replace("xMotivo", "STATUS SEFAZ");
                #endregion
            }
            catch (Exception ex)
            {
                statusSefazLabel1.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Red;
                statusSefazLabel1.Caption = ex.Message;
            }
        }
        private void statusLabel1_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Cadastros.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Fiscal.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Gerenciamento.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Estoque.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Financeiro.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //ovMenu_Relatorios.Show(new Point(barButtonItem8.Links[0].ScreenBounds.X + (barButtonItem8.Links[0].ScreenBounds.Width / 2), barButtonItem8.Size.Height));
            //ovMenu_Relatorios.di
            ovMenu_Relatorios.Show(new Point(MousePosition.X, MousePosition.Y));


        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Sistema.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovMenu_Ajuda.Show(new Point(MousePosition.X, MousePosition.Y)); ;
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            abrirPedido();
        }

        private void abrirPedido()
        {
            PedidoDeVenda mainDAV = new PedidoDeVenda();
            ShowForm(mainDAV, 1);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ovBTN_PedidoCompra_Click(sender, e);
        }

        private void caixaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCO_Caixa fCO_Caixa = new FCO_Caixa();
            ShowForm(fCO_Caixa, 1);
        }

        private void tipoDeOperaçãoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FCO_TipodeOperacao fCO_Caixa = new FCO_TipodeOperacao();
            ShowForm(fCO_Caixa, 1);
        }

        private void fluxoDeCaixaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GER_FluxoFinanceiro gER_FluxoDeCaixa = new GER_FluxoFinanceiro();
            ShowForm(gER_FluxoDeCaixa, 1);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowForm(new BIVendedoresEClientes(), 1);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void statusSefazLabel1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Consultar();
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GER_Romaneio frm = new GER_Romaneio();
            ShowForm(frm, 1);
        }

        private void movimentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new GFIN_MovimentoEstoque(), 1);
        }


        private void cFOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FCO_Cfop fCO_Cfop = new FCO_Cfop();
                fCO_Cfop.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ovBTN_Cadastro_Cliente_Click(sender, e);
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            abrirPedido();
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ovBTN_Gerenciamento_NFeVendas_Click(sender, e);
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GER_Romaneio frm = new GER_Romaneio();
            ShowForm(frm, 1);
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Produto fCO_Produto = new FCO_Produto();
            ShowForm(fCO_Produto, 1);
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ovBTN_PedidoCompra_Click(sender, e);
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Fornecedor fCO_Fornecedor = new FCO_Fornecedor();
            ShowForm(fCO_Fornecedor, 1);
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCOFIN_ContaReceber fCOFIN_ContaReceber = new FCOFIN_ContaReceber();
            ShowForm(fCOFIN_ContaReceber, 1);
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCOFIN_ContaPagar fCOFIN_ContaPagar = new FCOFIN_ContaPagar();
            ShowForm(fCOFIN_ContaPagar, 1);
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                new FCA_Emitente().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Cliente form = new FCO_Cliente();
            ShowForm(form, 1);
        }

        private void navBarItem12_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Transportadora fCO_Transportadora = new FCO_Transportadora();
            ShowForm(fCO_Transportadora, 1);
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Fornecedor fCO_Fornecedor = new FCO_Fornecedor();
            ShowForm(fCO_Fornecedor, 1);
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Usuario fCO_Usuario = new FCO_Usuario();
            ShowForm(fCO_Usuario, 1);
        }

        private void navBarItem15_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GER_FluxoFinanceiro form = new GER_FluxoFinanceiro();
            ShowForm(form, 1);
        }

        private void navBarItem16_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

        }

        private void navBarItem17_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_TipodeOperacao fCO_Caixa = new FCO_TipodeOperacao();
            ShowForm(fCO_Caixa, 1);
        }

        private void navBarItem18_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                FCO_IntegracaoFiscal fCO_IntegracaoFiscal = new FCO_IntegracaoFiscal();
                ShowForm(fCO_IntegracaoFiscal, 1);
            }
        }

        private void navBarItem19_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Portaria fCO_Portaria = new FCO_Portaria();
            ShowForm(fCO_Portaria, 1);
        }

        private void navBarItem20_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCA_AliquotaICMSPorEstado fCA_AliquotaICMSPorEstado = new FCA_AliquotaICMSPorEstado();
            ShowForm(fCA_AliquotaICMSPorEstado, 1);
        }

        private void navBarItem21_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Cfop fCO_Cfop = new FCO_Cfop();
            ShowForm(fCO_Cfop, 1);
        }

        private void navBarItem22_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            new FEST_EscolhaFormatoImportacaoNFe().ShowDialog(this);
        }

        private void navBarItem24_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FESTCO_Inventario fESTCO_Inventario = new FESTCO_Inventario();
            ShowForm(fESTCO_Inventario, 1);
        }

        private void navBarItem23_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FEST_GerenciarNFeEntrada fEST_GerenciarNFeEntrada = new FEST_GerenciarNFeEntrada();
            ShowForm(fEST_GerenciarNFeEntrada, 1);
        }

        private void navBarItem25_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCA_Transferencia fCA_Transferencia = new FCA_Transferencia();
            ShowForm(fCA_Transferencia, 1);
        }

        private void navBarItem26_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowForm(new GFIN_MovimentoEstoque(), 1);
        }

        private void navBarItem27_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                GER_NotasFiscaisConsumidor gER_NotasFiscaisConsumidor = new GER_NotasFiscaisConsumidor();
                ShowForm(gER_NotasFiscaisConsumidor, 1);
            }
        }

        private void navBarItem29_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GER_EventosRegistrados gER_EventosRegistrados = new GER_EventosRegistrados();
            ShowForm(gER_EventosRegistrados, 1);
        }

        private void navBarItem30_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Emitente emitente = FuncoesEmitente.GetEmitente();

            if (emitente == null)
            {
                MessageBox.Show(this, "Emitente não cadastrado!", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                GER_Manifesto gER_Manifesto = new GER_Manifesto();
                ShowForm(gER_Manifesto, 1);
            }
        }

        private void navBarItem28_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                new GER_ExportarNFCe().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem16_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_BandeiraCartao fCO_BandeiraCartao = new FCO_BandeiraCartao();
            ShowForm(fCO_BandeiraCartao, 1);
        }

        private void navBarItem31_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_SaldoEstoqueInicial fCO_SaldoEstoqueInicial = new FCO_SaldoEstoqueInicial();
            ShowForm(fCO_SaldoEstoqueInicial, 1);
        }

        private void navBarItem31_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                new FormConsultaSintegra(string.Empty).ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem32_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                Migrador migrador = new Migrador();
                migrador.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem33_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

        }

        private void navBarItem34_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_PerfilAcesso fCO_PerfilAcesso = new FCO_PerfilAcesso();
            ShowForm(fCO_PerfilAcesso, 1);
        }

        private void navBarItem35_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                new FCONFIG_Email().ShowDialog(this);
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                Tabelas tabelas = new Tabelas();
                tabelas.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void sistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ovMenu_Sistema.Show(new Point(MousePosition.X, MousePosition.Y));
            //ovMenu_Cadastros.Show(new Point(MousePosition.X, MousePosition.Y));
        }

        private void navBarItem36_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                AppAtualizar appAtualizar = new AppAtualizar();
                appAtualizar.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void button6_Click_2(object sender, EventArgs e)
        {


        }

        private void navBarItem37_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_FormaDePagamento fCO_FormaDePagamento = new FCO_FormaDePagamento();
            ShowForm(fCO_FormaDePagamento, 1);
        }

        private void navBarItem38_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            BIVendedoresEClientes dash = new BIVendedoresEClientes();
            ShowForm(dash, 1);
        }

        private void navBarItem39_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_ContaBancaria fCO_FormaContaBancaria = new FCO_ContaBancaria();
            ShowForm(fCO_FormaContaBancaria, 1);
        }

        private void navBarItem40_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
        }

        private void navBarItem41_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_HistoricoFinanceiro FCO_HistoricoFinanceiro = new FCO_HistoricoFinanceiro();
            ShowForm(FCO_HistoricoFinanceiro, 1);
        }

        private void navBarItem42_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_CentroCusto fCO_TipoTitulo = new FCO_CentroCusto();
            ShowForm(fCO_TipoTitulo, 1);
        }

        private void navBarItem43_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                ConfiguracaoSistema configuracao = new ConfiguracaoSistema();
                configuracao.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }

        }

        public int CategoriaID { get; set; }
        public int MarcaID { get; set; }

        public int NcmID { get; set; }

        public int IDPessoaVendedor { get; set; }

        public int GrupoEmpresaID { get; set; }

        public int EmpresaID { get; set; }

        public int TipoDeOPeracaoID { get; set; }

        public int PessoaTransportadorID { get; set; }

        public int PessoaCompradorID { get; set; }

        public int PessoaForncedorID { get; set; }

        private void button6_Click_3(object sender, EventArgs e)
        {
           
        }

        public string status { get; set; }
    

      

        private void navBarItem44_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                ReciboAvulsoForm reciboAvulsoForm = new ReciboAvulsoForm();
                reciboAvulsoForm.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem45_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                FiltroComercialVenda filtroComercial = new FiltroComercialVenda();
                filtroComercial.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem46_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Categorias FCO_Categorias = new FCO_Categorias();
            ShowForm(FCO_Categorias, 1);
        }

        private void navBarItem47_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            FCO_Marca FCO_Marca = new FCO_Marca();
            ShowForm(FCO_Marca, 1);
        }

        private void monitamentoDeTarefasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tarefasForm = new TarefasForm();
                tarefasForm.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem40_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                new ModeloImpressaoDav().ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem1_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new FCO_UnidadeMedida(), 1);
        }

        private void navBarItem48_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new FCO_FluxoCaixa(), 1);
        }

        private void navBarItem49_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                FiltroComercialCompra resultadosComerciais = new FiltroComercialCompra();
                resultadosComerciais.ShowDialog();
            }
            catch (Exception exception)
            {
                Alert(exception.Message);
            }
        }

        private void navBarItem50_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new GFIN_MEstoquePorVenda(), 1);
        }

        private void navBarItem51_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new GFIN_MEstoquePorCompra(), 1);
        }

        private void navBarBIVendasEProdutos_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            frmBiPDV frmBiPDV = new frmBiPDV();
            ShowForm(frmBiPDV, 1);
        }

        private void navBIVendedoresEClientes_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            BIVendedoresEClientes frmBiPDV = new BIVendedoresEClientes();
            ShowForm(frmBiPDV, 1);
        }

        private void navBarBIFornecedoresECompras_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new BIFornecedoresECompras(), 1);
        }

        private void navBaBIContasAPagar_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new BIContasAPagar(), 1);
        }

        private void navBarBIContasAReceber_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new BIContasAReceber(), 1);
        }

        private void navBarBIEstoque_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new BIEstoque(), 1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GFIN_Duplicata gFIN_Duplicata = new GFIN_Duplicata();
            gFIN_Duplicata.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FCO_ContaCobranca fCA_ContaCobranca = new FCO_ContaCobranca();
            fCA_ContaCobranca.ShowDialog();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            FCO_ContaCobranca fCO_ContaCobranca = new FCO_ContaCobranca();
            fCO_ContaCobranca.ShowDialog();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void navBarItem52_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new GFIN_Duplicata(), 1);
        }

        private void navBarItem53_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new FCO_ContaCobranca(), 1);
        }

        private void navBarItem54_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new GFIN_MEstoquePorProduto(), 1);
        }

        private void navBarItem55_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new FCO_Servico(), 1);
        }

        private void navBarItem56_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            ShowForm(new FCO_OrdemDeServico(), 1);
        }

        private void migradorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Migrador().ShowDialog();
        }

        private void sincronizadorParaONovoSistemaWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }

        private void navBarItemConversao_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            
        }

        private void conversaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCO_ConversaoUM fCO_ConversaoUM = new FCO_ConversaoUM();
            ShowForm(fCO_ConversaoUM, 1);
        }
    }
}

