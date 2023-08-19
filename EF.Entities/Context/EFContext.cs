using EF.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.Entities.Contexts
{
    public partial class EFContext : DbContext
    {
        public  EFContext(DbContextOptions<EFContext> options) : base(options)
        { 
        }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<LoginAttempt> LoginAttempts { get; set; }
    }
}
