using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HoanGames.Models
{
    [Table("players")]
    class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100), Unique]
        public string Name { get; set; }
    }
}
