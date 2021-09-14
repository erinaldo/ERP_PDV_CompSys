using ACBr.Net.Sat;
using System;
using System.Linq;
using System.Text;
using PDV.DAO.Entidades;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Entidades.PDV;
using PDV.VIEW.FRENTECAIXA.Forms;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using System.Windows.Forms;
using IntegracaoMFE;
using CFeImpressao;
using System.Diagnostics;
using DFe.Classes.Flags;
using PDV.VIEW.Forms.Cadastro;
using PDV.VIEW.FRENTECAIXA.Forms.PDV;
using PDV.DAO.Custom;
using System.ComponentModel;
using Gerene.DFe.EscPos;
using PDV.VIEW.App_Context;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.Integrador;
using Vip.Printer.Enums;
using System.Collections.Generic;

namespace PDV.VIEW.FRENTECAIXA.MFe.Emissao
{
    public class EmitirSAT
    {
        public MFE mfe { get; set; }
        public RetornoMFe retornoMFe;
        public RetornoPagamento retornoPagamento;
        public Emitente Empresa;
        public Caixa caixa;
        public ACBrSat acbrSat;
        FluxoCaixa fluxoCaixa;
        private CFe cfeAtual;
        private CFeCanc cfeCancAtual;
        private SatRede redeAtual;


        public class Log
        {
            public DateTime DataHora { get; set; }
            public string Mensagem { get; set; }
        }
        public static class WSMonitoramento
        {
            private static BindingList<Log> _Log { get; set; }
            public static BindingList<Log> Log
            {
                get
                {
                    if (_Log == null)
                    {
                        _Log = new BindingList<Log>();
                        _Log.AllowEdit = false;
                        _Log.AllowNew = true;
                        _Log.AllowRemove = true;
                    }

                    return _Log;
                }
                set
                {
                    _Log = value;
                }
            }

            public static void AddLog(string msg)
            {
                AddLog(new Log() { DataHora = DateTime.Now, Mensagem = msg });

            }
            public static void AddLog(Log log)
            {
                try
                {
                    Log.Add(log);

                    if (Log.Count > 220)
                    {
                        Log = new BindingList<Log>(Log.ToList());
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public Endereco endereco { get; set; }

        public UnidadeFederativa unidadeFederativa { get; set; }

        

        public ACBrIntegrador acbrIntegrador;
        public EmitirSAT()
        {
            try
            {
                Empresa = FuncoesEmitente.GetEmitente();
                 endereco = FuncoesEndereco.GetEndereco(Empresa.IDEndereco);
                unidadeFederativa = FuncoesUF.GetUnidadeFederativa(endereco.IDUnidadeFederativa ?? 0);
                acbrSat = new ACBrSat();
                this.acbrSat.Arquivos.PrefixoArqCFe = "AD";
                this.acbrSat.Arquivos.PrefixoArqCFeCanc = "ADC";
                this.acbrSat.Arquivos.SalvarCFe = true;
                this.acbrSat.Arquivos.SalvarCFeCanc = true;
                this.acbrSat.Arquivos.SalvarEnvio = true;
                this.acbrSat.Arquivos.SepararPorCNPJ = true;
                this.acbrSat.Arquivos.SepararPorMes = true;
                this.acbrSat.CodigoAtivacao =Empresa.CodigoAtivacao;

                this.acbrSat.Configuracoes.EmitCRegTrib = ACBr.Net.Sat.RegTrib.SimplesNacional;
                this.acbrSat.Configuracoes.EmitCRegTribISSQN = ACBr.Net.Sat.RegTribIssqn.Nenhum;

                this.acbrSat.Configuracoes.EmitIM = "";
                this.acbrSat.Configuracoes.EmitIndRatISSQN = ACBr.Net.Sat.RatIssqn.Nao;
                this.acbrSat.Configuracoes.IdeTpAmb = Empresa.Homologacao ? DFeTipoAmbiente.Homologacao : DFeTipoAmbiente.Producao;

                this.acbrSat.Configuracoes.InfCFeVersaoDadosEnt = 0.07M;//Convert.ToDecimal(String.Format("{0:00.0}", Global.Empresa.VersaoXML.ToString()));
                this.acbrSat.Configuracoes.IsUtf8 = true;
                this.acbrSat.Configuracoes.NumeroTentativasValidarSessao = 1;
                this.acbrSat.Configuracoes.RemoverAcentos = true;
                this.acbrSat.Configuracoes.ValidarNumeroSessaoResposta = false;

                if (Empresa.ModeloSAT == "Cded")
                    this.acbrSat.Modelo = ACBr.Net.Sat.ModeloSat.Cdecl;
                else if (Empresa.ModeloSAT == "StdCall")
                    this.acbrSat.Modelo = ACBr.Net.Sat.ModeloSat.StdCall;
                //MFE CEARA
                if (unidadeFederativa.Sigla== "CE")
                {
                    this.acbrSat.PathDll = "C:\\SAT\\mfe.dll";
                    acbrIntegrador = new ACBrIntegrador();
                    this.acbrSat.Configuracoes.EmitCNPJ = Empresa.Homologacao ? "08723218000186" : Empresa.CNPJ;
                    this.acbrSat.Configuracoes.EmitIE = Empresa.Homologacao ? "562377111111" : Empresa.InscricaoEstadual.PadLeft(12, '0');
                    this.acbrSat.Configuracoes.IdeCNPJ = Empresa.Homologacao ? "16716114000172" : Empresa.CNPJSoftwareHouse;
                    acbrIntegrador.Configuracoes.ChaveAcessoValidador = Empresa.chaveAcessoValidador;//25CFE38D-3B92-46C0-91CA-CFF751A82D3D Essa chave no momento está sendo utilizada tanto para Homologação como Produção para todo mundo
                    acbrIntegrador.Configuracoes.TimeOut = 6000;
                    acbrIntegrador.Configuracoes.PastaInput = Empresa.PastaInput;
                    acbrIntegrador.Configuracoes.PastaOutput = Empresa.PastaOutPut;
                    this.acbrSat.SignAC = Empresa.Homologacao ? "SGR-SAT SISTEMA DE GESTAO E RETAGUARDA DO SAT"
                        : Empresa.SignAC;

                    //Usando a biblioteca da MFE Integração para trabalhar com envio de pagamentos e resposta fiscal
                    mfe = new MFE();
                    //Preencher os Dados Obrigatórios para toda as funções
                    mfe.CodigoAtivacao = Empresa.CodigoAtivacao;// "12345678"; //Codigo de Ativação do Equipamento
                    mfe.GravarXmlEmPasta = true; //Se Deseja gravar o XML em uma pasta
                    mfe.LocalArquivoXML = @"C:\Temp"; //Se o campo acima for true deve informar o local
                    mfe.PastaImput = Empresa.PastaInput;//@"C:\Integrador\Input"; // Local da Pasta Input do Integrador (esta informado as configurações do  Integrador Fiscal)
                    mfe.PastaOutput = Empresa.PastaOutPut;//@"C:\Integrador\Output"; // Local da Pasta OutPut do Integrador (esta informado as configurações do  Integrador Fiscal)
                    mfe.TipoEnvioDLL = true; //Se utiliza o Software da Sefaz Drive MFE de Comunicação Direta, deve informar como true

                }
                else
                {
                    this.acbrSat.SignAC = Empresa.Homologacao ? "11111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111111112222222222222211111111111111222222222222221111111111111122222222222222111111111"
                       : Empresa.SignAC;
                    this.acbrSat.PathDll = "C:\\SAT\\SAT.dll";
                    this.acbrSat.Configuracoes.EmitCNPJ = Empresa.Homologacao ? "11111111111111" : Empresa.CNPJ;
                    this.acbrSat.Configuracoes.EmitIE = Empresa.Homologacao  ? "111111111111" : Empresa.InscricaoEstadual.PadLeft(12, '0');
                    this.acbrSat.Configuracoes.IdeCNPJ = Empresa.Homologacao  ? "22222222222222" : Empresa.CNPJSoftwareHouse;
                }

                if (!acbrSat.Ativo)
                    ToogleInitialize();
                acbrSat.ConsultarSAT();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro!");
                MessageBox.Show(ex.Message + "   " + ex.StackTrace ?? "");
            }
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
        private CFe novoCfe;
        public RetornoMFe TransmitirSAT(GPDV_PainelInicial TelaInicial , Caixa CaixaPDV)
        {
            try
            {
                caixa = CaixaPDV;
                this.acbrSat.Configuracoes.IdeNumeroCaixa = int.Parse(CaixaPDV.IDCaixa.ToString());
                WSMonitoramento.Log = null;
                TipoDeOperacao tipoDeOperacao;
                Configuracao config3 = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.TIPO_OPERACAO_PADRAO_PDV);
                if (config3 != null)
                {
                    var idtipo = Encoding.UTF8.GetString(config3.Valor);
                    tipoDeOperacao = FuncoesTipoDeOperacao.GetTipoDeOperacao(decimal.Parse(idtipo));
                    if (tipoDeOperacao == null)
                    {
                        throw new Exception("Tipo de operação padrão do PDV não foi definido nas configurações da venda. Defina um tipo de operação para vendas no PDV no retaguarda.");
                    }
                }
                else
                {
                    throw new Exception("Tipo de operação padrão do PDV não  foi definido nas configurações da venda. Defina um tipo de operação para vendas no PDV.");
                }
                retornoMFe = new RetornoMFe();
                retornoPagamento = new RetornoPagamento();
                novoCfe = acbrSat.NewCFe();
                WSMonitoramento.AddLog("Conectando com a SEFAZ Aguarde...");
                decimal? IDMovimentoFiscal;
               
                WSMonitoramento.AddLog("Transmitindo Cupom...");
                novoCfe.InfCFe.Ide.NumeroCaixa = int.Parse(CaixaPDV.IDCaixa.ToString()); //Pode ser uma serie ou o numero do Caixa de cada maquina
                #region Dest
                //Se cliente Identificado na venda informe os campos abaixo, senão não informe nenhum dos 2
                if (TelaInicial.Cliente != null)
                {
                    if(TelaInicial.Cliente.TipoDocumento == 1)
                    novoCfe.InfCFe.Dest.CPF = TelaInicial.Cliente._CPF_CNPJ;
                    else
                        novoCfe.InfCFe.Dest.CNPJ = TelaInicial.Cliente._CPF_CNPJ;
                    //ou CNPJ: novoCfe.InfCFe.Dest.CNPJ = "08723218000186";
                    novoCfe.InfCFe.Dest.Nome = TelaInicial.Cliente.Nome == null ? "NAO Identificado" : TelaInicial.Cliente.Nome; //Se informou CPF ou CNPJ deve informar o Nome, caso não souber o nome pode informar  "Nao Identificado"
                    
                }
                else
                {
                    novoCfe.InfCFe.Dest.Nome = "NAO Identificado";
                }
                #endregion

                #region Itens
                decimal ValorTributosNacionalTot = 0;
                decimal ValorTributosEstadualTot = 0;
                decimal ValorTributosMunicipalTot = 0;
                decimal ValorAproxImposTot = 0;
                int Item = 0;
                foreach (var item in TelaInicial.ITENS_VENDA)
                {
                    Item += 1;
                    Produto _Produto = FuncoesProduto.GetProduto(item.IDProduto);
                    IntegracaoFiscal IntegracaoFiscal = FuncoesIntegracaoFiscal.GetIntegracao(_Produto.IDIntegracaoFiscalNFCe.Value);

                    Ncm NcmVigente = FuncoesNcm.GetItemTributoVigente(FuncoesNcm.GetNCM(_Produto.IDNCM).Codigo, string.IsNullOrEmpty(_Produto.EXTipi) ? null : (decimal?)Convert.ToDecimal(_Produto.EXTipi), TelaInicial.VENDA.DataCadastro);
                    if (NcmVigente == null)
                    {
                        throw new Exception(string.Format("O produto \"{0}\" não possui tributação vigênte. Verifique o cadastro do IBPT e tente novamente.", _Produto.Descricao));
                    }
                    decimal ValorTributosNacional = (_Produto.ValorVenda * (NcmVigente.NacionalFederal / 100)) * item.Quantidade;
                    decimal ValorTributosEstadual = (_Produto.ValorVenda * (NcmVigente.Estadual / 100)) * item.Quantidade;
                    decimal ValorTributosMunicipal = (_Produto.ValorVenda * (NcmVigente.Municipal / 100)) * item.Quantidade;
                    decimal ValorAproxImpos = ValorTributosNacional + ValorTributosEstadual + ValorTributosMunicipal;

                    ValorTributosNacionalTot += ValorTributosNacional;
                    ValorTributosEstadualTot += ValorTributosEstadual;
                    ValorTributosMunicipalTot += ValorTributosMunicipalTot;
                    ValorAproxImposTot += ValorTributosNacional + ValorTributosEstadual + ValorTributosMunicipalTot;


                    var unidade = FuncoesUnidadeMedida.GetUnidadeMedida(_Produto.IDUnidadeDeMedida).Sigla;
                    //Fazer um Loop para cada item
                    CFeDet detCfe = novoCfe.InfCFe.Det.AddNew();
                    detCfe.NItem = Item;
                    detCfe.Prod.CProd = _Produto.Codigo;
                    detCfe.Prod.XProd = _Produto.Descricao;
                    detCfe.Prod.NCM = NcmVigente.Codigo.ToString();
                    string cest = _Produto.CEST == null ? _Produto.CEST : null;
                    if (novoCfe.InfCFe.VersaoDadosEnt.Equals(0.08M) && string.IsNullOrEmpty(cest)) // Se a versão for 0.08 e tiver CEST deve informar o campo abaixo
                        detCfe.Prod.CEST = cest;
                    detCfe.Prod.CFOP = FuncoesCFOP.GetCFOP(tipoDeOperacao.IDOperacaoFiscal).Codigo;
                    detCfe.Prod.UCom = unidade;
                    detCfe.Prod.QCom =unidade.Contains("KG") ? item.Quantidade / 1000 : item.Quantidade;
                    detCfe.Prod.VUnCom = item.ValorUnitarioItem; // Valor Unitario
                    detCfe.Prod.IndRegra = IndRegra.Arredondamento;
                    decimal desconto = item.DescontoValor;
                    if (desconto > 0) //Se tiver desconto no item informar
                        detCfe.Prod.VDesc = desconto;
                    detCfe.Imposto.VItem12741 = ValorAproxImpos; // Valor Aproximado dos tributos

                    //Impostos do Produto
                    det impostos = ImpostoProdutoMFe.GetDetalhe(TelaInicial.VENDA, item, 1, NFe.Classes.Informacoes.Emitente.CRT.SimplesNacional, ModeloDocumento.NFCe, Empresa);

                    if (impostos.imposto.ICMS.TipoICMS.GetType() == typeof(ICMS00))
                    {
                        var teste = (impostos.imposto.ICMS.TipoICMS as ICMS00).pICMS.ToString();

                        detCfe.Imposto.Imposto = new ImpostoIcms
                        {
                            Icms = new ImpostoIcms00
                            {
                                Orig = OrigemMercadoria.Nacional,
                                Cst = "00",
                                PIcms = (impostos.imposto.ICMS.TipoICMS as ICMS00).pICMS,
                                VIcms = 0,
                            }
                        };
                    }
                    else if (impostos.imposto.ICMS.TipoICMS.GetType() == typeof(ICMS40))
                    {
                        detCfe.Imposto.Imposto = new ImpostoIcms
                        {
                            Icms = new ImpostoIcms40
                            {
                                Orig = OrigemMercadoria.Nacional,
                                Cst = "40",
                            }
                        };
                    }

                    else if (impostos.imposto.ICMS.TipoICMS.GetType() == typeof(ICMSSN102))
                    {
                        detCfe.Imposto.Imposto = new ImpostoIcms
                        {
                            Icms = new ImpostoIcmsSn102
                            {
                                Orig = OrigemMercadoria.Nacional,
                                Csosn = "102",
                            }
                        };
                    }
                    else if (impostos.imposto.ICMS.TipoICMS.GetType() == typeof(ICMSSN102))
                    {
                        detCfe.Imposto.Imposto = new ImpostoIcms
                        {
                            Icms = new ImpostoIcmsSn102
                            {
                                Orig = OrigemMercadoria.Nacional,
                                Csosn = "102",
                            }
                        };
                    }

                    else if (impostos.imposto.ICMS.TipoICMS.GetType() == typeof(ICMSSN500))
                    {
                        detCfe.Imposto.Imposto = new ImpostoIcms
                        {
                            Icms = new ImpostoIcmsSn900
                            {
                                Orig = OrigemMercadoria.Nacional,
                                Csosn = "500",
                            }
                        };
                    }
                    //PIS
                    detCfe.Imposto.Pis.Pis = new ImpostoPisAliq
                    {
                        Cst = "01",
                    };
                    //COFINS
                    detCfe.Imposto.Cofins.Cofins = new ImpostoCofinsAliq
                    {
                        Cst = "01",
                    };

                    detCfe.InfAdProd = "";
                }

                #endregion

                #region Pagamentos
                WSMonitoramento.AddLog("Processando pagamentos...");
                //Fazer um Loop para cada Pagamento


                bool multipagamentos = false;
                lstPagamentoProcessado = new List<PagamentoMFe>();
                if (TelaInicial.PAGAMENTOS.Count > 1)
                {
                    multipagamentos = true;
                }
                foreach (var item in TelaInicial.PAGAMENTOS)
                {
                    
                    CFePgtoMp pag = novoCfe.InfCFe.Pagto.Pagamentos.AddNew();

                    var formaDePagamento = FuncoesFormaDePagamento.GetFormaDePagamento(item.IDFormaDePagamento);
                    //Se for pagamento em cartão de credito ou em debito colocar no cadastro de forma de pagamento transação a prazo
                    if (formaDePagamento.Transacao == 2)
                    {
                        WSMonitoramento.AddLog("Processando pagamento no cartão...");
                        pag.CMp = CodigoMP.CartaodeCredito;
                        if (novoCfe.InfCFe.VersaoDadosEnt.Equals(0.08M) && (pag.CMp == CodigoMP.CartaodeCredito || pag.CMp == CodigoMP.CartaodeDebito)) // Se versão = 0.08 e for Cartão deve informar o Código da Administadora de cartao
                            pag.CAdmC = 999;
                        pag.VMp = Convert.ToDecimal(item.Valor);

                        if (unidadeFederativa.Sigla == "CE")
                        {
                            retornoPagamento = EnviarPagamento(novoCfe, multipagamentos, pag.VMp, TelaInicial);
                            WSMonitoramento.AddLog($"Pagamento Enviado com Sucesso (CREDITO)...");

                            if (retornoPagamento.EnviouPagamento == false)
                            {
                                retornoMFe.Enviado = false;
                                retornoMFe.CFe = null;
                                retornoMFe.Resposta = "Operação Cancelada problemas ao processar pagamentos";
                            }
                        }
                    }
                   
                    else 
                    {
                        WSMonitoramento.AddLog("Enviando pagamento (DINHEIRO)...");
                        pag.CMp = CodigoMP.Dinheiro;
                        pag.VMp = Convert.ToDecimal(item.Valor);
                        WSMonitoramento.AddLog($"Pagamento Enviado com sucesso (DINHEIRO)...");
                    }
                }

                #endregion

                #region Informações Adicinais
                //novoCfe.InfCFe.Pagto.VTroco = TelaInicial.VENDA.Troco;
                //novoCfe.InfCFe.Total.VCFe = TelaInicial.VENDA.ValorTotal;
                novoCfe.InfCFe.InfAdic.InfCpl = "Trib. aprox.R$: " + ValorAproxImposTot + "  Sendo,Federal :" + ValorTributosNacionalTot + " Estadual : " + ValorTributosEstadualTot + "  .Lei fed 12741/2012"; //Informações Complementares
                novoCfe.InfCFe.InfAdic.InfCpl = ValorAproxImposTot.ToString("n2");
                #endregion

                #region Autorização
                WSMonitoramento.AddLog("Autorizando cupom...");

                if (!acbrSat.Ativo) ToogleInitialize();
                var ret = acbrSat.EnviarDadosVenda(novoCfe);
                cfeAtual = ret.Venda;

                String xmlCFess = novoCfe.GetXml();

                String TextoRetorno = "";
                TextoRetorno = $"{ret.CodigoDeRetorno }  {ret.MensagemRetorno}";
                WSMonitoramento.AddLog($"{TextoRetorno.ToString()}");

              
                if (ret.CodigoDeRetorno == 6000)
                {
                     TextoRetorno = $"{ret.CodigoDeRetorno} Cupom foi Autorizado com sucesso . Chave da Nota {ret.ChaveConsulta}";
                    //Gravar no BD o XML da Venda ou os dados da Nota

                
                    String xmlCFe = cfeAtual.GetXml();
                    //Inserir Informações Documento Fiscal
                    IDMovimentoFiscal = PDV.DAO.DB.Utils.Sequence.GetNextID("MOVIMENTOFISCAL", "IDMOVIMENTOFISCAL");
                    PDVControlador.BeginTransaction();
                    WSMonitoramento.AddLog("Salvando o cupom...");
                    if (!FuncoesMovimentoFiscal.Salvar(new MovimentoFiscal()
                    {
                        IDMovimentoFiscal = IDMovimentoFiscal.Value,
                        Cancelada = 0,
                        cStat = Convert.ToDecimal(ret.CodigoDeRetorno),
                        DataRecebimento = DateTime.Now,
                        DataEmissao = DateTime.Now,
                        Emitida = 1,
                        IDVenda = TelaInicial.VENDA.IDVenda,
                        Serie = 99,
                        xMotivo = ret.MensagemRetorno,
                        Chave = cfeAtual.InfCFe.Id,
                        XMLEnvio = Encoding.Default.GetBytes(cfeAtual.GetXml()),
                        XMLRetorno = Encoding.Default.GetBytes(cfeAtual.GetXml()),
                        Numero = cfeAtual.InfCFe.Ide.CNf,
                        Contingencia = 0,
                        TipoDocumento = cfeAtual.InfCFe.Ide.Modelo,
                        Ambiente = (decimal)cfeAtual.InfCFe.Ide.TpAmb

                    }))
                    {
                        PDVControlador.Rollback();
                        throw new Exception("Não foi possível salvar a CF-e.");
                       
                    }
                    PDVControlador.Commit();
                    CFe cFe = new CFe();
                    cFe = CFe.Load(xmlCFe);
                    //Resposta Fiscal

                    WSMonitoramento.AddLog("Enviando resposta fiscal...");

                    if (unidadeFederativa.Sigla == "CE")
                    {
                        
                        EnviarRespostaFiscal(cFe, TelaInicial);
                    }
                    retornoMFe.Enviado = true;
                    retornoMFe.CFe = cFe;
                    retornoMFe.Resposta = ret.MensagemSEFAZ;

                    DAO.Entidades.Configuracao configNomeImpressora = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.CHAVE_CONFIGUACAODANFE_NOMEIMPRESSORA);
                    var impressora = configNomeImpressora != null ? Encoding.UTF8.GetString(configNomeImpressora.Valor) : string.Empty;
                    if (impressora == null)
                    {
                        WSMonitoramento.AddLog("Erro");
                        WSMonitoramento.AddLog("Impressora não configurada...");
                    }
                    else
                    {
                       

                        WSMonitoramento.AddLog("Imprimindo o cupom...");
                        using (var _printer = new SatPrinter()) //ou new NFCePrinter() para NFCe
                        {
                            int tipoimpressora = 0;
                            _printer.TipoImpressora = (PrinterType)tipoimpressora;//(PrinterType)tipoimpressora;//Epson para EscPos / Bematech para EscBema / Daruma para EscDaruma
                            _printer.NomeImpressora = impressora;
                            _printer.CortarPapel = true;
                            _printer.ProdutoDuasLinhas = false;
                            _printer.UsarBarrasComoCodigo = false;
                            _printer.DocumentoCancelado = false; //Exibe tarja "Documento cancelado na impressão"
                            _printer.Logotipo = Empresa.Logomarca; //Impressão do logotipo, não obrigatório
                            _printer.TipoPapel = TipoPapel.Tp80mm; //ou TipoPapel.Tp58mm
                            _printer.Imprimir(xmlCFe); //Imprime o documento fiscal
                        }
                                             
                        WSMonitoramento.AddLog("Sucesso");
                    }

                  //  MessageBox.Show("Cupom Autorizado com sucesso!");
                }
                else
                {
                    WSMonitoramento.AddLog($"{TratarRetornoSAT(ret.CodigoDeRetorno)}");

                    MessageBox.Show(TratarRetornoSAT(ret.CodigoDeRetorno));
                    retornoMFe.Enviado = false;
                    retornoMFe.CFe = null;
                    retornoMFe.Resposta = ret.RetornoStr;
                    throw new Exception(ret.RetornoStr);
                }
                #endregion

                return retornoMFe;
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro ao transmitir o SAT: {ex.Message}");
                WSMonitoramento.AddLog($"Erro ao transmitir o SAT: {ex.Message}");
                retornoMFe.Enviado = false;
                retornoMFe.CFe = null;
                retornoMFe.Resposta = "Erro :" + ex.Message;
               return retornoMFe;
            }
        }
        public void EnviarRespostaFiscal(CFe cFe, GPDV_PainelInicial TelaInicial)
        {
            //          string _chaveAcesso = cfeAtual.InfCFe.Id;
            //if(lstPagamentoProcessado != null)
            //{
            //    foreach (var item in lstPagamentoProcessado)
            //    {

            //        var resposta = acbrIntegrador.RespostaFiscal(
            //            idFila: int.Parse(item.IDPagamento.ToString()),
            //            chaveAcesso: _chaveAcesso,
            //            nsu: item.NSU.ToString(),
            //            numeroAprovacao: item.Autorizacao,
            //            bandeira: item.Bandeira,
            //            adquirinte: item.Adquirente,
            //            impressaofiscal: "",
            //            numeroDocumento: TelaInicial.VENDA.IDVenda.ToString(),
            //            cnpj: Empresa.CNPJ
            //        );

            //        var ret = resposta.GetXml();
            //    }

            //}
            var _chaveAcesso = cfeAtual.InfCFe.Id;
            foreach (var item in lstPagamentoProcessado)
            {

                string chaveAcessoValidador = "25CFE38D-3B92-46C0-91CA-CFF751A82D3D"; // Essa chave no momento está sendo utilizada tanto para Homologação como Produção para todo mundo
                string chaveAcesso = _chaveAcesso; //Quando for modelo 59 (MFE) deve informar o CFe na frente da Chave de Acesso
                string NSU = item.NSU;
                string autorizacao = item.Autorizacao;
                string nomeBandeira = item.Bandeira;
                string nomeAdquirente = item.Adquirente;
                string cnpjLoja = Empresa.CNPJ; //CNPJ do comercio
                string numeroNotaFiscal = cfeAtual.InfCFe.Ide.NCFe.ToString();
                var retornoMFE = mfe.RespostaFiscal(chaveAcessoValidador, chaveAcesso, NSU, autorizacao, nomeBandeira, nomeAdquirente, cnpjLoja, numeroNotaFiscal,"");
                var result = "Codigo: " + retornoMFE.CodigoRetorno + Environment.NewLine +
                                                   "Descrição: " + retornoMFE.DescricaoRetorno + Environment.NewLine +
                                                   "Retorno VFP-E: idRespostaFiscal = " + mfe.RetornoVFPE == null ? "" : mfe.RetornoVFPE.IdRespostaFiscal.ToString();
                if (retornoMFE.VFeRealizada())
                {
                    //Gravar no BD o IDRespostaFiscal
                    long idRespostaFiscal = mfe.RetornoVFPE.IdRespostaFiscal;
                    TelaInicial.VENDA.IdRespostaFiscal = idRespostaFiscal.ToString();
                }
                else
                {

                }
            }

        }

        public PagamentoMFe PagamentoProcessado;
        public List<PagamentoMFe> lstPagamentoProcessado;
        public RetornoPagamento EnviarPagamento(CFe cFe, bool Multiforma, decimal Valor, GPDV_PainelInicial TelaInicial)
        {
            try
            {
              
                //Modo antigo pelo acbr sat mas estava dando problema pois não estava retornando o idpagamento
                //var _chaveRequisicao = Guid.NewGuid().ToString();
                //var resposta = acbrIntegrador.EnviarPagamento(
                //    chaveRequisicao: _chaveRequisicao,
                //    estabelecimento: "10",
                //    serialPOS: caixa.SerialPOS,
                //    cnpj: Empresa.CNPJ,
                //    icmsBase: TelaInicial.TotalVenda,
                //    valorTotalVenda: Valor,
                //    origemPagamento: "POS",
                //    habilitarMultiplosPagamentos: Multiforma,
                //    habilitarControleAntiFraude: false,
                //    codigoMoeda: "BRL",
                //    emitirCupomNFCE: false
                //);
                //var respostaRetorno = resposta.GetXml();

                PagamentoProcessado = new PagamentoMFe();
                string chaveAcessoValidador = "25CFE38D-3B92-46C0-91CA-CFF751A82D3D"; // Essa chave no momento está sendo utilizada tanto para Homologação como Produção para todo mundo
                string cnpjLoja = Empresa.CNPJ;//"30146465000116"; //CNPJ do comercio
                string cnpjAdquirente = ""; //CNPJ do Adquirente, senão souber ainda qual o Adquirente não informe
                string estabelecimento = ""; //MarchandID do Estabelecimento, senão souber ainda qual não informe
                decimal baseICMS = TelaInicial.TotalVenda; //Base de ICMS do Total da Venda
                decimal valorTotalVenda = Valor; //Valor do Total da Venda
                string origemPagamento = "PDV" + caixa.NumeroCaixa ?? "SEM NU"; // Campo para informar alguma informação para busca posterior na Sefaz
                string serialPos = caixa.SerialPOS; // Número do Serial POS
                bool habilitarMultiplosPagamentos = Multiforma; //Se tem varios cartões de pagamento informar true


                RetornosMFE retornoMFE = mfe.EnviarPagamento(chaveAcessoValidador, cnpjLoja, cnpjAdquirente, estabelecimento, baseICMS, valorTotalVenda, origemPagamento, serialPos, habilitarMultiplosPagamentos);
                var result = "Codigo: " + retornoMFE.CodigoRetorno + Environment.NewLine +
                                                   "Descrição: " + retornoMFE.DescricaoRetorno + Environment.NewLine +
                                                   "Retorno VFP-E: IdPagamento " + (mfe.RetornoVFPE == null ? "" : mfe.RetornoVFPE.IdPagamento + " - idLocal: " + mfe.RetornoVFPE.IdLocal.ToString());
                if (retornoMFE.VFeRealizada())
                {

                    if (mfe.RetornoVFPE.IdLocal)
                    {
                        retornoPagamento.EnviouPagamento = false;
                        
                    }

                    else
                    {
                        int idPagamento = mfe.RetornoVFPE.IdPagamento;
                        bool idLocal = mfe.RetornoVFPE.IdLocal;
                        PagamentoMFe pagamentoMFe = new PagamentoMFe();
                        pagamentoMFe.IDVenda = TelaInicial.VENDA.IDVenda;
                        pagamentoMFe.DataPagamento = DateTime.Now;
                        pagamentoMFe.IDPagamento = idPagamento;
                        pagamentoMFe.IDLocal = idPagamento;
                        pagamentoMFe.ValorCartao = Valor.ToString("n2");
                        lstPagamentoProcessado.Add(PagamentoProcessado);
                        FCA_PagamentoMFe fCA_PagamentoMFe = new FCA_PagamentoMFe(pagamentoMFe);
                        //fCA_PagamentoMFe.TopMost = true;
                        fCA_PagamentoMFe.ShowDialog();

                        if (fCA_PagamentoMFe.Cancelado == true)
                        {
                            retornoPagamento.EnviouPagamento = false;
                            PagamentoProcessado = fCA_PagamentoMFe.Pagamento;
                           
                        }
                        else
                        {
                            retornoPagamento.EnviouPagamento = true;
                           
                        }
                    }
                }
                return retornoPagamento;
            }
            catch (Exception ex)
            {
                retornoPagamento.EnviouPagamento = false;
                return retornoPagamento;
                throw;
            }

        }
        public bool VerificarIntegrador()
        {
            var processoAPI = Process.GetProcessesByName("Integrador");
            if (processoAPI.Length == 0)
            {
                MessageBox.Show("Abra o integrador", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        GPDV_ProgressoMFe formProgresso = null;


        public void CancelarSAT(CFe cFe)
        {
            retornoMFe = new RetornoMFe();
            try
            {
                cfeCancAtual = new CFeCanc(cFe);

                retornoMFe = new RetornoMFe();
                retornoPagamento = new RetornoPagamento();
                if (!acbrSat.Ativo) ToogleInitialize();
                var ret = cfeCancAtual != null ? acbrSat.CancelarUltimaVenda(cfeCancAtual) : acbrSat.CancelarUltimaVenda(cfeAtual);
                if (ret.CodigoDeRetorno != 7000)
                {
                    retornoMFe.Enviado = false;
                    retornoMFe.Resposta = $"Erro ao cancelar:{ret.MensagemRetorno}.";

                }
                else
                {
                    MessageBox.Show("Cancelamento homologado com sucesso!");
                    cfeCancAtual = ret.Cancelamento;
                    retornoMFe.Enviado = true;
                    retornoMFe.Resposta = $"Erro ao cancelar:{ret.MensagemRetorno}.";
                    retornoMFe.CFeCanc = cfeCancAtual;
                }
            }
            catch (Exception ex)
            {
                retornoMFe.Enviado = false;
                retornoMFe.Resposta = $"Erro ao cancelar:{ex.Message}.";
                WSMonitoramento.AddLog($"Ocorreu um problema ao processar o MFe: {ex.Message}");
                MessageBox.Show($"Ocorreu um problema ao processar o MFe: {ex.Message}");
               
            }
           
        }
        public void ConsultarSAT()
        {
            retornoMFe = new RetornoMFe();
            try
            {
                var ret = acbrSat.ConsultarStatusOperacional();
                    retornoMFe.Resposta = $"Status:{ret.RetornoStr}.";
                   
                
            }
            catch (Exception ex)
            {
                retornoMFe.Enviado = false;
                retornoMFe.Resposta = $"Erro ao cancelar:{ex.Message}.";
                WSMonitoramento.AddLog($"Ocorreu um problema ao processar o MFe: {ex.Message}");
                MessageBox.Show($"Ocorreu um problema ao processar o MFe: {ex.Message}");

            }

        }

        public string TratarRetornoSAT(int pCodigo)
        {
            switch (pCodigo)
            {
                case 100:
                    return "CF-e-SAT processado com sucesso";
                    
                case 101:
                    return "CF-e-SAT de cancelamento processado com sucesso";
                    
                case 102:
                    return "CF-e-SAT processado - verificar inconsistencias";
                    
                case 103:
                    return "CF-e-SAT de cancelamento processado - verificar inconsistencias";
                    
                case 104:
                    return "N-o Existe Atualização do Software";
                    
                case 105:
                    return "Lote recebido com sucesso";
                    
                case 106:
                    return "Lote Processado";
                    
                case 107:
                    return "Lote em Processamento";
                    
                case 108:
                    return "Lote n-o localizado";
                    
                case 109:
                    return "Servi-o em Operacao";
                    
                case 110:
                    return "Status SAT recebido com sucesso";
                    
                case 112:
                    return "Assinatura do AC Registrada";
                    
                case 113:
                    return "Consulta cadastro com uma ocorrencia";
                    
                case 114:
                    return "Consulta cadastro com mais de uma ocorrencia";
                    
                case 115:
                    return "Solicita--o de dados efetuada com sucesso";
                    
                case 116:
                    return "Atualização do SB pendente";
                    
                case 117:
                    return "Solicitação de Arquivo de Parametriza--o efetuada com sucesso";
                    
                case 118:
                    return "Logs extra-dos com sucesso";
                    
                case 119:
                    return "Comandos da SEFAZ pendentes";
                    
                case 120:
                    return "N-o existem comandos da SEFAZ pendentes";
                    
                case 121:
                    return "Certificado Digital criado com sucesso";
                    
                case 122:
                    return "CRT recebido com sucesso";
                    
                case 123:
                    return "Adiar transmiss-o do lote";
                    
                case 124:
                    return "Adiar transmiss-o do CF-e";
                    
                case 125:
                    return "CF-e de teste de produ--o emitido com sucesso";
                    
                case 126:
                    return "CF-e de teste de ativa--o emitido com sucesso";
                    
                case 127:
                    return "Erro na emiss-o de CF-e de teste de produ--o";
                    
                case 128:
                    return "Erro na emiss-o de CF-e de teste de ativa--o";
                    
                case 129:
                    return "Solicita--es de emiss-o de certificados excedidas. (Somente ocorrer- no ambiente de testes)";
                    
                case 200:
                    return "Rejeição: Status do equipamento SAT difere do esperado";
                    
                case 201:
                    return "Rejeição: Falha na Verifica--o da Assinatura do N-mero de seguran-a";
                    
                case 202:
                    return "Rejeição: Falha no reconhecimento da autoria ou integridade do arquivo digital";
                    
                case 203:
                    return "Rejeição: Emissor n-o Autorizado para emiss-o da CF-e-SAT";
                    
                case 204:
                    return "Rejeição: Duplicidade de CF-e-SAT";
                    
                case 205:
                    return "Rejeição: Equipamento SAT encontra-se Ativo";
                    
                case 206:
                    return "Rejeição: Hora de Emiss-o do CF-e-SAT posterior - hora de recebimento.";
                    
                case 207:
                    return "Rejeição: CNPJ do emitente invalido.";
                    
                case 208:
                    return "Rejeição: Equipamento SAT encontra-se Desativado";
                    
                case 209:
                    return "Rejeição: IE do emitente Invalida";
                    
                case 210:
                    return "Rejeição: Intervalo de tempo entre o CF-e-SAT emitido e a emiss-o do respectivo CF-e-SAT de cancelamento - maior que 30 (trinta) minutos.";
                    
                case 211:
                    return "Rejeição: CNPJ n-o corresponde ao informado no processo de transfer-ncia.";
                    
                case 212:
                    return "Rejeição: Data de Emiss-o do CF-e-SAT posterior - data de recebimento.";
                    
                case 213:
                    return "Rejeição: CNPJ-Base do Emitente difere do CNPJ-Base do Certificado Digital";
                    
                case 214:
                    return "Rejeição: Tamanho da mensagem excedeu o limite estabelecido";
                    
                case 215:
                    return "Rejeição: Falha no schema XML";
                    
                case 216:
                    return "Rejeição: Chave de Acesso difere da cadastrada";
                    
                case 217:
                    return "Rejeição: CF-e-SAT n-o consta na base de dados da SEFAZ";
                    
                case 218:
                    return "Rejeição: CF-e-SAT j- esta cancelado na base de dados da SEFAZ";
                    
                case 219:
                    return "Rejeição: CNPJ n-o corresponde ao informado no processo de declara--o de posse.";
                    
                case 220:
                    return "Rejeição: Valor do rateio do desconto sobre subtotal do item (N) invalido.";
                    
                case 221:
                    return "Rejeição: Aplicativo Comercial n-o vinculado ao SAT";
                    
                case 222:
                    return "Rejeição: Assinatura do Aplicativo Comercial Invalida";
                    
                case 223:
                    return "Rejeição: CNPJ do transmissor do lote difere do CNPJ do transmissor da consulta";
                    
                case 224:
                    return "Rejeição: CNPJ da Software House invalido.";
                    
                case 225:
                    return "Rejeição: Falha no Schema XML do lote de CFe";
                    
                case 226:
                    return "Rejeição: C-digo da UF do Emitente diverge da UF receptora";
                    
                case 227:
                    return "Rejeição: Erro na Chave de Acesso - Campo Id - falta a literal CFe";
                    
                case 228:
                    return "Rejeição: Valor do rateio do acr-scimo sobre subtotal do item (N) invalido.";
                    
                case 229:
                    return "Rejeição: IE do emitente n-o informada";
                    
                case 230:
                    return "Rejeição: IE do emitente n-o autorizada para uso do SAT";
                    
                case 231:
                    return "Rejeição: IE do emitente n-o vinculada ao CNPJ";
                    
                case 232:
                    return "Rejeição: CNPJ do destinat-rio do CF-e-SAT de cancelamento diferente daquele do CF-e-SAT a ser cancelado.";
                    
                case 233:
                    return "Rejeição: CPF do destinat-rio do CF-e-SAT de cancelamento diferente daquele do CF-e-SAT a ser cancelado.";
                    
                case 234:
                    return "Alerta: Raz-o Social/Nome do destinat-rio em branco";
                    
                case 235:
                    return "Rejeição: CNPJ do destinatario Invalido";
                    
                case 236:
                    return "Rejeição: Chave de Acesso com d-gito verificador invalido.";
                    
                case 237:
                    return "Rejeição: CPF do destinatario Invalido";
                    
                case 238:
                    return "Rejeição: CNPJ do emitente do CF-e-SAT de cancelamento diferente do CNPJ do CF-e-SAT a ser cancelado.";
                    
                case 239:
                    return "Rejeição: Vers-o do arquivo XML n-o suportada";
                    
                case 240:
                    return "Rejeição: Valor total do CF-e-SAT de cancelamento diferente do Valor total do CF-e-SAT a ser cancelado.";
                    
                case 241:
                    return "Rejeição: diferen-a de transmiss-o e recebimento da mensagem superior a 5 minutos.";
                    
                case 242:
                    return "Alerta: CFe dentro do lote est-o fora de ordem.";
                    
                case 243:
                    return "Rejeição: XML Mal Formado";
                    
                case 244:
                    return "Rejeição: CNPJ do Certificado Digital difere do CNPJ da Matriz e do CNPJ do Emitente";
                    
                case 245:
                    return "Rejeição: CNPJ Emitente n-o autorizado para uso do SAT";
                    
                case 246:
                    return "Rejeição: Campo cUF inexistente no elemento cfeCabecMsg do SOAP Header";
                    
                case 247:
                    return "Rejeição: Sigla da UF do Emitente diverge da UF receptora";
                    
                case 248:
                    return "Rejeição: UF do Recibo diverge da UF autorizadora";
                    
                case 249:
                    return "Rejeição: UF da Chave de Acesso diverge da UF receptora";
                    
                case 250:
                    return "Rejeição: UF informada pelo SAT, n-o - atendida pelo Web Service";
                    
                case 251:
                    return "Rejeição: Certificado enviado n-o confere com o escolhido na declara--o de posse";
                    
                case 252:
                    return "Rejeição: Ambiente informado diverge do Ambiente de recebimento";
                    
                case 253:
                    return "Rejeição: Digito Verificador da chave de acesso composta Invalida";
                    
                case 254:
                    return "Rejeição: Elemento cfeCabecMsg inexistente no SOAP Header";
                    
                case 255:
                    return "Rejeição: CSR enviado invalido.";
                    
                case 256:
                    return "Rejeição: CRT enviado invalido.";
                    
                case 257:
                    return "Rejeição: N-mero do s-rie do equipamento invalido.";
                    
                case 258:
                    return "Rejeição: Data e/ou hora do envio Invalida";
                    
                case 259:
                    return "Rejeição: Vers-o do leiaute Invalida";
                    
                case 260:
                    return "Rejeição: UF inexistente";
                    
                case 261:
                    return "Rejeição: Assinatura digital n-o encontrada";
                    
                case 262:
                    return "Rejeição: CNPJ da software house n-o est- ativo";
                    
                case 263:
                    return "Rejeição: CNPJ do contribuinte n-o est- ativo";
                    
                case 264:
                    return "Rejeição: Base da receita federal est- indispon-vel";
                    
                case 265:
                    return "Rejeição: N-mero de s-rie inexistente no cadastro do equipamento";
                    
                case 266:
                    return "Falha na comunica--o com a AC-SAT";
                    
                case 267:
                    return "Erro desconhecido na gera--o do certificado pela AC-SAT";
                    
                case 268:
                    return "Rejeição: Certificado est- fora da data de validade.";
                    
                case 269:
                    return "Rejeição: Tipo de atividade Invalida";
                    
                case 270:
                    return "Rejeição: Chave de acesso do CFe a ser cancelado invalido.";
                    
                case 271:
                    return "Rejeição: Ambiente informado no CF-e difere do Ambiente de recebimento cadastrado.";
                    
                case 272:
                    return "Rejeição: Valor do troco negativo.";
                    
                case 273:
                    return "Rejeição: Servi-o Solicitado invalido.";
                    
                case 274:
                    return "Rejeição: Equipamento n-o possui declara--o de posse";
                    
                case 275:
                    return "Rejeição: Status do equipamento diferente de Fabricado";
                    
                case 276:
                    return "Rejeição: Diferen-a de dias entre a data de emiss-o e de recep--o maior que o prazo legal";
                    
                case 277:
                    return "Rejeição: CNPJ do emitente n-o est- ativo junto - Sefaz na data de emiss-o";
                    
                case 278:
                    return "Rejeição: IE do emitente n-o est- ativa junto - Sefaz na data de emiss-o";
                    
                case 280:
                    return "Rejeição: Certificado Transmissor invalido.";
                    
                case 281:
                    return "Rejeição: Certificado Transmissor Data Validade";
                    
                case 282:
                    return "Rejeição: Certificado Transmissor sem CNPJ";
                    
                case 283:
                    return "Rejeição: Certificado Transmissor - erro Cadeia de Certifica--o";
                    
                case 284:
                    return "Rejeição: Certificado Transmissor revogado";
                    
                case 285:
                    return "Rejeição: Certificado Transmissor difere ICP-Brasil";
                    
                case 286:
                    return "Rejeição: Certificado Transmissor erro no acesso a LCR";
                    
                case 287:
                    return "Rejeição: C-digo Munic-pio do FG - ISSQN: d-gito invalido. Exceto os c-digos descritos no Anexo 2 que apresentam d-gito invalido.";
                    
                case 288:
                    return "Rejeição: Data de emiss-o do CF-e-SAT a ser cancelado Invalida";
                    
                case 289:
                    return "Rejeição: C-digo da UF informada diverge da UF solicitada";
                    
                case 290:
                    return "Rejeição: Certificado Assinatura invalido.";
                    
                case 291:
                    return "Rejeição: Certificado Assinatura Data Validade";
                    
                case 292:
                    return "Rejeição: Certificado Assinatura sem CNPJ";
                    
                case 293:
                    return "Rejeição: Certificado Assinatura - erro Cadeia de Certifica--o";
                    
                case 294:
                    return "Rejeição: Certificado Assinatura revogado";
                    
                case 295:
                    return "Rejeição: Certificado Raiz difere dos V-lidos";
                    
                case 296:
                    return "Rejeição: Certificado Assinatura erro no acesso a LCR";
                    
                case 297:
                    return "Rejeição: Assinatura difere do calculado";
                    
                case 298:
                    return "Rejeição: Assinatura difere do padr-o do Projeto";
                    
                case 299:
                    return "Rejeição: Hora de emiss-o do CF-e-SAT a ser cancelado Invalida";
                    
                case 402:
                    return "Rejeição: XML da -rea de dados com codifica--o diferente de UTF-8";
                    
                case 403:
                    return "Rejeição: Vers-o do leiaute do CF-e-SAT n-o - v-lida";
                    
                case 404:
                    return "Rejeição: Uso de prefixo de namespace n-o permitido";
                    
                case 405:
                    return "Alerta: Vers-o do leiaute do CF-e-SAT n-o - a mais atual";
                    
                case 406:
                    return "Rejeição: Vers-o do Software B-sico do SAT n-o - valida.";
                    
                case 407:
                    return "Rejeição: Indicador de CF-e-SAT cancelamento invalido. (diferente de -C? e -?)";
                    
                case 408:
                    return "Rejeição: Valor total do CF-e-SAT maior que o somatorio dos valores de Meio de Pagamento empregados em seu pagamento.";
                    
                case 409:
                    return "Rejeição: Valor total do CF-e-SAT supera o m-ximo permitido no arquivo de Parametriza--o de Uso";
                    
                case 410:
                    return "Rejeição: UF informada no campo cUF n-o - atendida pelo Web Service";
                    
                case 411:
                    return "Rejeição: Campo versaoDados inexistente no elemento cfeCabecMsg do SOAP Header";
                    
                case 412:
                    return "Rejeição: CFe de cancelamento n-o corresponde ao CFe anteriormente gerado";
                    
                case 420:
                    return "Rejeição: Cancelamento para CF-e-SAT j- cancelado";
                    
                case 450:
                    return "Rejeição: Modelo da CF-e-SAT diferente de 59";
                    
                case 452:
                    return "Rejeição: n-mero de s-rie do SAT invalido. ou n-o autorizado.";
                    
                case 453:
                    return "Rejeição: Ambiente de processamento invalido. (diferente de 1 e 2)";
                    
                case 454:
                    return "Rejeição: CNPJ da Software House invalido.";
                    
                case 455:
                    return "Rejeição: Assinatura do Aplicativo Comercial n-o - v-lida.";
                    
                case 456:
                    return "Rejeição: C-digo de Regime tribut-rio invalido";
                    
                case 457:
                    return "Rejeição: C-digo de Natureza da Operacao para ISSQN invalido.";
                    
                case 458:
                    return "Rejeição: Raz-o Social/Nome do destinat-rio em branco";
                    
                case 459:
                    return "Rejeição: C-digo do produto ou servi-o em branco";
                    
                case 460:
                    return "Rejeição: GTIN do item (N) invalido.";
                    
                case 461:
                    return "Rejeição: Descri--o do produto ou servi-o em branco";
                    
                case 462:
                    return "Rejeição: CFOP n-o - de Operacao de sa-da prevista para CF-e-SAT";
                    
                case 463:
                    return "Rejeição: Unidade comercial do produto ou servi-o em branco";
                    
                case 464:
                    return "Rejeição: Quantidade Comercial do item (N) invalido.";
                    
                case 465:
                    return "Rejeição: Valor unit-rio do item (N) invalido.";
                    
                case 466:
                    return "Rejeição: Valor bruto do item (N) difere de quantidade * Valor Unit-rio, considerando regra de arred/trunc.";
                    
                case 467:
                    return "Rejeição: Regra de calculo do item (N) Invalida";
                    
                case 468:
                    return "Rejeição: Valor do desconto do item (N) invalido.";
                    
                case 469:
                    return "Rejeição: Valor de outras despesas acess-rias do item (N) invalido.";
                    
                case 470:
                    return "Rejeição: Valor l-quido do Item do CF-e difere de Valor Bruto de Produtos e serviços - desconto + Outras Despesas Acess-rias - rateio do desconto sobre subtotal + rateio do acr-scimo sobre subtotal ";
                    
                case 471:
                    return "Rejeição: origem da mercadoria do item (N) invalido. (difere de 0, 1, 2, 3, 4, 5, 6 e 7)";
                    
                case 472:
                    return "Rejeição: CST do Item (N) invalido. (diferente de 00, 20, 90)";
                    
                case 473:
                    return "Rejeição: Aliquota efetiva do ICMS do item (N) invalido.";
                    
                case 474:
                    return "Rejeição: Valor l-quido do ICMS do Item (N) difere de Valor do Item * Aliquota Efetiva";
                    
                case 475:
                    return "Rejeição: CST do Item (N) invalido. (diferente de 40 e 41 e 50 e 60)";
                    
                case 476:
                    return "Rejeição: C-digo de situa--o da Operacao - Simples Nacional - do Item (N) invalido. (diferente de 102, 300 e 500)";
                    
                case 477:
                    return "Rejeição: C-digo de situa--o da Operacao - Simples Nacional - do Item (N) invalido. (diferente de 900)";
                    
                case 478:
                    return "Rejeição: C-digo de Situa--o Tribut-ria do PIS invalido. (diferente de 01 e 02)";
                    
                case 479:
                    return "Rejeição: Base de c-lculo do PIS do item (N) invalido.";
                    
                case 480:
                    return "Rejeição: Aliquota do PIS do item (N) invalido.";
                    
                case 481:
                    return "Rejeição: Valor do PIS do Item (N) difere de Base de Calculo * Aliquota do PIS";
                    
                case 482:
                    return "Rejeição: C-digo de Situa--o Tribut-ria do PIS invalido. (diferente de 03)";
                    
                case 483:
                    return "Rejeição: Qtde Vendida do item (N) invalido.";
                    
                case 484:
                    return "Rejeição: Aliquota do PIS em R$ do item (N) invalido.";
                    
                case 485:
                    return "Rejeição: Valor do PIS do Item (N) difere de Qtde Vendida* Aliquota do PIS em R$";
                    
                case 486:
                    return "Rejeição: C-digo de Situa--o Tribut-ria do PIS invalido. (diferente de 04, 06, 07, 08 e 09)";
                    
                case 487:
                    return "Rejeição: C-digo de Situa--o Tribut-ria do PIS invalido. (diferente de 49)";
                    
                case 488:
                    return "Rejeição: C-digo de Situa--o Tribut-ria do PIS invalido. (diferente de 99)";
                    
                case 489:
                    return "Rejeição: Valor do PIS do Item (N) difere de Qtde Vendida* Aliquota do PIS em R$ e difere de Base de Calculo * Aliquota do PIS";
                    
                case 490:
                    return "Rejeição: C-digo de Situa--o Tribut-ria da COFINS invalido. (diferente de 01 e 02)";
                    
                case 491:
                    return "Rejeição: Base de c-lculo do COFINS do item (N) invalido.";
                    
                case 492:
                    return "Rejeição: Aliquota da COFINS do item (N) invalido.";
                    
                case 493:
                    return "Rejeição: Valor da COFINS do Item (N) difere de Base de Calculo * Aliquota da COFINS";
                    
                case 494:
                    return "Rejeição: C-digo de Situa--o Tribut-ria da COFINS invalido. (diferente de 03)";
                    
                case 495:
                    return "Rejeição: Valor do COFINS do Item (N) difere de Qtde Vendida* Aliquota do COFINS em R$ e difere de Base de Calculo * Aliquota do COFINS";
                    
                case 496:
                    return "Rejeição: Aliquota da COFINS em R$ do item (N) invalido.";
                    
                case 497:
                    return "Rejeição: Valor da COFINS do Item (N) difere de Qtde Vendida* Aliquota da COFINS em R$";
                    
                case 498:
                    return "Rejeição: C-digo de Situa--o Tribut-ria da COFINS invalido. (diferente de 04, 06, 07, 08 e 09)";
                    
                case 499:
                    return "Rejeição: C-digo de Situa--o Tribut-ria da COFINS invalido. (diferente de 49)";
                    
                case 500:
                    return "Rejeição: C-digo de Situa--o Tribut-ria da COFINS invalido. (diferente de 99)";
                    
                case 501:
                    return "Rejeição: Operacao com tributa--o de ISSQN sem informar a Inscri--o Municipal";
                    
                case 502:
                    return "Rejeição: Erro na Chave de Acesso - Campo Id n-o corresponde - concatena--o dos campos correspondentes";
                    
                case 503:
                    return "Rejeição: Valor das dedu--es para o ISSQN do item (N) invalido.";
                    
                case 504:
                    return "Rejeição: Valor da Base de Calculo do ISSQN do Item (N) difere de Valor do Item - Valor das dedu--es";
                    
                case 505:
                    return "Rejeição: Aliquota efetiva do ISSQN do item (N) n-o - maior ou igual a 2,00 (2%) e menor ou igual a 5,00 (5%).";
                    
                case 506:
                    return "Valor do ISSQN do Item (N) difere de Valor da Base de Calculo do ISSQN * Aliquota Efetiva do ISSQN";
                    
                case 507:
                    return "Rejeição: Indicador de rateio para ISSQN invalido.";
                    
                case 508:
                    return "Rejeição: Item da lista de serviços do ISSQN do item (N) invalido.";
                    
                case 509:
                    return "Rejeição: C-digo municipal de Tributa--o do ISSQN do Item (N) em branco.";
                    
                case 510:
                    return "Rejeição: C-digo de Natureza da Operacao para ISSQN invalido.";
                    
                case 511:
                    return "Rejeição: Indicador de Incentivo Fiscal do ISSQN do item (N) invalido. (diferente de 1 e 2)";
                    
                case 512:
                    return "Rejeição: Total do PIS difere do somatorio do PIS dos itens";
                    
                case 513:
                    return "Rejeição: Total do COFINS difere do somatorio do COFINS dos itens";
                    
                case 514:
                    return "Rejeição: Total do PIS-ST difere do somatorio do PIS-ST dos itens";
                    
                case 515:
                    return "Rejeição: Total do COFINS-STdifere do somatorio do COFINS-ST dos itens";
                    
                case 516:
                    return "Rejeição: Total de Outras Despesas Acess-rias difere do somatorio de Outras Despesas Acess-rias (acr-scimo) dos itens";
                    
                case 517:
                    return "Rejeição: Total dos Itens difere do somatorio do valor l-quido dos itens";
                    
                case 518:
                    return "Rejeição: Informado grupo de totais do ISSQN sem informar grupo de valores de ISSQN";
                    
                case 519:
                    return "Rejeição: Total da BC do ISSQN difere do somatorio da BC do ISSQN dos itens";
                    
                case 520:
                    return "Rejeição: Total do ISSQN difere do somatorio do ISSQN dos itens";
                    
                case 521:
                    return "Rejeição: Total do PIS sobre serviços difere do somatorio do PIS dos itens de serviços";
                    
                case 522:
                    return "Rejeição: Total do COFINS sobre serviços difere do somatorio do COFINS dos itens de serviços";
                    
                case 523:
                    return "Rejeição: Total do PIS-ST sobre serviços difere do somatorio do PIS-ST dos itens de serviços";
                    
                case 524:
                    return "Rejeição: Total do COFINS-ST sobre serviços difere do somatorio do COFINS-ST dos itens de serviços";
                    
                case 525:
                    return "Rejeição: Valor de Desconto sobre total invalido.";
                    
                case 526:
                    return "Rejeição: Valor de Acr-scimo sobre total invalido.";
                    
                case 527:
                    return "Rejeição: C-digo do Meio de Pagamento invalido.";
                    
                case 528:
                    return "Rejeição: Valor do Meio de Pagamento invalido.";
                    
                case 529:
                    return "Rejeição: Valor de desconto sobre subtotal difere do somatorio dos seus rateios nos itens.";
                    
                case 530:
                    return "Rejeição: Operacao com tributa--o de ISSQN sem informar a Inscri--o Municipal";
                    
                case 531:
                    return "Rejeição: Valor de acr-scimo sobre subtotal difere do somatorio dos seus rateios nos itens.";
                    
                case 532:
                    return "Rejeição: Total do ICMS difere do somatorio dos itens";
                    
                case 533:
                    return "Rejeição: Valor aproximado dos tributos do CF-e-SAT - Lei 12741/12 invalido.";
                    
                case 534:
                    return "Rejeição: Valor aproximado dos tributos do Produto ou servi-o - Lei 12741/12 invalido.";
                    
                case 535:
                    return "Rejeição: c-digo da credenciadora de cart-o de d-bito ou cr-dito invalido.";
                    
                case 537:
                    return "Rejeição: Total do Desconto difere do somatorio dos itens";
                    
                case 539:
                    return "Rejeição: Duplicidade de CF-e-SAT, com diferen-a na Chave de Acesso [99999999999999999999999999999999999999999]";
                    
                case 540:
                    return "Rejeição: CNPJ da Software House + CNPJ do emitente assinado no campo -signAC- difere do informado no campo -CNPJvalue- ";
                    
                case 555:
                    return "Rejeição: Tipo autorizador do protocolo diverge do -rg-o Autorizador";
                    
                case 564:
                    return "Rejeição: Total dos Produtos ou serviços difere do somatorio do valor dos Produtos ou serviços dos itens";
                    
                case 600:
                    return "Servi-o Temporariamente Indispon-vel";
                    
                case 601:
                    return "CF-e-SAT inid-neo por recep--o fora do prazo";
                    
                case 602:
                    return "Rejeição: Status do equipamento n-o permite ativa--o";
                    
                case 603:
                    return "Arquivo invalido.";
                    
                case 604:
                    return "Erro desconhecido na verifica--o de comandos";
                    
                case 605:
                    return "Tamanho do arquivo invalido.";
                    
                case 999:
                    return "Rejeição: Erro n-o catalogado";

                
                    

                default:
                    return "Rejeição catalogada na nota t-cnica 2013/001.";
                    
            }
        }
    }

    
}
