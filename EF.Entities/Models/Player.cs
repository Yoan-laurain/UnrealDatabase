using System.ComponentModel.DataAnnotations;

namespace EF.Entities.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
