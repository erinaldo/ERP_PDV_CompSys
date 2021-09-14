

using MataFome_NET.Model;
using PDV.DAO.Entidades;

namespace MataFome_NET.Controller
{
    public class ProdutoController
    {
        public static ContextMataFome contextMataFome;
        static ProdutoController()
        {

            contextMataFome = new ContextMataFome();
        }
        public static void AtualizarProdutos(Produto produto)
        {
            try
            {
                Products products = new Products()
                {
                    ExternalID = produto.Codigo,
                    Name = produto.Descricao,
                    Description = produto.Descricao,
                    Price = produto.ValorVenda,
                    ImageLink = produto.ImagemProdutoLink,
                    Active = true
                };
                contextMataFome.Products.Add(products);
                contextMataFome.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static void AtualizarCategorias(Categoria categoria)
        {
            try
            {
                Categories categories = new Categories()
                {
                    Name = categoria.Descricao,
                };

            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

}