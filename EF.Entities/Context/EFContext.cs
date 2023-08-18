using EF.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entities.Context
{
    public partial class EFContext : DbContext
    {
        public  EFContext(DbContextOptions<EFContext> options) : base(options)
        { 
        }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Item> Items { get; set; }
    }
}
