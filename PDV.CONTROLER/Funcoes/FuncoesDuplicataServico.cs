using PDV.DAO.Custom;
using PDV.DAO.DB.Controller;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using System.Collections.Generic;

namespace PDV.CONTROLER.Funcoes
{
    public static class FuncoesDuplicataServico
    {
        public static List<DuplicataServico> GetDuplicatasServicos(decimal idOrdemDeServico)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM DUPLICATASERVICO WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOrdemDeServico;
                oSQL.Open();
                return new DataTableParser<DuplicataServico>().ParseDataTable(oSQL.dtDados);
            }
        }

        public static bool Salvar(DuplicataServico duplicataServico)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"INSERT INTO DUPLICATASERVICO (
                                IDDUPLICATASERVICO,
                                DESCRICAO,
                                DATAVENCIMENTO,
                                VALOR,
                                IDFORMADEPAGAMENTO,
                                IDORDEMDESERVICO
                             ) VALUES (
                                @IDDUPLICATASERVICO,
                                @DESCRICAO,
                                @DATAVENCIMENTO,
                                @VALOR,
                                @IDFORMADEPAGAMENTO,
                                @IDORDEMDESERVICO
                            )
                              ";
                oSQL.ParamByName["IDDUPLICATASERVICO"] = duplicataServico.IDDuplicataServico;
                oSQL.ParamByName["DESCRICAO"] = duplicataServico.Descricao;
                oSQL.ParamByName["DATAVENCIMENTO"] = duplicataServico.DataVencimento;
                oSQL.ParamByName["VALOR"] = duplicataServico.Valor;
                oSQL.ParamByName["IDFORMADEPAGAMENTO"] = duplicataServico.IDFormaDePagamento;
                oSQL.ParamByName["IDORDEMDESERVICO"] = duplicataServico.IDOrdemDeServico;
                return oSQL.ExecSQL() == 1;
            }
        }

        public static void Remover(decimal idOrdemDeServico)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "DELETE FROM DUPLICATASERVICO WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOrdemDeServico;
                oSQL.ExecSQL();
            }
        }
    }
}
