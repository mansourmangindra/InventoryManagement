using System;
using System.Collections.Generic;
using DataAccess.DbContexts.InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement;

public partial class InventoryManagementDbContext : DbContext
{
    //public InventoryManagementDbContext()
    //{
    //}

    public InventoryManagementDbContext(DbContextOptions<InventoryManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<AuditTrailDetail> AuditTrailDetails { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleModule> RoleModules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=localhost;Database=InventoryManagement;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTrailDetail>(entity =>
        {
            entity.HasOne(d => d.AuditTrail).WithMany(p => p.AuditTrailDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditTrailDetail_AuditTrail");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(e => e.ModuleId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).ValueGeneratedNever();
        });

        modelBuilder.Entity<RoleModule>(entity =>
        {
            entity.Property(e => e.RoleModuleId).ValueGeneratedNever();

            entity.HasOne(d => d.Module).WithMany(p => p.RoleModules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModule_Module");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
