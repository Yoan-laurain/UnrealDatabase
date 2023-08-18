using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entities.Models
{
    public partial class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Player? Player { get; set; }
    }
}
