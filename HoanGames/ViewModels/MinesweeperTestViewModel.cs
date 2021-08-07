using System.Collections.Generic;

namespace HoanGames.ViewModels
{
    class MinesweeperTestViewModel
    {
        public int BoardHeight { get; set; } = 4;
        public int BoardWidth { get; set; } = 4;
        public List<Cell> BoardList { get; set; } = new List<Cell>();

        public MinesweeperTestViewModel()
        {
            int id = 0;
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    //Add Cell object to Board
                    BoardList.Add(new Cell(id++, j, i));

                    /*
                    Button playerMove;
                    BoardGrid.Children.Add(playerMove = new Button()
                    {
                        Padding = 0,
                    }, j, i);
                    playerMove.Clicked += OnPlayerMove;
                    */
                }
            }
        }
    }

    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; }
        public Cell(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}
