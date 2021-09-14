using IntegradorZeusPDV.DB;
using IntegradorZeusPDV.DB.DB.Controller;
using IntegradorZeusPDV.MODEL.ClassesPDV;
using System.Collections.Generic;
using System.Data;

namespace IntegradorZeusPDV.CONTROLLER.Funcoes
{
    public class FuncoesVenda
    {

        public static List<Venda> GetVendasDoDia()
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                //oSQL.SQL = @"SELECT *
                //               FROM VENDA
                //             WHERE DATACADASTRO::TIMESTAMP = CURRENT_DATE ";
                oSQL.SQL = @"SELECT *
                               FROM VENDA
                               ORDER BY IDVENDA
                               LIMIT 10";
                oSQL.Open();
                return EntityUtil<Venda>.ParseDataTable(oSQL.dtDados);
            }
        }
        
    }
}
