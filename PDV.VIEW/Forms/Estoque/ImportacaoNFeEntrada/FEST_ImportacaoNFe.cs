using DFe.Classes.Entidades;
using DFe.Utils;
using MetroFramework;
using MetroFramework.Forms;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Custom;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.Estoque.NFeImportacao;
using PDV.DAO.Entidades.Estoque.PedidoDeCompra;
using PDV.DAO.Entidades.Estoque.Suprimentos;
using PDV.DAO.Entidades.Financeiro;
using PDV.DAO.Entidades.NFe;
using PDV.DAO.Entidades.PDV;
using PDV.UTIL;
using PDV.VIEW.App_Context;
using PDV.VIEW.Forms.Cadastro;
using PDV.VIEW.Forms.Estoque.ImportacaoNFeEntrada.UControl;
using PDV.VIEW.Forms.Gerenciamento.DAC;
using PDV.VIEW.Forms.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDV.VIEW.Forms.Estoque.ImportacaoNFeEntrada
{
    public partial class metroButton5 : DevExpress.XtraEditors.XtraForm
    {
        private NFeEntrada NFeEntrada = null;
        private List<ItemNFeEntrada> Itens = null;
        private PedidoCompra PedidoCompra = null;
        private decimal IDNFeEntrada;

        private Fornecedor Fornecedor = null;
        private Transportadora Transportadora = null;

        private decimal somaTotalParcelas = 0;

        private string NOME_TELA = "IMPORTAÇÃO DA NF-E";


        NFe.Classes.NFe ArquivoNFe = null;
        public metroButton5(NFe.Classes.NFe _ArquivoNFe, decimal IDNFeEntrada = -1)
        {
            InitializeComponent();
            ArquivoNFe = _ArquivoNFe;
            this.IDNFeEntrada = IDNFeEntrada;
            FORMASPAGAMENTO = FuncoesFormaDePagamento.GetFormasPagamento();
            ovCMB_FormaPagamento.DataSource = FORMASPAGAMENTO;
            ovCMB_FormaPagamento.ValueMember = "idformadepagamento";
            ovCMB_FormaPagamento.DisplayMember = "identificacaodescricaoformabandeira";
            ovCMB_FormaPagamento.SelectedItem = null;


            List<TipoDeOperacao> listTipoDeOperacao = FuncoesTipoDeOperacao.GetTiposDeOperacaoPorTipoDeMovimento(TipoDeOperacao.Entrada);
            metroComboBoxTipoDeOperacao.DataSource = listTipoDeOperacao;
            metroComboBoxTipoDeOperacao.ValueMember = "idtipodeoperacao";
            metroComboBoxTipoDeOperacao.DisplayMember = "nome";
            metroComboBoxTipoDeOperacao.SelectedItem = null;

            PreencherObjetoNFe();
            PreencherTela();

            labelFaltaValor.Text = ovTXT_TotalNFe.Text;
            labelTrocoValor.Text = "R$ 0,00";

            ovTL_Produtos.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            metroTabControl2.SelectedTab = metroTabPage5;
        }

        private void PreencherObjetoNFe()
        {
            try
            {
                Fornecedor = FuncoesFornecedor.GetFornecedorPorCNPJ(ArquivoNFe.infNFe.emit.CNPJ);
                if (Fornecedor == null)
                {
                    decimal idUF = FuncoesUF.GetUnidadeFederativa(ArquivoNFe.infNFe.emit.enderEmit.UF.ToString()).IDUnidadeFederativa;
                    Endereco endereco = new Endereco()
                    {
                        IDEndereco = Sequence.GetNextID("ENDERECO", "IDENDERECO"),
                        Cep = ArquivoNFe.infNFe.emit.enderEmit.CEP,
                        UnidadeFederativa = ArquivoNFe.infNFe.emit.enderEmit.UF.ToString(),
                        Bairro = ArquivoNFe.infNFe.emit.enderEmit.xBairro,
                        Logradouro = ArquivoNFe.infNFe.emit.enderEmit.xLgr,
                        Numero = Convert.ToDecimal(ArquivoNFe.infNFe.emit.enderEmit.nro),
                        Complemento = ArquivoNFe.infNFe.emit.enderEmit.xCpl,
                        Telefone = ArquivoNFe.infNFe.emit.enderEmit.fone.ToString(),
                        IDUnidadeFederativa = idUF
                    };

                    Fornecedor = new Fornecedor()
                    {
                        IDFornecedor = Sequence.GetNextID("FORNECEDOR", "IDFORNECEDOR"),
                        IDEndereco = endereco.IDEndereco,
                        Descricao = ArquivoNFe.infNFe.emit.xFant,
                        RazaoSocial = ArquivoNFe.infNFe.emit.xFant,
                        CNPJ = ArquivoNFe.infNFe.emit.CNPJ,
                        InscricaoEstadual = Convert.ToDecimal(ArquivoNFe.infNFe.emit.IE)
                    };
                    FuncoesEndereco.Salvar(endereco, DAO.Enum.TipoOperacao.INSERT);
                    FuncoesFornecedor.Salvar(Fornecedor, DAO.Enum.TipoOperacao.INSERT);
                }

                if (ArquivoNFe.infNFe.transp.transporta != null)
                    Transportadora = FuncoesTransportadora.GetTransportadoraPorCNPJ(ArquivoNFe.infNFe.transp.transporta.CNPJ);

                /* Regra: Para importar a NFe, precisa antes cadastrar o Fornecedor. */


                if (this.IDNFeEntrada > 0)
                {
                    NFeEntrada = FuncoesNFeEntrada.GetNFeEntradaPorId(this.IDNFeEntrada);
                }
                else
                {
                    NFeEntrada = new NFeEntrada
                    {

                        ESP = string.Empty,
                        MARCA = string.Empty,
                        VERPROC = null,
                        XMOTIVO = null,

                        IDNFeEntrada = IDNFeEntrada > 0 ? IDNFeEntrada : Sequence.GetNextID("NFEENTRADA", "IDNFEENTRADA"),
                        IDFornecedor = Fornecedor == null ? -1 : Fornecedor.IDFornecedor,
                        IDTransportadora = Transportadora == null ? null : (decimal?)Transportadora.IDTransportadora,
                        IDUsuario = Contexto.USUARIOLOGADO.IDUsuario,
                        DataImportacao = DateTime.Now,
                        AUTCNPJ = ArquivoNFe.infNFe.emit.CNPJ,
                        AUTCPF = ArquivoNFe.infNFe.emit.CPF == null ? "" : "",
                        CDV = ArquivoNFe.infNFe.ide.cDV,
                        CHNFE = ArquivoNFe.infNFe.Id.Replace("NFe", string.Empty),
                        CMUNFG = ArquivoNFe.infNFe.emit.enderEmit.cMun.ToString(),
                        CNF = Convert.ToInt32(ArquivoNFe.infNFe.ide.cNF),
                        CSTAT = 100,
                        CUF = FuncoesMunicipio.ConverterUF(ArquivoNFe.infNFe.emit.enderEmit.UF.ToString()).ToString(),//ArquivoNFe.infNFe.emit.enderEmit.UF,
                        UF = ArquivoNFe.infNFe.emit.enderEmit.UF.ToString(),
                        DHCONT = ArquivoNFe.infNFe.ide.dhCont.Year < 2000 ? null : (DateTime?)ArquivoNFe.infNFe.ide.dhCont.UtcDateTime,
                        DHEMI = ArquivoNFe.infNFe.ide.dhEmi.UtcDateTime,
                        DHSAIENT = ArquivoNFe.infNFe.ide.dhSaiEnt != null ? ArquivoNFe.infNFe.ide.dhSaiEnt.Value.UtcDateTime : new DateTime(0001, 01, 01),
                        FINNFE = (int)ArquivoNFe.infNFe.ide.finNFe,
                        IDDEST = ArquivoNFe.infNFe.ide.idDest.HasValue ? (int?)ArquivoNFe.infNFe.ide.idDest : null,
                        INDFINAL = ArquivoNFe.infNFe.ide.indFinal.HasValue ? (int?)ArquivoNFe.infNFe.ide.indFinal : null,
                        //INDPAG = ArquivoNFe.infNFe.ide.indPag == null ? (int)ArquivoNFe.infNFe.ide.indPag : ,
                        INDPRES = ArquivoNFe.infNFe.ide.indPres.HasValue ? (int?)ArquivoNFe.infNFe.ide.indPres : null,
                        INFADFISCO = ArquivoNFe.infNFe.infAdic == null ? null : ArquivoNFe.infNFe.infAdic.infAdFisco,
                        INFCPL = ArquivoNFe.infNFe.infAdic == null ? null : ArquivoNFe.infNFe.infAdic.infCpl,
                        MODDOC = (int)ArquivoNFe.infNFe.ide.mod,
                        MODFRETE = (int)ArquivoNFe.infNFe.transp.modFrete,
                        NATOPE = ArquivoNFe.infNFe.ide.natOp,
                        NNF = Convert.ToInt32(ArquivoNFe.infNFe.ide.nNF),
                        NVOL = ArquivoNFe.infNFe.transp != null ? (ArquivoNFe.infNFe.transp.vol?.FirstOrDefault()?.nVol) : null,
                        PESOB = ArquivoNFe.infNFe.transp != null ? (ArquivoNFe.infNFe.transp.vol?.FirstOrDefault()?.pesoB) : null,
                        PESOL = ArquivoNFe.infNFe.transp != null ? (ArquivoNFe.infNFe.transp.vol?.FirstOrDefault()?.pesoL) : null,
                        PLACA = ArquivoNFe.infNFe.transp.veicTransp != null ? ArquivoNFe.infNFe.transp.veicTransp.placa : null,
                        QVOL = ArquivoNFe.infNFe.transp != null ? (ArquivoNFe.infNFe.transp.vol?.FirstOrDefault()?.qVol) : null,
                        PROCEMI = (int)ArquivoNFe.infNFe.ide.procEmi,
                        SERIE = ArquivoNFe.infNFe.ide.serie,
                        TPAMB = (int)ArquivoNFe.infNFe.ide.tpAmb,
                        TPEMIS = (int)ArquivoNFe.infNFe.ide.tpEmis,
                        TPIMP = (int)ArquivoNFe.infNFe.ide.tpImp,
                        TPNF = (int)ArquivoNFe.infNFe.ide.tpNF,
                        VBC = ArquivoNFe.infNFe.total.ICMSTot.vBC,
                        VBCST = ArquivoNFe.infNFe.total.ICMSTot.vBCST,
                        VCOFINS = ArquivoNFe.infNFe.total.ICMSTot.vCOFINS,
                        VDESC = ArquivoNFe.infNFe.total.ICMSTot.vDesc,
                        VFCPUFDEST = ArquivoNFe.infNFe.total.ICMSTot.vFCPUFDest,
                        VFRETE = ArquivoNFe.infNFe.total.ICMSTot.vFrete,
                        VICMS = ArquivoNFe.infNFe.total.ICMSTot.vICMS,
                        VICMSUFDEST = ArquivoNFe.infNFe.total.ICMSTot.vICMSUFDest,
                        VICMSUFREMET = ArquivoNFe.infNFe.total.ICMSTot.vICMSUFRemet,
                        VIPI = ArquivoNFe.infNFe.total.ICMSTot.vIPI,
                        VNF = ArquivoNFe.infNFe.total.ICMSTot.vNF,
                        VOUTRO = ArquivoNFe.infNFe.total.ICMSTot.vOutro,
                        VPIS = ArquivoNFe.infNFe.total.ICMSTot.vPIS,
                        VPROD = ArquivoNFe.infNFe.total.ICMSTot.vProd,
                        VSEG = ArquivoNFe.infNFe.total.ICMSTot.vSeg,
                        VST = ArquivoNFe.infNFe.total.ICMSTot.vST,
                        VTOTTRIB = ArquivoNFe.infNFe.total.ICMSTot.vTotTrib,
                        XJUST = ArquivoNFe.infNFe.ide.xJust,

                        XCAMPO = ArquivoNFe.infNFe.infAdic?.obsFisco?.FirstOrDefault()?.xCampo,
                        XTEXTO = ArquivoNFe.infNFe.infAdic?.obsFisco?.FirstOrDefault()?.xTexto,
                        NPROT = ArquivoNFe.infNFe.infAdic?.procRef?.FirstOrDefault()?.nProc,

                    };

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void PreencherTela()
        {
            /* Cabeçalho da NFe */
            ovTXT_CodNatOp.Text = ArquivoNFe.infNFe.ide.natOp;
            ovTXT_Chave.Text = ArquivoNFe.infNFe.Id != null ? ArquivoNFe.infNFe.Id.Replace("NFe", "") : "";
            ovTXT_Tipo.Text = "IMPORTAÇÃO";
            ovTXT_Finalidade.Text = ArquivoNFe.infNFe.ide.finNFe.ToString();
            ovTXT_TipoAtendimento.Text = ArquivoNFe.infNFe.ide.indPres.ToString();
            ovTXT_Numero.Text = ArquivoNFe.infNFe.ide.nNF.ToString();
            ovTXT_Emissao.Text = ArquivoNFe.infNFe.ide.dhEmi.ToString();
            ovTXT_Serie.Text = ArquivoNFe.infNFe.ide.serie.ToString();

            /* Fornecedor - Emitente da NFe */
            ovTXT_Fornecedor.Text = ArquivoNFe.infNFe.emit.xFant;
            ovTXT_FornecedorCNPJCPF.Text = ArquivoNFe.infNFe.emit.CNPJ;
            ovTXT_FornecedorCep.Text = ArquivoNFe.infNFe.emit.enderEmit.CEP;
            ovTXT_FornecedorUF.Text = ArquivoNFe.infNFe.emit.enderEmit.UF.ToString();
            ovTXT_FornecedorBairro.Text = ArquivoNFe.infNFe.emit.enderEmit.xBairro;
            ovTXT_FornecedorInscEstadual.Text = ArquivoNFe.infNFe.emit.IE;
            ovTXT_FornecedorLogradouro.Text = ArquivoNFe.infNFe.emit.enderEmit.xLgr;
            ovTXT_FornecedorNumero.Text = ArquivoNFe.infNFe.emit.enderEmit.nro;
            ovTXT_ClienteComplemento.Text = ArquivoNFe.infNFe.emit.enderEmit.xCpl;
            ovTXT_FornecedorCidade.Text = ArquivoNFe.infNFe.emit.enderEmit.xMun;
            ovTXT_FornecedorFone.Text = ArquivoNFe.infNFe.emit.enderEmit.fone.ToString();

            /* Transportadora */
            if (ArquivoNFe.infNFe.transp.transporta != null)
            {
                ovTXT_Transportadora.Text = ArquivoNFe.infNFe.transp.transporta != null ? ArquivoNFe.infNFe.transp.transporta.xNome : "<Não Informado>";
                ovTXT_TransportadoraCNPJCPF.Text = ArquivoNFe.infNFe.transp.transporta != null ? ArquivoNFe.infNFe.transp.transporta.CNPJ : "<Não Informado>";

                ovTXT_TransportadoraPlaca.Text = ArquivoNFe.infNFe.transp.veicTransp != null ? ArquivoNFe.infNFe.transp.veicTransp.placa : string.Empty;
                ovTXT_UFTransportadora.Text = ArquivoNFe.infNFe.transp.veicTransp != null ? ArquivoNFe.infNFe.transp.veicTransp.UF : string.Empty;

            }
            else
            {


                ovTXT_Transportadora.Text = "<Não Informado>";
                ovTXT_TransportadoraCNPJCPF.Text = "<Não Informado>";

                ovTXT_TransportadoraPlaca.Text = string.Empty;
                ovTXT_UFTransportadora.Text = string.Empty;


                ovCKB_FreteSemFrete.Checked = true;
            }

            /* Frete Por */
            switch (ArquivoNFe.infNFe.transp.modFrete)
            {
                case NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfSemFrete:
                    ovCKB_FreteSemFrete.Checked = true;
                    break;
                case NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfContaTerceiros:
                    ovCKB_FreteTerceiros.Checked = true;
                    break;
                case NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfProprioContaRemente:
                    ovCKB_FreteEmitente.Checked = true;
                    break;
                case NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfContaDestinatario:
                    ovCKB_FreteDestinatario.Checked = true;
                    break;
            }

            /* CondicaoPagamento */
            switch (ArquivoNFe.infNFe.ide.indPag)
            {
                case NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento.ipOutras:
                    ovCKB_Outros.Checked = true;
                    break;
                case NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento.ipPrazo:
                    ovCKB_Aprazo.Checked = true;
                    break;
                case NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento.ipVista:
                    ovCKB_Avista.Checked = true;
                    break;
            }

            PreencherProdutos();
            PreencherTotaisFinanceiro();
            CarregarFinanceiro(true);
        }

        private void PreencherProdutos()
        {
            /*
				1. Consulta por CProd na tabela de ProdutoFornecedor.
				2. Se Não existir, Consulta pelo EAN.
				3. Se Não existir, Listar todos os produtos pelo NCM                 
			 */
            List<ItemNFeEntrada> Itens;

            if (this.IDNFeEntrada > 0)
            {
                NFeEntrada = FuncoesNFeEntrada.GetNFeEntradaPorId(this.IDNFeEntrada);
                Itens = FuncoesNFeEntrada.GetItemNFeEntradaPorId(this.IDNFeEntrada);


                foreach (var item in Itens)
                {
                    Produto Prod = FuncoesProduto.GetProduto(item.IDProduto);
                    var itemNfeEntrada = FuncoesItemNFeEntrada.GetItemNfeEntradaPorProdutoID(item.IDProduto);
                    item.DescricaoProduto = Prod.Descricao;
                    /*
                      CENQ = d.imposto.IPI != null ? d.imposto.IPI.cEnq.ToString() : null,
                        CEST = d.prod.CEST,
                        CFOP = d.prod.CFOP.ToString(),
                        DescricaoProduto = d.prod.xProd,
                        CEAN = string.IsNullOrEmpty(d.prod.cEAN) ? null : (decimal?)Convert.ToDecimal(d.prod.cEAN),
                        INDTOT = (int)d.prod.indTot,
                        NCM = d.prod.NCM,
                        QTRIB = d.prod.qTrib,
                        QCOM = d.prod.qCom,
                        UCOM = d.prod.uCom,
                        VOUTRO = d.prod.vOutro ?? 0,
                        UTRIB = d.prod.uTrib,
                        VPROD = d.prod.vProd,
                        VUNTRIB = d.prod.vUnTrib,
                        VUNCOM = d.prod.vUnCom,
                        XPROD = d.prod.xProd,
                        VFRETE = d.prod.vFrete ?? 0,
                        VDESC = d.prod.vDesc ?? 0,
                        VSEG = d.prod.vSeg ?? 0,
                        QuantidadeEntrada = d.prod.qCom,
                        UNEntrada = d.prod.uCom,
                        UNSaida = d.prod.uCom,
                        QuantidadeSaida = d.prod.qCom,
                        Valor = d.prod.vUnCom,
                     */
                    item.CEAN = string.IsNullOrEmpty(Prod.EAN) ? null : (decimal?)Convert.ToDecimal(Prod.EAN);
                    item.Valor = item.vuncom;
                    item.QuantidadeEntrada = item.QCOM;
                    item.QuantidadeSaida = item.QCOM;
                    item.UNEntrada = item.UCOM;
                    item.UNSaida = item.UCOM;


                    UC_ItemNFeEntrada UCControl = new UC_ItemNFeEntrada(item);

                    UCControl.Margin = new Padding(5);
                    ovTL_Produtos.Controls.Add(UCControl);


                }
                
            }
            else
            {

                Itens = new List<ItemNFeEntrada>();


                foreach (det d in ArquivoNFe.infNFe.det)
                {
                    Produto Prod = FuncoesProduto.GetProdutoPorCodigoEFornecedor(d.prod.cProd, Fornecedor == null ? -1 : Fornecedor.IDFornecedor);
                    if (Prod == null)
                    {
                        string Ean = d.prod.cEAN;
                        if (string.IsNullOrEmpty(Ean))
                            Ean = d.prod.cEANTrib;

                        if (!string.IsNullOrEmpty(Ean))
                            Prod = FuncoesProduto.GetProdutoPorEAN(Ean);
                    }

                    ItemNFeEntrada _Prod = new ItemNFeEntrada
                    {
                        IDItemNFeEntrada = Sequence.GetNextID("ITEMNFEENTRADA", "IDITEMNFEENTRADA"),
                        IDNFeEntrada = NFeEntrada.IDNFeEntrada,
                        IDProduto = Prod == null ? -1 : Prod.IDProduto,
                        CEANTRIB = d.prod.cEANTrib,
                        CENQ = d.imposto.IPI != null ? d.imposto.IPI.cEnq.ToString() : null,
                        CEST = d.prod.CEST,
                        CFOP = d.prod.CFOP.ToString(),
                        DescricaoProduto = d.prod.xProd,
                        CEAN = string.IsNullOrEmpty(d.prod.cEAN) ? null : (decimal?)Convert.ToDecimal(d.prod.cEAN),
                        INDTOT = (int)d.prod.indTot,
                        NCM = d.prod.NCM,
                        QTRIB = d.prod.qTrib,
                        QCOM = d.prod.qCom,
                        UCOM = d.prod.uCom,
                        VOUTRO = d.prod.vOutro ?? 0,
                        UTRIB = d.prod.uTrib,
                        VPROD = d.prod.vProd,
                        VUNTRIB = d.prod.vUnTrib,
                        vuncom = d.prod.vUnCom,
                        XPROD = d.prod.xProd,
                        VFRETE = d.prod.vFrete ?? 0,
                        VDESC = d.prod.vDesc ?? 0,
                        VSEG = d.prod.vSeg ?? 0,
                        QuantidadeEntrada = d.prod.qCom,
                        UNEntrada = d.prod.uCom,
                        UNSaida = d.prod.uCom,
                        QuantidadeSaida = d.prod.qCom,
                        Valor = d.prod.vUnCom,
                        CProd = d.prod.cProd
                    };
                    if (d.imposto.ICMS != null) PreencherICMS(d.imposto.ICMS.TipoICMS, _Prod);
                    if (d.imposto.PIS != null) PreencherPIS(d.imposto.PIS.TipoPIS, _Prod);
                    if (d.imposto.COFINS != null) PreencherCOFINS(d.imposto.COFINS.TipoCOFINS, _Prod);
                    if (d.imposto.ICMSUFDest != null) PreencherPartilhaICMS(d.imposto.ICMSUFDest, _Prod);
                    Itens.Add(_Prod);

                    UC_ItemNFeEntrada UCControl = new UC_ItemNFeEntrada(_Prod);

                    UCControl.Margin = new Padding(5);
                    ovTL_Produtos.Controls.Add(UCControl);
                }

            }


        }

        private void PreencherTotaisFinanceiro()
        {
            ovTXT_ICMS.Text = ArquivoNFe.infNFe.total.ICMSTot.vICMS.ToString("c2");
            ovTXT_ICMSST.Text = ArquivoNFe.infNFe.total.ICMSTot.vST.ToString("c2");
            ovTXT_IPI.Text = ArquivoNFe.infNFe.total.ICMSTot.vIPI.ToString("c2");
            ovTXT_PIS.Text = ArquivoNFe.infNFe.total.ICMSTot.vPIS.ToString("c2");
            ovTXT_COFINS.Text = ArquivoNFe.infNFe.total.ICMSTot.vCOFINS.ToString("c2");
            ovTXT_Seguro.Text = ArquivoNFe.infNFe.total.ICMSTot.vSeg.ToString("c2");
            ovTXT_Frete.Text = ArquivoNFe.infNFe.total.ICMSTot.vFrete.ToString("c2");
            ovTXT_Outros.Text = ArquivoNFe.infNFe.total.ICMSTot.vOutro.ToString("c2");
            ovTXT_BaseCalculo.Text = ArquivoNFe.infNFe.total.ICMSTot.vBC.ToString("c2");
            ovTXT_TributosAproximados.Text = ArquivoNFe.infNFe.total.ICMSTot.vTotTrib.ToString("c2");
            ovTXT_Desconto.Text = ArquivoNFe.infNFe.total.ICMSTot.vDesc.ToString("c2");
            ovTXT_TotalNF.Text = ArquivoNFe.infNFe.total.ICMSTot.vNF.ToString("c2");
            ovTXT_TotalNFe.Text = ArquivoNFe.infNFe.total.ICMSTot.vNF.ToString("c2");
        }


        private void PreencherICMS(ICMSBasico ICMS, ItemNFeEntrada Prod)
        {
            /* Simples Nacional */
            if (ICMS.GetType() == typeof(ICMSSN101))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMSSN101).CSOSN).ToString();
                Prod.ORIG = ((int)(ICMS as ICMSSN101).orig).ToString();
            }

            if (ICMS.GetType() == typeof(ICMSSN102))
            {
                Prod.CSTICMS = Csosnicms.Csosn102.ToString();
                Prod.ORIG = OrigemMercadoria.OmNacional.ToString();
            }

            if (ICMS.GetType() == typeof(ICMSSN500))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMSSN500).CSOSN).ToString();
                Prod.ORIG = ((int)(ICMS as ICMSSN500).orig).ToString();
            }

            if (ICMS.GetType() == typeof(ICMSSN900))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMSSN101).CSOSN).ToString();
                Prod.ORIG = ((int)(ICMS as ICMSSN101).orig).ToString();
            }

            if (ICMS.GetType() == typeof(ICMSSN201))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMSSN101).CSOSN).ToString();
                Prod.ORIG = ((int)(ICMS as ICMSSN101).orig).ToString();
                Prod.PMVAST = (ICMS as ICMSSN201).pMVAST ?? 0;
                Prod.PREDBCST = (ICMS as ICMSSN201).pRedBCST ?? 0;
                Prod.MODBCST = (int)(ICMS as ICMSSN201).modBCST;
                Prod.VBCST = (ICMS as ICMSSN201).vBCST;
                Prod.VICMSST = (ICMS as ICMSSN201).vICMSST;
                Prod.PICMSST = (ICMS as ICMSSN201).pICMSST;
            }

            if (ICMS.GetType() == typeof(ICMSSN202))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMSSN202).CSOSN).ToString();
                Prod.ORIG = ((int)(ICMS as ICMSSN202).orig).ToString();
                Prod.PMVAST = (ICMS as ICMSSN202).pMVAST ?? 0;
                Prod.PREDBCST = (ICMS as ICMSSN202).pRedBCST ?? 0;
                Prod.MODBCST = (int)(ICMS as ICMSSN202).modBCST;
                Prod.VBCST = (ICMS as ICMSSN202).vBCST;
                Prod.VICMSST = (ICMS as ICMSSN202).vICMSST;
                Prod.PICMSST = (ICMS as ICMSSN202).pICMSST;
            }

            /* Regime Normal */
            if (ICMS.GetType() == typeof(ICMS00))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS00).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS00).orig).ToString();
                Prod.MODBC = (int)(ICMS as ICMS00).modBC;
                Prod.VBCST = (ICMS as ICMS00).vBC;
                Prod.PICMS = (ICMS as ICMS00).pICMS;
                Prod.VICMS = (ICMS as ICMS00).vICMS;
            }

            if (ICMS.GetType() == typeof(ICMS10))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS10).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS10).orig).ToString();
                Prod.PICMS = (ICMS as ICMS10).pICMS;
                Prod.VICMS = (ICMS as ICMS10).vICMS;
                Prod.PICMSST = (ICMS as ICMS10).pICMSST;
                Prod.PMVAST = (ICMS as ICMS10).pMVAST ?? 0;
                Prod.VBCST = (ICMS as ICMS10).vBCST;
                Prod.VICMSST = (ICMS as ICMS10).vICMSST;
            }

            if (ICMS.GetType() == typeof(ICMS20))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS20).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS20).orig).ToString();
                Prod.MODBC = (int)(ICMS as ICMS20).modBC;
                Prod.PICMS = (ICMS as ICMS20).pICMS;
                Prod.PREDBC = (ICMS as ICMS20).pRedBC;
                Prod.VICMS = (ICMS as ICMS20).vICMS;
            }

            if (ICMS.GetType() == typeof(ICMS30))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS30).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS30).orig).ToString();
                Prod.MODBCST = (int)(ICMS as ICMS30).modBCST;
                Prod.PMVAST = (int)(ICMS as ICMS30).pMVAST;
                Prod.PREDBCST = (int)(ICMS as ICMS30).pRedBCST;
                Prod.VBCST = (int)(ICMS as ICMS30).vBCST;
                Prod.PICMSST = (int)(ICMS as ICMS30).pICMSST;
                Prod.VICMSST = (int)(ICMS as ICMS30).vICMSST;
            }

            if (ICMS.GetType() == typeof(ICMS40))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS40).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS40).orig).ToString();
            }

            if (ICMS.GetType() == typeof(ICMS60))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS60).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS60).orig).ToString();
            }

            if (ICMS.GetType() == typeof(ICMS90))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS90).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS90).orig).ToString();
            }

            if (ICMS.GetType() == typeof(ICMS51))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS51).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS51).orig).ToString();
                Prod.VICMS = (ICMS as ICMS51).vICMS ?? 0;
                Prod.MODBC = (int)(ICMS as ICMS51).modBC;
                Prod.PICMS = (ICMS as ICMS51).pICMS ?? 0;
            }

            if (ICMS.GetType() == typeof(ICMS70))
            {
                Prod.CSTICMS = ((int)(ICMS as ICMS51).CST).ToString();
                Prod.ORIG = ((int)(ICMS as ICMS51).orig).ToString();
                Prod.MODBC = (int)(ICMS as ICMS70).modBC;
                Prod.PREDBC = (ICMS as ICMS70).pRedBC;
                Prod.PICMS = (ICMS as ICMS70).pICMS;
                Prod.VICMS = (ICMS as ICMS70).vICMS;
                Prod.MODBCST = (int)(ICMS as ICMS70).modBCST;
                Prod.PMVAST = (ICMS as ICMS70).pMVAST ?? 0;
                Prod.PREDBCST = (ICMS as ICMS70).pRedBCST ?? 0;
                Prod.VBCST = (ICMS as ICMS70).vBCST;
                Prod.PICMSST = (ICMS as ICMS70).pICMSST;
                Prod.VICMSST = (ICMS as ICMS70).vICMSST;
            }
        }
        private void PreencherPIS(PISBasico PISBasico, ItemNFeEntrada Prod)
        {
            if (PISBasico.GetType() == typeof(PISAliq))
            {
                Prod.CSTPIS = ((int)(PISBasico as PISAliq).CST).ToString();
                Prod.PPIS = (PISBasico as PISAliq).pPIS;
                Prod.VBCPIS = (PISBasico as PISAliq).vBC;
                Prod.VPIS = (PISBasico as PISAliq).vPIS;
            }
            if (PISBasico.GetType() == typeof(PISQtde))
            {
                Prod.CSTPIS = ((int)(PISBasico as PISQtde).CST).ToString();
                Prod.PPIS = (PISBasico as PISQtde).vAliqProd;
                Prod.VBCPIS = (PISBasico as PISQtde).qBCProd;
                Prod.VPIS = (PISBasico as PISQtde).vPIS;
            }
            if (PISBasico.GetType() == typeof(PISNT))
            {
                Prod.CSTPIS = ((int)(PISBasico as PISNT).CST).ToString();
            }
            if (PISBasico.GetType() == typeof(PISOutr))
            {
                Prod.CSTPIS = ((int)(PISBasico as PISOutr).CST).ToString();
                Prod.PPIS = (PISBasico as PISOutr).pPIS ?? 0;
                Prod.VBCPIS = (PISBasico as PISOutr).vBC ?? 0;
                Prod.VPIS = (PISBasico as PISOutr).vPIS ?? 0;
            }
        }
        private void PreencherCOFINS(COFINSBasico COFINSBasico, ItemNFeEntrada Prod)
        {
            if (COFINSBasico.GetType() == typeof(COFINSAliq))
            {
                Prod.CSTCOFINS = ((int)(COFINSBasico as COFINSAliq).CST).ToString();
                Prod.PCOFINS = (COFINSBasico as COFINSAliq).pCOFINS;
                Prod.VBCOFINS = (COFINSBasico as COFINSAliq).vBC;
                Prod.VCOFINS = (COFINSBasico as COFINSAliq).vCOFINS;
            }
            if (COFINSBasico.GetType() == typeof(COFINSQtde))
            {
                Prod.CSTCOFINS = ((int)(COFINSBasico as COFINSQtde).CST).ToString();
                Prod.PCOFINS = (COFINSBasico as COFINSQtde).vAliqProd;
                Prod.VBCOFINS = (COFINSBasico as COFINSQtde).qBCProd;
                Prod.VCOFINS = (COFINSBasico as COFINSQtde).vCOFINS;
            }
            if (COFINSBasico.GetType() == typeof(COFINSNT))
            {
                Prod.CSTCOFINS = ((int)(COFINSBasico as COFINSNT).CST).ToString();
            }
            if (COFINSBasico.GetType() == typeof(COFINSOutr))
            {
                Prod.CSTCOFINS = ((int)(COFINSBasico as COFINSOutr).CST).ToString();
                Prod.PCOFINS = (COFINSBasico as COFINSOutr).pCOFINS ?? 0;
                Prod.VBCOFINS = (COFINSBasico as COFINSOutr).vBC ?? 0;
                Prod.VCOFINS = (COFINSBasico as COFINSOutr).vCOFINS ?? 0;
            }
        }
        private void PreencherPartilhaICMS(ICMSUFDest ICMSPartilha, ItemNFeEntrada Prod)
        {
            Prod.VBCUFDEST = ICMSPartilha.vBCUFDest;
            Prod.PFCPUFDEST = ICMSPartilha.pFCPUFDest.Value;
            Prod.PICMSINTER = ICMSPartilha.pICMSInter;
            Prod.PICMSUFDEST = ICMSPartilha.pICMSUFDest;
            Prod.PICMSINTERPART = ICMSPartilha.pICMSInterPart;
            Prod.VFCPUFDEST = ICMSPartilha.vFCPUFDest.Value;
            Prod.VICMSUFDEST = ICMSPartilha.vICMSUFDest;
            Prod.VICMSUFREMET = ICMSPartilha.vICMSUFRemet;
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            // Salvar
            if (!FuncoesNFeEntrada.ExisteNfeEntrada(this.IDNFeEntrada))
            {
                try
                {
                    PDVControlador.BeginTransaction();
                    ValidaImportacao();

                    decimal idPedidoDeCompra = PedidoCompra != null ? PedidoCompra.IDPedidoCompra : Sequence.GetNextID("PEDIDOCOMPRA", "IDPEDIDOCOMPRA");
                    decimal idFornecedor = -1;
                    decimal totalPedidoCompra = 0;
                    decimal idTipoDeOperacao = Convert.ToDecimal(metroComboBoxTipoDeOperacao.SelectedValue);
                    List<ItemPedidoCompra> itensPedidoCompra = new List<ItemPedidoCompra>();

                    /* Salvar Fornecedor */
                    if (Fornecedor == null)
                    {
                        Endereco EndFornecedor = new Endereco
                        {
                            IDEndereco = Sequence.GetNextID("ENDERECO", "IDENDERECO"),
                            Bairro = ArquivoNFe.infNFe.emit.enderEmit.xBairro,
                            Cep = string.IsNullOrEmpty(ArquivoNFe.infNFe.emit.enderEmit.CEP) ? null : ArquivoNFe.infNFe.emit.enderEmit.CEP,
                            Complemento = ArquivoNFe.infNFe.emit.enderEmit.xCpl,
                            Logradouro = ArquivoNFe.infNFe.emit.enderEmit.xLgr,
                            Numero = 11,//string.IsNullOrEmpty(ArquivoNFe.infNFe.emit.enderEmit.nro) ? null : (decimal?)Convert.ToDecimal(ArquivoNFe.infNFe.emit.enderEmit.nro),
                            Telefone = ArquivoNFe.infNFe.emit.enderEmit.fone.ToString(),
                            IDMunicipio = FuncoesMunicipio.GetMunicipioPorCodigo(Convert.ToDecimal(ArquivoNFe.infNFe.emit.enderEmit.cMun)).IDMunicipio,
                            IDPais = 1058,
                            IDUnidadeFederativa = FuncoesUF.GetUnidadeFederativaPorSigla(ArquivoNFe.infNFe.emit.enderEmit.UF.ToString()).IDUnidadeFederativa,
                        };

                        Fornecedor fornecedor = new Fornecedor
                        {
                            IDFornecedor = Sequence.GetNextID("FORNECEDOR", "IDFORNECEDOR"),
                            CNPJ = ArquivoNFe.infNFe.emit.CNPJ,
                            Descricao = ArquivoNFe.infNFe.emit.xNome,
                            Email = null,
                            IDEndereco = EndFornecedor.IDEndereco,
                            InscricaoEstadual = string.IsNullOrEmpty(ArquivoNFe.infNFe.emit.IE) ? null : (decimal?)Convert.ToDecimal(ArquivoNFe.infNFe.emit.IE),
                            Isento = 0,
                            RazaoSocial = ArquivoNFe.infNFe.emit.xFant
                        };

                        if (!FuncoesEndereco.Salvar(EndFornecedor, DAO.Enum.TipoOperacao.INSERT))
                            throw new Exception("Não foi possível salvar o Endereço do Fornecedor.");

                        if (!FuncoesFornecedor.Salvar(fornecedor, DAO.Enum.TipoOperacao.INSERT))
                            throw new Exception("Não foi possível salvar o Fornecedor.");
                        else
                        {
                            try
                            {
                                Fornecedor = FuncoesFornecedor.GetFornecedor(fornecedor.IDFornecedor);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Não foi possível salvar o Fornecedor.");
                            }
                        }
                    }
                    PedidoCompra = new PedidoCompra()
                    {
                        IDPedidoCompra = idPedidoDeCompra,
                        IDFornecedor = Fornecedor.IDFornecedor,
                        IDTipoDeOperacao = idTipoDeOperacao,
                        IDComprador = Contexto.USUARIOLOGADO.IDUsuario,
                        Status = 1
                    };

                    if (!FuncoesPedidoCompra.Salvar(PedidoCompra))
                        throw new Exception("Não foi possível salvar o Pedido Compra");

                    /* Salvar NFe Entrada */
                    NFeEntrada.IDFornecedor = idFornecedor = Fornecedor.IDFornecedor;
                    NFeEntrada.IDPedidoCompra = PedidoCompra == null ? null : (decimal?)PedidoCompra.IDPedidoCompra;
                    if (!FuncoesNFeEntrada.Salvar(NFeEntrada))
                        throw new Exception("Não foi possível salvar a NF-E de Entrada.");

                    if (!FuncoesNFeEntrada.SalvarXML(Encoding.Default.GetBytes(FuncoesXml.ClasseParaXmlString(ArquivoNFe)), NFeEntrada.IDNFeEntrada))
                        throw new Exception("Não foi possível salvar o Xml da NF-E de Entrada.");

                    /* Salvar Produto e ProdutoFornecedor */
                    foreach (Control c in ovTL_Produtos.Controls)
                    {
                        var Control = (c as UC_ItemNFeEntrada);
                        decimal ValorProduto = Control._ItemNFeEntrada.VPROD;
                        var FatorConversao = (Control.ovCMB_FatorConversao.SelectedItem as ConversaoUnidadeDeMedida);


                        if (FatorConversao != null)
                        {
                            Control._ItemNFeEntrada.QuantidadeSaida = (Control._ItemNFeEntrada.QuantidadeEntrada * FatorConversao.Fator);
                            ValorProduto = Control._ItemNFeEntrada.Valor / Control._ItemNFeEntrada.QuantidadeSaida;
                        }

                        if (FuncoesProduto.GetProduto(Control._ItemNFeEntrada.IDProduto) != null)
                        {
                            // Atualizar Código CProd
                            if (FuncoesProdutoFornecedor.GetProdutoPorCodigoEProduto(Control._ItemNFeEntrada.IDProduto, Fornecedor.IDFornecedor, Control._ItemNFeEntrada.CProd) == null)
                                if (!FuncoesProdutoFornecedor.Salvar(new ProdutoFornecedor()
                                {
                                    IDProduto = Control._ItemNFeEntrada.IDProduto,
                                    IDFornecedor = Fornecedor.IDFornecedor,
                                    CProd = Control._ItemNFeEntrada.CProd,
                                    IDProdutoFornecedor = Sequence.GetNextID("PRODUTOFORNECEDOR", "IDPRODUTOFORNECEDOR"),

                                }))
                                    throw new Exception("Não foi possível salvar o Produto Fornecedor.");

                            /* Atualizar dados do Produto ? */

                            if (!FuncoesProduto.AtualizarValorCusto(Control._ItemNFeEntrada.IDProduto, ValorProduto))
                                throw new Exception("Não foi possível atualizar o Valor de Custo do Produto");


                        }
                        else
                        {
                            // Produto Não Existe no Sistema.
                            Control._ItemNFeEntrada.IDProduto = Sequence.GetNextID("PRODUTO", "IDPRODUTO");
                            UnidadeMedida UnProd = FuncoesUnidadeMedida.GetUnidadeMedidaPorSigla(Control._ItemNFeEntrada.UCOM);
                            if (UnProd == null)
                            {
                                UnProd = new UnidadeMedida
                                {
                                    IDUnidadeDeMedida = Sequence.GetNextID("UNIDADEDEMEDIDA", "IDUNIDADEDEMEDIDA"),
                                    Descricao = Control._ItemNFeEntrada.UCOM,
                                    Sigla = Control._ItemNFeEntrada.UCOM
                                };

                                if (!FuncoesUnidadeMedida.Salvar(UnProd, DAO.Enum.TipoOperacao.INSERT))
                                    throw new Exception("Não foi possível salvar a Unidade de Medida.");
                            }

                            if (Control.metroComboBoxIntegracaoFiscal.SelectedItem == null)
                            {
                                throw new Exception($"Informe a integracao fiscal do produto { Control._ItemNFeEntrada.XPROD}.");
                            }
                            decimal integracaoFiscal = Control.metroComboBoxIntegracaoFiscal.SelectedItem != null ? (Control.metroComboBoxIntegracaoFiscal.SelectedItem as IntegracaoFiscal).IDIntegracaoFiscal : 0;

                            // Inserir Produto e CProd
                            Produto Prod = new Produto
                            {
                                IDProduto = Control._ItemNFeEntrada.IDProduto,
                                IDUnidadeDeMedida = UnProd.IDUnidadeDeMedida,
                                Ativo = 1,
                                CEST = Control._ItemNFeEntrada.CEST == null ? Control._ItemNFeEntrada.CEST : " ",
                                Codigo = Control._ItemNFeEntrada.IDProduto.ToString(),
                                Descricao = Control._ItemNFeEntrada.XPROD,
                                EAN = !Control._ItemNFeEntrada.CEAN.HasValue ? Control._ItemNFeEntrada.CEANTRIB : Control._ItemNFeEntrada.CEAN.Value.ToString(),
                                EXTipi = null,
                                IDCategoria = null,
                                IDSubCategoria = null,
                                IDMarca = null,
                                IDIntegracaoFiscalNFCe = integracaoFiscal,
                                IDIntegracaoFiscalNFe = integracaoFiscal,
                                IDOrigemProduto = FuncoesItemNFeEntrada.GetIDOrigem(Control._ItemNFeEntrada.ORIG),
                                IDAlmoxarifadoEntrada = (Control.ovCMB_AlmoxarifadoEntrada.SelectedItem as Almoxarifado).IDAlmoxarifado,
                                IDAlmoxarifadoSaida = null,
                                Trib_AliqCOFINS = Control._ItemNFeEntrada.PCOFINS,
                                Trib_AliqIPI = Control._ItemNFeEntrada.PIPI,
                                Trib_AliqPIS = Control._ItemNFeEntrada.PPIS,
                                Trib_RedBCICMS = Control._ItemNFeEntrada.PREDBC,
                                Trib_MVA = Control._ItemNFeEntrada.PMVAST,
                                Trib_RedBCICMSST = Control._ItemNFeEntrada.PREDBCST,
                                Trib_AliqICMSDif = 0,
                                ValorCusto = ValorProduto,
                                ValorVenda = Control.txtPreçoDeVenda.Text == null ? 0 : decimal.Parse(Control.txtPreçoDeVenda.Text),
                                IDNCM = FuncoesNcm.GetNCMPorCodigo(Convert.ToDecimal(Control._ItemNFeEntrada.NCM)).IDNCM

                            };

                            if (!FuncoesProduto.SalvarProduto(Prod, DAO.Enum.TipoOperacao.INSERT))
                                throw new Exception("Não foi possível salvar o Produto.");

                            /* Salvar ProdutoFornecedor */
                            if (!FuncoesProdutoFornecedor.Salvar(new ProdutoFornecedor
                            {
                                IDProduto = Prod.IDProduto,
                                IDFornecedor = Fornecedor.IDFornecedor,
                                CProd = Control._ItemNFeEntrada.CProd,
                                IDProdutoFornecedor = Sequence.GetNextID("PRODUTOFORNECEDOR", "IDPRODUTOFORNECEDOR")
                            }))
                                throw new Exception("Não foi possível salvar o Produto Fornecedor.");
                        }
                        ItemPedidoCompra itemPedidoCompra = new ItemPedidoCompra()
                        {
                            IDItemPedidoCompra = Sequence.GetNextID("ITEMPEDIDOCOMPRA", "IDITEMPEDIDOCOMPRA"),
                            IDPedidoCompra = PedidoCompra.IDPedidoCompra,
                            IDProduto = Control._ItemNFeEntrada.IDProduto,
                            CodigoItem = Control._ItemNFeEntrada.CEAN.ToString(),
                            ValorUnitario = Control._ItemNFeEntrada.Valor,
                            DescricaoItem = Control._ItemNFeEntrada.DescricaoProduto,
                            Descricao = Control._ItemNFeEntrada.DescricaoProduto,
                            Quantidade = Control._ItemNFeEntrada.QuantidadeEntrada,
                            IDUsuario = Contexto.USUARIOLOGADO.IDUsuario,
                            Total = Control._ItemNFeEntrada.QuantidadeEntrada * Control._ItemNFeEntrada.Valor

                        };
                        totalPedidoCompra += itemPedidoCompra.Total;

                        if (!FuncoesItemPedidoCompra.Salvar(itemPedidoCompra, DAO.Enum.TipoOperacao.INSERT))
                            throw new Exception($"Não foi possível salvar o item pedido compra {itemPedidoCompra.Descricao} ({itemPedidoCompra.IDItemPedidoCompra})");

                        /* Salvar Itens Entrada */
                        if (FatorConversao != null)
                            Control._ItemNFeEntrada.IDConversaoUnidadeDeMedida = FatorConversao.IDConversaoUnidadeDeMedida;

                        if (!FuncoesItemNFeEntrada.Salvar(Control._ItemNFeEntrada))
                            throw new Exception("Não foi possível salvar os Itens da NF-E.");

                        /* Movimento de Estoque */
                        if (!FuncoesMovimentoEstoque.Salvar(new DAO.Entidades.Estoque.Movimento.MovimentoEstoque
                        {
                            IDMovimentoEstoque = Sequence.GetNextID("MOVIMENTOESTOQUE", "IDMOVIMENTOESTOQUE"),
                            DataMovimento = DateTime.Now,
                            IDAlmoxarifado = (Control.ovCMB_AlmoxarifadoEntrada.SelectedItem as Almoxarifado).IDAlmoxarifado,
                            IDItemNFeEntrada = Control._ItemNFeEntrada.IDItemNFeEntrada,
                            IDProduto = Control._ItemNFeEntrada.IDProduto,
                            Quantidade = Control._ItemNFeEntrada.QuantidadeSaida,
                            Tipo = 0,
                            SaldoAtual = 0,
                            IDItemVenda = null,
                            IDProdutoNFe = null,
                            IDItemTransferenciaEstoque = null,
                            IDItemInventario = null,
                            Descricao = "Faturamento de compra (via importação xml)",
                            IDItemPedidoCompra = itemPedidoCompra.IDItemPedidoCompra

                        }))
                            throw new Exception("Não foi possível salvar o Movimento de Estoque.");
                    }
                    PedidoCompra.Total = totalPedidoCompra;
                    if (!FuncoesPedidoCompra.Salvar(PedidoCompra))
                        throw new Exception("Não foi possível salvar o Pedido Compra");


                    /* Gerar Financeiro */
                    if (DUPLICATAS != null && DUPLICATAS.Rows.Count > 0)
                        ZeusUtil.GerarFinanceiroNFeEntrada(PedidoCompra, DUPLICATAS, Fornecedor.IDFornecedor, NFeEntrada.IDNFeEntrada);

                    //Inserie DAC - Faturado Status 

                    //Inserir o Financeiro
                    GerarContasAPagar();


                    //Gerar Dupliatas DAC
                    FuncoesDuplicataDAC.ExcluirPorPedidoCompra(idPedidoDeCompra);

                    for (int i = 0; i < DUPLICATAS.Rows.Count; i++)
                    {
                        DataRow dataRow = DUPLICATAS.Rows[i];

                        DuplicataDAC duplicataDAC = new DuplicataDAC()
                        {
                            IDDuplicataDAC = Sequence.GetNextID("DUPLICATADAC", "IDDUPLICATADAC"),
                            IDCompra = idPedidoDeCompra,
                            IDFormaDePagamento = Convert.ToDecimal(dataRow["IDFORMADEPAGAMENTO"]),
                            Valor = Convert.ToDecimal(dataRow["VALOR"]),
                            Pagamento = Convert.ToDecimal(dataRow["PAGAMENTO"]),
                            DataVencimento = Convert.ToDateTime(dataRow["DATAVENCIMENTO"])
                        };

                        FuncoesDuplicataDAC.SalvarDuplicataDAC(duplicataDAC);
                    }

                    PDVControlador.Commit();
                    MessageBox.Show(this, "NF-E Importada com Sucesso.", NOME_TELA);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(this, Ex.Message, NOME_TELA);
                    PDVControlador.Rollback();
                    Fornecedor = null;
                }
            }
            else //atualizar
            {
                try
                {
                    PDVControlador.BeginTransaction();
                    //ValidaImportacao();

                    NFeEntrada nfeEntrada;
                    PedidoCompra pedidoCompra = new PedidoCompra();
                    if (this.IDNFeEntrada > 0)
                    {
                        nfeEntrada = FuncoesNFeEntrada.GetNFeEntradaPorId(this.IDNFeEntrada);

                        pedidoCompra = FuncoesPedidoCompra.GetPedidoCompra(Convert.ToDecimal(nfeEntrada.IDPedidoCompra));
                    }
                    PedidoCompra = pedidoCompra;

                    decimal idPedidoDeCompra = PedidoCompra != null ? PedidoCompra.IDPedidoCompra : Sequence.GetNextID("PEDIDOCOMPRA", "IDPEDIDOCOMPRA");
                    decimal idFornecedor = -1;
                    decimal totalPedidoCompra = 0;
                    decimal idTipoDeOperacao = Convert.ToDecimal(metroComboBoxTipoDeOperacao.SelectedValue);
                    List<ItemPedidoCompra> itensPedidoCompra = new List<ItemPedidoCompra>();

                    /* Salvar Fornecedor */
                    if (Fornecedor == null)
                    {
                        Fornecedor fornecedor = FuncoesFornecedor.GetFornecedorPorCNPJ(ArquivoNFe.infNFe.emit.CNPJ);
                        Endereco EndFornecedor = FuncoesEndereco.GetEndereco(fornecedor.IDEndereco);



                        if (!FuncoesEndereco.Salvar(EndFornecedor, DAO.Enum.TipoOperacao.UPDATE))
                            throw new Exception("Não foi possível salvar o Endereço do Fornecedor.");

                        if (!FuncoesFornecedor.Salvar(fornecedor, DAO.Enum.TipoOperacao.UPDATE))
                            throw new Exception("Não foi possível salvar o Fornecedor.");
                        else
                        {
                            try
                            {
                                Fornecedor = FuncoesFornecedor.GetFornecedor(fornecedor.IDFornecedor);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Não foi possível salvar o Fornecedor.");
                            }
                        }
                    }
                    
                   /* PedidoCompra = new PedidoCompra()
                    {
                        IDPedidoCompra = idPedidoDeCompra,
                        IDFornecedor = Fornecedor.IDFornecedor,
                        IDTipoDeOperacao = idTipoDeOperacao,
                        IDComprador = Contexto.USUARIOLOGADO.IDUsuario,
                        Status = 1
                    };*/

                    if (!FuncoesPedidoCompra.Salvar(pedidoCompra))
                        throw new Exception("Não foi possível salvar o Pedido Compra");

                    /* Salvar NFe Entrada */
                    NFeEntrada.IDFornecedor = idFornecedor = Fornecedor.IDFornecedor;
                    NFeEntrada.IDPedidoCompra = pedidoCompra == null ? null : (decimal?)pedidoCompra.IDPedidoCompra;
                    if (!FuncoesNFeEntrada.Editar(NFeEntrada, this.IDNFeEntrada))
                        throw new Exception("Não foi possível salvar a NF-E de Entrada.");

                    if (!FuncoesNFeEntrada.AtualizarXml(Encoding.Default.GetBytes(FuncoesXml.ClasseParaXmlString(ArquivoNFe)), this.IDNFeEntrada))
                        throw new Exception("Não foi possível salvar o Xml da NF-E de Entrada.");

                    var itensPedidosCompra = FuncoesItemPedidoCompra.GetItemPedidoCompraPorPedidoCompra(pedidoCompra.IDPedidoCompra);
                    foreach(var item in itensPedidosCompra)
                    {
                        if (!FuncoesMovimentoEstoque.ExcluirMovimentoEstoquePorItemPedidoCompra(item.IDItemPedidoCompra))
                            MessageBox.Show("Erro na exclusão de MovimentoEstoque por ItemPedidoVenda.ID: "+ item.IDItemPedidoCompra);
                    }
                    //apagar todos itens pedido compra por pedido de compra
                    FuncoesItemPedidoCompra.RemoverPorPedidoCompra(pedidoCompra.IDPedidoCompra);

                    //var MovimentoEstoque = FuncoesMovimentoEstoque.ExcluirMovimentoEstoquePorItemPedidoCompra();
                    /* Salvar Produto e ProdutoFornecedor */
                    foreach (Control c in ovTL_Produtos.Controls)
                    {
                        var Control = (c as UC_ItemNFeEntrada);
                        decimal ValorProduto = Control._ItemNFeEntrada.VPROD;
                        var FatorConversao = (Control.ovCMB_FatorConversao.SelectedItem as ConversaoUnidadeDeMedida);


                        if (FatorConversao != null)
                        {
                            Control._ItemNFeEntrada.QuantidadeSaida = (Control._ItemNFeEntrada.QuantidadeEntrada * FatorConversao.Fator);
                            ValorProduto = Control._ItemNFeEntrada.Valor / Control._ItemNFeEntrada.QuantidadeSaida;
                        }

                        if (FuncoesProduto.GetProduto(Control._ItemNFeEntrada.IDProduto) != null)
                        {
                            // Atualizar Código CProd
                            if (FuncoesProdutoFornecedor.GetProdutoPorCodigoEProduto(Control._ItemNFeEntrada.IDProduto, Fornecedor.IDFornecedor, Control._ItemNFeEntrada.CProd) == null)
                                if (!FuncoesProdutoFornecedor.Salvar(new ProdutoFornecedor()
                                {
                                    IDProduto = Control._ItemNFeEntrada.IDProduto,
                                    IDFornecedor = Fornecedor.IDFornecedor,
                                    CProd = Control._ItemNFeEntrada.CProd,
                                    IDProdutoFornecedor = Sequence.GetNextID("PRODUTOFORNECEDOR", "IDPRODUTOFORNECEDOR"),

                                }))
                                    throw new Exception("Não foi possível salvar o Produto Fornecedor.");

                            /* Atualizar dados do Produto ? */

                            if (!FuncoesProduto.AtualizarValorCusto(Control._ItemNFeEntrada.IDProduto, ValorProduto))
                                throw new Exception("Não foi possível atualizar o Valor de Custo do Produto");


                        }
                        else
                        {
                            // Produto Não Existe no Sistema.
                            Control._ItemNFeEntrada.IDProduto = Sequence.GetNextID("PRODUTO", "IDPRODUTO");
                            UnidadeMedida UnProd = FuncoesUnidadeMedida.GetUnidadeMedidaPorSigla(Control._ItemNFeEntrada.UCOM);
                            if (UnProd == null)
                            {
                                UnProd = new UnidadeMedida
                                {
                                    IDUnidadeDeMedida = Sequence.GetNextID("UNIDADEDEMEDIDA", "IDUNIDADEDEMEDIDA"),
                                    Descricao = Control._ItemNFeEntrada.UCOM,
                                    Sigla = Control._ItemNFeEntrada.UCOM
                                };

                                if (!FuncoesUnidadeMedida.Salvar(UnProd, DAO.Enum.TipoOperacao.UPDATE))
                                    throw new Exception("Não foi possível salvar a Unidade de Medida.");
                            }

                            //descomentar essa linha qnd status for colocado na logica
                            if (Control.metroComboBoxIntegracaoFiscal.SelectedItem == null)
                            {
                                throw new Exception($"Informe a integracao fiscal do produto { Control._ItemNFeEntrada.XPROD}.");
                            }
                            decimal integracaoFiscal = Control.metroComboBoxIntegracaoFiscal.SelectedItem != null ? (Control.metroComboBoxIntegracaoFiscal.SelectedItem as IntegracaoFiscal).IDIntegracaoFiscal : 0;

                            // Inserir Produto e CProd
                            Produto Prod = new Produto
                            {
                                IDProduto = Control._ItemNFeEntrada.IDProduto,
                                IDUnidadeDeMedida = UnProd.IDUnidadeDeMedida,
                                Ativo = 1,
                                CEST = Control._ItemNFeEntrada.CEST == null ? Control._ItemNFeEntrada.CEST : " ",
                                Codigo = Control._ItemNFeEntrada.IDProduto.ToString(),
                                Descricao = Control._ItemNFeEntrada.XPROD,
                                EAN = !Control._ItemNFeEntrada.CEAN.HasValue ? Control._ItemNFeEntrada.CEANTRIB : Control._ItemNFeEntrada.CEAN.Value.ToString(),
                                EXTipi = null,
                                IDCategoria = null,
                                IDSubCategoria = null,
                                IDMarca = null,
                                IDIntegracaoFiscalNFCe = integracaoFiscal,
                                IDIntegracaoFiscalNFe = integracaoFiscal,
                                IDOrigemProduto = FuncoesItemNFeEntrada.GetIDOrigem(Control._ItemNFeEntrada.ORIG),
                                IDAlmoxarifadoEntrada = (Control.ovCMB_AlmoxarifadoEntrada.SelectedItem as Almoxarifado).IDAlmoxarifado,
                                IDAlmoxarifadoSaida = null,
                                Trib_AliqCOFINS = Control._ItemNFeEntrada.PCOFINS,
                                Trib_AliqIPI = Control._ItemNFeEntrada.PIPI,
                                Trib_AliqPIS = Control._ItemNFeEntrada.PPIS,
                                Trib_RedBCICMS = Control._ItemNFeEntrada.PREDBC,
                                Trib_MVA = Control._ItemNFeEntrada.PMVAST,
                                Trib_RedBCICMSST = Control._ItemNFeEntrada.PREDBCST,
                                Trib_AliqICMSDif = 0,
                                ValorCusto = ValorProduto,
                                ValorVenda = Control.txtPreçoDeVenda.Text == null ? 0 : decimal.Parse(Control.txtPreçoDeVenda.Text),
                                IDNCM = FuncoesNcm.GetNCMPorCodigo(Convert.ToDecimal(Control._ItemNFeEntrada.NCM)).IDNCM

                            };

                            if (!FuncoesProduto.SalvarProduto(Prod, DAO.Enum.TipoOperacao.UPDATE))
                                throw new Exception("Não foi possível salvar o Produto.");

                            /* Salvar ProdutoFornecedor */
                            if (!FuncoesProdutoFornecedor.Salvar(new ProdutoFornecedor
                            {
                                IDProduto = Prod.IDProduto,
                                IDFornecedor = Fornecedor.IDFornecedor,
                                CProd = Control._ItemNFeEntrada.CProd,
                                IDProdutoFornecedor = Sequence.GetNextID("PRODUTOFORNECEDOR", "IDPRODUTOFORNECEDOR")
                            }))
                                throw new Exception("Não foi possível salvar o Produto Fornecedor.");
                        }
                        ItemPedidoCompra itemPedidoCompra = new ItemPedidoCompra()
                        {
                            IDItemPedidoCompra = Sequence.GetNextID("ITEMPEDIDOCOMPRA", "IDITEMPEDIDOCOMPRA"),
                            IDPedidoCompra = pedidoCompra.IDPedidoCompra,
                            IDProduto = Control._ItemNFeEntrada.IDProduto,
                            CodigoItem = Control._ItemNFeEntrada.CEAN.ToString(),
                            ValorUnitario = Control._ItemNFeEntrada.Valor,
                            DescricaoItem = Control._ItemNFeEntrada.DescricaoProduto,
                            Descricao = Control._ItemNFeEntrada.DescricaoProduto,
                            Quantidade = Control._ItemNFeEntrada.QuantidadeEntrada,
                            IDUsuario = Contexto.USUARIOLOGADO.IDUsuario,
                            Total = Control._ItemNFeEntrada.QuantidadeEntrada * Control._ItemNFeEntrada.Valor

                        };
                        totalPedidoCompra += itemPedidoCompra.Total;
                        //colocar como insert, pois o item pedido compra é ecluido mais acima
                        if (!FuncoesItemPedidoCompra.Salvar(itemPedidoCompra, DAO.Enum.TipoOperacao.INSERT))
                            throw new Exception($"Não foi possível salvar o item pedido compra {itemPedidoCompra.Descricao} ({itemPedidoCompra.IDItemPedidoCompra})");

                        /* Salvar Itens Entrada */
                        if (FatorConversao != null)
                            Control._ItemNFeEntrada.IDConversaoUnidadeDeMedida = FatorConversao.IDConversaoUnidadeDeMedida;

                        if (FuncoesItemNFeEntrada.ExisteItemNfeEntradaPorID(Control._ItemNFeEntrada.IDItemNFeEntrada))
                            FuncoesItemNFeEntrada.Remover(Control._ItemNFeEntrada.IDItemNFeEntrada);


                        if (!FuncoesItemNFeEntrada.Salvar(Control._ItemNFeEntrada))
                            throw new Exception("Não foi possível salvar os Itens da NF-E.");

                        /* Movimento de Estoque */
                        if (!FuncoesMovimentoEstoque.Salvar(new DAO.Entidades.Estoque.Movimento.MovimentoEstoque
                        {
                            IDMovimentoEstoque = Sequence.GetNextID("MOVIMENTOESTOQUE", "IDMOVIMENTOESTOQUE"),
                            DataMovimento = DateTime.Now,
                            IDAlmoxarifado = (Control.ovCMB_AlmoxarifadoEntrada.SelectedItem as Almoxarifado).IDAlmoxarifado,
                            IDItemNFeEntrada = Control._ItemNFeEntrada.IDItemNFeEntrada,
                            IDProduto = Control._ItemNFeEntrada.IDProduto,
                            Quantidade = Control._ItemNFeEntrada.QuantidadeSaida,
                            Tipo = 0,
                            SaldoAtual = 0,
                            IDItemVenda = null,
                            IDProdutoNFe = null,
                            IDItemTransferenciaEstoque = null,
                            IDItemInventario = null,
                            Descricao = "Faturamento de compra (via importação xml)",
                            IDItemPedidoCompra = itemPedidoCompra.IDItemPedidoCompra

                        }))
                            throw new Exception("Não foi possível salvar o Movimento de Estoque.");
                    }
                    pedidoCompra.Total = totalPedidoCompra;
                    if (!FuncoesPedidoCompra.Salvar(pedidoCompra))
                        throw new Exception("Não foi possível salvar o Pedido Compra");


                    /* Gerar Financeiro */
                    if (DUPLICATAS != null && DUPLICATAS.Rows.Count > 0)
                        ZeusUtil.GerarFinanceiroNFeEntrada(pedidoCompra, DUPLICATAS, Fornecedor.IDFornecedor, NFeEntrada.IDNFeEntrada);

                    //Inserie DAC - Faturado Status 

                    //verficar se existe contas a pagar
                    //caso sim -> excluir contas a pagar por idnfeentrada

                    //Inserir o Financeiro
                    GerarContasAPagar();


                    //Gerar Dupliatas DAC
                    FuncoesDuplicataDAC.ExcluirPorPedidoCompra(idPedidoDeCompra);

                    for (int i = 0; i < DUPLICATAS.Rows.Count; i++)
                    {
                        DataRow dataRow = DUPLICATAS.Rows[i];

                        DuplicataDAC duplicataDAC = new DuplicataDAC()
                        {
                            IDDuplicataDAC = Sequence.GetNextID("DUPLICATADAC", "IDDUPLICATADAC"),
                            IDCompra = idPedidoDeCompra,
                            IDFormaDePagamento = Convert.ToDecimal(dataRow["IDFORMADEPAGAMENTO"]),
                            Valor = Convert.ToDecimal(dataRow["VALOR"]),
                            Pagamento = Convert.ToDecimal(dataRow["PAGAMENTO"]),
                            DataVencimento = Convert.ToDateTime(dataRow["DATAVENCIMENTO"])
                        };

                        FuncoesDuplicataDAC.SalvarDuplicataDAC(duplicataDAC);
                    }

                    PDVControlador.Commit();
                    MessageBox.Show(this, "NF-E Editada com Sucesso.", NOME_TELA);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(this, Ex.Message, NOME_TELA);
                    PDVControlador.Rollback();
                    Fornecedor = null;
                }
            }
        }

        private void ValidaImportacao()
        {
            foreach (Control c in ovTL_Produtos.Controls)
            {
                var Control = (c as UC_ItemNFeEntrada);
                if (Control._ItemNFeEntrada.IDProduto == -1 && !Control.ovCKB_IncluirItem.Checked)
                    throw new Exception("Foram encontrados Itens Não identificados. Verifique!");
                else if (!Control.ovCKB_IncluirItem.Checked)
                {
                    if (Control._ItemNFeEntrada.IDProduto == -1)
                        throw new Exception("Foram encontrados Itens Não identificados. Verifique!");
                }

                if (Control.ovCMB_AlmoxarifadoEntrada.SelectedItem == null)
                    throw new Exception("Foram encontrados Itens sem Almoxarifado de Entrada. Verifique!");
            }


            if (metroComboBoxTipoDeOperacao.SelectedItem == null)
                throw new Exception("Preencha o Campo Tipo de Operação");


            decimal totalNfe = Convert.ToDecimal(ovTXT_TotalNFe.Text.Replace("R$", "").Trim());
            if (totalNfe > somaTotalParcelas)
                throw new Exception("A soma total das parcelas não pode ser menor que o valor total NF-e");


        }
        private void GerarContasAPagar()
        {
            TipoDeOperacao tipoDeOperacao = FuncoesTipoDeOperacao.GetTipoDeOperacao(PedidoCompra.IDTipoDeOperacao);

            if (tipoDeOperacao.GerarFinanceiro)
            {
                List<decimal> pagamentos = new List<decimal>();
                foreach (DataRow dataRow in DUPLICATAS.Rows)
                {
                    if (!pagamentos.Contains(Convert.ToInt32(dataRow["PAGAMENTO"])))
                        pagamentos.Add(Convert.ToInt32(dataRow["PAGAMENTO"]));
                }
                foreach (var pagamento in pagamentos)
                {
                    int numParcela = 1;
                    foreach (DataRow dataRow in DUPLICATAS.Rows)
                    {
                        if (Convert.ToInt32(dataRow["PAGAMENTO"]) == pagamento)
                        {

                            FormaDePagamento formaDePagamento = FuncoesFormaDePagamento.GetFormaDePagamento(Convert.ToDecimal(dataRow["IDFORMADEPAGAMENTO"]));
                            int fatorPeriodicidade = FatorPeriodicidade(formaDePagamento);
                            ContaPagar contaPagar = new ContaPagar()
                            {
                                IDContaPagar = Sequence.GetNextID("CONTAPAGAR", "IDCONTAPAGAR"),
                                IDPedidoCompra = PedidoCompra.IDPedidoCompra,
                                IDFormaDePagamento = formaDePagamento.IDFormaDePagamento,
                                IDContaBancaria = tipoDeOperacao.IDContaBancaria,
                                IDCentroCusto = tipoDeOperacao.IdCentroCusto,
                                IDHistoricoFinanceiro = tipoDeOperacao.IDHistoricoFinanceiro,
                                IDFornecedor = PedidoCompra.IDFornecedor,

                                Ord = numParcela.ToString(),
                                Parcela = formaDePagamento.Qtd_Parcelas,

                                Titulo = "",
                                Emissao = DateTime.Now,
                                Vencimento = DateTime.Now.AddDays(fatorPeriodicidade),
                                Situacao = formaDePagamento.Transacao == 1 ? 3 : 1,
                                Fluxo = DateTime.Now,
                                Origem = "Fatura de Compra",
                                ComplmHisFin = "Faturamento de compra de codigo :" + PedidoCompra.IDPedidoCompra + ".",

                                Valor = PedidoCompra.Total / formaDePagamento.Qtd_Parcelas,
                                Multa = 0,
                                Juros = 0,
                                Desconto = 0,
                                Saldo = formaDePagamento.Transacao == 1 ? 0 : PedidoCompra.Total / formaDePagamento.Qtd_Parcelas,
                                ValorTotal = PedidoCompra.Total / formaDePagamento.Qtd_Parcelas
                            };

                            if (!FuncoesContaPagar.Salvar(contaPagar, DAO.Enum.TipoOperacao.INSERT))
                                throw new Exception($"Não foi possível salvar o Lançamento da Compra {PedidoCompra.IDPedidoCompra}.");

                            if (formaDePagamento.Transacao == 1)
                            {
                                BaixaPagamento baixaPagamento = new BaixaPagamento()
                                {
                                    IDContaPagar = contaPagar.IDContaPagar,
                                    IDBaixaPagamento = Sequence.GetNextID("BAIXAPAGAMENTO", "IDBAIXAPAGAMENTO"),
                                    Valor = contaPagar.ValorTotal,
                                    IDFormaDePagamento = Convert.ToDecimal(contaPagar.IDFormaDePagamento),
                                    IDHistoricoFinanceiro = Convert.ToDecimal(contaPagar.IDHistoricoFinanceiro),
                                    IDContaBancaria = Convert.ToDecimal(contaPagar.IDContaBancaria),
                                    Baixa = DateTime.Now,

                                };
                                if (!FuncoesBaixaPagamento.Salvar(baixaPagamento, DAO.Enum.TipoOperacao.INSERT))
                                    throw new Exception($"Não foi possível salvar a Baixa da Compra {PedidoCompra.IDPedidoCompra}.");
                            }
                        }
                        numParcela++;
                    }
                }
            }
        }

        /* Financeiro */
        private List<FormaDePagamento> FORMASPAGAMENTO = null;
        private DataTable DUPLICATAS = null;

        private void CarregarFinanceiro(bool BuscarBanco)
        {
            if (BuscarBanco)
                DUPLICATAS = FuncoesDuplicataNFe.GetDuplicatas(-1);

            gridControlVencimentos.DataSource = DUPLICATAS;
            AjustaHeaderFinanceiro();
            ovTXT_TotalFinanceiro.Text = DUPLICATAS.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).Sum(o => Convert.ToDecimal(o["VALOR"])).ToString("c2");
            gridViewVencimentos.BestFitColumns();
        }


        private int FatorPeriodicidade(FormaDePagamento formaDePagamento)
        {
            int fatorPeriodicidade = 0;

            switch (formaDePagamento.Periodicidade)
            {
                case "Diário":
                    fatorPeriodicidade = 1;
                    break;
                case "Semanal":
                    fatorPeriodicidade = 7;
                    break;
                case "Quinzenal":
                    fatorPeriodicidade = 15;
                    break;
                case "Mensal":
                    fatorPeriodicidade = 30;
                    break;
                case "Trimestral":
                    fatorPeriodicidade = 90;
                    break;
                case "Semestral":
                    fatorPeriodicidade = 180;
                    break;
                case "Anual":
                    fatorPeriodicidade = 365;
                    break;
            }

            return fatorPeriodicidade;

        }
        private void AjustaHeaderFinanceiro()
        {
            for (int i = 0; i < gridViewVencimentos.Columns.Count; i++)
            {
                switch (i)
                {
                    case 2:
                        gridViewVencimentos.Columns[i].Caption = "NÚMERO DA PARCELA";
                        break;
                    case 3:
                        gridViewVencimentos.Columns[i].Caption = "VENCIMENTO";
                        break;
                    case 4:
                        gridViewVencimentos.Columns[i].Caption = "VALOR DO VENCIMENTO";
                        gridViewVencimentos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        gridViewVencimentos.Columns[i].DisplayFormat.FormatString = "c2";
                        break;
                    default:
                        gridViewVencimentos.Columns[i].Visible = false;
                        break;
                }
            }
        }

        private void LimparDuplicatas()
        {
            foreach (DataRowView dr in DUPLICATAS.DefaultView)
                dr.Delete();
            gridControlVencimentos.DataSource = DUPLICATAS;
            AjustaHeaderFinanceiro();
            gridViewVencimentos.BestFitColumns();

            LabelsFinanceiro();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            LimparDuplicatas();

        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            InserirParcelas();
        }

        private void InserirParcelas()
        {
            // Gerar Parcelas.
            if (ovCMB_FormaPagamento.SelectedItem == null)
            {
                MessageBox.Show(this, "Selecione a Forma de Pagamento.", NOME_TELA);
                return;
            }

            decimal idFormaDePagamento = (ovCMB_FormaPagamento.SelectedItem as FormaDePagamento).IDFormaDePagamento;
            FormaDePagamento formaDePagamento = FuncoesFormaDePagamento.GetFormaDePagamento(idFormaDePagamento);

            int periodicidade = Periodicidade.Fator(formaDePagamento.Periodicidade);
            DateTime vencimento = DateTime.Today.AddDays(periodicidade);

            double valorParcela = Math.Round(Convert.ToDouble(textEditValorPagar.EditValue) / formaDePagamento.Qtd_Parcelas, 2);

            int pagamento = -1;
            foreach (DataRow dataRow in DUPLICATAS.Rows)
            {
                int rowPagamento = Convert.ToInt32(dataRow["PAGAMENTO"]);
                pagamento = pagamento < rowPagamento ? rowPagamento : pagamento;
            }
            pagamento++;
            for (int i = 0; i < formaDePagamento.Qtd_Parcelas; i++)
            {
                vencimento = vencimento.Date.AddMonths(i == 0 ? 0 : 1);
                DataRow dr = DUPLICATAS.NewRow();
                dr["IDDUPLICATANFE"] = Sequence.GetNextID("DUPLICATANFE", "IDDUPLICATANFE");
                dr["IDNFE"] = -1;
                dr["NUMERODOCUMENTO"] = i + 1;
                dr["DATAVENCIMENTO"] = vencimento.Date;
                dr["VALOR"] = valorParcela;
                dr["IDFORMADEPAGAMENTO"] = idFormaDePagamento;
                dr["PAGAMENTO"] = pagamento;

                DUPLICATAS.Rows.Add(dr);
            }



            CarregarFinanceiro(false);

            LabelsFinanceiro();


            textEditValorPagar.EditValue = 0;
            textEditValorPagar.Focus();
            ovCMB_FormaPagamento.SelectedIndex = -1;
        }
        private void FEST_ImportacaoNFe_Load(object sender, EventArgs e)
        {
            ///* Verificar se a NFe já foi importada. */
            //if (FuncoesNFeEntrada.IsNFeImportada(ArquivoNFe.infNFe.Id.Replace("NFe", string.Empty)))
            //	if (MessageBox.Show(this, $"A NF-E {ArquivoNFe.infNFe.Id.Replace("NFe", string.Empty)} Já foi importada.", NOME_TELA) == DialogResult.OK)
            //		Close();
        }

        private void textEditValorPagar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (labelTrocoValor.Text == "R$ 0,00")
                    InserirParcelas();
            }

        }

        private void ovCMB_FormaPagamento_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(textEditValorPagar.EditValue) == 0)
                textEditValorPagar.EditValue = Convert.ToDecimal(labelFaltaValor.Text.Replace("R$", ""));


            textEditValorPagar.Focus();
        }

        private void simpleButtonCancelarParcela_Click(object sender, EventArgs e)
        {
            //Essa lista irá receber os id's das duplicatas nfe que estão selecionadas
            List<decimal> ids = new List<decimal>();

            foreach (int i in gridViewVencimentos.GetSelectedRows())
            {
                ids.Add(Convert.ToDecimal(DUPLICATAS.Rows[i][0]));
            }

            for (int i = 0; i < DUPLICATAS.Rows.Count; i++)
            {
                //Se o id duplicata nfe da linha estiver condo na lista de ids a linha será deletada 
                if (ids.Contains(Convert.ToDecimal(DUPLICATAS.Rows[i][0])))
                    DUPLICATAS.Rows[i].Delete();
            }

            CarregarFinanceiro(false);
            LabelsFinanceiro();

        }
        private void LabelsFinanceiro()
        {
            somaTotalParcelas = 0;
            for (int i = 0; i < DUPLICATAS.Rows.Count; i++)
            {
                somaTotalParcelas += Convert.ToDecimal(DUPLICATAS.Rows[i]["VALOR"]);
            }

            decimal totalNfe = Convert.ToDecimal(ovTXT_TotalNFe.Text.Replace("R$", "").Trim());
            labelFaltaValor.Text = totalNfe > somaTotalParcelas ? "R$ " + (totalNfe - somaTotalParcelas).ToString() : "R$ 0,00";
            labelTrocoValor.Text = totalNfe < somaTotalParcelas ? "R$ " + Math.Round(somaTotalParcelas - totalNfe, 2).ToString() : "R$ 0,00";

            simpleButtonCancelarParcela.Enabled = DUPLICATAS.Rows.Count > 0;
        }

    }
}