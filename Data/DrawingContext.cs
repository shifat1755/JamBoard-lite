using CDB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDB.Data
{
    public class DrawingContext : DbContext
    {
        public DrawingContext(DbContextOptions<DrawingContext> options) : base(options) { }
        public DbSet<DrawingState> DrawingStates { get; set; }
        public DbSet<Drawing> Drawings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drawing>()
                .HasKey(DPK => DPK.Name);

            modelBuilder.Entity<DrawingState>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<DrawingState>()
                .HasOne<Drawing>()
                .WithMany(i=>i.states)
                .HasForeignKey(k => k.DrawingName)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
