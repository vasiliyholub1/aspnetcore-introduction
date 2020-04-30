﻿using AspNetCore.Introduction.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Introduction.Repositories
{
    public partial class AspNetCoreIntroductionContext : DbContext
    {
        public AspNetCoreIntroductionContext()
        {
        }

        public AspNetCoreIntroductionContext(DbContextOptions<AspNetCoreIntroductionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Territories> Territories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Categories>(entity =>
            //{
            //    entity.HasKey(x => x.CategoryId);

            //    entity.HasIndex(x => x.CategoryName)
            //        .HasName("CategoryName");

            //    entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            //    entity.Property(e => e.CategoryName)
            //        .IsRequired()
            //        .HasMaxLength(15);

            //    entity.Property(e => e.Description).HasColumnType("ntext");

            //    entity.Property(e => e.Picture).HasColumnType("image");
            //});

            //modelBuilder.Entity<Customers>(entity =>
            //{
            //    entity.HasKey(x => x.CustomerId);

            //    entity.HasIndex(x => x.City)
            //        .HasName("City");

            //    entity.HasIndex(x => x.CompanyName)
            //        .HasName("CompanyName");

            //    entity.HasIndex(x => x.PostalCode)
            //        .HasName("PostalCode");

            //    entity.HasIndex(x => x.Region)
            //        .HasName("Region");

            //    entity.Property(e => e.CustomerId)
            //        .HasColumnName("CustomerID")
            //        .HasMaxLength(5)
            //        .IsFixedLength();

            //    entity.Property(e => e.Address).HasMaxLength(60);

            //    entity.Property(e => e.City).HasMaxLength(15);

            //    entity.Property(e => e.CompanyName)
            //        .IsRequired()
            //        .HasMaxLength(40);

            //    entity.Property(e => e.ContactName).HasMaxLength(30);

            //    entity.Property(e => e.ContactTitle).HasMaxLength(30);

            //    entity.Property(e => e.Country).HasMaxLength(15);

            //    entity.Property(e => e.Fax).HasMaxLength(24);

            //    entity.Property(e => e.Founded).HasColumnType("date");

            //    entity.Property(e => e.Phone).HasMaxLength(24);

            //    entity.Property(e => e.PostalCode).HasMaxLength(10);

            //    entity.Property(e => e.Region).HasMaxLength(15);
            //});

            modelBuilder.Entity<EmployeeTerritories>(entity =>
            {
                entity.HasKey(x => new { x.EmployeeId, x.TerritoryId })
                    .IsClustered(false);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("TerritoryID")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Employees");

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(x => x.TerritoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Territories");
            });

            //modelBuilder.Entity<Employees>(entity =>
            //{
            //    entity.HasKey(x => x.EmployeeId);

            //    entity.HasIndex(x => x.LastName)
            //        .HasName("LastName");

            //    entity.HasIndex(x => x.PostalCode)
            //        .HasName("PostalCode");

            //    entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            //    entity.Property(e => e.Address).HasMaxLength(60);

            //    entity.Property(e => e.BirthDate).HasColumnType("datetime");

            //    entity.Property(e => e.City).HasMaxLength(15);

            //    entity.Property(e => e.Country).HasMaxLength(15);

            //    entity.Property(e => e.Extension).HasMaxLength(4);

            //    entity.Property(e => e.FirstName)
            //        .IsRequired()
            //        .HasMaxLength(10);

            //    entity.Property(e => e.HireDate).HasColumnType("datetime");

            //    entity.Property(e => e.HomePhone).HasMaxLength(24);

            //    entity.Property(e => e.LastName)
            //        .IsRequired()
            //        .HasMaxLength(20);

            //    entity.Property(e => e.Notes).HasColumnType("ntext");

            //    entity.Property(e => e.Photo).HasColumnType("image");

            //    entity.Property(e => e.PhotoPath).HasMaxLength(255);

            //    entity.Property(e => e.PostalCode).HasMaxLength(10);

            //    entity.Property(e => e.Region).HasMaxLength(15);

            //    entity.Property(e => e.Title).HasMaxLength(30);

            //    entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);

            //    entity.HasOne(d => d.ReportsToNavigation)
            //        .WithMany(p => p.InverseReportsToNavigation)
            //        .HasForeignKey(x => x.ReportsTo)
            //        .HasConstraintName("FK_Employees_Employees");
            //});

            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Invoices");

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.ExtendedPrice).HasColumnType("money");

                entity.Property(e => e.Freight).HasColumnType("money");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Region).HasMaxLength(15);

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.Salesperson)
                    .IsRequired()
                    .HasMaxLength(31);

                entity.Property(e => e.ShipAddress).HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                entity.Property(e => e.ShipName).HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.Property(e => e.ShipperName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(x => new { x.OrderId, x.ProductId })
                    .HasName("PK_Order_Details");

                entity.ToTable("Order Details");

                entity.HasIndex(x => x.OrderId)
                    .HasName("OrdersOrder_Details");

                entity.HasIndex(x => x.ProductId)
                    .HasName("ProductsOrder_Details");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(x => x.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Products");
            });

            //modelBuilder.Entity<Orders>(entity =>
            //{
            //    entity.HasKey(x => x.OrderId);

            //    entity.HasIndex(x => x.CustomerId)
            //        .HasName("CustomersOrders");

            //    entity.HasIndex(x => x.EmployeeId)
            //        .HasName("EmployeesOrders");

            //    entity.HasIndex(x => x.OrderDate)
            //        .HasName("OrderDate");

            //    entity.HasIndex(x => x.ShipPostalCode)
            //        .HasName("ShipPostalCode");

            //    entity.HasIndex(x => x.ShipVia)
            //        .HasName("ShippersOrders");

            //    entity.HasIndex(x => x.ShippedDate)
            //        .HasName("ShippedDate");

            //    entity.Property(e => e.OrderId).HasColumnName("OrderID");

            //    entity.Property(e => e.CustomerId)
            //        .HasColumnName("CustomerID")
            //        .HasMaxLength(5)
            //        .IsFixedLength();

            //    entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            //    entity.Property(e => e.Freight)
            //        .HasColumnType("money")
            //        .HasDefaultValueSql("((0))");

            //    entity.Property(e => e.OrderDate).HasColumnType("datetime");

            //    entity.Property(e => e.RequiredDate).HasColumnType("datetime");

            //    entity.Property(e => e.ShipAddress).HasMaxLength(60);

            //    entity.Property(e => e.ShipCity).HasMaxLength(15);

            //    entity.Property(e => e.ShipCountry).HasMaxLength(15);

            //    entity.Property(e => e.ShipName).HasMaxLength(40);

            //    entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

            //    entity.Property(e => e.ShipRegion).HasMaxLength(15);

            //    entity.Property(e => e.ShippedDate).HasColumnType("datetime");

            //    entity.HasOne(d => d.Customer)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(x => x.CustomerId)
            //        .HasConstraintName("FK_Orders_Customers");

            //    entity.HasOne(d => d.Employee)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(x => x.EmployeeId)
            //        .HasConstraintName("FK_Orders_Employees");

            //    entity.HasOne(d => d.ShipViaNavigation)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(x => x.ShipVia)
            //        .HasConstraintName("FK_Orders_Shippers");
            //});

            //modelBuilder.Entity<Products>(entity =>
            //{
            //    entity.HasKey(x => x.ProductId);

            //    entity.HasIndex(x => x.CategoryId)
            //        .HasName("CategoryID");

            //    entity.HasIndex(x => x.ProductName)
            //        .HasName("ProductName");

            //    entity.HasIndex(x => x.SupplierId)
            //        .HasName("SuppliersProducts");

            //    entity.Property(e => e.ProductId).HasColumnName("ProductID");

            //    entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            //    entity.Property(e => e.ProductName)
            //        .IsRequired()
            //        .HasMaxLength(40);

            //    entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);

            //    entity.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

            //    entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            //    entity.Property(e => e.UnitPrice)
            //        .HasColumnType("money")
            //        .HasDefaultValueSql("((0))");

            //    entity.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

            //    entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

            //    entity.HasOne(d => d.Category)
            //        .WithMany(p => p.Products)
            //        .HasForeignKey(x => x.CategoryId)
            //        .HasConstraintName("FK_Products_Categories");

            //    entity.HasOne(d => d.Supplier)
            //        .WithMany(p => p.Products)
            //        .HasForeignKey(x => x.SupplierId)
            //        .HasConstraintName("FK_Products_Suppliers");
            //});

            //modelBuilder.Entity<Regions>(entity =>
            //{
            //    entity.HasKey(x => x.RegionId)
            //        .HasName("PK_Region")
            //        .IsClustered(false);

            //    entity.Property(e => e.RegionId)
            //        .HasColumnName("RegionID")
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.RegionDescription)
            //        .IsRequired()
            //        .HasMaxLength(50)
            //        .IsFixedLength();
            //});

            //modelBuilder.Entity<Shippers>(entity =>
            //{
            //    entity.HasKey(x => x.ShipperId);

            //    entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

            //    entity.Property(e => e.CompanyName)
            //        .IsRequired()
            //        .HasMaxLength(40);

            //    entity.Property(e => e.Phone).HasMaxLength(24);
            //});

            //modelBuilder.Entity<Suppliers>(entity =>
            //{
            //    entity.HasKey(x => x.SupplierId);

            //    entity.HasIndex(x => x.CompanyName)
            //        .HasName("CompanyName");

            //    entity.HasIndex(x => x.PostalCode)
            //        .HasName("PostalCode");

            //    entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            //    entity.Property(e => e.Address).HasMaxLength(60);

            //    entity.Property(e => e.City).HasMaxLength(15);

            //    entity.Property(e => e.CompanyName)
            //        .IsRequired()
            //        .HasMaxLength(40);

            //    entity.Property(e => e.ContactName).HasMaxLength(30);

            //    entity.Property(e => e.ContactTitle).HasMaxLength(30);

            //    entity.Property(e => e.Country).HasMaxLength(15);

            //    entity.Property(e => e.Fax).HasMaxLength(24);

            //    entity.Property(e => e.HomePage).HasColumnType("ntext");

            //    entity.Property(e => e.Phone).HasMaxLength(24);

            //    entity.Property(e => e.PostalCode).HasMaxLength(10);

            //    entity.Property(e => e.Region).HasMaxLength(15);
            //});

            //modelBuilder.Entity<Territories>(entity =>
            //{
            //    entity.HasKey(x => x.TerritoryId)
            //        .IsClustered(false);

            //    entity.Property(e => e.TerritoryId)
            //        .HasColumnName("TerritoryID")
            //        .HasMaxLength(20);

            //    entity.Property(e => e.RegionId).HasColumnName("RegionID");

            //    entity.Property(e => e.TerritoryDescription)
            //        .IsRequired()
            //        .HasMaxLength(50)
            //        .IsFixedLength();

            //    entity.HasOne(d => d.Region)
            //        .WithMany(p => p.Territories)
            //        .HasForeignKey(x => x.RegionId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Territories_Region");
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}