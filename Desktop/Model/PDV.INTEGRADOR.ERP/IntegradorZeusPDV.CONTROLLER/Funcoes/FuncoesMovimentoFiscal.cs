using IntegradorZeusPDV.DB;
using IntegradorZeusPDV.DB.DB.Controller;
using IntegradorZeusPDV.MODEL.ClassesPDV;

namespace IntegradorZeusPDV.CONTROLLER.Funcoes
{
    public class FuncoesMovimentoFiscal
    {
        public static MovimentoFiscal GetMovimentoFiscalPorVenda(decimal IDVenda)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM MOVIMENTOFISCAL WHERE IDVENDA = @IDVENDA";
                oSQL.ParamByName["IDVENDA"] = IDVenda;
                oSQL.Open();
                return EntityUtil<MovimentoFiscal>.ParseDataRow(oSQL.dtDados.Rows[0]);
            }
        }
        
    }
}
