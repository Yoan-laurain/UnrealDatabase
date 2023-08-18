using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entities.Models
{
    public partial class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
