using PDV.CONTROLER.Funcoes;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using PDV.DAO.Enum;
using PDV.VIEW.App_Context;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Cadastro
{
    public partial class FCA_Servico : DevExpress.XtraEditors.XtraForm
    {
        private string NOME_TELA = "CADASTRO DE SERVIÇO";

        public Servico Servico { get; set; }

        public decimal IDServico 
        { 
            get => Convert.ToDecimal(textEditID.EditValue);
            set => textEditID.EditValue = value;
        }

        public string Descricao
        {
            get => textEditDescricao.EditValue.ToString();
            set => textEditDescricao.EditValue = value;
        }

        public decimal IDCategoria
        {
            get => Convert.ToDecimal(gridLookUpEditCategoria.EditValue);
            set => gridLookUpEditCategoria.EditValue = value;
        }

        public decimal IDUnidadeDeMedida
        {
            get => Convert.ToDecimal(gridLookUpEditUnidadeDeMedida.EditValue);
            set => gridLookUpEditUnidadeDeMedida.EditValue = value;
        }

        public decimal Valor
        {
            get => Convert.ToDecimal(spinEditValor.EditValue);
            set => spinEditValor.EditValue = value;
        }

        public FCA_Servico(Servico servico)
        {
            Inicializar(servico);
        }

        public FCA_Servico(decimal idServico)
        {
            var servico = FuncoesServico.GetServico(idServico);
            Inicializar(servico);
        }

        private void Inicializar(Servico servico)
        {
            InitializeComponent();
            Servico = servico;
            PreencherCampos();
        }

        private void PreencherCampos()
        {
            PreencherGridLookUps();
            IDServico = Servico.IDServico;
            IDCategoria = Servico.IDCategoria;
            IDUnidadeDeMedida = Servico.IDUnidadeDeMedida;
            Descricao = Servico.Descricao;
            Valor = Servico.Valor;
        }

        private void PreencherGridLookUps()
        {
            PreencherGridLookUpCategoria();
            PreencherGridLookUpUnidadeDeMedida();
        }

        private void PreencherGridLookUpUnidadeDeMedida()
        {
            gridLookUpEditUnidadeDeMedida.Properties.DataSource = FuncoesUnidadeMedida
                .GetUnidadesMedida()
                .OrderBy(u => u.Descricao)
                .Select(u => new { u.IDUnidadeDeMedida, u.Descricao })
                .ToList();
            gridLookUpEditUnidadeDeMedida.Properties.ValueMember = "IDUnidadeDeMedida";
            gridLookUpEditUnidadeDeMedida.Properties.DisplayMember = "Descricao";
        }

        private void PreencherGridLookUpCategoria()
        {
            gridLookUpEditCategoria.Properties.DataSource = FuncoesCategoria
                .GetCategorias()
                .OrderBy(u => u.Descricao)
                .Select(c => new { c.IDCategoria, c.Descricao })
                .ToList();
            gridLookUpEditCategoria.Properties.ValueMember = "IDCategoria";
            gridLookUpEditCategoria.Properties.DisplayMember = "Descricao";
        }

        private void btnSalvar_Click(object sender, System.EventArgs e)
        {
            Salvar();
        }

        private void Salvar()
        {
            try
            {
                PDVControlador.BeginTransaction();
                Validar();
                var tipoOperacao = PopularAtributos() ? TipoOperacao.INSERT: TipoOperacao.UPDATE;
                FuncoesServico.Salvar(Servico, tipoOperacao);
                PDVControlador.Commit();
                Alert("Serviço salvo com sucesso");
                Close();
            }
            catch (Exception exception)
            {
                PDVControlador.Rollback();
                Alert(exception.Message);
            }
        }

        private void Validar()
        {
            if (Descricao == string.Empty)
                throw new Exception("Preencha o campo descrição");
            if (IDCategoria < 1)
                throw new Exception("Preencha o campo categoria");
            if (IDUnidadeDeMedida < 1)
                throw new Exception("Preencha o campo unidade de medida");
        }

        private bool PopularAtributos()
        {
            var novo = IDServico == 0;
            Servico.IDServico = novo ? Sequence.GetNextID("SERVICO", "IDSERVICO") : IDServico;           
            Servico.IDCategoria = IDCategoria;
            Servico.IDUnidadeDeMedida = IDUnidadeDeMedida;
            Servico.Descricao = Descricao;
            Servico.Valor = Valor;
            return novo;
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg, NOME_TELA, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public decimal GetIDServico()
        {
            return Servico.IDServico;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var form = new FCA_Categoria(new Categoria());
            form.ShowDialog();
            PreencherGridLookUpCategoria();
            IDCategoria = form.GetIDCategoria();
        }

        private void buttonAdicionarCliente_Click(object sender, EventArgs e)
        {
            var form = new FCA_UnidadeMedida(new UnidadeMedida());
            form.ShowDialog();
            PreencherGridLookUpUnidadeDeMedida();
            IDUnidadeDeMedida = form.GetIDUnidadeDeMedida();
        }
    }
}