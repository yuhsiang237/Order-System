using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OrderSystem.Models
{
    public partial class OrderSystemContext : DbContext
    {
        public OrderSystemContext()
        {
        }

        public OrderSystemContext(DbContextOptions<OrderSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductInventory> ProductInventories { get; set; }
        public virtual DbSet<ProductProductCategoryRelationship> ProductProductCategoryRelationships { get; set; }
        public virtual DbSet<ReturnShipmentOrder> ReturnShipmentOrders { get; set; }
        public virtual DbSet<ReturnShipmentOrderDetail> ReturnShipmentOrderDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ShipmentOrder> ShipmentOrders { get; set; }
        public virtual DbSet<ShipmentOrderDetail> ShipmentOrderDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=OrderSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(200);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentUnit).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Number).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {

                entity.ToTable("ProductCategory");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ProductInventory>(entity =>
            {
                entity.ToTable("ProductInventory");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Unit).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<ProductProductCategoryRelationship>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ReturnShipmentOrder>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Remarks).HasMaxLength(500);
            });

            modelBuilder.Entity<ReturnShipmentOrderDetail>(entity =>
            {
                entity.Property(e => e.Unit).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ShipmentOrder>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SignName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ShipmentOrderDetail>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.ProductNumber).HasMaxLength(100);

                entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ProductUnit).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Account).HasMaxLength(100);

                entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.Salt)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
