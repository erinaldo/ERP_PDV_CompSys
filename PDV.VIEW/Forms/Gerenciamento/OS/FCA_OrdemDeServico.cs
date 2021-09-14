using DevExpress.XtraEditors;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.Estoque.Movimento;
using PDV.DAO.Enum;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Cadastro;
using PDV.VIEW.Forms.Util;
using PDV.VIEW.Forms.Vendas.NFe;
using PDV.VIEW.FRENTECAIXA.Forms.PDV.DAV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Sequence = PDV.DAO.DB.Utils.Sequence;


namespace PDV.VIEW.Forms.Gerenciamento.OS
{
    public partial class FCA_OrdemDeServico : Form
    {
        public BindingList<ItemOrdemDeServico> ServicosItens
        {
            get => _ServicosItens;
            set
            {
                _ServicosItens = value;
                CalcularTotalVenda();
                _ServicosItens.ListChanged += (s, e) => CalcularTotalVenda();
                gridControl1.DataSource = _ServicosItens;
            }
        }
        public BindingList<ItemOrdemDeServico> _ServicosItens { get; set; } = new BindingList<ItemOrdemDeServico>();

        public BindingList<DuplicataServico> Duplicatas
        {
            get => _Duplicatas;
            set
            {
                _Duplicatas = value;
                CalcularTotalVenda();
                _Duplicatas.ListChanged += (s, e) => CalcularTotalVenda();
                gridControl2.DataSource = _Duplicatas;
            }
        }
        public BindingList<DuplicataServico> _Duplicatas { get; set; } = new BindingList<DuplicataServico>();

        public OrdemDeServico OrdemDeServico { get; set; }

        public Cliente Cliente
        {
            get => _Cliente;
            set
            {
                _Cliente = value;
                if (value != null)
                {
                    clientetextEdit.EditValue = value._DESCRICAO;
                }
                else
                {
                    clientetextEdit.EditValue = null;
                }
            }
        }
        public Cliente _Cliente { get; set; }

        public Servico Servico
        {
            get => _Servico;
            set
            {
                _Servico = value;
                if (value != null)
                {
                    servicotextEdit.EditValue = descricaoProdutoLabelControl1.Text = value.Descricao;
                    PrecoItem = value.Valor;
                }
                else
                {
                    servicotextEdit.EditValue = null;
                    descricaoProdutoLabelControl1.Text = "...";
                    PrecoItem = 1;
                }
                CalcularTotalItem();
            }
        }

        public Servico _Servico { get; set; }
        public Usuario Vendedor
        {
            get => _Vendedor;
            set
            {
                _Vendedor = value;
                if (value != null)
                {
                    vendedortextEdit.EditValue = value.Nome;
                }
                else
                {
                    vendedortextEdit.EditValue = null;
                }
            }
        }

        public Usuario _Vendedor { get; set; }

        public int QuantidadeItem
        {
            get => Convert.ToInt32(spinQuantidade.EditValue);
            set => spinQuantidade.EditValue = value;
        }

        public decimal PrecoItem
        {
            get => Convert.ToDecimal(spinPreco.EditValue);
            set => spinPreco.EditValue = DecToStr(value);
        }

        public decimal TotalItem
        {
            get => Convert.ToDecimal(txtTotal.EditValue);
            set => txtTotal.EditValue = DecToStr(value);
        }

        public decimal DescontoPorcentagemItem
        {
            get => Convert.ToDecimal(txtDescontoPorcent.EditValue);
            set => txtDescontoPorcent.EditValue = DecToStr(value);
        }

        public decimal DescontoValorItem
        {
            get => Convert.ToDecimal(spinDescontoValor.EditValue);
            set => spinDescontoValor.EditValue = DecToStr(value);
        }

        public decimal ValorFormaPagamamento
        {
            get => Convert.ToDecimal(valorPagtoText.EditValue);
            set => valorPagtoText.EditValue = DecToStr(value);
        }

        public decimal TotalPago
        {
            set => totalPagtosText.EditValue = DecToStr(value);
            get => Convert.ToDecimal(totalPagtosText.EditValue);
        }
        public decimal TotalGeral
        {
            set => totalGeralText.EditValue = DecToStr(value);
            get => Convert.ToDecimal(totalGeralText.EditValue);
        }

        public decimal TotalDesconto
        {
            set => totalDescontoTextEdit.EditValue = DecToStr(value);
            get => Convert.ToDecimal(totalDescontoTextEdit.EditValue);
        }

        public decimal Troco
        {
            get => Convert.ToDecimal(trocoTextEdit.EditValue);
            set
            {
                trocoTextEdit.EditValue = DecToStr(value);
                trocoTextEdit.BackColor = Color.White;
                if (Troco < 0)
                    trocoTextEdit.BackColor = Color.Red;
                if (Troco > 0)
                    trocoTextEdit.BackColor = Color.Green;
            }
        }

        public decimal IDFormaDePagamento
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(gridLookUpEditFormaPagamento.EditValue);
                }
                catch (FormatException)
                {
                    return 0;
                }
               
            }
            set => gridLookUpEditFormaPagamento.EditValue = value;
        }

        public decimal IDTipoDeOperacao
        {
            get => Convert.ToDecimal(gridLookUpEditTipoDeOperacao.EditValue);
            set => gridLookUpEditTipoDeOperacao.EditValue = value;
        }

        public string Observacao
        {
            get => observacaoRichTextBox.Text;
            set => observacaoRichTextBox.Text = value;
        }

        public DateTime DataCadastro
        {
            get => dateTimePicker1.Value;
            set => dateTimePicker1.Value = value;
        }

        public decimal IDItemServicoSelecionado
        {
            get => Grids.GetValorDec(gridView1, "IDItemOrdemDeServico");
        }

        public decimal IDDuplicataSelecionada
        {
            get => Grids.GetValorDec(gridView2, "IDDuplicataServico");
        }

        public ItemOrdemDeServico ItemServicoSelecionado
        {
            get => ServicosItens.FirstOrDefault(s => s.IDItemOrdemDeServico == IDItemServicoSelecionado);
        }
        public FCA_OrdemDeServico(OrdemDeServico ordemDeServico = null)
        {
            Inicializar(ordemDeServico);
        }

        public FCA_OrdemDeServico(decimal idOrdemDeServico)
        {
            Inicializar(FuncoesOrdemServico.GetOrdemDeServico(idOrdemDeServico));
        }

        private void Inicializar(OrdemDeServico ordemDeServico)
        {
            OrdemDeServico = ordemDeServico ?? new OrdemDeServico();
            InitializeComponent();
            PreencherGridsViewLookUp();
            PreencherOrdemDeServico();
            SetarGrids();
            ChecarStatus();
        }

        private void ChecarStatus()
        {
            if (OrdemDeServico.Status != DAO.Enum.StatusPedido.Aberto)
            {
                var buttons = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => typeof(SimpleButton).IsAssignableFrom(f.FieldType))
                    .Select(f => (SimpleButton)f.GetValue(this))
                    .Where(v => v != null);
                foreach (var button in buttons)
                {
                    button.Enabled = false;
                }
                
            }
            if (OrdemDeServico.IDOrdemDeServico != 0)
            {
                if (OrdemDeServico.Status == DAO.Enum.StatusPedido.Faturado)
                {
                    labelStatus.Text = "FATURADO";
                    labelStatus.ForeColor = Color.LightGreen;

                }
                if (OrdemDeServico.Status == DAO.Enum.StatusPedido.Cancelado)
                {
                    labelStatus.Text = "CANCELADO";
                    labelStatus.ForeColor = Color.OrangeRed;
                }
                if (OrdemDeServico.Status == DAO.Enum.StatusPedido.Aberto)
                {
                    labelStatus.Text = "ABERTO";
                }
            }
            
        }

        private void PreencherOrdemDeServico()
        {
            Cliente = FuncoesCliente.GetCliente(OrdemDeServico.IDCliente);
            Vendedor = FuncoesUsuario.GetUsuario(OrdemDeServico.IDVendedor);
            IDTipoDeOperacao = OrdemDeServico.IDTipoDeOperacao;
            Observacao = OrdemDeServico.Observacao;
            Duplicatas = new BindingList<DuplicataServico>();
            ServicosItens = new BindingList<ItemOrdemDeServico>();
            if (OrdemDeServico.IDOrdemDeServico != 0)
            {
                DataCadastro = OrdemDeServico.DataCadastro;

                Duplicatas = new BindingList<DuplicataServico>(FuncoesDuplicataServico.GetDuplicatasServicos(OrdemDeServico.IDOrdemDeServico));
                ServicosItens = new BindingList<ItemOrdemDeServico>(FuncoesItemOrdemDeServico.GetItensOrdemServico(OrdemDeServico.IDOrdemDeServico));
            }

        }

        private void SetarGrids()
        {
            Grids.FormatGrid(ref gridView1);
            Grids.FormatColumnType(ref gridView1, new List<string>
            {
                "IDServico",
                "IDOrdemDeServico"
            }, GridFormats.VisibleFalse);

            Grids.FormatColumnType(ref gridView1, new List<string>
            {
                "ValorUnitarioItem",
                "SubTotal",
                "DescontoValor"
            }, GridFormats.Finance);

            Grids.FormatGrid(ref gridView2);

            Grids.FormatColumnType(ref gridView2, new List<string>
            {
                "IDFormaDePagamento",
                "IDOrdemDeServico"
            }, GridFormats.VisibleFalse);

            Grids.FormatColumnType(ref gridView2, "Valor", GridFormats.Finance);

        }

        private void CalcularTotalVenda()
        {
            TotalPago = Duplicatas.Sum(x => x.Valor);
            TotalDesconto = ServicosItens.Sum(s => s.DescontoValor);
            TotalGeral = ServicosItens.Sum(s => s.SubTotal);
            Troco = TotalPago - TotalGeral;
        }

        private void PreencherGridsViewLookUp()
        {
            PreencherGridLookUpTipoOperacao();
            PreencherGridLookUpFormaPagamento();

        }

        private void PreencherGridLookUpFormaPagamento()
        {
            gridLookUpEditFormaPagamento.Properties.DataSource = FuncoesFormaDePagamento.GetFormasPagamentos();

            gridLookUpEditFormaPagamento.Properties.DataSource = FuncoesFormaDePagamento.GetFormasPagamentos()
                .Select(s => new { Cod = s.IDFormaDePagamento, Nome = s.Identificacao })
                .OrderBy(x => x.Cod)
                .ToList();
            gridLookUpEditFormaPagamento.Properties.ValueMember = "Cod";
        }

        private void PreencherGridLookUpTipoOperacao()
        {
            gridLookUpEditTipoDeOperacao.Properties.DataSource =
                FuncoesTipoDeOperacao.GetTiposDeOperacaoPorTipoDeMovimento(MovimentoEstoque.Saida)
                .Select(to => new { Cod = to.IDTipoDeOperacao, to.Nome })
                .OrderBy(to => to.Cod)
                .ToList();
            gridLookUpEditTipoDeOperacao.Properties.ValueMember = "Cod";
        }

        private void pesquisarClienteSimpleButton_Click(object sender, System.EventArgs e)
        {
            BuscarCliente();
        }

        private void BuscarCliente()
        {
            var pesquisarCliente = new OSPesquisarClientes();
            pesquisarCliente.ShowDialog();
            if (pesquisarCliente.Codigo != null && pesquisarCliente.Codigo != "0")
            {
                Cliente = FuncoesCliente.GetCliente(Convert.ToDecimal(pesquisarCliente.Codigo));
            }

        }

        private void buscarProdutosimpleButton_Click(object sender, EventArgs e)
        {
            BuscarServico();
        }

        private void BuscarServico()
        {
            var pesquisarServico = new OSPesquisarServicos();
            pesquisarServico.ShowDialog();
            if (pesquisarServico.IDServico != 0)
            {
                Servico = FuncoesServico.GetServico(pesquisarServico.IDServico);
                spinQuantidade.Select();
                spinQuantidade.SelectAll();
            }
        }


        private void incluirSimpleButton_Click(object sender, EventArgs e)
        {
            AdicionarServico();
        }

        private void AdicionarServico()
        {
            try
            {
                ValidarItem();
                ServicosItens.Add(new ItemOrdemDeServico
                {
                    IDItemOrdemDeServico = Sequence.GetNextID("ITEMORDEMDESERVICO", "IDITEMORDEMDESERVICO"),
                    IDServico = Servico.IDServico,
                    Descricao = Servico.Descricao,
                    Quantidade = QuantidadeItem,
                    ValorUnitarioItem = PrecoItem,
                    DescontoPorcentagem = DescontoPorcentagemItem,
                    DescontoValor = DescontoValorItem,
                    SubTotal = TotalItem
                });
                Servico = new Servico();
                QuantidadeItem = 1;
                DescontoValorItem = 0;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
            
        }

        private void ValidarItem()
        {
            if (TotalItem < 0)
                throw new Exception("O total do item não pode ser menor a 0");
        }

        private void spinQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            CalcularTotalItem();
            if (e.KeyCode == Keys.Enter)
                AdicionarServico();

        }

        private void CalcularTotalItem()
        {
            TotalItem = (PrecoItem - DescontoValorItem) * QuantidadeItem;
        }

        private void spinPreco_EditValueChanged(object sender, EventArgs e)
        {
            CalcularTotalItem();
        }

        private void adicionarFormaPagamentoSimpleButton_Click(object sender, EventArgs e)
        {
            AdicionarDuplicata();
        }

        private void AdicionarDuplicata()
        {
            try
            {
                ValidarFormaPagamento();
                var formaDePagamento = FuncoesFormaDePagamento.GetFormaDePagamento(IDFormaDePagamento);
                var fatorPeriodicidade = Periodicidade.Fator(formaDePagamento.Periodicidade);

                var ultimoVencimento = DateTime.Today;
                if (formaDePagamento.Qtd_Parcelas == 0)
                {
                    Alert("Forma de pagamento escolhida não está configurada. Verifique a quantidade de parcelas.");
                    return;
                }

                if (fatorPeriodicidade == 1)
                {
                    ultimoVencimento = ultimoVencimento.AddDays(formaDePagamento.Qtd_Parcelas - fatorPeriodicidade);
                    formaDePagamento.Qtd_Parcelas = 1;
                }

                for (int i = 0; i < formaDePagamento.Qtd_Parcelas; i++)
                {
                    var duplicata = new DuplicataServico
                    {
                        IDDuplicataServico = Sequence.GetNextID("DUPLICATASERVICO", "IDDUPLICATASERVICO"),
                        IDFormaDePagamento = formaDePagamento.IDFormaDePagamento,
                        Descricao = formaDePagamento.Descricao + "(" + formaDePagamento.Identificacao + ")",
                        Valor = ValorFormaPagamamento / formaDePagamento.Qtd_Parcelas,
                        DataVencimento = ultimoVencimento.AddDays(fatorPeriodicidade)
                    };
                    ultimoVencimento = duplicata.DataVencimento;
                    Duplicatas.Add(duplicata);
                }
                IDFormaDePagamento = 0;
                ValorFormaPagamamento = 0;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
          
        }

        private void ValidarFormaPagamento()
        {
            if (IDFormaDePagamento == 0)
                throw new Exception("Escolha uma forma de pagamento");
            if (ValorFormaPagamamento < 0)
                throw new Exception("O valor da forma de pagamento não pode ser menor do que 0");
        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void salvarSimpleButton_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void Salvar()
        {
            try
            {
                PDVControlador.BeginTransaction();
                ValidarOS();
                var operacao = TipoOperacao.UPDATE;
                if (OrdemDeServico.IDOrdemDeServico == 0)
                {
                    OrdemDeServico.IDOrdemDeServico = Sequence.GetNextID("ORDEMDESERVICO", "IDORDEMDESERVICO");
                    OrdemDeServico.DataFaturamento = null;
                    operacao = TipoOperacao.INSERT;
                }
                else
                {
                    FuncoesItemOrdemDeServico.Remover(OrdemDeServico.IDOrdemDeServico);
                    FuncoesDuplicataServico.Remover(OrdemDeServico.IDOrdemDeServico);
                }

                OrdemDeServico.DataCadastro = DataCadastro;
                OrdemDeServico.IDCliente = Cliente.IDCliente;
                OrdemDeServico.ValorTotal = TotalGeral;
                OrdemDeServico.Observacao = Observacao;
                OrdemDeServico.IDTipoDeOperacao = IDTipoDeOperacao;
                OrdemDeServico.IDVendedor = Vendedor.IDUsuario;

                if (!FuncoesOrdemServico.Salvar(OrdemDeServico, operacao))
                    throw new Exception("Não foi possível salvar a ordem de serviço.");

                foreach (var item in ServicosItens)
                {
                    item.IDOrdemDeServico = OrdemDeServico.IDOrdemDeServico;
                    FuncoesItemOrdemDeServico.Salvar(item);
                }

                foreach (var item in Duplicatas)
                {
                    item.IDOrdemDeServico = OrdemDeServico.IDOrdemDeServico;
                    FuncoesDuplicataServico.Salvar(item);
                }

                PDVControlador.Commit();
                Alert("Ordem de serviço salva com sucesso");
                Close();
            }
            catch (Exception exception)
            {
                PDVControlador.Rollback();
                Alert(exception.Message);
            }
        }

        private void ValidarOS()
        {
            if (Cliente == null)
                throw new Exception("Informe o cliente");
            if (Vendedor == null)
                throw new Exception("Informe o vendedor");
            if (IDTipoDeOperacao == 0)
                throw new Exception("Informe o tipo de operação.");
            if (!ServicosItens.Any())
                throw new Exception("A ordem de serviço deve possuir no mínimo um item");
        }

        private string DecToStr(decimal value, string format = "n2")
        {
            return value.ToString(format);
        }

        private void LimparDuplicatas()
        {
            if (Confirm("Deseja remover todas as duplicatas?") == DialogResult.Yes)
                Duplicatas.Clear();
        }

        private void gridLookUpEditFormaPagamento_EditValueChanged(object sender, EventArgs e)
        {
            ValorFormaPagamamento = Troco < 0 ? Math.Abs(Troco) : 0;
            valorPagtoText.Select();
            valorPagtoText.SelectAll();
        }

        private void valorPagtoText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AdicionarDuplicata();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RemoverFormaDePagamento();
        }

        private void RemoverFormaDePagamento()
        {
            if (Confirm("Deseja remover a duplicata selecionada?") == DialogResult.Yes)
                Duplicatas.Remove(Duplicatas.FirstOrDefault(d => d.IDDuplicataServico == IDDuplicataSelecionada));
        }

        private DialogResult Confirm(string msg)
        {
            return MessageBox.Show(msg, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        private void cancelarItemSimpleButton1_Click(object sender, EventArgs e)
        {
            RemoverItem();
        }

        private void RemoverItem()
        {
            ServicosItens.Remove(ItemServicoSelecionado);
        }

        private void AlterarItemSimpleButton_Click(object sender, EventArgs e)
        {
            if (ServicosItens.Any())
            {
                var item = ItemServicoSelecionado;
                Servico = FuncoesServico.GetServico(item.IDServico);
                PrecoItem = item.ValorUnitarioItem;
                QuantidadeItem = Convert.ToInt32(item.Quantidade);
                DescontoValorItem = item.DescontoValor;
                RemoverItem();
            }
            else
                Alert("Não há itens para editar");
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            LimparDuplicatas();
        }

        private void FCA_OrdemDeServico_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpHandler(e.KeyCode);
        }

        private void KeyUpHandler(Keys key)
        {
            var dict = new Dictionary<Keys, Action>();
            dict.Add(Keys.F1, BuscarCliente);
            dict.Add(Keys.F2, BuscarVendedor);
            dict.Add(Keys.F6, BuscarServico);
            dict.Add(Keys.F12, Close);
            dict.Add(Keys.Escape, Close);
            dict.FirstOrDefault(d => d.Key == key).Value?.Invoke();

        }

        private void BuscarVendedor()
        {
            var pesquisarVendedor = new OSPesquisarVendedores();
            pesquisarVendedor.ShowDialog();
            Vendedor = FuncoesUsuario.GetUsuario(pesquisarVendedor.Codigo);
        }

        private void pesquisarVendedorSimpleButton_Click(object sender, EventArgs e)
        {
            BuscarVendedor();
        }

        private void spinDescontoValor_EditValueChanged(object sender, EventArgs e)
        {
            if (PrecoItem != 0)
                DescontoPorcentagemItem = DescontoValorItem * 100 / PrecoItem;
            else
                DescontoValorItem = DescontoPorcentagemItem = PrecoItem;
            CalcularTotalItem();
        }

        private void txtDescontoPorcent_EditValueChanged(object sender, EventArgs e)
        {
            if (PrecoItem != 0)
                DescontoValorItem = DescontoPorcentagemItem * PrecoItem / 100;
            else
                DescontoValorItem = DescontoPorcentagemItem = PrecoItem;
            CalcularTotalItem();
        }

        private void buttonAdicionarCliente_Click(object sender, EventArgs e)
        {
            var fCA_Cliente = new FCA_Cliente(new Cliente());
            fCA_Cliente.ShowDialog();
            Cliente = FuncoesCliente.GetCliente(fCA_Cliente.GetClienteID());
        }

        private void buttonAdicionarProduto_Click(object sender, EventArgs e)
        {
            var form = new FCA_Servico(new Servico());
            form.ShowDialog();
            Servico = FuncoesServico.GetServico(form.GetIDServico());
        }

        private void buttonAdicionarVendedor_Click(object sender, EventArgs e)
        {
            var fCA_Usuario = new FCA_Usuario(new Usuario());
            fCA_Usuario.ShowDialog();
            Vendedor = FuncoesUsuario.GetUsuario(fCA_Usuario.GetUsuarioID());
        }

        private void buttonAdicionarTipoDeOperacao_Click(object sender, EventArgs e)
        {
            var fCA_TipoDeOperacao = new FCA_TipoDeOperacao(new TipoDeOperacao(), Operacao.DeSaida);
            fCA_TipoDeOperacao.ShowDialog();
            PreencherGridLookUpTipoOperacao();
            gridLookUpEditTipoDeOperacao.EditValue = fCA_TipoDeOperacao.GetTipoDeOperacaoID();
        }

        private void buttonAdicionarFormaDePagamento_Click(object sender, EventArgs e)
        {
            var fCA_FormaDePagamento = new FCA_FormaDePagamento(new FormaDePagamento());
            fCA_FormaDePagamento.ShowDialog();
            PreencherGridLookUpFormaPagamento();
            if (fCA_FormaDePagamento.GetFormaDePagamentoID() >= 0)
                gridLookUpEditFormaPagamento.EditValue = fCA_FormaDePagamento.GetFormaDePagamentoID();
        }
    }
}