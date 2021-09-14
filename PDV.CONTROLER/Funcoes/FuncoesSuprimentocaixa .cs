using System;
using System.Data;
using PDV.DAO.DB.Controller;
using PDV.DAO.Entidades;

namespace PDV.CONTROLER.Funcoes
{
    public class FuncoesSuprimentoCaixa
    {
        public static bool Salvar(SuprimentoCaixa suprimento)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"INSERT INTO SUPRIMENTOCAIXA 
                               (IDSUPRIMENTOCAIXA, IDUSUARIO, IDUSUARIOCADASTRO, IDFLUXOCAIXA, DATASUPRIMENTOCAIXA, VALOR, OBSERVACAO)
                             VALUES
                               (@IDSUPRIMENTOCAIXA, @IDUSUARIO, @IDUSUARIOCADASTRO, @IDFLUXOCAIXA, @DATASUPRIMENTOCAIXA, @VALOR, @OBSERVACAO)";
                oSQL.ParamByName["IDSUPRIMENTOCAIXA"] = suprimento.IDSuprimentocaixa;
                oSQL.ParamByName["IDUSUARIO"] = suprimento.IDUsuario;
                oSQL.ParamByName["IDUSUARIOCADASTRO"] = suprimento.IDUsuarioCadastro;
                oSQL.ParamByName["IDFLUXOCAIXA"] = suprimento.IDFluxoCaixa;
                oSQL.ParamByName["DATASUPRIMENTOCAIXA"] = suprimento.DataSuprimentocaixa;
                oSQL.ParamByName["VALOR"] = suprimento.Valor;
                oSQL.ParamByName["OBSERVACAO"] = suprimento.Observacao;
                return oSQL.ExecSQL() == 1;
            }
        }

        public static DataTable GetSuprimentoPorFluxoDeCaixa(decimal IDFluxoCaixa)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"SELECT * FROM SUPRIMENTOCAIXA WHERE IDFLUXOCAIXA = @IDFLUXOCAIXA";
                oSQL.ParamByName["IDFLUXOCAIXA"] = IDFluxoCaixa;
                oSQL.Open();
                return oSQL.dtDados;
            }
        }
    }
}
