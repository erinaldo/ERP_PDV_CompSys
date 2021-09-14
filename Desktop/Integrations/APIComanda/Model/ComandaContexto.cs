namespace APIComanda.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ComandaContexto : DbContext
    {
        public ComandaContexto()
            : base("name=ComandaContexto")
        {
        }

        public virtual DbSet<PDVCliente> PDVCliente { get; set; }
        public virtual DbSet<PDVMesa> PDVMesa { get; set; }
        public virtual DbSet<PDVPedido> PDVPedido { get; set; }
        public virtual DbSet<PDVPedidoItem> PDVPedidoItem { get; set; }
        public virtual DbSet<PDVProduto> PDVProduto { get; set; }
        public virtual DbSet<PDVVendedor> PDVVendedor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
