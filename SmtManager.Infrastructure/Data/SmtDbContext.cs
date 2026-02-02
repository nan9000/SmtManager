using Microsoft.EntityFrameworkCore;
using SmtManager.Core.Entities;

namespace SmtManager.Infrastructure.Data;

public class SmtDbContext : DbContext
{
    public SmtDbContext(DbContextOptions<SmtDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<OrderBoard> OrderBoards { get; set; }
    public DbSet<BoardComponent> BoardComponents { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureOrderBoard(modelBuilder);
        ConfigureBoardComponent(modelBuilder);
        ConfigureUser(modelBuilder);
    }

    private void ConfigureOrderBoard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderBoard>()
            .HasKey(ob => new { ob.OrderId, ob.BoardId });

        modelBuilder.Entity<OrderBoard>()
            .HasOne(ob => ob.Order)
            .WithMany(o => o.OrderBoards)
            .HasForeignKey(ob => ob.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderBoard>()
            .HasOne(ob => ob.Board)
            .WithMany(b => b.OrderBoards)
            .HasForeignKey(ob => ob.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureBoardComponent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardComponent>()
            .HasKey(bc => new { bc.BoardId, bc.ComponentId });

        modelBuilder.Entity<BoardComponent>()
            .HasOne(bc => bc.Board)
            .WithMany(b => b.BoardComponents)
            .HasForeignKey(bc => bc.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardComponent>()
            .HasOne(bc => bc.Component)
            .WithMany(c => c.BoardComponents)
            .HasForeignKey(bc => bc.ComponentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
