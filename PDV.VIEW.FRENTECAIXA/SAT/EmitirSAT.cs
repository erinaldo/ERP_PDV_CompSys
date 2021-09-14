using ACBr.Net.DFe.Core.Common;
using ACBr.Net.Sat;
using CFeImpressao;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.PDV;
using PDV.VIEW.FRENTECAIXA.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDV.VIEW.FRENTECAIXA.SAT
{
  
    public  class EmitirSAT
    {
        public  ACBrSat acbrSat;
        public  Emitente Emitente;
        public  Caixa caixa;
        private CFe cfeAtual;
        private CFeCanc cfeCancAtual;
        private SatRede redeAtual;
        private readonly ACBrConfig config;
        public EmitirSAT(GPDV_PainelInicial TelaInicial)
        {

            FluxoCaixa fluxoCaixa = FuncoesFluxoCaixa.GetFluxoCaixa(TelaInicial.VENDA.IDFluxoCaixa);
            Emitente = FuncoesEmitente.GetEmitente();
            caixa = FuncoesCaixa.GetCaixa(fluxoCaixa.CaixaID);
            /// 
            acbrSat = new ACBrSat();
            this.acbrSat.Arquivos.PrefixoArqCFe = "AD";
            this.acbrSat.Arquivos.PrefixoArqCFeCanc = "ADC";
            this.acbrSat.Arquivos.SalvarCFe = true;
            this.acbrSat.Arquivos.SalvarCFeCanc = true;
            this.acbrSat.Arquivos.SalvarEnvio = true;
            this.acbrSat.Arquivos.SepararPorCNPJ = true;
            this.acbrSat.Arquivos.SepararPorMes = true;
            this.acbrSat.CodigoAtivacao = Emitente.CodigoAtivacao;
            this.acbrSat.Configuracoes.EmitCNPJ = Emitente.Homologacao == true ? "11111111111111" : Emitente.CNPJ;
            this.acbrSat.Configuracoes.EmitCRegTrib = ACBr.Net.Sat.RegTrib.SimplesNacional;
            this.acbrSat.Configuracoes.EmitCRegTribISSQN = ACBr.Net.Sat.RegTribIssqn.Nenhum;
            this.acbrSat.Configuracoes.EmitIE = Emitente.Homologacao == true ? "111111111111" : Emitente.InscricaoEstadual.PadLeft(12, '0');
            this.acbrSat.Configuracoes.EmitIM = "";
            this.acbrSat.Configuracoes.EmitIndRatISSQN = ACBr.Net.Sat.RatIssqn.Nao;
            this.acbrSat.Configuracoes.IdeCNPJ = Emitente.Homologacao == true ? "22222222222222" : Emitente.CNPJSoftwareHouse;
            this.acbrSat.Configuracoes.IdeNumeroCaixa = int.Parse(caixa.IDCaixa.ToString());
            this.acbrSat.Configuracoes.IdeTpAmb = ACBr.Net.DFe.Core.Common.DFeTipoAmbiente.Homologacao;
            this.acbrSat.Configuracoes.InfCFeVersaoDadosEnt = decimal.Parse(Emitente.VersaoXML);
                this.acbrSat.Configuracoes.IsUtf8 = true;
            this.acbrSat.Configuracoes.NumeroTentativasValidarSessao = 1;
            this.acbrSat.Configuracoes.RemoverAcentos = true;
            this.acbrSat.Configuracoes.ValidarNumeroSessaoResposta = false;
            this.acbrSat.IntegradorFiscal.Configuracoes.ChaveAcessoValidador = Emitente.chaveAcessoValidador;

            if (Emitente.ModeloSAT == "Cded")
                this.acbrSat.Modelo = ACBr.Net.Sat.ModeloSat.Cdecl;
            else if (Emitente.ModeloSAT == "StdCall")
                this.acbrSat.Modelo = ACBr.Net.Sat.ModeloSat.StdCall;
            else if (Emitente.ModeloSAT == "Normal")
                this.acbrSat.Modelo = ACBr.Net.Sat.ModeloSat.MFeIntegrador;

            this.acbrSat.PathDll = "C:\\SAT\\SAT.dll";
            this.acbrSat.SignAC = Emitente.Homologacao == true ? "11111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111"
                : Emitente.SignAC;

            if (!acbrSat.Ativo) 
                ToogleInitialize();
            acbrSat.ConsultarSAT();

        }
        private void ToogleInitialize()
        {
            if (acbrSat.Ativo)
            {
                acbrSat.Desativar();
                //btnIniDesini.Text = @"Inicializar";
            }
            else
            {
                acbrSat.Ativar();
             //  btnIniDesini.Text = @"Desinicializar";
            }
        }

        private void LoadConfig()
        {
            if (acbrSat.Ativo) ToogleInitialize();

            if (Emitente.ModeloSAT == "Cded")
                config.Get("ModeloSat", ModeloSat.Cdecl);
            else if (Emitente.ModeloSAT == "StdCall")
                config.Get("ModeloSat", ModeloSat.StdCall);
            else if (Emitente.ModeloSAT == "Normal")
                config.Get("ModeloSat", ModeloSat.MFeIntegrador);

            config.Get("DllPath", @"C:\SAT\SAT.dll");
            config.Get("Ambiente", Emitente.Homologacao == true ? DFeTipoAmbiente.Homologacao : DFeTipoAmbiente.Producao);
            config.Get("Ativacao", Emitente.CodigoAtivacao);
            if (caixa.TipoDeVenda == "MFe")
            {
                config.Get("CodUF", "23");
            }
            else
            {
                config.Get("CodUF", "35");
            }
            config.Get("VersaoCFe", Emitente.VersaoXML);
            config.Get("UTF8", false);
            config.Get("RemoveAcentos", false);
            config.Get("SaveEnvio", true);
            config.Get("SaveCFe", true);
            config.Get("SaveCFeCanc", true);
            config.Get("SepararCNPJ", true);
            config.Get("SepararData", true);
            config.Get("EmitCNPJ", Emitente.Homologacao == true ? "11111111111111" : Emitente.CNPJ);
            config.Get("EmitIE", Emitente.Homologacao == true ? "111111111111" : Emitente.InscricaoEstadual.PadLeft(12, '0'));
            config.Get("EmiRegTrib", RegTrib.SimplesNacional);
            config.Get("EmiRegTribISSQN", RegTribIssqn.Nenhum);
            config.Get("EmiRatIISQN", RatIssqn.Sim);
            config.Get("IdeCNPJ", Emitente.Homologacao == true ? "22222222222222" : Emitente.CNPJSoftwareHouse);
            config.Get("SignAC", Emitente.Homologacao == true ? "11111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111"
                : Emitente.SignAC);
            config.Get("MFePathEnvio", Emitente.PastaInput);
            config.Get("MFePathResposta", Emitente.PastaOutPut);
            config.Get("MFeTimeOut", 45000M);
            config.Get("ChaveAcessoValidador", Emitente.chaveAcessoValidador);
            config.Save();

           // MessageBox.Show(this, @"Configurações Carregada com sucesso !", @"S@T Demo");
        }

        private void SaveConfig(bool msg = true)
        {
            if (acbrSat.Ativo) ToogleInitialize();

            if (Emitente.ModeloSAT == "Cded")
                config.Set("ModeloSat", ModeloSat.Cdecl);
            else if (Emitente.ModeloSAT == "StdCall")
                config.Set("ModeloSat", ModeloSat.StdCall);
            else if (Emitente.ModeloSAT == "Normal")
                config.Set("ModeloSat", ModeloSat.MFeIntegrador);

            config.Set("DllPath", @"C:\SAT\SAT.dll");
            config.Set("Ambiente", Emitente.Homologacao == true ? DFeTipoAmbiente.Homologacao : DFeTipoAmbiente.Producao);
            config.Set("Ativacao", Emitente.CodigoAtivacao);
            if (caixa.TipoDeVenda == "MFe")
            {
                config.Set("CodUF", "23");
            }
            else
            {
                config.Set("CodUF", "35");
            }
            config.Set("VersaoCFe", Emitente.VersaoXML);
            config.Set("UTF8", false);
            config.Set("RemoveAcentos", false);
            config.Set("SaveEnvio", true);
            config.Set("SaveCFe", true);
            config.Set("SaveCFeCanc", true);
            config.Set("SepararCNPJ", true);
            config.Set("SepararData", true);
            config.Set("EmitCNPJ", Emitente.Homologacao == true ? "11111111111111" : Emitente.CNPJ);
            config.Set("EmitIE", Emitente.Homologacao == true ? "111111111111" : Emitente.InscricaoEstadual.PadLeft(12, '0'));
            config.Set("EmiRegTrib", RegTrib.SimplesNacional);
            config.Set("EmiRegTribISSQN", RegTribIssqn.Nenhum);
            config.Set("EmiRatIISQN", RatIssqn.Sim);
            config.Set("IdeCNPJ", Emitente.Homologacao == true ? "22222222222222" : Emitente.CNPJSoftwareHouse);
            config.Set("SignAC", Emitente.Homologacao == true ? "11111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111"
                : Emitente.SignAC);
            config.Set("MFePathEnvio", Emitente.PastaInput);
            config.Set("MFePathResposta", Emitente.PastaOutPut);
            config.Set("MFeTimeOut", 45000M);
            config.Set("ChaveAcessoValidador", Emitente.chaveAcessoValidador);
            config.Save();

            //if (msg)
            //{
            //    MessageBox.Show(this, @"Configurações Salva com sucesso !", @"S@T Demo");
            //}
        }

    }
}
