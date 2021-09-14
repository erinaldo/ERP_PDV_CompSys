using DFe.Utils;
using NFe.Classes;
using NFe.Classes.Servicos.Recepcao;
using NFe.Danfe.Fast.NFCe;
using NFe.Utils.NFe;
using PDV.CONTROLER.Funcoes;
using PDV.CONTROLLER.NFCE.Configuracao;
using PDV.DAO.Custom;
using System.Text;

namespace PDV.CONTROLLER.NFCE.Impressao
{
    public class ImpressaoNFCe : IConfigNFCe
    {
        public RetornoImpressaoNFCe ImprimirNFCe(decimal IDMovimentoFiscal)
        {
            var nfe = new NFe.Classes.NFe().CarregarDeXmlString(FuncoesMovimentoFiscal.GetXMLEnvio(IDMovimentoFiscal));
            retEnviNFe retEnvi = null;

            string XmlRetorno = FuncoesMovimentoFiscal.GetXMLRetorno(IDMovimentoFiscal);
            if (!string.IsNullOrEmpty(XmlRetorno))
                retEnvi = FuncoesXml.XmlStringParaClasse<retEnviNFe>(XmlRetorno);

            var nfeProc = new nfeProc()
            {
                NFe = nfe,
                protNFe = retEnvi.protNFe,
                versao = retEnvi.versao
            };

            RetornoImpressaoNFCe Retorno = new RetornoImpressaoNFCe();

            Retorno.danfe = new DanfeFrNfce(nfe, CONFIG_NFCe.ConfiguracaoDanfeNfce, CONFIG_NFCe.ConfiguracaoCsc.CIdToken, CONFIG_NFCe.ConfiguracaoCsc.Csc);
            if (retEnvi != null)
                Retorno.danfe = new DanfeFrNfce(nfeProc, CONFIG_NFCe.ConfiguracaoDanfeNfce, CONFIG_NFCe.ConfiguracaoCsc.CIdToken, CONFIG_NFCe.ConfiguracaoCsc.Csc);

            DAO.Entidades.Configuracao configExibirDialogo = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGUACAODANFE_EXIBIRCAIXADIALOGO);
            if (configExibirDialogo != null && "1".Equals(Encoding.UTF8.GetString(configExibirDialogo.Valor)))
                Retorno.isVisualizar = true;
            else
            {
                DAO.Entidades.Configuracao configNomeImpressora = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGUACAODANFE_NOMEIMPRESSORA);
                Retorno.isVisualizar = false;
                Retorno.NomeImpressora = configNomeImpressora != null ? Encoding.UTF8.GetString(configNomeImpressora.Valor) : string.Empty;
                Retorno.isCaixaDialogo = configExibirDialogo == null ? false : "1".Equals(Encoding.UTF8.GetString(configExibirDialogo.Valor));
            }
            return Retorno;
        }
    }

    public class RetornoImpressaoNFCe
    {
        public DanfeFrNfce danfe { get; set; }
        public bool isVisualizar { get; set; }
        public bool isCaixaDialogo { get; set; }
        public string NomeImpressora { get; set; }
    }
}