﻿using webshop.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace webshop.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(c => c.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(c => c.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(c => c.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(c => c.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            
            b.HasOne(c => c.Customer);
        });

        modelBuilder.Entity<Customer>(b =>
        {
          b.HasOne(c => c.User);
          b.HasOne(c => c.SavedAddress);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.HasOne(o => o.Customer);
            b.HasOne(o => o.BillingAddress);
            b.HasOne(o => o.DeliveryAddress);
            b.HasMany(o => o.Products);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.HasKey(o => new { o.OrderId, o.ProductId});
        });

        
        modelBuilder.Entity<Product>().HasData(
          new Product() { Id = 1, Name = "Nike Sneakers",Price= 700, Description= "Freshen Up Your Look At Nike!", ParentCategoryId =1 },
          new Product() { Id = 2, Name = "Lee Cooper", Price = 700, Description = "Freshen Up Your Look !" , ParentCategoryId = 1 },
          new Product() { Id = 3, Name = "Red tape", Price = 700  , Description = "better quality!", ParentCategoryId = 1 },
           new Product() { Id = 4, Name = "Belts", Price = 400, Description = "Smart Look !", ParentCategoryId = 2 },
          new Product() { Id = 5, Name = "Perfume", Price = 550, Description = " Perfume !", ParentCategoryId = 2 },
          new Product() { Id = 6, Name = "Sunglasses", Price = 1200, Description = "better quality!", ParentCategoryId = 2 },
           new Product() { Id = 7, Name = "Jackets", Price = 700, Description = "Freshen Up Your Look At Nike!", ParentCategoryId = 3 },
          new Product() { Id = 8, Name = "Pants", Price = 700, Description = "Perfume !", ParentCategoryId = 3 },
          new Product() { Id = 9, Name = "Shirts", Price = 700, Description = "better quality!", ParentCategoryId = 3 }

        //  ProductCategories =  (ICollection<ProductCategory>) 
        //new List<ProductCategory>() { new ProductCategory() { CategoryId = 3, ProductId = 1, Relevance = 100 } } }
        );

            //modelBuilder.Entity<ProductCategory>().HasData(
            //  new ProductCategory() { CategoryId = 3, ProductId = 1, Relevance = 100 }
            //);

            //modelBuilder.Entity<Category>(b =>
            //{
            //    //b.HasOne(c => c.ParentCategory).WithMany(c => c.SubCategories);
            //    b.HasMany<Category>(c => c.SubCategories).WithOne(c => c.ParentCategory);
            //    b.HasMany(c => c.Products).WithOne(pc => pc.Category).HasForeignKey(pc => pc.CategoryId);
            //});
            modelBuilder.Entity<Category>().HasData(
          new Category() { Id = 1, CategoryName = "Shoes", ParentCategoryId = 1 },
          new Category() { Id = 2, CategoryName = "Accessories", ParentCategoryId = 2 },
          new Category() { Id = 3, CategoryName = "Clothes", ParentCategoryId = 3 }
        );

        modelBuilder.Entity<ProductCategory>(b =>
        {
            b.HasKey(pc => new { pc.CategoryId, pc.ProductId});
        });
    }

    public DbSet<Address> Addresses { get; set; }
    //public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<webshop.Models.ProductDetails> ProductDetails { get; set; }
    //public DbSet<Store> Stores { get; set; }
  }
} 
