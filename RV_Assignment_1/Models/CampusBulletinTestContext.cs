using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RV_Assignment_1.Models;

public partial class CampusBulletinTestContext : DbContext
{
    public CampusBulletinTestContext()
    {
    }

    public CampusBulletinTestContext(DbContextOptions<CampusBulletinTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BulletinCategory> BulletinCategories { get; set; }

    public virtual DbSet<BulletinPost> BulletinPosts { get; set; }

    public virtual DbSet<MemberAccount> MemberAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BulletinCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Bulletin__19093A0BCA036B4A");

            entity.Property(e => e.CategoryName).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<BulletinPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Bulletin__AA12601825F8A259");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Source).HasMaxLength(250);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(150);

            entity.HasOne(d => d.Category).WithMany(p => p.BulletinPosts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BulletinPost_Category");
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__MemberAc__349DA5A6164A8FB6");

            entity.HasIndex(e => e.Email, "UQ__MemberAc__A9D105345F973CC5").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
