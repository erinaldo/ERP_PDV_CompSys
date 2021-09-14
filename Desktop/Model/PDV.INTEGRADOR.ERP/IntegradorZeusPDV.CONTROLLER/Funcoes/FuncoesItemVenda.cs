using IntegradorZeusPDV.DB;
using IntegradorZeusPDV.DB.DB.Controller;
using IntegradorZeusPDV.MODEL.ClassesPDV;
using System.Collections.Generic;

namespace IntegradorZeusPDV.CONTROLLER.Funcoes
{
    public class FuncoesItemVenda
    {
        public static List<ItemVenda> GetItens(decimal IDVenda)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"SELECT * FROM ITEMVENDA WHERE IDVENDA = @IDVENDA";
                oSQL.ParamByName["IDVENDA"] = IDVenda;
                oSQL.Open();
                return EntityUtil<ItemVenda>.ParseDataTable(oSQL.dtDados);
            }
        }
    }
}
