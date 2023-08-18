namespace EF.Entities.Models
{
    public partial class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item>? Items { get; set; }
    }
}
