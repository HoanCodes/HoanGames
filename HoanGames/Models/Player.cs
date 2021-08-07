using SQLite;

namespace HoanGames.Models
{
    [Table("players")]
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100), Unique]
        public string Name { get; set; }
    }
}
