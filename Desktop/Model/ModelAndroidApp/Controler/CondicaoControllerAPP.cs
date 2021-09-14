using ModelAndroidApp.ModelAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndroidApp.Controler
{
    
    public static class CondicaoControllerAPP
    {
       
        public static bool SalvarCondicao(Condicao forma)
        {
            try
            {
                using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
                {
                    db.Condicao.Add(forma);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
          
            return true;
        }
        public static void ExcluirCondicao()
        {
            using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
            {
                db.Database.ExecuteSqlCommand("Delete Condicao");
            }
        }

    }
}
