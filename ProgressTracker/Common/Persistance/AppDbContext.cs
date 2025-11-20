using System;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(@"
                Server=localhost\sqlexpress;
                Database=PTDB;
                User Id=asdf;
                Password=asdf;
                TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "adminpass",
                FirstName = "Admini",
                LastName = "Strator"
            });

        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany()
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Members)
            .WithMany()
            .UsingEntity<ProjectMember>(
                pm => pm
                        .HasOne(pm => pm.User)
                        .WithMany()
                        .HasForeignKey(pm => pm.UserId),
                pm => pm
                        .HasOne(pm => pm.Project)
                        .WithMany()
                        .HasForeignKey(pm => pm.ProjectId),
                pm =>
                    pm.HasKey(t => new { t.ProjectId, t.UserId })
            );
    }
}
