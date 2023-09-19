using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OperationAPI.Entities;

namespace OperationAPI.Data;

public partial class OperationDbContext : DbContext
{
    public OperationDbContext()
    {
    }

    public OperationDbContext(DbContextOptions<OperationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<OperationPlanned> OperationPlanneds { get; set; }

    public virtual DbSet<OperationStarted> OperationStarteds { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operation>(entity =>
        {
            entity.ToTable("Operation", "HH");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OperationPlanned>(entity =>
        {
            entity.ToTable("OperationPlanned", "HH");

            entity.HasOne(d => d.Operation).WithMany(p => p.OperationPlanneds)
                .HasForeignKey(d => d.OperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationPlanned_Operation");
        });

        modelBuilder.Entity<OperationStarted>(entity =>
        {
            entity.ToTable("OperationStarted", "HH");

            entity.HasOne(d => d.OperationPlanned).WithMany(p => p.OperationStarteds)
                .HasForeignKey(d => d.OperationPlannedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationStarted_Operation");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
