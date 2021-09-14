using PDV.DAO.Custom;
using PDV.DAO.DB.Controller;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using System.Collections.Generic;

namespace PDV.CONTROLER.Funcoes
{
    public static class FuncoesItemOrdemDeServico
    {
        public static List<ItemOrdemDeServico> GetItensOrdemServico(decimal idOrdemDeServico)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM ITEMORDEMDESERVICO WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOrdemDeServico;
                oSQL.Open();

                return new DataTableParser<ItemOrdemDeServico>().ParseDataTable(oSQL.dtDados);
            }
        }

        public static bool Salvar(ItemOrdemDeServico itemOrdemDeServico)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"INSERT INTO ITEMORDEMDESERVICO (
                                IDITEMORDEMDESERVICO,
                                IDSERVICO,
                                IDORDEMDESERVICO,
                                DESCRICAO,
                                QUANTIDADE,
                                DESCONTOPORCENTAGEM,
                                DESCONTOVALOR,
                                VALORUNITARIOITEM,
                                SUBTOTAL
                             ) VALUES (
                                @IDITEMORDEMDESERVICO,
                                @IDSERVICO,
                                @IDORDEMDESERVICO,
                                @DESCRICAO,
                                @QUANTIDADE,
                                @DESCONTOPORCENTAGEM,
                                @DESCONTOVALOR,
                                @VALORUNITARIOITEM,
                                @SUBTOTAL
                            )
                              ";
                oSQL.ParamByName["IDITEMORDEMDESERVICO"] = itemOrdemDeServico.IDItemOrdemDeServico;
                oSQL.ParamByName["IDSERVICO"] = itemOrdemDeServico.IDServico;
                oSQL.ParamByName["IDORDEMDESERVICO"] = itemOrdemDeServico.IDOrdemDeServico;
                oSQL.ParamByName["DESCRICAO"] = itemOrdemDeServico.Descricao;
                oSQL.ParamByName["QUANTIDADE"] = itemOrdemDeServico.Quantidade;
                oSQL.ParamByName["DESCONTOPORCENTAGEM"] = itemOrdemDeServico.DescontoPorcentagem;
                oSQL.ParamByName["DESCONTOVALOR"] = itemOrdemDeServico.DescontoValor;
                oSQL.ParamByName["VALORUNITARIOITEM"] = itemOrdemDeServico.ValorUnitarioItem;
                oSQL.ParamByName["SUBTOTAL"] = itemOrdemDeServico.SubTotal;
                return oSQL.ExecSQL() == 1;

            }
        }

        public static void Remover(decimal idOrdemDeServico)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "DELETE FROM ITEMORDEMDESERVICO WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOrdemDeServico;
                oSQL.ExecSQL();

            }
        }

    }
}
