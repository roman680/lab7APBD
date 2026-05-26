using lab7.Models;
using Microsoft.EntityFrameworkCore;

namespace lab7.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PC> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PC>(entity =>
        {
            entity.ToTable("PCs");
            entity.HasKey(pc => pc.Id);

            entity.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(pc => pc.Weight)
                .IsRequired()
                .HasColumnType("float");

            entity.Property(pc => pc.Warranty)
                .IsRequired();

            entity.Property(pc => pc.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(pc => pc.Stock)
                .IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.ToTable("Components");
            entity.HasKey(component => component.Code);

            entity.Property(component => component.Code)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnType("char(10)");

            entity.Property(component => component.Name)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(component => component.Description)
                .HasColumnType("nvarchar(max)");

            entity.Property(component => component.ComponentManufacturerId)
                .IsRequired();

            entity.Property(component => component.ComponentTypeId)
                .IsRequired();

            entity.HasOne(component => component.ComponentManufacturer)
                .WithMany(manufacturer => manufacturer.Components)
                .HasForeignKey(component => component.ComponentManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(component => component.ComponentType)
                .WithMany(type => type.Components)
                .HasForeignKey(component => component.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.ToTable("ComponentTypes");
            entity.HasKey(type => type.Id);

            entity.Property(type => type.Abbreviation)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(type => type.Name)
                .IsRequired()
                .HasMaxLength(150);
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.ToTable("ComponentManufacturers");
            entity.HasKey(manufacturer => manufacturer.Id);

            entity.Property(manufacturer => manufacturer.Abbreviation)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(manufacturer => manufacturer.FullName)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(manufacturer => manufacturer.FoundationDate)
                .IsRequired()
                .HasColumnType("date");
        });

        modelBuilder.Entity<PCComponent>(entity =>
        {
            entity.ToTable("PCComponents");
            entity.HasKey(pcComponent => new { pcComponent.PCId, pcComponent.ComponentCode });

            entity.Property(pcComponent => pcComponent.ComponentCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnType("char(10)");

            entity.Property(pcComponent => pcComponent.Amount)
                .IsRequired();

            entity.HasOne(pcComponent => pcComponent.PC)
                .WithMany(pc => pc.PCComponents)
                .HasForeignKey(pcComponent => pcComponent.PCId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pcComponent => pcComponent.Component)
                .WithMany(component => component.PCComponents)
                .HasForeignKey(pcComponent => pcComponent.ComponentCode)
                .OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer
            {
                Id = 1,
                Abbreviation = "INTEL",
                FullName = "Intel Corporation",
                FoundationDate = new DateOnly(1968, 7, 18)
            },
            new ComponentManufacturer
            {
                Id = 2,
                Abbreviation = "NVIDIA",
                FullName = "NVIDIA Corporation",
                FoundationDate = new DateOnly(1993, 4, 5)
            },
            new ComponentManufacturer
            {
                Id = 3,
                Abbreviation = "KSTON",
                FullName = "Kingston Technology Company",
                FoundationDate = new DateOnly(1987, 10, 17)
            }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC
            {
                Id = 1,
                Name = "Gaming Beast X",
                Weight = 12.5,
                Warranty = 36,
                CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),
                Stock = 5
            },
            new PC
            {
                Id = 2,
                Name = "Office Mini Pro",
                Weight = 4.2,
                Warranty = 24,
                CreatedAt = new DateTime(2026, 5, 9, 10, 30, 0),
                Stock = 12
            },
            new PC
            {
                Id = 3,
                Name = "Creator Workstation",
                Weight = 15.8,
                Warranty = 48,
                CreatedAt = new DateTime(2026, 5, 10, 14, 15, 0),
                Stock = 3
            }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component
            {
                Code = "CPU-I7001X",
                Name = "Intel Core i7-14700K",
                Description = "High performance desktop processor",
                ComponentManufacturerId = 1,
                ComponentTypeId = 1
            },
            new Component
            {
                Code = "GPU-RTX01X",
                Name = "NVIDIA GeForce RTX 4070",
                Description = "Graphics card for gaming and creative work",
                ComponentManufacturerId = 2,
                ComponentTypeId = 2
            },
            new Component
            {
                Code = "RAM-32GB1X",
                Name = "Kingston Fury 32GB DDR5",
                Description = "DDR5 memory kit",
                ComponentManufacturerId = 3,
                ComponentTypeId = 3
            }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "CPU-I7001X", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "GPU-RTX01X", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "RAM-32GB1X", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "CPU-I7001X", Amount = 1 },
            new PCComponent { PCId = 3, ComponentCode = "GPU-RTX01X", Amount = 2 }
        );
    }
}
