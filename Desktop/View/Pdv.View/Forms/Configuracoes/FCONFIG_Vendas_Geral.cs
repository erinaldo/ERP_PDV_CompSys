using MetroFramework;
using MetroFramework.Forms;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Custom;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.Financeiro;
using PDV.VIEW.App_Context;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Configuracoes
{
    public partial class FCONFIG_Vendas_Geral : DevExpress.XtraEditors.XtraForm
    {
        private string NOME_TELA = "GERAL";
        public FCONFIG_Vendas_Geral()
        {
            InitializeComponent();
            PreencherTela();
        }

        private void PreencherTela()
        {
            Configuracao config = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGURACAOPEDIDOVENDA_DESCONTO_POR);
            if (config != null)
            {
                if (Encoding.UTF8.GetString(config.Valor).Equals("0"))
                    ovRB_Percentual.Checked = true;
                else
                    ovRB_Valor.Checked = true;
            }

            //TEF AUT
            Configuracao config1 = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.TEF_CHAVE_AUTENTICACAO);
            if (config1 != null)
            {
                ovTXT_CHAVE_ATENTICACAO.Text = Encoding.UTF8.GetString(config1.Valor);
            }

            //Tipo de Operação PAdrao APP
            Configuracao config2 = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.TIPO_OPERACAO_PADRAO_APP);
            if (config2 != null)
            {
                gridLookUpEditTipoDeOperacao.EditValue = Encoding.UTF8.GetString(config2.Valor);
            }

            Configuracao config3 = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.TIPO_OPERACAO_PADRAO_PDV);
            if (config3 != null)
            {
                tipoOperacaoPadraoPDVGridLookUpEdit.EditValue = Encoding.UTF8.GetString(config3.Valor);
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
            return base.ProcessDialogKey(keyData);
        }
        private void metroButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            try
            {
                PDVControlador.BeginTransaction();

                if (!FuncoesConfiguracao.Salvar(ChavesConfiguracao.CHAVE_CONFIGURACAOPEDIDOVENDA_DESCONTO_POR, Encoding.Default.GetBytes(ovRB_Percentual.Checked ? "0" : "1")))
                    throw new Exception("Não foi possível salvar o desconto.");

                if (!FuncoesConfiguracao.Salvar(ChavesConfiguracao.TEF_CHAVE_AUTENTICACAO, Encoding.Default.GetBytes(ovTXT_CHAVE_ATENTICACAO.Text)))
                    throw new Exception("Não foi possível salvar a chave TEF.");

                if (!FuncoesConfiguracao.Salvar(ChavesConfiguracao.TIPO_OPERACAO_PADRAO_APP, Encoding.Default.GetBytes(gridLookUpEditTipoDeOperacao.EditValue.ToString())))
                    throw new Exception("Não foi possível salvar o tipo de operação padrão app.");

                if (!FuncoesConfiguracao.Salvar(ChavesConfiguracao.TIPO_OPERACAO_PADRAO_PDV, Encoding.Default.GetBytes(tipoOperacaoPadraoPDVGridLookUpEdit.EditValue.ToString())))
                    throw new Exception("Não foi possível salvar o tipo de operação padrão pdv.");

                PDVControlador.Commit();
                MessageBox.Show(this, "Configurações salvas com sucesso.", NOME_TELA);
                Close();
            }
            catch (Exception Ex)
            {
                PDVControlador.Rollback();
                MessageBox.Show(this, Ex.Message, NOME_TELA);
            }
        }

        private void FCONFIG_Vendas_Geral_Load(object sender, EventArgs e)
        {
            var tipoDeOperacao = FuncoesTipoDeOperacao.GetTiposDeOperacaoPorTipoDeMovimento(TipoDeOperacao.Saida)
                                                        .Select(s => new { Cod = s.IDTipoDeOperacao, Nome = s.Nome })
                                                        .OrderBy(x => x.Cod).ToList();


            gridLookUpEditTipoDeOperacao.Properties.DataSource = tipoDeOperacao;
            tipoOperacaoPadraoPDVGridLookUpEdit.Properties.DataSource = tipoDeOperacao;

        }
    }
}
