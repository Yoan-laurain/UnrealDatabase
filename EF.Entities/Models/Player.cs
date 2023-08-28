using System.ComponentModel.DataAnnotations;

namespace EF.Entities.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Required]
        public string? Guid { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
