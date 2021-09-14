using PDV.DAO.Custom;
using PDV.DAO.DB.Controller;
using PDV.DAO.Entidades.Estoque.NFeImportacao;
using System;

namespace PDV.CONTROLER.Funcoes
{
    public class FuncoesItemNFeEntrada
    {
        public static bool Salvar(ItemNFeEntrada ItemEntrada)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"INSERT INTO ITEMNFEENTRADA(
                                     IDITEMNFEENTRADA, IDNFEENTRADA, IDPRODUTO, IDCONVERSAOUNIDADEDEMEDIDA, 
                                     CEAN, XPROD, NCM, CEST, CFOP, UCOM, QCOM, VUNCOM, VPROD, CEANTRIB, 
                                     UTRIB, QTRIB, VUNTRIB, VFRETE, VSEG, VDESC, VOUTRO, INDTOT, VTOTTRIB, 
                                     ORIG, CSTICMS, MODBC, PREDBC, VBCICMS, PICMS, VICMS, MODBCST, 
                                     PMVAST, PREDBCST, VBCST, PICMSST, VICMSST, VBCUFDEST, PFCPUFDEST, 
                                     PICMSUFDEST, PICMSINTER, PICMSINTERPART, VFCPUFDEST, VICMSUFDEST, 
                                     VICMSUFREMET, CENQ, CSTIPI, VBCIPI, PIPI, VIPI, CSTPIS, VBCPIS, 
                                     PPIS, VPIS, CSTCOFINS, VBCOFINS, PCOFINS, VCOFINS, CPROD)
                             VALUES (@IDITEMNFEENTRADA, @IDNFEENTRADA, @IDPRODUTO, @IDCONVERSAOUNIDADEDEMEDIDA,
                                     @CEAN, @XPROD, @NCM, @CEST, @CFOP, @UCOM, @QCOM, @VUNCOM, @VPROD, @CEANTRIB,
                                     @UTRIB, @QTRIB, @VUNTRIB, @VFRETE, @VSEG, @VDESC, @VOUTRO, @INDTOT, @VTOTTRIB,
                                     @ORIG, @CSTICMS, @MODBC, @PREDBC, @VBCICMS, @PICMS, @VICMS, @MODBCST,
                                     @PMVAST, @PREDBCST, @VBCST, @PICMSST, @VICMSST, @VBCUFDEST, @PFCPUFDEST,
                                     @PICMSUFDEST, @PICMSINTER, @PICMSINTERPART, @VFCPUFDEST, @VICMSUFDEST,
                                     @VICMSUFREMET, @CENQ, @CSTIPI, @VBCIPI, @PIPI, @VIPI, @CSTPIS, @VBCPIS,
                                     @PPIS, @VPIS, @CSTCOFINS, @VBCOFINS, @PCOFINS, @VCOFINS, @CPROD)";
                oSQL.ParamByName["IDITEMNFEENTRADA"] = ItemEntrada.IDItemNFeEntrada;
                oSQL.ParamByName["IDNFEENTRADA"] = ItemEntrada.IDNFeEntrada;
                oSQL.ParamByName["IDPRODUTO"] = ItemEntrada.IDProduto;
                oSQL.ParamByName["IDCONVERSAOUNIDADEDEMEDIDA"] = ItemEntrada.IDConversaoUnidadeDeMedida;
                oSQL.ParamByName["CEAN"] = ItemEntrada.CEAN;
                oSQL.ParamByName["XPROD"] = ItemEntrada.XPROD;
                oSQL.ParamByName["NCM"] = ItemEntrada.NCM;
                oSQL.ParamByName["CEST"] = ItemEntrada.CEST;
                oSQL.ParamByName["CFOP"] = ItemEntrada.CFOP;
                oSQL.ParamByName["UCOM"] = ItemEntrada.UCOM;
                oSQL.ParamByName["QCOM"] = ItemEntrada.QCOM;
                oSQL.ParamByName["VUNCOM"] = ItemEntrada.vuncom;
                oSQL.ParamByName["VPROD"] = ItemEntrada.VPROD;
                oSQL.ParamByName["CEANTRIB"] = ItemEntrada.CEANTRIB;
                oSQL.ParamByName["UTRIB"] = ItemEntrada.UTRIB;
                oSQL.ParamByName["QTRIB"] = ItemEntrada.QTRIB;
                oSQL.ParamByName["VUNTRIB"] = ItemEntrada.VUNTRIB;
                oSQL.ParamByName["VFRETE"] = ItemEntrada.VFRETE;
                oSQL.ParamByName["VSEG"] = ItemEntrada.VSEG;
                oSQL.ParamByName["VDESC"] = ItemEntrada.VDESC;
                oSQL.ParamByName["VOUTRO"] = ItemEntrada.VOUTRO;
                oSQL.ParamByName["INDTOT"] = ItemEntrada.INDTOT;
                oSQL.ParamByName["VTOTTRIB"] = ItemEntrada.VTOTTRIB;
                oSQL.ParamByName["ORIG"] = GetIDOrigem(ItemEntrada.ORIG);
                oSQL.ParamByName["CSTICMS"] = GetCodigoCSTICMS(ItemEntrada.CSTICMS);
                oSQL.ParamByName["MODBC"] = ItemEntrada.MODBC;
                oSQL.ParamByName["PREDBC"] = ItemEntrada.PREDBC;
                oSQL.ParamByName["VBCICMS"] = ItemEntrada.VBCICMS;
                oSQL.ParamByName["PICMS"] = ItemEntrada.PICMS;
                oSQL.ParamByName["VICMS"] = ItemEntrada.VICMS;
                oSQL.ParamByName["MODBCST"] = ItemEntrada.MODBCST;
                oSQL.ParamByName["PMVAST"] = ItemEntrada.PMVAST;
                oSQL.ParamByName["PREDBCST"] = ItemEntrada.PREDBCST;
                oSQL.ParamByName["VBCST"] = ItemEntrada.VBCST;
                oSQL.ParamByName["PICMSST"] = ItemEntrada.PICMSST;
                oSQL.ParamByName["VICMSST"] = ItemEntrada.VICMSST;
                oSQL.ParamByName["VBCUFDEST"] = ItemEntrada.VBCUFDEST;
                oSQL.ParamByName["PFCPUFDEST"] = ItemEntrada.PFCPUFDEST;
                oSQL.ParamByName["PICMSUFDEST"] = ItemEntrada.PICMSUFDEST;
                oSQL.ParamByName["PICMSINTER"] = ItemEntrada.PICMSINTER;
                oSQL.ParamByName["PICMSINTERPART"] = ItemEntrada.PICMSINTERPART;
                oSQL.ParamByName["VFCPUFDEST"] = ItemEntrada.VFCPUFDEST;
                oSQL.ParamByName["VICMSUFDEST"] = ItemEntrada.VICMSUFDEST;
                oSQL.ParamByName["VICMSUFREMET"] = ItemEntrada.VICMSUFREMET;
                oSQL.ParamByName["CENQ"] = ItemEntrada.CENQ;
                oSQL.ParamByName["CSTIPI"] = ItemEntrada.CSTIPI;
                oSQL.ParamByName["VBCIPI"] = ItemEntrada.VBCIPI;
                oSQL.ParamByName["PIPI"] = ItemEntrada.PIPI;
                oSQL.ParamByName["VIPI"] = ItemEntrada.VIPI;
                oSQL.ParamByName["CSTPIS"] = ItemEntrada.CSTPIS;
                oSQL.ParamByName["VBCPIS"] = ItemEntrada.VBCPIS;
                oSQL.ParamByName["PPIS"] = ItemEntrada.PPIS;
                oSQL.ParamByName["VPIS"] = ItemEntrada.VPIS;
                oSQL.ParamByName["CSTCOFINS"] = ItemEntrada.CSTCOFINS;
                oSQL.ParamByName["VBCOFINS"] = ItemEntrada.VBCOFINS;
                oSQL.ParamByName["PCOFINS"] = ItemEntrada.PCOFINS;
                oSQL.ParamByName["VCOFINS"] = ItemEntrada.VCOFINS;
                oSQL.ParamByName["CPROD"] = ItemEntrada.CProd;
                return oSQL.ExecSQL() == 1;
            }
        }
        public static decimal GetIDOrigem(string descricao)
        {
            decimal idOrigem = -1;

            //De acordo com o enum "OrigemMercadoria"
            switch (descricao)
            {
                case "OmNacional":
                    idOrigem = 0;
                    break;
                case "OmEstrangeiraImportacaoDireta":
                    idOrigem = 1;
                    break;
                case "OmEstrangeiraAdquiridaBrasil":
                    idOrigem = 2;
                    break;
                case "OmNacionalConteudoImportacaoSuperior40":
                    idOrigem = 3;
                    break;
                case "OmNacionalProcessosBasicos":
                    idOrigem = 4;
                    break;
                case "OmNacionalConteudoImportacaoInferiorIgual40":
                    idOrigem = 5;
                    break;
                case "OmEstrangeiraImportacaoDiretaSemSimilar":
                    idOrigem = 6;
                    break;
                case "OmEstrangeiraAdquiridaBrasilSemSimilar":
                    idOrigem = 7;
                    break;
                case "OmNacionalConteudoImportacaoSuperior70":
                    idOrigem = 8;
                    break;
            }
            try
            {
                idOrigem = Convert.ToDecimal(descricao);
            }
            catch (FormatException)
            {
               
            }
            

            return idOrigem;

        }
        public static decimal GetCodigoCSTICMS(string descricao)
        {
            decimal codigo = 000;
            if(descricao.Length == 8)
                codigo = Convert.ToDecimal(descricao.Substring(5));

            return codigo;
        
         }
        public static ItemNFeEntrada GetItemNfeEntradaPorProdutoID(decimal IDProduto)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM ITEMNFEENTRADA WHERE IDPRODUTO = @IDPRODUTO";
                oSQL.ParamByName["IDPRODUTO"] = IDProduto;
                oSQL.Open();
               // if (oSQL.IsEmpty)
                //    return null;

                return EntityUtil<ItemNFeEntrada>.ParseDataRow(oSQL.dtDados.Rows[0]);
            }
        }
        public static bool ExisteItemNfeEntradaPorID(decimal IDItemNfeEntrada)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM ITEMNFEENTRADA WHERE IDITEMNFEENTRADA = @IDITEMNFEENTRADA";
                oSQL.ParamByName["IDITEMNFEENTRADA"] = IDItemNfeEntrada;
                oSQL.Open();
                return !oSQL.IsEmpty;
            }
        }

        public static bool Remover(decimal IDItemNfeEntrada)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "DELETE FROM ITEMNFEENTRADA WHERE IDITEMNFEENTRADA = @IDITEMNFEENTRADA";
                oSQL.ParamByName["IDITEMNFEENTRADA"] = IDItemNfeEntrada;
                return oSQL.ExecSQL() == 1;
            }
        }
    }
}


