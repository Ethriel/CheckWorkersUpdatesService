﻿using CheckWorkers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CheckWorkers.Entity
{
    public partial class WorkerCompanyPetContext : DbContext
    {
        public WorkerCompanyPetContext()
        {
        }

        public WorkerCompanyPetContext(DbContextOptions<WorkerCompanyPetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Worker> Worker { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionStrings.Default);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(500);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.Property(e => e.Dob)
                      .HasColumnName("DOB")
                      .HasColumnType("datetime");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(75);

                entity.Property(e => e.TimeUpdated)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Company)
                      .WithMany(p => p.Workers)
                      .HasForeignKey(d => d.CompanyId)
                      .HasConstraintName("FK__Worker__CompanyI__276EDEB3");
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}