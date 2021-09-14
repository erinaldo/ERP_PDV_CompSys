using PDV.DAO.Custom;
using PDV.DAO.DB.Controller;
using PDV.DAO.Entidades.Estoque.Suprimentos;
using PDV.DAO.Enum;
using System.Collections.Generic;
using System.Data;

namespace PDV.CONTROLER.Funcoes
{
    public class FuncoesConversaoUnidadeMedida
    {
        public static bool Salvar(ConversaoUnidadeDeMedida Conversao, TipoOperacao Op)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                switch(Op)
                {
                    case TipoOperacao.INSERT:
                        oSQL.SQL = @"INSERT INTO CONVERSAOUNIDADEDEMEDIDA
                                        (IDCONVERSAOUNIDADEDEMEDIDA, IDPRODUTO, IDUNIDADEDEMEDIDAENTRADA, IDUNIDADEDEMEDIDASAIDA, FATOR)
                                     VALUES 
                                        (@IDCONVERSAOUNIDADEDEMEDIDA, @IDPRODUTO, @IDUNIDADEDEMEDIDAENTRADA, @IDUNIDADEDEMEDIDASAIDA, @FATOR)";
                        break;
                    case TipoOperacao.UPDATE:
                        oSQL.SQL = @"UPDATE CONVERSAOUNIDADEDEMEDIDA
                                      SET IDPRODUTO = @IDPRODUTO,
                                          IDUNIDADEDEMEDIDAENTRADA = @IDUNIDADEDEMEDIDAENTRADA,
                                          IDUNIDADEDEMEDIDASAIDA = IDUNIDADEDEMEDIDASAIDA,
                                          FATOR = @FATOR
                                      WHERE IDCONVERSAOUNIDADEDEMEDIDA = @IDCONVERSAOUNIDADEDEMEDIDA";
                        break; 
                }
                oSQL.ParamByName["IDCONVERSAOUNIDADEDEMEDIDA"] = Conversao.IDConversaoUnidadeDeMedida;
                oSQL.ParamByName["IDPRODUTO"] = Conversao.IDProduto;
                oSQL.ParamByName["IDUNIDADEDEMEDIDAENTRADA"] = Conversao.IDUnidadeDeMedidaEntrada;
                oSQL.ParamByName["IDUNIDADEDEMEDIDASAIDA"] = Conversao.IDUnidadeDeMedidaSaida;
                oSQL.ParamByName["FATOR"] = Conversao.Fator;
                return oSQL.ExecSQL() == 1;
            }
        }

        public static bool Remover(decimal IDConversaoUnidadeDeMedida)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "DELETE FROM CONVERSAOUNIDADEDEMEDIDA WHERE IDCONVERSAOUNIDADEDEMEDIDA = IDCONVERSAOUNIDADEDEMEDIDA";
                oSQL.ParamByName["IDCONVERSAOUNIDADEDEMEDIDA"] = IDConversaoUnidadeDeMedida;
                return oSQL.ExecSQL() == 1;
            }
        }

        public static DataTable GetConversoes(string Produto)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = $@"SELECT PRODUTO.DESCRICAO AS PRODUTO,
                                     UNENTRADA.SIGLA AS UNIDADEDEMEDIDAENTRADA,
                                     UNSAIDA.SIGLA AS UNIDADEDEMEDIDASAIDA,       
                                     CONVERSAOUNIDADEDEMEDIDA.FATOR,
                              
                                     CONVERSAOUNIDADEDEMEDIDA.IDCONVERSAOUNIDADEDEMEDIDA,
                                     CONVERSAOUNIDADEDEMEDIDA.IDPRODUTO,       
                                     CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDAENTRADA,
                                     CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDASAIDA
                                FROM CONVERSAOUNIDADEDEMEDIDA
                                 INNER JOIN PRODUTO ON (CONVERSAOUNIDADEDEMEDIDA.IDPRODUTO = PRODUTO.IDPRODUTO)
                                 INNER JOIN UNIDADEDEMEDIDA UNENTRADA ON (CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDAENTRADA = UNENTRADA.IDUNIDADEDEMEDIDA)
                                 INNER JOIN UNIDADEDEMEDIDA UNSAIDA ON (CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDASAIDA = UNSAIDA.IDUNIDADEDEMEDIDA)
                              WHERE UPPER(PRODUTO.DESCRICAO) LIKE UPPER('%{Produto}%')";
                oSQL.Open();
                return oSQL.dtDados;
            }
        }

        public static ConversaoUnidadeDeMedida GetConversao(decimal IDConversaoUnidadeDeMedida)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM CONVERSAOUNIDADEDEMEDIDA WHERE IDCONVERSAOUNIDADEDEMEDIDA = @IDCONVERSAOUNIDADEDEMEDIDA";
                oSQL.ParamByName["IDCONVERSAOUNIDADEDEMEDIDA"] = IDConversaoUnidadeDeMedida;
                oSQL.Open();
                return EntityUtil<ConversaoUnidadeDeMedida>.ParseDataRow(oSQL.dtDados.Rows[0]);
            }
        }

        public static bool Existe(decimal IDConversaoUnidadeDeMedida)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT 1 FROM CONVERSAOUNIDADEDEMEDIDA WHERE IDCONVERSAOUNIDADEDEMEDIDA = @IDCONVERSAOUNIDADEDEMEDIDA";
                oSQL.ParamByName["IDCONVERSAOUNIDADEDEMEDIDA"] = IDConversaoUnidadeDeMedida;
                oSQL.Open();
                return !oSQL.IsEmpty;
            }
        }

        public static List<ConversaoUnidadeDeMedida> GetConversoesPorProduto(decimal IDProduto)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"SELECT CONVERSAOUNIDADEDEMEDIDA.IDCONVERSAOUNIDADEDEMEDIDA,
                                    CONVERSAOUNIDADEDEMEDIDA.IDPRODUTO,       
                                    CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDAENTRADA,
                                    CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDASAIDA,
                                    CONVERSAOUNIDADEDEMEDIDA.FATOR,
                                    UNENT.SIGLA AS UNENTRADA,
                                    UNSAI.SIGLA AS UNSAIDA
                               FROM CONVERSAOUNIDADEDEMEDIDA
                                INNER JOIN UNIDADEDEMEDIDA UNENT ON (CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDAENTRADA = UNENT.IDUNIDADEDEMEDIDA)
                                INNER JOIN UNIDADEDEMEDIDA UNSAI ON (CONVERSAOUNIDADEDEMEDIDA.IDUNIDADEDEMEDIDASAIDA = UNSAI.IDUNIDADEDEMEDIDA)
                             WHERE CONVERSAOUNIDADEDEMEDIDA.IDPRODUTO = @IDPRODUTO";
                oSQL.ParamByName["IDPRODUTO"] = IDProduto;
                oSQL.Open();
                return new DataTableParser<ConversaoUnidadeDeMedida>().ParseDataTable(oSQL.dtDados);
            }
        }
    }
}