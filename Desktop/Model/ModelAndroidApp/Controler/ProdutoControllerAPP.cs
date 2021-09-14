using ModelAndroidApp.ModelAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndroidApp.Controler
{
    public  static class ProdutoControllerAPP
    {
        public  static bool SalvarProdutoAPP(Produto produto)
        {
            try
            {
                using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
                {
                    db.Produto.Add(produto);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool SalvarEstoqueAPP(Estoque estoque)
        {
            try
            {
                using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
                {
                    db.Estoque.Add(estoque);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void ExcluirEstoque()
        {
            using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
            {
                db.Database.ExecuteSqlCommand("Delete Estoque");
            }
        }
        public static void ExcluirProduto()
        {
            using (ModelAndroidApp.ModelAndroid.ModelDadosApp db = new ModelAndroidApp.ModelAndroid.ModelDadosApp())
            {
                db.Database.ExecuteSqlCommand("Delete Produto");
            }
        }
    }
}
