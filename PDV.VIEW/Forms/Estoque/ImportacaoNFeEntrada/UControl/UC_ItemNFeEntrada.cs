using System.Windows.Forms;
using PDV.DAO.Entidades.Estoque.NFeImportacao;
using PDV.DAO.Entidades.Estoque.Suprimentos;
using System.Collections.Generic;
using PDV.CONTROLER.Funcoes;
using System.Drawing;
using System;
using System.Linq;
using PDV.DAO.Entidades;
using PDV.UTIL.Components;
using PDV.VIEW.Forms.Cadastro;
using System.Globalization;
using PDV.VIEW.Forms.Cadastro.Suprimentos;

namespace PDV.VIEW.Forms.Estoque.ImportacaoNFeEntrada.UControl
{
    public partial class UC_ItemNFeEntrada : UserControl
    {
        public ItemNFeEntrada _ItemNFeEntrada = null;
        public ConversaoUnidadeDeMedida _Conversao = null;
        public List<ConversaoUnidadeDeMedida> Conversoes = null;
        public List<Almoxarifado> Almoxarifados = null;
        public List<IntegracaoFiscal> IntegracaoFiscals = null;

        public UC_ItemNFeEntrada(ItemNFeEntrada ItemNFeEntrada)
        {
            InitializeComponent();
            _ItemNFeEntrada = ItemNFeEntrada;

            Almoxarifados = FuncoesAlmoxarifado.GetAlmoxarifados();
            ovCMB_AlmoxarifadoEntrada.DataSource = Almoxarifados;
            ovCMB_AlmoxarifadoEntrada.DisplayMember = "descricaoapresentacao";
            ovCMB_AlmoxarifadoEntrada.ValueMember = "idalmoxarifado";
            ovCMB_AlmoxarifadoEntrada.SelectedItem = null;


            IntegracaoFiscals = FuncoesIntegracaoFiscal.GetIntegracoes();
            metroComboBoxIntegracaoFiscal.DataSource = IntegracaoFiscals;
            metroComboBoxIntegracaoFiscal.DisplayMember = "descricao";
            metroComboBoxIntegracaoFiscal.ValueMember = "idintegracaoFiscal";
            metroComboBoxIntegracaoFiscal.SelectedItem = null;






            CarregaConversoes();
            button1.PerformClick();
            PreencherTela();
        }

        private void PreencherTela()
        {
            if (_ItemNFeEntrada == null)
                return;

            ovTXT_Ean.Text = !_ItemNFeEntrada.CEAN.HasValue ? "<Não Informado>" : _ItemNFeEntrada.CEAN.ToString();
            ovTXT_Produto.Text = _ItemNFeEntrada.DescricaoProduto;
            ovTXT_UNEntrada.Text = _ItemNFeEntrada.UNEntrada;
            ovTXT_UNSaida.Text = _ItemNFeEntrada.UNSaida;
            ovTXT_QtdeEntrada.Text = _ItemNFeEntrada.QuantidadeEntrada.ToString("n4");
            ovTXT_QtdeSaida.Text = _ItemNFeEntrada.QuantidadeSaida.ToString("n4");
            ovTXT_ValorCusto.Text = _ItemNFeEntrada.Valor.ToString("c2");
            ovTXT_ValorTotal.Text = (_ItemNFeEntrada.Valor * _ItemNFeEntrada.QuantidadeEntrada).ToString("c2");

            //txtPreçoDeVenda.Text = "";
            //txtMargemLucro.Text = "";
            //calcularMargemDeLucroPorValorVenda();

            if (_ItemNFeEntrada.IDProduto != -1)
            {
                ovCMB_AlmoxarifadoEntrada.SelectedItem = Almoxarifados.Where(o => o.IDAlmoxarifado == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDAlmoxarifadoEntrada).FirstOrDefault();
                metroComboBoxIntegracaoFiscal.SelectedItem = IntegracaoFiscals.Where(o => o.IDIntegracaoFiscal == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDIntegracaoFiscalNFe).FirstOrDefault();
            }

            AtualizaLocalizacao();
        }

        private void ovCKB_IncluirItem_CheckedChanged(object sender, System.EventArgs e)
        {
            ovBTN_Localizar.Enabled = !ovCKB_IncluirItem.Checked;
            buAddItem.Enabled = !ovCKB_IncluirItem.Checked;
            if (!ovCKB_IncluirItem.Checked)
            {
                ovTXT_Status.Text = "<Item Não Localizado>";
                ovTXT_Status.ForeColor = Color.DarkRed;
            }
            else
            {
                ovTXT_Status.Text = "<Item Será Inserido>";
                ovTXT_Status.ForeColor = Color.DarkGreen;
                ovBTN_Localizar.Enabled = false;
            }
        }

        private void ovBTN_Localizar_Click(object sender, System.EventArgs e)
        {
            FEST_SeletorProduto Seletor = new FEST_SeletorProduto(Convert.ToDecimal(_ItemNFeEntrada.NCM));
            Seletor.ShowDialog(this);

            if (Seletor.drProduto != null)
            {
                _ItemNFeEntrada.IDProduto = Convert.ToDecimal(Seletor.drProduto["IDPRODUTO"]);
                ovCMB_AlmoxarifadoEntrada.SelectedItem = Almoxarifados.Where(o => o.IDAlmoxarifado == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDAlmoxarifadoEntrada).FirstOrDefault();
                metroComboBoxIntegracaoFiscal.SelectedItem = IntegracaoFiscals.Where(o => o.IDIntegracaoFiscal == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDIntegracaoFiscalNFe).FirstOrDefault();
                AtualizaLocalizacao();
                ovBTN_Localizar.Enabled = true;
                ovCKB_IncluirItem.Enabled = false;
                CarregaConversoes();
                _ItemNFeEntrada.IDProduto = Convert.ToDecimal(Seletor.drProduto["IDPRODUTO"]);
                ovTXT_Ean.Text = Seletor.drProduto["EAN"].ToString();
                ovTXT_Produto.Text = Seletor.drProduto["descricao"].ToString();
                //puxa unidade de saida que foi cadastrada no produto
                /*var produto = FuncoesProduto.GetProduto(Convert.ToDecimal(Seletor.drProduto["IDPRODUTO"]));
                var unidadeMedida = FuncoesUnidadeMedida.GetUnidadeMedida(produto.IDUnidadeDeMedida);
                ovTXT_UNSaida.Text = unidadeMedida.Sigla;*/

            }
        }

        private void AtualizaLocalizacao()
        {
            if (_ItemNFeEntrada.IDProduto == -1)
            {
                ovTXT_Status.Text = "<Item Não Localizado>";
                ovTXT_Status.ForeColor = Color.DarkRed;
                btnEditarProduto.Enabled = false;
                checkBoxPrecoAtual.Enabled = false;
                checkBoxPrecoAtual.Checked = false;
            }
            else
            {
                ovTXT_Status.Text = "<Item Localizado>";
                ovTXT_Status.ForeColor = Color.DarkGreen;
                ovBTN_Localizar.Enabled = false;
                ovCKB_IncluirItem.Enabled = false;
                ovCKB_IncluirItem.Checked = false;
                buAddItem.Enabled = false;
                btnEditarProduto.Enabled = true;
                checkBoxPrecoAtual.Enabled = true;
            }
        }

        private void CarregaConversoes()
        {
            Conversoes = FuncoesConversaoUnidadeMedida.GetConversoesPorProduto(_ItemNFeEntrada.IDProduto);
            ovCMB_FatorConversao.DataSource = Conversoes;
            ovCMB_FatorConversao.SelectedItem = null;
            ovCMB_FatorConversao.SelectedIndex = -1;
            ovCMB_FatorConversao.DisplayMember = "descricao";
            ovCMB_FatorConversao.ValueMember = "idconversaounidadedemedida";
        }
        private void ovCMB_FatorConversao_SelectedValueChanged(object sender, EventArgs e)
        {
            var FatorConversao = (ovCMB_FatorConversao.SelectedItem as ConversaoUnidadeDeMedida);
            if (FatorConversao != null)
            {
                ovTXT_UNEntrada.Text = FatorConversao.UNENTRADA;
                ovTXT_UNSaida.Text = FatorConversao.UNSAIDA;

                decimal QuantidadeSaida = _ItemNFeEntrada.QuantidadeEntrada * FatorConversao.Fator;
                decimal ValorCusto = _ItemNFeEntrada.Valor / QuantidadeSaida;

                ovTXT_QtdeSaida.Text = QuantidadeSaida.ToString("n4");
                ovTXT_ValorCusto.Text = ValorCusto.ToString("c2");
                ovTXT_ValorTotal.Text = (ValorCusto * QuantidadeSaida).ToString("c2");
                calcularMargemDeLucroPorValorVenda();
            }
            else
                PreencherTela();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ovCMB_FatorConversao.SelectedItem = null;
        }

        private void txtPreçoDeVenda_TextChanged(object sender, EventArgs e)
        {
            DecimalMoeda.Moeda(ref txtPreçoDeVenda);
        }

        private void buAddItem_Click(object sender, EventArgs e)
        {
            FCA_Produtos fCA_Produtos;

            var id = _ItemNFeEntrada.IDProduto;

            if (id == -1)
            {
                Produto produtoNfe = new Produto();
                produtoNfe.Descricao = _ItemNFeEntrada.DescricaoProduto;
                produtoNfe.EAN = (decimal?)Convert.ToDecimal(_ItemNFeEntrada.CEAN) == 0 ? null : _ItemNFeEntrada.CEAN.ToString();
                produtoNfe.ValorVenda = _ItemNFeEntrada.VPROD;
                produtoNfe.ValorCusto = _ItemNFeEntrada.vuncom;
        //CEANTRIB = d.prod.cEANTrib,
        //CENQ = d.imposto.IPI != null ? d.imposto.IPI.cEnq.ToString() : null,
        //CEST = d.prod.CEST,
        //CFOP = d.prod.CFOP.ToString(),
        //DescricaoProduto = d.prod.xProd,
        //CEAN = string.IsNullOrEmpty(d.prod.cEAN) ? null : (decimal?)Convert.ToDecimal(d.prod.cEAN),
        //INDTOT = (int)d.prod.indTot,
        //NCM = d.prod.NCM,
        //QTRIB = d.prod.qTrib,
        //QCOM = d.prod.qCom,
        //UCOM = d.prod.uCom,
        //VOUTRO = d.prod.vOutro ?? 0,
        //UTRIB = d.prod.uTrib,
        //VPROD = d.prod.vProd,
        //VUNTRIB = d.prod.vUnTrib,
        //VUNCOM = d.prod.vUnCom,
        //XPROD = d.prod.xProd,
        //VFRETE = d.prod.vFrete ?? 0,
        //VDESC = d.prod.vDesc ?? 0,
        //VSEG = d.prod.vSeg ?? 0,
        //QuantidadeEntrada = d.prod.qCom,
        //UNEntrada = d.prod.uCom,
        //UNSaida = d.prod.uCom,
        //QuantidadeSaida = d.prod.qCom,
        //Valor = d.prod.vUnCom,
        //CProd = d.prod.cProd


                fCA_Produtos = new FCA_Produtos(produtoNfe);
                fCA_Produtos.ovTXT_Identificacao_CEST.Text = _ItemNFeEntrada.CEST;

                var ncm = FuncoesNcm.GetNCMPorCodigo(Convert.ToDecimal(_ItemNFeEntrada.NCM));
                fCA_Produtos.ovTXT_BuscarNCM.Text = _ItemNFeEntrada.NCM;
                fCA_Produtos.ovTXT_NCM.Text = ncm.Descricao;


            }
            else
                fCA_Produtos = new FCA_Produtos(FuncoesProduto.GetProduto(id));


            fCA_Produtos.ShowDialog();

            Produto produtoNovo = fCA_Produtos.GetProduto();

            if (produtoNovo.IDProduto > 0)
            {
                ovTXT_Produto.Text = produtoNovo.Descricao;
                _ItemNFeEntrada.IDProduto = produtoNovo.IDProduto;
                AtualizaLocalizacao();
                buAddItem.Enabled = false;

                ovCMB_AlmoxarifadoEntrada.SelectedItem = Almoxarifados.Where(o => o.IDAlmoxarifado == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDAlmoxarifadoEntrada).FirstOrDefault();
                metroComboBoxIntegracaoFiscal.SelectedItem = IntegracaoFiscals.Where(o => o.IDIntegracaoFiscal == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDIntegracaoFiscalNFe).FirstOrDefault();
            }
            //produtotextEdit.Text = produto.Descricao;
            //LocalizarPorduto();

        }

        private void txtMargemLucro_TextChanged(object sender, EventArgs e)
        {
            /*var valor_real = _ItemNFeEntrada.Valor * (Convert.ToDecimal(txtMargemLucro.Text == "" ? "0":txtMargemLucro.Text) / 100);
            valor_real += _ItemNFeEntrada.Valor;
            txtPreçoDeVenda.Text = valor_real.ToString("N2");*/
        }

        private void txtMargemLucro_KeyUp(object sender, KeyEventArgs e)
        {
            var valor = Convert.ToDecimal(ovTXT_ValorCusto.Text.Replace("R$", "").Replace(" ", ""));
            var valor_real = valor * (Convert.ToDecimal(txtMargemLucro.Text == "" ? "0" : txtMargemLucro.Text) / 100);
            valor_real += valor;
            txtPreçoDeVenda.Text = valor_real.ToString("N2");
        }
        private void calcularMargemDeLucroPorValorVenda()
        {
            var valor = Convert.ToDecimal(ovTXT_ValorCusto.Text.Replace("R$", "").Replace(" ", ""));
            var valor_real = (Convert.ToDecimal(txtPreçoDeVenda.Text) / valor) * 100 - 100;
            txtMargemLucro.Text = String.Format("{0:0.##}", valor_real);
        }
        private void txtPreçoDeVenda_KeyUp(object sender, KeyEventArgs e)
        {
            calcularMargemDeLucroPorValorVenda();
        }

        private void btnEditarProduto_Click(object sender, EventArgs e)
        {
            FCA_Produtos t = new FCA_Produtos(FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto));
            t.ShowDialog(this);
        }

        private void btnAdcionarFatorConversao_Click(object sender, EventArgs e)
        {
            //Produto produto = FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto);
            ConversaoUnidadeDeMedida Conversao = new ConversaoUnidadeDeMedida();
            var unidadeMedida = FuncoesUnidadeMedida.GetUnidadeMedida(_ItemNFeEntrada.UNEntrada);
            Conversao.IDProduto = _ItemNFeEntrada.IDProduto;
            FCA_ConversaoUM t = new FCA_ConversaoUM(Conversao, unidadeMedida);
            t.ShowDialog(this);
            CarregaConversoes();
        }

        private void checkBoxPrecoAtual_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPrecoAtual.Checked)
            {
                txtPreçoDeVenda.ReadOnly = true;
                txtMargemLucro.ReadOnly = true;
                var produto = FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto);
                txtPreçoDeVenda.Text = produto.ValorVenda.ToString("N2");
                calcularMargemDeLucroPorValorVenda();
            }
            else
            {
                txtPreçoDeVenda.ReadOnly = false;
                txtMargemLucro.ReadOnly = false;
                //txtPreçoDeVenda.Text = ovTXT_ValorCusto.Text.Replace("R$", "").Replace(" ", "");
                txtPreçoDeVenda.Text = "";
                txtMargemLucro.Text = "";
                //calcularMargemDeLucroPorValorVenda();
            }
        }
    }
}