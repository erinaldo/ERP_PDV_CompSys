using System.Windows.Forms;
using PDV.DAO.Entidades.Estoque.NFeImportacao;
using PDV.DAO.Entidades.Estoque.Suprimentos;
using System.Collections.Generic;
using PDV.CONTROLER.Funcoes;
using System.Drawing;
using System;
using System.Linq;

namespace PDV.VIEW.Forms.Estoque.ImportacaoNFeEntrada.UControl
{
    public partial class UC_ItemNFeEntrada : UserControl
    {
        public ItemNFeEntrada _ItemNFeEntrada = null;
        public ConversaoUnidadeDeMedida _Conversao = null;
        public List<ConversaoUnidadeDeMedida> Conversoes = null;
        public List<Almoxarifado> Almoxarifados = null;

        public UC_ItemNFeEntrada(ItemNFeEntrada ItemNFeEntrada)
        {
            InitializeComponent();
            _ItemNFeEntrada = ItemNFeEntrada;

            Almoxarifados = FuncoesAlmoxarifado.GetAlmoxarifados();
            ovCMB_AlmoxarifadoEntrada.DataSource = Almoxarifados;
            ovCMB_AlmoxarifadoEntrada.DisplayMember = "descricaoapresentacao";
            ovCMB_AlmoxarifadoEntrada.ValueMember = "idalmoxarifado";
            ovCMB_AlmoxarifadoEntrada.SelectedItem = null;

            CarregaConversoes();
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

            if (_ItemNFeEntrada.IDProduto != -1)
                ovCMB_AlmoxarifadoEntrada.SelectedItem = Almoxarifados.Where(o => o.IDAlmoxarifado == FuncoesProduto.GetProduto(_ItemNFeEntrada.IDProduto).IDAlmoxarifadoEntrada).FirstOrDefault();

            AtualizaLocalizacao();
        }

        private void ovCKB_IncluirItem_CheckedChanged(object sender, System.EventArgs e)
        {
            ovBTN_Localizar.Enabled = !ovCKB_IncluirItem.Checked;

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
                AtualizaLocalizacao();
                CarregaConversoes();
            }
        }

        private void AtualizaLocalizacao()
        {
            if (_ItemNFeEntrada.IDProduto == -1)
            {
                ovTXT_Status.Text = "<Item Não Localizado>";
                ovTXT_Status.ForeColor = Color.DarkRed;
            }
            else
            {
                ovTXT_Status.Text = "<Item Localizado>";
                ovTXT_Status.ForeColor = Color.DarkGreen;
                ovBTN_Localizar.Enabled = false;
                ovCKB_IncluirItem.Enabled = false;
            }
        }

        private void CarregaConversoes()
        {
            Conversoes = FuncoesConversaoUnidadeMedida.GetConversoesPorProduto(_ItemNFeEntrada.IDProduto);
            ovCMB_FatorConversao.DataSource = Conversoes;
            ovCMB_FatorConversao.SelectedItem = null;
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
            }
            else
                PreencherTela();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ovCMB_FatorConversao.SelectedItem = null;
        }
    }
}