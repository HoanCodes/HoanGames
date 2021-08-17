using SQLite;

namespace HoanGames.Models
{
    [Table("games")]
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int BoardMines { get; set; }

        //Can store Cell List here later
    }
}
