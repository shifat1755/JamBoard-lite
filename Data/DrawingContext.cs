using CDB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDB.Data
{
    public class DrawingContext : DbContext
    {
        public DrawingContext(DbContextOptions<DrawingContext> options) : base(options) { }
        public DbSet<UserBasedData> UserBasedData { get; set; }
        public DbSet<Drawing> Drawings { get; set; }
    }
}
