using ModelAndroidApp.ModelAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndroidApp.Controler
{
    public  static class EmitenteControllerAPP
    {
        public  static bool SalvarEmpresaAPP(Empresa empresa)
        {
            try
            {
                using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
                {
                    db.Empresa.Add(empresa);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void ExcluirEmresa()
        {
            using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
            {
                db.Database.ExecuteSqlCommand("Delete Empresa");
            }
        }
    }
}
