﻿using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using PDV.VIEW.App_Context;
using System;
using PDV.DAO.DB.Utils;
using PDV.VIEW.Forms.Util;
using PDV.VIEW.Forms.Seletores;
using PDV.DAO.Custom;
using PDV.UTIL;
using System.Data;
using PDV.DAO.Entidades.Estoque.Suprimentos;
using System.Drawing;
using DevExpress.XtraEditors;
using PDV.DAO.Entidades.Estoque.InventarioEstoque;
using BaseProdutos;
using PDV.DAO.Enum;
using PDV.UTIL.InventarioUtil;

namespace PDV.VIEW.Forms.Cadastro
{
    public partial class FCA_Produtos : DevExpress.XtraEditors.XtraForm
    {
        private string NOME_TELA = "CADASTRO DE PRODUTOS";
        private Produto Produto = null;
        private DataTable ProdutosFornecedor = null;
        public List<ProdutoComposicao> Composicoes { get; set; } = null;

        /* Combos */
        private List<OrigemProduto> Origem = FuncoesOrigemProduto.GetOrigensProduto();
        private List<UnidadeMedida> Unidades = FuncoesUnidadeMedida.GetUnidadesMedida();
        private Ncm NcmSelecionado = null;
        private List<Marca> Marcas = FuncoesMarca.GetMarcasDeProduto();
        private List<Categoria> Categorias = FuncoesCategoria.GetCategorias();
        private List<Categoria> SubCategorias = FuncoesCategoria.GetCategorias();
        private List<IntegracaoFiscal> IntegNFe = FuncoesIntegracaoFiscal.GetIntegracoes();
        private List<IntegracaoFiscal> IntegNFCe = FuncoesIntegracaoFiscal.GetIntegracoes();
        private List<Almoxarifado> AlmoxEntrada = FuncoesAlmoxarifado.GetAlmoxarifados();
        private List<Almoxarifado> AlmoxSaida = FuncoesAlmoxarifado.GetAlmoxarifados();
        public static readonly decimal[] idsMenuItem = { 15 };

        public bool IsNovoProduto { get => Produto.IDProduto == -1; }

        public FCA_Produtos(Produto _P, bool pdv = false)
        {
            InitializeComponent();
            Produto = _P;
            PreencherCampos();

            CarregarPermissoes(pdv);

            if (IsNovoProduto)
                ovTXT_Identificacao_EAN.Select();

        }

        private void PreencherCampos()
        {
            PreencherUnidadeDeMedida();

            ovCMB_Tributos_Origem.DataSource = Origem;
            ovCMB_Tributos_Origem.DisplayMember = "descricao";
            ovCMB_Tributos_Origem.ValueMember = "codigo";
            ovCMB_Tributos_Origem.SelectedItem = null;

            ovCMB_Marca.DataSource = Marcas;
            ovCMB_Marca.DisplayMember = "descricao";
            ovCMB_Marca.ValueMember = "idmarca";
            ovCMB_Marca.SelectedItem = null;

            ovCMB_Categoria.DataSource = Categorias;
            ovCMB_Categoria.DisplayMember = "descricao";
            ovCMB_Categoria.ValueMember = "idcategoria";
            ovCMB_Categoria.SelectedItem = null;

            ovCMB_SubCategoria.DataSource = SubCategorias;
            ovCMB_SubCategoria.DisplayMember = "descricao";
            ovCMB_SubCategoria.ValueMember = "idcategoria";
            ovCMB_SubCategoria.SelectedItem = null;

            ovCMB_IntegracaoNFe.DataSource = IntegNFe;
            ovCMB_IntegracaoNFe.DisplayMember = "descricao";
            ovCMB_IntegracaoNFe.ValueMember = "idintegracaofiscal";
            ovCMB_IntegracaoNFe.SelectedItem = null;

            ovCMB_IntegracaoNFCe.DataSource = IntegNFCe;
            ovCMB_IntegracaoNFCe.DisplayMember = "descricao";
            ovCMB_IntegracaoNFCe.ValueMember = "idintegracaofiscal";
            ovCMB_IntegracaoNFCe.SelectedItem = null;

            ovCMB_AlmoxEntrada.DataSource = AlmoxEntrada;
            ovCMB_AlmoxEntrada.DisplayMember = "descricaoapresentacao";
            ovCMB_AlmoxEntrada.ValueMember = "idalmoxarifado";

            ovCMB_AlmoxSaida.DataSource = AlmoxSaida;
            ovCMB_AlmoxSaida.DisplayMember = "descricaoapresentacao";
            ovCMB_AlmoxSaida.ValueMember = "idalmoxarifado";

            cmbTipoDeProduto.SelectedIndex = Produto.TipoDeProduto - 1;


            PreencherComposicoes(true);
            //if (Contexto.USUARIOLOGADO.Root != 1)
            //    button2.Enabled = Contexto.ITENSMENU.Where(o => o.Codigo == 12).FirstOrDefault() != null;

            PreencherTela();

            ovTXT_Identificacao_EAN.GotFocus += OvTXT_Identificacao_EAN_GotFocus;
            metroTabControl2.SelectedTab = metroTabPage5;

            ovTXT_ValorCusto.AplicaAlteracoes();
            ovTXT_ValorVenda.AplicaAlteracoes();
            ovTXT_PercentualGanho.AplicaAlteracoes();
            ovTXT_TribRedBCIcms.AplicaAlteracoes();
            ovTXT_TribRedBCIcmsST.AplicaAlteracoes();
            ovTXT_Trib_ICMSDeferido.AplicaAlteracoes();
            ovTXT_TribMVA.AplicaAlteracoes();
            ovTXT_Trib_AliqIPI.AplicaAlteracoes();
            ovTXT_TribAliqPIS.AplicaAlteracoes();
            ovTXT_TribAliqCofins.AplicaAlteracoes();
        }

        private void CarregarPermissoes(bool pdv)
        {
            if (!pdv)
            {
                var idsMenuItemCategoria = FCA_Categoria.idsMenuItem;
                PermissoesUtil.VerificarPermissaoParaTela(idsMenuItemCategoria, ref buttonAdicionarCategoria);
                PermissoesUtil.VerificarPermissaoParaTela(idsMenuItemCategoria, ref buttonAdicionarSubCategoria);
                PermissoesUtil.VerificarPermissaoParaTela(FCA_UnidadeMedida.idsMenuItem, ref buttonAdicionarUnidade);
                PermissoesUtil.VerificarPermissaoParaTela(FCA_Marca.idsMenuItem, ref buttonAdicionarMarca);
            }
        }

        private void PreencherUnidadeDeMedida()
        {
            ovCMB_UnidadeMedida.DisplayMember = "sigla";
            ovCMB_UnidadeMedida.ValueMember = "idunidadedemedida";
            ovCMB_UnidadeMedida.DataSource = Unidades;
            ovCMB_UnidadeMedida.SelectedItem = null;
        }


        private void PreencherComposicoes(bool banco)
        {
            if (banco)
                Composicoes = FuncoesProdutoComposicao.GetComposicoesPorComposto(Produto.IDProduto);

            gridControlComposicoes.DataSource = Composicoes;

            FormatarGridComposicao();
        }

        public Produto GetProduto() { return Produto; }
        private void OvTXT_Identificacao_EAN_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ovTXT_Identificacao_EAN.Text))
                ovTXT_Identificacao_EAN.SelectionStart = 0;
            else
                ovTXT_Identificacao_EAN.SelectAll();
        }

        private void PreencherTela()
        {
            ovTXT_Identificacao_CodigoProduto.Text = string.IsNullOrEmpty(Produto.Codigo) ? ZeusUtil.GetProximoCodigo("PRODUTO", "CODIGO").ToString() : Produto.Codigo;
            ovTXT_Identificacao_DescricaoProduto.Text = Produto.Descricao;
            CodBalancaTextBox.Text = Produto.CodigoBalanca;

            NcmSelecionado = FuncoesNcm.GetNCM(Produto.IDNCM);
            ovTXT_BuscarNCM.Text = NcmSelecionado == null ? string.Empty : NcmSelecionado.Codigo.ToString();
            ovTXT_NCM.Text = NcmSelecionado == null ? string.Empty : NcmSelecionado.Descricao;
            ovTXT_Identificacao_EAN.Text = Produto.EAN;
            ovCMB_UnidadeMedida.SelectedItem = Unidades.Where(o => o.IDUnidadeDeMedida == Produto.IDUnidadeDeMedida).FirstOrDefault();
            ovCMB_Tributos_Origem.SelectedItem = Origem.Where(o => o.IDOrigemProduto == Produto.IDOrigemProduto).FirstOrDefault();
            ovTXT_Identificacao_EXTIPI.Text = Produto.EXTipi;
            ovTXT_Identificacao_CEST.Text = Produto.CEST;
            ovCKB_Ativo.Checked = Produto.Ativo == 1;
            TXT_estoque_maximo.Text = Produto.EstoqueMaximo.ToString();
            TXT_estoque_minimo.Text = Produto.EstoqueMinimo.ToString();
            textEditSaldoEstoque.Text = Produto.SaldoEstoque.ToString();
            textEditSaldoEstoque.Enabled = Produto.IDProduto == -1;
            ovCKB_AlterarDescricao.Checked = Produto.AlterarDescricao == 1;
            chbxParaVender.Checked = Produto.ParaVender;
            VenderSemSaldocheckBox.Checked = Produto.VenderSemSaldo == 1 ? true : false;



            if (Produto.IDMarca.HasValue)
                ovCMB_Marca.SelectedItem = Marcas.Where(o => o.IDMarca == Produto.IDMarca).FirstOrDefault();

            if (Produto.IDCategoria.HasValue)
                ovCMB_Categoria.SelectedItem = Categorias.Where(o => o.IDCategoria == Produto.IDCategoria).FirstOrDefault();

            if (Produto.IDSubCategoria.HasValue)
                ovCMB_SubCategoria.SelectedItem = SubCategorias.Where(o => o.IDCategoria == Produto.IDSubCategoria).FirstOrDefault();

            ovTXT_ValorCusto.Value = Produto.ValorCusto;
            ovTXT_ValorVenda.Value = Produto.ValorVenda;
            ovTXT_ValorVendaPrazo.Value = Produto.ValorVendaPrazo;
            AtualizaPercentual();
            AtualizaPercentualPrazo();

            /* Aba Tribitação */
            ovCMB_IntegracaoNFe.SelectedItem = IntegNFe.Where(o => o.IDIntegracaoFiscal == (Produto.IDIntegracaoFiscalNFe ?? -1)).FirstOrDefault();
            ovCMB_IntegracaoNFCe.SelectedItem = IntegNFCe.Where(o => o.IDIntegracaoFiscal == (Produto.IDIntegracaoFiscalNFCe ?? -1)).FirstOrDefault();
            ovTXT_TribMVA.Value = Produto.Trib_MVA;
            ovTXT_TribRedBCIcms.Value = Produto.Trib_RedBCICMS;
            ovTXT_TribRedBCIcmsST.Value = Produto.Trib_RedBCICMSST;
            ovTXT_Trib_AliqIPI.Value = Produto.Trib_AliqIPI;
            ovTXT_TribAliqPIS.Value = Produto.Trib_AliqPIS;
            ovTXT_TribAliqCofins.Value = Produto.Trib_AliqCOFINS;
            ovTXT_Trib_ICMSDeferido.Value = Produto.Trib_AliqICMSDif;

            /* Almoxarifados */
            ovCMB_AlmoxEntrada.SelectedItem = AlmoxEntrada.Where(o => o.IDAlmoxarifado == Produto.IDAlmoxarifadoEntrada).FirstOrDefault();
            ovCMB_AlmoxSaida.SelectedItem = AlmoxSaida.Where(o => o.IDAlmoxarifado == Produto.IDAlmoxarifadoSaida).FirstOrDefault();
            CarregarProdutosFornecedor(true);

            //Imagem
            linkTextEdit.Text = Produto.ImagemProdutoLink;
            imageProdutoPictureBox.Image = FuncoesProduto.ConvertByteToImage(Produto.ImagemProduto);

        }

        private void ovBTN_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void ovBTN_Salvar_Click(object sender, EventArgs e)
        {
            if (!ValidouDados())
                return;
            try
            {
                PDVControlador.BeginTransaction();

                DAO.Enum.TipoOperacao Op = DAO.Enum.TipoOperacao.UPDATE;


                /* Informações de Identificação */
                Produto.Codigo = ovTXT_Identificacao_CodigoProduto.Text;
                Produto.Descricao = ovTXT_Identificacao_DescricaoProduto.Text.Trim().ToUpper();
                Produto.CodigoBalanca = CodBalancaTextBox.Text;
                Produto.IDNCM = NcmSelecionado.IDNCM;
                Produto.EAN = ovTXT_Identificacao_EAN.Text;
                Produto.EXTipi = ovTXT_Identificacao_EXTIPI.Text;
                Produto.TipoDeProduto = cmbTipoDeProduto.SelectedIndex + 1;

                if (ovCMB_Tributos_Origem.SelectedItem == null)
                    ovCMB_Tributos_Origem.SelectedIndex = 0;

                Produto.IDOrigemProduto = (ovCMB_Tributos_Origem.SelectedItem as OrigemProduto).IDOrigemProduto;
                Produto.IDUnidadeDeMedida = (ovCMB_UnidadeMedida.SelectedItem as UnidadeMedida).IDUnidadeDeMedida;
                Produto.CEST = ovTXT_Identificacao_CEST.Text;
                Produto.ValorCusto = ovTXT_ValorCusto.Value;
                Produto.ValorVenda = ovTXT_ValorVenda.Value;
                Produto.ParaVender = chbxParaVender.Checked;
                Produto.AlterarDescricao = ovCKB_AlterarDescricao.Checked ? 1 : 0;
                Produto.ValorVendaPrazo = ovTXT_ValorVendaPrazo.Value;
                Produto.EstoqueMinimo = Decimal.Parse(TXT_estoque_minimo.Text);
                Produto.EstoqueMaximo = Decimal.Parse(TXT_estoque_maximo.Text);
                Produto.SaldoEstoque = Convert.ToDecimal(textEditSaldoEstoque.Text);
                Produto.VenderSemSaldo = VenderSemSaldocheckBox.Checked == true ? 1 : 0;

                Produto.IDMarca = null;
                if (ovCMB_Marca.SelectedItem != null)
                    Produto.IDMarca = (ovCMB_Marca.SelectedItem as Marca).IDMarca;

                Produto.IDCategoria = null;
                if (ovCMB_Categoria.SelectedItem != null)
                    Produto.IDCategoria = (ovCMB_Categoria.SelectedItem as Categoria).IDCategoria;

                Produto.IDSubCategoria = null;
                if (ovCMB_SubCategoria.SelectedItem != null)
                    Produto.IDSubCategoria = (ovCMB_SubCategoria.SelectedItem as Categoria).IDCategoria;

                Produto.IDIntegracaoFiscalNFe = IntegracaoFiscal.IntegracaoNFe;
                if ((ovCMB_IntegracaoNFe.SelectedItem as IntegracaoFiscal) != null)
                    Produto.IDIntegracaoFiscalNFe = (ovCMB_IntegracaoNFe.SelectedItem as IntegracaoFiscal).IDIntegracaoFiscal;

                Produto.IDIntegracaoFiscalNFCe = IntegracaoFiscal.IntegracaoNFCe;
                if ((ovCMB_IntegracaoNFCe.SelectedItem as IntegracaoFiscal) != null)
                    Produto.IDIntegracaoFiscalNFCe = (ovCMB_IntegracaoNFCe.SelectedItem as IntegracaoFiscal).IDIntegracaoFiscal;

                Produto.Trib_MVA = ovTXT_TribMVA.Value;
                Produto.Trib_RedBCICMS = ovTXT_TribRedBCIcms.Value;
                Produto.Trib_RedBCICMSST = ovTXT_TribRedBCIcmsST.Value;
                Produto.Trib_AliqIPI = ovTXT_Trib_AliqIPI.Value;
                Produto.Trib_AliqPIS = ovTXT_TribAliqPIS.Value;
                Produto.Trib_AliqCOFINS = ovTXT_TribAliqCofins.Value;
                Produto.Ativo = ovCKB_Ativo.Checked ? 1 : 0;
                Produto.Trib_AliqICMSDif = ovTXT_Trib_ICMSDeferido.Value;

                Produto.ImagemProdutoLink = linkTextEdit.Text;
                Produto.ImagemProduto = FuncoesProduto.ConvertImageToByte(imageProdutoPictureBox.Image);

                SalvarComposicoes();



                if (IsNovoProduto)
                {
                    Produto.IDProduto = Sequence.GetNextID("PRODUTO", "IDPRODUTO");
                    SalvarProduto(TipoOperacao.INSERT);

                    var inventario = new Inventario()
                    {
                        IDInventario = Sequence.GetNextID("INVENTARIO", "IDINVENTARIO"),
                        DataInventario = DateTime.Now
                    };

                    FuncoesInventario.Salvar(inventario, DAO.Enum.TipoOperacao.INSERT);

                    var itemInventario = new ItemInventario()
                    {
                        IDItemInventario = Sequence.GetNextID("ITEMINVENTARIO", "IDITEMINVENTARIO"),
                        Quantidade = Produto.SaldoEstoque,
                        IDAlmoxarifado = Produto.IDAlmoxarifadoEntrada,
                        IDProduto = Produto.IDProduto,
                        IDInventario = inventario.IDInventario
                    };
                    FuncoesInventario.SalvarItemInventario(itemInventario, DAO.Enum.TipoOperacao.INSERT);
                    InventarioUtil.Processar(itemInventario);
                }
                else
                {
                    SalvarProduto(Op);
                }

                PDVControlador.Commit();
                MessageBox.Show(this, "Produto salvo com sucesso.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception Ex)
            {
                PDVControlador.Rollback();
                MessageBox.Show(this, Ex.Message, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SalvarProduto(TipoOperacao Op)
        {

            if (!FuncoesProduto.SalvarProduto(Produto, Op))
                throw new Exception("Não foi possível salvar o Produto.");
        }

        private void SalvarComposicoes()
        {
            FuncoesProdutoComposicao.RemoverPorProdutoComposto(Produto.IDProduto);

            foreach (var composicao in Composicoes)
            {
                if (!FuncoesProdutoComposicao.Salvar(composicao))
                    throw new Exception("Não foi possível salvar as composições");
            }
        }

        private bool ValidouDados()
        {
            if (ovCMB_UnidadeMedida.Text.Contains("kg") || ovCMB_UnidadeMedida.Text.Contains("kilo") || ovCMB_UnidadeMedida.Text.Contains("Quilo") || ovCMB_UnidadeMedida.Text.Contains("KG"))
            {
                if (CodBalancaTextBox.Text.Length != 6)
                {
                    MessageBox.Show(this, "O código da balança deve ter 6 posições.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CodBalancaTextBox.SelectAll();
                    return false;
                }
            }

            /* Aba de Identificação */
            if (string.IsNullOrEmpty(ovTXT_Identificacao_EAN.Text.Trim()))
            {
                MessageBox.Show(this, "Preencha o Código de Barras.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ovTXT_Identificacao_EAN.SelectAll();
                return false;
            }


            if (FuncoesProduto.ExisteCodigoDeBarras(Produto.IDProduto, ovTXT_Identificacao_EAN.Text.Trim()))
            {
                MessageBox.Show(this, "Outro produto já possui este código de barras.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ovTXT_Identificacao_EAN.SelectAll();
                return false;
            }
        
        if (ovTXT_Identificacao_DescricaoProduto.Text == "" || string.IsNullOrEmpty(ovTXT_Identificacao_DescricaoProduto.Text))
            {
            if (FuncoesProduto.ExisteCodigoBalanca(Produto.IDProduto, CodBalancaTextBox.Text.Trim()))
            {
                MessageBox.Show(this, "Outro produto já possui este código de balança.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
        ovTXT_Identificacao_EAN.SelectAll();
                return false;
            }
}
            if (string.IsNullOrEmpty(ovTXT_Identificacao_CodigoProduto.Text.Trim()))
            {
                MessageBox.Show(this, "Preencha o Código do Produto.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ovTXT_Identificacao_CodigoProduto.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(ovTXT_Identificacao_DescricaoProduto.Text.Trim()))
            {
                MessageBox.Show(this, "Preencha a Descrição do Produto.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ovTXT_Identificacao_DescricaoProduto.SelectAll();
                return false;
            }
            if (cmbTipoDeProduto.SelectedIndex ==  -1)
            {
                MessageBox.Show(this, "Preencha o Tipo de Produto.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (NcmSelecionado == null)
            {
                MessageBox.Show(this, "Selecione a NCM.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //if (string.IsNullOrEmpty(ovTXT_Identificacao_EAN.Text))
            //{
            //    MessageBox.Show(this, "Selecione o Código de Barras.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ovTXT_Identificacao_EAN.SelectAll();
            //    return false;
            //}

            if (ovCMB_UnidadeMedida.SelectedItem == null)
            {
                MessageBox.Show(this, "Selecione a Unidade de Medida.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ovCMB_UnidadeMedida.SelectAll();
                return false;
            }

            //if (ovTXT_ValorCusto.Value == 0)
            //{
            //    MessageBox.Show(this, "O Preço de Custo deve ser maior que zero.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ovTXT_ValorVenda.Select();
            //    return false;
            //}

            //if (ovTXT_ValorVenda.Value == 0)
            //{
            //    MessageBox.Show(this, "O Preço de Venda deve ser maior que zero.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ovTXT_ValorVenda.Select();
            //    return false;
            //}

            if (ovCMB_AlmoxEntrada.SelectedItem == null)
            {
                MessageBox.Show(this, "Selecione o Almoxarifado de Entrada.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (ovCMB_AlmoxSaida.SelectedItem == null)
            {
                MessageBox.Show(this, "Selecione o Almoxarifado de Saida.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }


            //if (ovCMB_IntegracaoNFe.SelectedItem == null)
            //{
            //    MessageBox.Show(this, "Selecione a Integração Fiscal NF-e.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ovCMB_IntegracaoNFe.SelectAll();
            //    return false;
            //}
            //
            //if (ovCMB_IntegracaoNFCe.SelectedItem == null)
            //{
            //    MessageBox.Show(this, "Selecione a Integração Fiscal NFC-e.", NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ovCMB_IntegracaoNFCe.SelectAll();
            //    return false;
            //}

                return true;
        }

        private void ovTXT_BuscarNCM_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NcmSelecionado = FuncoesNcm.GetNCMPorCodigo(Convert.ToDecimal(ZeusUtil.SomenteNumeros(ovTXT_BuscarNCM.Text)));
                if (NcmSelecionado == null)
                {
                    MessageBox.Show(this, "NCM não encontrado.");
                    ovTXT_BuscarNCM.Select();
                    ovTXT_BuscarNCM.SelectAll();
                }
                else
                {
                    ovTXT_BuscarNCM.Text = NcmSelecionado == null ? string.Empty : NcmSelecionado.Codigo.ToString();
                    ovTXT_NCM.Text = NcmSelecionado == null ? string.Empty : NcmSelecionado.Descricao;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FCA_UnidadeMedida t = new FCA_UnidadeMedida(new UnidadeMedida());
            t.ShowDialog(this);
            Unidades = FuncoesUnidadeMedida.GetUnidadesMedida();
            ovCMB_UnidadeMedida.DataSource = Unidades;
        }

        private void AtualizarUnidadesMedidasCadastradas(decimal? IDUnComerial)
        {
            Unidades = FuncoesUnidadeMedida.GetUnidadesMedida();
            ovCMB_UnidadeMedida.DataSource = Unidades;

            if (IDUnComerial.HasValue)
                ovCMB_UnidadeMedida.SelectedItem = Unidades.Where(o => o.IDUnidadeDeMedida == IDUnComerial.Value).FirstOrDefault();
        }

        private void ovTXT_ValorCusto_ValueChanged(object sender, EventArgs e)
        {
            AtualizaPercentual();
            AtualizaPercentualPrazo();
        }

        private void ovTXT_ValorVenda_ValueChanged(object sender, EventArgs e)
        {
            AtualizaPercentual();
        }

        private void ovTXT_ValorVendaPrazo_ValueChanged(object sender, EventArgs e)
        {
            AtualizaPercentualPrazo();
        }

        private void ovTXT_PercentualGanho_ValueChanged(object sender, EventArgs e)
        {
            AtualizaCustoVenda();
        }

        private void AtualizaPercentual()
        {
            decimal Diferenca = ovTXT_ValorVenda.Value == 0 || ovTXT_ValorCusto.Value == 0 ? 0 : ovTXT_ValorVenda.Value - ovTXT_ValorCusto.Value;
            if (Diferenca <= 0)
            {
                ovTXT_PercentualGanho.Value = 0;
                return;
            }

            ovTXT_PercentualGanho.Value = (Diferenca / ovTXT_ValorCusto.Value) * 100;


        }

        private void AtualizaPercentualPrazo()
        {
            decimal Diferenca = ovTXT_ValorVendaPrazo.Value == 0 || ovTXT_ValorCusto.Value == 0 ? 0 : ovTXT_ValorVendaPrazo.Value - ovTXT_ValorCusto.Value;
            if (Diferenca <= 0)
            {
                ovTXT_PercentualGanhoPrazo.Value = 0;
                return;
            }

            ovTXT_PercentualGanhoPrazo.Value = (Diferenca / ovTXT_ValorCusto.Value) * 100;


        }

        private void AtualizaCustoVenda()
        {
            ovTXT_ValorVenda.Value = ovTXT_ValorCusto.Value + ovTXT_ValorCusto.Value * (ovTXT_PercentualGanho.Value / 100);

        }

        private void button1_Click(object sender, EventArgs e)
        {


            FCA_Marca t = new FCA_Marca(new Marca());
            t.SetCheckedEnabledFalse(0);
            t.ShowDialog(this);
            Marcas = FuncoesMarca.GetMarcas();
            ovCMB_Marca.DataSource = Marcas;

        }

        private void AtualizarMarcasCadastradas(decimal? IDMarca)
        {
            Marcas = FuncoesMarca.GetMarcas();

            ovCMB_Marca.DataSource = Marcas;

            if (IDMarca.HasValue)
                ovCMB_Marca.SelectedItem = Marcas.Where(o => o.IDMarca == IDMarca.Value).FirstOrDefault();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FCA_Categoria t = new FCA_Categoria(new Categoria());
            t.ShowDialog(this);
            Categorias = FuncoesCategoria.GetCategorias();
            ovCMB_SubCategoria.DataSource = Categorias;
            ovCMB_Categoria.DataSource = Categorias;
        }

        private void AtualizarCategoriasCadastradas(decimal? IDCategoria, decimal? IDSubCategoria)
        {
            Categorias = FuncoesCategoria.GetCategorias();
            SubCategorias = FuncoesCategoria.GetCategorias();

            ovCMB_Categoria.DataSource = Categorias;
            ovCMB_SubCategoria.DataSource = SubCategorias;

            if (IDCategoria.HasValue)
                ovCMB_Categoria.SelectedItem = Categorias.Where(o => o.IDCategoria == IDCategoria.Value).FirstOrDefault();

            if (IDSubCategoria.HasValue)
                ovCMB_SubCategoria.SelectedItem = SubCategorias.Where(o => o.IDCategoria == IDSubCategoria.Value).FirstOrDefault();
        }

        private void ovTXT_Identificacao_EAN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ovTXT_Identificacao_EAN.Text))
                ovTXT_Identificacao_EAN.SelectionStart = 0;
            else
                ovTXT_Identificacao_EAN.SelectAll();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new FGR_GeradorCodigoProduto(this, ovTXT_Identificacao_CodigoProduto.Text).ShowDialog(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SEL_Ncm SeletorNCM = new SEL_Ncm();
            SeletorNCM.ShowDialog(this);
            if (SeletorNCM.NCMSelecionado != null)
            {
                NcmSelecionado = EntityUtil<Ncm>.ParseDataRow(SeletorNCM.NCMSelecionado);
                ovTXT_BuscarNCM.Text = NcmSelecionado == null ? string.Empty : NcmSelecionado.Codigo.ToString();
                ovTXT_NCM.Text = NcmSelecionado == null ? string.Empty : NcmSelecionado.Descricao;
            }
        }

        private void CarregarProdutosFornecedor(bool Banco)
        {
            if (Banco)
                ProdutosFornecedor = FuncoesProdutoFornecedor.GetCodigosPorProduto(Produto.IDProduto);

            gridControl1.DataSource = ProdutosFornecedor;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            gridView1.BestFitColumns();
            AjustaHeaderTextGridProdutosFornecedor();
        }

        private void AjustaHeaderTextGridProdutosFornecedor()
        {
            
            gridView1.Columns[0].Caption = "FORNECEDOR";
            gridView1.Columns[1].Caption = "CÓDIGO";
            gridView1.Columns[2].Visible = false;
            gridView1.Columns[3].Visible = false;
            gridView1.Columns[4].Visible = false;
        }
        

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog _with1 = openFileDialog1;

                _with1.Filter = ("Image Files |*.png; *.bmp; *.jpg;*.jpeg; *.gif;");
                _with1.FilterIndex = 4;
                //Resetar the file name
                openFileDialog1.FileName = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    imageProdutoPictureBox.Image = Image.FromFile(openFileDialog1.FileName);

                }
                string anexo = Convert.ToString(openFileDialog1.FileName);
                if (anexo != string.Empty)
                {
                    linkTextEdit.Text = FuncoesProduto.AdicionarArquivo((System.IO.FileStream)openFileDialog1.OpenFile()).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FCA_Produtos_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void ovTXT_PercentualGanhoPrazo_ValueChanged(object sender, EventArgs e)
        {
            decimal valor = (ovTXT_PercentualGanhoPrazo.Value + 100) * ovTXT_ValorCusto.Value / 100;
            if (ovTXT_ValorVendaPrazo.Value != valor)
                ovTXT_ValorVendaPrazo.Value = valor;

        }

        private void imageProdutoPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var formComposicao = new FCA_Composicao();
            formComposicao.ShowDialog();

            if (formComposicao.Composicao.IdProduto > 0)
            {
                var composicao = new ProdutoComposicao()
                {
                    IdProdutoComposicao = Sequence.GetNextID("PRODUTOCOMPOSICAO", "IDPRODUTOCOMPOSICAO"),
                    IdProduto = formComposicao.Composicao.IdProduto,
                    IdProdutoComposto = Produto.IDProduto,
                    Quantidade = formComposicao.Composicao.Quantidade,
                    Descricao = FuncoesProduto.GetProduto(formComposicao.Composicao.IdProduto).Descricao
                };
                Composicoes.Add(composicao);
                PreencherComposicoes(false);              
            }
        }

        private void FormatarGridComposicao()
        {
            Grids.FormatColumnType(ref gridViewComposicoes, new List<string>() 
            { 
                "idproduto",
                "idprodutocomposto"
            }, GridFormats.VisibleFalse);
            Grids.FormatGrid(ref gridViewComposicoes);
        }

        private void editarusuariometroButton4_Click(object sender, EventArgs e)
        {
            var id = Grids.GetValorDec(gridViewComposicoes, "IdProdutoComposicao");
            var composicao = Composicoes.SingleOrDefault(c => c.IdProdutoComposicao == id);
            var formComposicao = new FCA_Composicao(composicao);

            formComposicao.ShowDialog();
            
            composicao = formComposicao.Composicao;
            composicao.Descricao = FuncoesProduto.GetProduto(formComposicao.Composicao.IdProduto).Descricao;
            
            PreencherComposicoes(false);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (MensagemSimOuNao("Deseja remover essa composição?") == DialogResult.Yes)
            {
                var id = Grids.GetValorDec(gridViewComposicoes, "IdProdutoComposicao");
                var composicao = Composicoes.SingleOrDefault(c => c.IdProdutoComposicao == id);
                Composicoes.Remove(composicao);
                PreencherComposicoes(false);
            }            
        }

        private DialogResult MensagemSimOuNao(string msg)
        {
            return MessageBox.Show(msg, "Cadastro de Produto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void metroComboBoxTipoDeProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
    
            HabilitarComposicao(cmbTipoDeProduto.SelectedIndex == 1);
        }

        private void HabilitarComposicao(bool habilitar)
        {
            gridControlComposicoes.Enabled = 
                btnEditarComposicao.Enabled = 
                btnNovaComposicao.Enabled = 
                btnRemoverComposicao.Enabled = habilitar;

        }

        private void Alert(string message)
        {
            MessageBox.Show(message, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ovTXT_Identificacao_EAN_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && IsNovoProduto && ovTXT_Identificacao_EAN.Text != "")
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var produtosBase = BaseProdutosController.GetProdutosPorEAN(ovTXT_Identificacao_EAN.Text);
                    Cursor.Current = Cursors.Default;
                    if (produtosBase.Count == 0)
                        throw new Exception("Não há nenhum produto com o EAN informado na base de dados");
                    if (produtosBase.Count == 1)
                    {
                        Produto = ConversorProdutoBase.GerarProdutoLocal(produtosBase.FirstOrDefault());
                        PreencherCampos();
                    }
                }
                catch (Exception exception)
                {
                    Alert(exception.Message);
                }

            }
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
