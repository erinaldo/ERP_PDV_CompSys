using PDV.CONTROLER.Funcoes;
using PDV.CONTROLER.FuncoesRelatorios;
using PDV.DAO.Entidades;
using System.Data;
using System.Drawing;
using System.IO;
namespace PDV.REPORTS.Reports.Modelo_2
{ 
    public partial class Modelo2Servico : DevExpress.XtraReports.UI.XtraReport
    {
        public Modelo2Servico(decimal idOs)
        {
            InitializeComponent();
            Emitente Emit = FuncoesEmitente.GetEmitente();
            Endereco End = FuncoesEndereco.GetEndereco(Emit.IDEndereco);
            UnidadeFederativa UnFed = FuncoesUF.GetUnidadeFederativa(End.IDUnidadeFederativa.Value);
            Municipio Mun = FuncoesMunicipio.GetMunicipio(End.IDMunicipio.Value);
            using (var ms = new MemoryStream(Emit.Logomarca))
                xrPictureBox1.Image = Image.FromStream(ms);
            empresaXrLabel.Text = Emit.RazaoSocial;
            cnpjxrLabel.Text = Emit.CNPJ;
            enderecoxrLabel.Text = $"{End.Logradouro}, {End.Numero}, {End.Bairro} , {Mun.Descricao} - {UnFed.Sigla}";
            telefoneEmpresaXrLabel.Text = End.Telefone;
            DataTable dt = FuncoesOrdemServico.GetOrdemDeServicoModelo2(idOs);

            dt.TableName = "objectDataSource1";
            objectDataSource1.DataSource = dt;
            
        }

    }
}
