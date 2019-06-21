﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestProject.Data.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<BillingAddresses> BillingAddresses { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OrdersDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Articles>(entity =>
            {
                entity.HasIndex(e => e.OrderOxId);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.OrderOx)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.OrderOxId);
            });

            modelBuilder.Entity<BillingAddresses>(entity =>
            {
                entity.HasIndex(e => e.OrderOxId)
                    .IsUnique();

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.OrderOx)
                    .WithOne(p => p.BillingAddresses)
                    .HasForeignKey<BillingAddresses>(d => d.OrderOxId);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OxId);

                entity.Property(e => e.OxId).ValueGeneratedNever();

                entity.Property(e => e.OrderDatetime).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasIndex(e => e.OrderOxId);

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.OrderOx)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderOxId);
            });
        }
    }
}
