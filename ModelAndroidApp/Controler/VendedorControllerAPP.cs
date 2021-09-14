using ModelAndroidApp.ModelAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndroidApp.Controler
{
    
    public static class VendedorControllerAPP
    {
       
        public static bool SalvarVendedor(Vendedor vendedor)
        {
            try
            {
                using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
                {
                    db.Vendedor.Add(vendedor);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
          
            return true;
        }
        public static void ExcluirVendedor()
        {
            using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
            {
                db.Database.ExecuteSqlCommand("Delete Vendedor");
            }
        }

    }
}
