using MetroFramework.Forms;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades;
using PDV.DAO.Enum;
using PDV.UTIL;
using PDV.VIEW.Forms.Consultas;
using System;
using System.Data;
using System.Windows.Forms;

namespace PDV.VIEW.FRENTECAIXA.Forms
{
    public partial class GPDV_IdentificarCliente : DevExpress.XtraEditors.XtraForm
    {
        private GPDV_PainelInicial PainelInicial = null;
        public DataRow DRCliente = null;
        public Cliente Cliente = null;
        public bool Identificar = false;
        private bool ClienteNovo = false;
        public string IDCliente = "";

        private string _TipoCliente = "1";


        public GPDV_IdentificarCliente(GPDV_PainelInicial _PainelInicial)
        {
            InitializeComponent();
            Cliente = new Cliente();
            PainelInicial = _PainelInicial;
            ovTXT_ClienteEncontrado.Text = "";
            ovTXT_CPFCNPJ.LostFocus += OvTXT_CPFCNPJ_LostFocus;
            ovTXT_TipoPessoa.LostFocus += ovTXT_TipoPessoa_LostFocus;
            AplicarMascara();
        }

        private void ovTXT_TipoPessoa_LostFocus(object sender, EventArgs e)
        {
            if (!_TipoCliente.Equals(ovTXT_TipoPessoa.Text))
            {
                AplicarMascara();
                ovTXT_ClienteEncontrado.Text = string.Empty;
                ovTXT_EmailCliente.Text = string.Empty;
            }
        }

        private void AplicarMascara()
        {
            switch (ovTXT_TipoPessoa.Text.Trim())
            {
                case "0":
                    ovTXT_TipoDocumento.Text = "* CNPJ:";
                    ovTXT_CPFCNPJ.Mask = "##,###,###/####-##";
                    ovTXT_CPFCNPJ.Text = string.Empty;
                    ovTXT_ClienteEncontrado.Text = string.Empty;
                    break;
                case "1":
                    ovTXT_TipoDocumento.Text = "* CPF:";
                    ovTXT_CPFCNPJ.Mask = "###,###,###-##";
                    ovTXT_CPFCNPJ.Text = string.Empty;
                    ovTXT_ClienteEncontrado.Text = string.Empty;
                    break;
            }
        }

        private void OvTXT_CPFCNPJ_LostFocus(object sender, EventArgs e)
        {
            //AplicarMascara();

            if (!(ovTXT_TipoPessoa.Text.Trim().Equals("0") || ovTXT_TipoPessoa.Text.Trim().Equals("1")))
                ovTXT_TipoPessoa.Text = "1";

            DRCliente = FuncoesCliente.GetClientePorTipoEDocumento(Convert.ToDecimal(ovTXT_TipoPessoa.Text.Trim()), ZeusUtil.SomenteNumeros(ovTXT_CPFCNPJ.Text).ToString());
            if (DRCliente == null)
            {
                ovTXT_EmailCliente.Text = string.Empty;
                ovTXT_ClienteEncontrado.Text = "Novo Cliente";
                ovTXT_ClienteEncontrado.ForeColor = System.Drawing.Color.Red;
                ovTXT_NomeCliente.Visible = true;
                ClienteNovo = true;
                ovTXT_NomeCliente.Focus();
            }
            else
            {
                ovTXT_EmailCliente.Text = DRCliente["EMAIL"].ToString();
                ovTXT_ClienteEncontrado.Text = DRCliente["NOME"].ToString(); ;
                ovTXT_ClienteEncontrado.ForeColor = System.Drawing.Color.Green;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Identificar = false;
                    Close();
                    break;
                case Keys.F12: //IDENTIFICAR O CLIENTE
                    SalvarIdentificacao();
                    break;
                case Keys.F3:
                    Consultar();
                    break;

            }
            return base.ProcessDialogKey(keyData);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            SalvarIdentificacao();
        }

        private void SalvarIdentificacao()
        {
            try
            {
                if (string.IsNullOrEmpty(ZeusUtil.SomenteNumeros(ovTXT_CPFCNPJ.Text)))
                {
                    ovTXT_CPFCNPJ.Select();

                    Cliente = FuncoesCliente.GetClienteCPFCNPJ(ovTXT_CPFCNPJ.Text);

                    return;
                }
                if (ClienteNovo)
                {
                    try
                    {
                        Cliente cliente = new Cliente();
                        cliente.Ativo = 1;
                        cliente.Nome = ovTXT_NomeCliente.Text;
                        cliente.TipoDocumento = decimal.Parse(ovTXT_TipoPessoa.Text);
                        cliente.IDEndereco = null;
                        cliente.IDContato = null;
                        if (ovTXT_TipoPessoa.Text == "1")
                            cliente.CPF = ovTXT_CPFCNPJ.Text.Replace(".", "").Replace("-", "");
                        else
                        {
                            cliente.RazaoSocial = ovTXT_NomeCliente.Text;
                            cliente.CNPJ = ovTXT_CPFCNPJ.Text.Replace(".", "").Replace("-", "").Replace("/", "");
                        }
                        cliente.Email = ovTXT_EmailCliente.Text;
                        TipoOperacao Op = TipoOperacao.INSERT;
                        FuncoesCliente.Salvar(cliente, Op);
                        Cliente = FuncoesCliente.GetClienteCPFCNPJ(ovTXT_CPFCNPJ.Text);
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.InnerException.ToString(); // throw new Exception("Não foi possível salvar o cliente");
                    }
                }
                Identificar = true;
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Identificação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        private void metroButton10_Click(object sender, EventArgs e)
        {
            SalvarIdentificacao();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        private void Consultar()
        {
            FCO_Cliente cliente = new FCO_Cliente(true);
            cliente.ShowDialog();
            if (cliente.Cliente != null)
            {
                ovTXT_NomeCliente.Text = cliente.Cliente.NomeFantasia == null ?  cliente.Cliente.Nome :"SEM IDENTIFICAÇÃO";
                if(cliente.Cliente.TipoDocumento == 1)
                ovTXT_CPFCNPJ.Text = cliente.Cliente.CPF;
                else
                    ovTXT_CPFCNPJ.Text = cliente.Cliente.CNPJ;
                ovTXT_EmailCliente.Text = cliente.Cliente.Email;
                ovTXT_TipoPessoa.Text = cliente.Cliente.TipoDocumento.ToString();
                Cliente = FuncoesCliente.GetClienteCPFCNPJ(cliente.Cliente.CPF?? cliente.Cliente.CNPJ);
            }
            OvTXT_CPFCNPJ_LostFocus(ovTXT_CPFCNPJ, null);
        }
    }
}
