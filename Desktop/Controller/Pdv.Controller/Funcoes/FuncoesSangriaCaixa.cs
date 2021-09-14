using System;
using System.Data;
using PDV.DAO.DB.Controller;
using PDV.DAO.Entidades;

namespace PDV.CONTROLER.Funcoes
{
    public class FuncoesSangriaCaixa
    {
        public static bool Salvar(SangriaCaixa Sangria)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"INSERT INTO SANGRIACAIXA 
                               (IDSANGRIACAIXA, IDUSUARIO, IDUSUARIOCADASTRO, IDFLUXOCAIXA, DATASANGRIA, VALOR, OBSERVACAO)
                             VALUES
                               (@IDSANGRIACAIXA, @IDUSUARIO, @IDUSUARIOCADASTRO, @IDFLUXOCAIXA, @DATASANGRIA, @VALOR, @OBSERVACAO)";
                oSQL.ParamByName["IDSANGRIACAIXA"] = Sangria.IDSangriaCaixa;
                oSQL.ParamByName["IDUSUARIO"] = Sangria.IDUsuario;
                oSQL.ParamByName["IDUSUARIOCADASTRO"] = Sangria.IDUsuarioCadastro;
                oSQL.ParamByName["IDFLUXOCAIXA"] = Sangria.IDFluxoCaixa;
                oSQL.ParamByName["DATASANGRIA"] = Sangria.DataSangria;
                oSQL.ParamByName["VALOR"] = Sangria.Valor;
                oSQL.ParamByName["OBSERVACAO"] = Sangria.Observacao;
                return oSQL.ExecSQL() == 1;
            }
        }

        public static DataTable GetSangriasPorFluxoDeCaixa(decimal IDFluxoCaixa)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"SELECT * FROM SANGRIACAIXA WHERE IDFLUXOCAIXA = @IDFLUXOCAIXA";
                oSQL.ParamByName["IDFLUXOCAIXA"] = IDFluxoCaixa;
                oSQL.Open();
                return oSQL.dtDados;
            }
        }
    }
}
