using System;
using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public partial class ProjectbootcampContext : DbContext
{
    public ProjectbootcampContext()
    {
    }

    public ProjectbootcampContext(DbContextOptions<ProjectbootcampContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MstUser> MstUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mst_user_pkey");

            entity.ToTable("mst_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Nama)
                .HasMaxLength(30)
                .HasColumnName("nama");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
