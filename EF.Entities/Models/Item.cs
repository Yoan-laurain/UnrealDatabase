﻿namespace EF.Entities.Models
{
    public partial class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Player>? Players { get; set; }
    }
}
