using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kurcash.Models.DatabaseContextModels
{
    public partial class FlowerShopDBContext : DbContext
    {

        public static FlowerShopDBContext _context = new FlowerShopDBContext();
        public FlowerShopDBContext()
        {
        }
        public static FlowerShopDBContext GetContext()
        {
            return new FlowerShopDBContext();
        }
        public FlowerShopDBContext(DbContextOptions<FlowerShopDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bucket> Buckets { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Flower> Flowers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=FlowerShopDB;Username=postgres;Password=route");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bucket>(entity =>
            {
                entity.ToTable("Bucket");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreationDate)
                    .HasPrecision(6)
                    .HasColumnName("creation_date");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Flower>(entity =>
            {
                entity.ToTable("Flower");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.BucketId).HasColumnName("bucket_id");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasDefaultValueSql("pg_read_binary_file('D:\\image.png'::text)");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Bucket)
                    .WithMany(p => p.Flowers)
                    .HasForeignKey(d => d.BucketId)
                    .HasConstraintName("fk_flower_bucket");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Flowers)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("fk_flower_color");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Flowers)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_flower_order");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Flowers)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_flower_type");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.BucketId).HasColumnName("bucket_id");

                entity.Property(e => e.CreationDate)
                    .HasPrecision(6)
                    .HasColumnName("creation_date");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price");

                entity.HasOne(d => d.Bucket)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BucketId)
                    .HasConstraintName("fk_order_bucket");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname)
                    .HasColumnType("character varying")
                    .HasColumnName("fullname");

                entity.Property(e => e.Login)
                    .HasMaxLength(100)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(400)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasDefaultValueSql("2");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_bucket");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
