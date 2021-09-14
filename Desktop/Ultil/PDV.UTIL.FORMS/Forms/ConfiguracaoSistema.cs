using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.API.Native;
using MetroFramework;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.DB.CriptografiaMD5;
using PDV.DAO.DB.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PDV.UTIL.FORMS.Forms
{
    public partial class ConfiguracaoSistema : DevExpress.XtraEditors.XtraForm
    {
        private IniFile iniFile = null;
        public ConfiguracaoSistema()
        {
            iniFile = new IniFile(CaminhoSolution + (System.Diagnostics.Debugger.IsAttached ? "\\PDV.VIEW\\App_Data\\Start.ini" : "\\App_Data\\Start.ini"));
            InitializeComponent();          
            IniciarConfiguracao();
           
        }


        public void ShowDialog()
        {
#if DEBUG
           
            base.ShowDialog();
#else
            var senhaMestraForm = new SenhaMestraForm();
            senhaMestraForm.ShowDialog();
            if (senhaMestraForm.SenhaCorreta)
                base.ShowDialog();
#endif




        } 

        public static string CaminhoSolution
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached ?
                    Path.GetFullPath(".").Substring(0, Path.GetFullPath(".").IndexOf("PDV.VIEW")) :
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
        public void IniciarConfiguracao()
        {
            servidorTextEdit.Text = iniFile.GetValue("Conexao_PDV", "servidor");
            portaTextEdit.Text = iniFile.GetValue("Conexao_PDV", "porta");
            bancoTextEdit1.Text = iniFile.GetValue("Conexao_PDV", "banco");
            usuarioTextEdit.Text = iniFile.GetValue("Conexao_PDV", "usuario");
            senhatextEdit.Text = Criptografia.DecodificaSenha(iniFile.GetValue("Conexao_PDV", "senha"));
            maxpooltextEdit.Text = iniFile.GetValue("Conexao_PDV", "max_pool_size");
            apibaseProdutostextEdit.Text = iniFile.GetValue("Conexao_PDV", "apibaseproduto");
            apiForcaVendastextEdit.Text = iniFile.GetValue("Conexao_PDV", "apiforcadevendas");
            apicomandatextEdit.Text = iniFile.GetValue("Conexao_PDV", "apicomanda");
            apilicenseTextEdit.Text = iniFile.GetValue("Conexao_PDV", "apilicense");
            caminhoDoPostGreeSQTextEdit.Text = iniFile.GetValue("Conexao_PDV", "caminhoorigempostgreeSql");
            caminhoDestinoBackupTextEdit.Text = iniFile.GetValue("Conexao_PDV", "caminhodestinobackup");
            stringConexaoForcaVendasTextEdit.Text = iniFile.GetValue("Conexao_PDV", "stringconexaoforcavendas");
            
            stringConexaoBaseProdutosTextEdit.Text = iniFile.GetValue("Conexao_PDV", "stringconexaobaseprodutos");
            textEditPortaImpressora.Text = iniFile.GetValue("Conexao_PDV", "portaimpressora");
            stringConexaoMataFomeTextEdit.Text = iniFile.GetValue("Conexao_PDV", "stringconexaomatafomedelivery");

        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            try
            {
                string folder = caminhoDestinoBackupTextEdit.Text; //nome do diretorio a ser criado
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                //string ApiLicense = Criptografia.CodificaSenha(apilicenseTextEdit.Text);
                string Senha = Criptografia.CodificaSenha(senhatextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "servidor", servidorTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "porta", portaTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "banco", bancoTextEdit1.Text);
                iniFile.SetValue("Conexao_PDV", "usuario", usuarioTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "senha", Senha);
                iniFile.SetValue("Conexao_PDV", "max_pool_size", maxpooltextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "apibaseproduto", apibaseProdutostextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "apiforcadevendas", apiForcaVendastextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "apicomanda", apicomandatextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "apilicense", apilicenseTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "caminhoorigempostgreeSql", caminhoDoPostGreeSQTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "caminhodestinobackup", caminhoDestinoBackupTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "stringconexaoforcavendas", stringConexaoForcaVendasTextEdit.Text);
                
                iniFile.SetValue("Conexao_PDV", "stringconexaobaseprodutos", stringConexaoBaseProdutosTextEdit.Text);
                iniFile.SetValue("Conexao_PDV", "portaimpressora", textEditPortaImpressora.Text);
                iniFile.SetValue("Conexao_PDV", "stringconexaomatafomedelivery", stringConexaoMataFomeTextEdit.Text);
                iniFile.Save(CaminhoSolution + (System.Diagnostics.Debugger.IsAttached ? "\\PDV.VIEW\\App_Data\\Start.ini" : "\\App_Data\\Start.ini"));
                MessageBox.Show(this, "Configurações salvas com sucesso.", "Configuração");
                Application.Exit();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Configuração");
            }
        }
       
        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            try
            {
                string folder = caminhoDestinoBackupTextEdit.Text; //nome do diretorio a ser criado
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string senha = senhatextEdit.Text;
                FuncaoBackup.PostgreSqlDump(caminhoDoPostGreeSQTextEdit.Text, caminhoDestinoBackupTextEdit.Text + "Backup"+bancoTextEdit1.Text , servidorTextEdit.Text, portaTextEdit.Text,bancoTextEdit1.Text,usuarioTextEdit.Text, senha);
               MessageBox.Show("Backup realizado com sucesso","Sucesso",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao fazer o backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            new ConversorCriptografia().Show();
        }

        private void ConfiguracaoSistema_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           
        }

        private void simpleButton3_Click_2(object sender, EventArgs e)
        {
            
        }
    }
}
