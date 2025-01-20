using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Store.Models;

public partial class H60AssignmentDbCWContext : IdentityDbContext<IdentityUser>
{
    public H60AssignmentDbCWContext()
    {
    }

    public H60AssignmentDbCWContext(DbContextOptions<H60AssignmentDbCWContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasOne<IdentityUser>()
            .WithOne()
            .HasForeignKey<Customer>(c => c.UserId)
            .IsRequired();

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

            entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.ProdCat).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdCatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryID);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.ProdCat)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { CategoryID = 1, ProdCat = "Foil Boards" },
            new ProductCategory { CategoryID = 2, ProdCat = "Front Wings" },
            new ProductCategory { CategoryID = 3, ProdCat = "Stabilizers" },
            new ProductCategory { CategoryID = 4, ProdCat = "Fuselage" },
            new ProductCategory { CategoryID = 5, ProdCat = "Masts" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductID = 1, ProdCatId = 2, Description = "Sonar MA1850v2", Manufacturer = "North", Stock = 4, BuyPrice = 750.00m, SellPrice = 900.00m },
            new Product { ProductID = 2, ProdCatId = 2, Description = "G 1000 Front Wing V1", Manufacturer = "Slingshot", Stock = 8, BuyPrice = 550.00m, SellPrice = 630.00m },
            new Product { ProductID = 3, ProdCatId = 2, Description = "ART v2 999", Manufacturer = "Axis", Stock = 7, BuyPrice = 640.00m, SellPrice = 720.00m },
            new Product { ProductID = 4, ProdCatId = 1, Description = "Pump Foilboard 24L", Manufacturer = "Axis", Stock = 12, BuyPrice = 900.00m, SellPrice = 1090.00m },
            new Product { ProductID = 5, ProdCatId = 1, Description = "Puddle Pumper V1", Manufacturer = "Slingshot", Stock = 14, BuyPrice = 350.00m, SellPrice = 400.00m },
            new Product { ProductID = 6, ProdCatId = 1, Description = "SCOOP", Manufacturer = "North", Stock = 15, BuyPrice = 1200.00m, SellPrice = 1480.00m, Notes = "Test Note" },
            new Product { ProductID = 7, ProdCatId = 4, Description = "Black Standard Fuselage", Manufacturer = "Axis", Stock = 34, BuyPrice = 300.00m, SellPrice = 350.00m },
            new Product { ProductID = 8, ProdCatId = 4, Description = "Phantasm Fuselage", Manufacturer = "Slingshot", Stock = 47, BuyPrice = 190.00m, SellPrice = 240.00m },
            new Product { ProductID = 9, ProdCatId = 4, Description = "SONAR CARBON FUSELAGE", Manufacturer = "North", Stock = 23, BuyPrice = 360.00m, SellPrice = 400.00m },
            new Product { ProductID = 10, ProdCatId = 5, Description = "PRO Ultra High Modulus Carbon 800", Manufacturer = "Axis", Stock = 9, BuyPrice = 2550.00m, SellPrice = 2750.00m },
            new Product { ProductID = 11, ProdCatId = 5, Description = "Phantasm Carbon Mast V1.1", Manufacturer = "Slingshot", Stock = 11, BuyPrice = 1010.00m, SellPrice = 1160.00m },
            new Product { ProductID = 12, ProdCatId = 5, Description = "SONAR CF85", Manufacturer = "North", Stock = 18, BuyPrice = 1000.00m, SellPrice = 1150.00m },
            new Product { ProductID = 13, ProdCatId = 3, Description = "460 V2 Pump Carbon Rear Wing", Manufacturer = "Axis", Stock = 56, BuyPrice = 210.00m, SellPrice = 260.00m },
            new Product { ProductID = 14, ProdCatId = 3, Description = "Phantasm Stabilizer 340 Turbo-Tail V1", Manufacturer = "Slingshot", Stock = 46, BuyPrice = 195.00m, SellPrice = 230.00m },
            new Product { ProductID = 15, ProdCatId = 3, Description = "SONAR S320", Manufacturer = "North", Stock = 61, BuyPrice = 220.00m, SellPrice = 270.00m },
            new Product { ProductID = 16, ProdCatId = 1, Description = "POCKET CARBON CUSTOM", Manufacturer = "F-ONE", Stock = 4, BuyPrice = 850.00m, SellPrice = 1000.00m },
            new Product { ProductID = 17, ProdCatId = 2, Description = "SK8 Front Wing", Manufacturer = "F-ONE", Stock = 3, BuyPrice = 525.00m, SellPrice = 675.00m },
            new Product { ProductID = 18, ProdCatId = 4, Description = "FUSELAGE CARBON SHORT", Manufacturer = "F-ONE", Stock = 11, BuyPrice = 320.00m, SellPrice = 390.00m },
            new Product { ProductID = 19, ProdCatId = 5, Description = "CARBON MAST 16", Manufacturer = "F-ONE", Stock = 5, BuyPrice = 380.00m, SellPrice = 470.00m },
            new Product { ProductID = 20, ProdCatId = 3, Description = "MONOBLOC TAIL CARVING", Manufacturer = "F-ONE", Stock = 17, BuyPrice = 400.00m, SellPrice = 460.00m }
        );
        
        // modelBuilder.Entity<Customer>().HasData(
        //     new Customer() { CustomerId = 1, FirstName = "Ben", LastName = "Raymond", Email = "Ben@Raymond.com", PhoneNumber = "1231231234", Province = "QC", CreditCard = "1234123412341234", Password = "?Benjamin1!", UserId = ""},
        //     new Customer() { CustomerId = 2, FirstName = "Ryan", LastName = "Somers", Email = "Ryan@Somers.com", PhoneNumber = "3213214321", Province = "QC", CreditCard = "4321432143214321", Password = "?Ryan1!", UserId = ""},
        //     new Customer() { CustomerId = 3, FirstName = "Matteo", LastName = "Falasconi", Email = "Matteo@Falasconi.com", PhoneNumber = "1234567890", Province = "QC", CreditCard = "1234567812345678", Password = "?Matteo1!", UserId = "" }
        // );

        // modelBuilder.Entity<Order>().HasData(
        //     new Order() { OrderId = 1, CustomerId = 1, DateCreated = new DateTime(2024, 8, 13), DateFulfilled = new DateTime(2024, 8, 18), Total = (decimal)2190.00, Taxes = (decimal)344.43 },
        //     new Order() { OrderId = 2, CustomerId = 3, DateCreated = new DateTime(2024, 7, 19), DateFulfilled = new DateTime(2024, 7, 27), Total = (decimal)1150.00, Taxes = (decimal)501.66 }
        // );

        // modelBuilder.Entity<OrderItem>().HasData(
        //     new OrderItem() { OrderItemId = 1, OrderId = 1, ProductId = 5, Quantity = 1, Price = (decimal)400.00 },
        //     new OrderItem() { OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = (decimal)630.00 },
        //     new OrderItem() { OrderItemId = 3, OrderId = 1, ProductId = 11, Quantity = 1, Price = (decimal)1160.00 },
        //     new OrderItem() { OrderItemId = 4, OrderId = 2, ProductId = 12, Quantity = 1, Price = (decimal)1150.00 }
        // );

        // modelBuilder.Entity<ShoppingCart>().HasData(
        //     new ShoppingCart() { CartId = 1, CustomerId = 2, DateCreated = new DateTime(2024, 9, 10) }
        // );

        // modelBuilder.Entity<CartItem>().HasData(
        //     new CartItem() { CartItemId = 1, CartId = 1, ProductId = 6, Quantity = 1, Price = (decimal)1480.00},
        //     new CartItem() { CartItemId = 2, CartId = 1, ProductId = 15, Quantity = 3, Price = (decimal)270.00}
        // );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
