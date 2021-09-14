namespace MataFome_NET.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContextMataFome : DbContext
    {
        public ContextMataFome()
            : base("name=ContextMataFome")
        {
        }

        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<AdminUsers> AdminUsers { get; set; }
        public virtual DbSet<AppSettings> AppSettings { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<OrderProducts> OrderProducts { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Categories)
                .Map(m => m.ToTable("Products_Categories").MapLeftKey("CategoryID").MapRightKey("ProductID"));

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.OrderProducts)
                .WithRequired(e => e.Orders)
                .HasForeignKey(e => e.OrderID);
        }
    }
}
