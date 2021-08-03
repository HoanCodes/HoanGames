using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoanGames.ViewModels
{
    public class MinesweeperViewModel
    {

        bool _holdingFlag;
        public bool HoldingFlag 
        {
            get
            {
                return _holdingFlag;
            }
            set
            {
                _holdingFlag = value;

                FlagCommand.ChangeCanExecute();
            }
        }
        bool PlayerLost { get; set; }
        int NumOfMoves { get; set; } = 0;
        int NumOfMines { get; set; } = 8;
        int BoardWidth { get; } = 6;
        int BoardHeight { get; } = 6;
        List<Cell> Board { get; set; } = new List<Cell>();
        Grid grid { get; set; }
        public Command FlagCommand { get; }
        public Command RestartCommand { get; }


        public MinesweeperViewModel(Grid gridInput)
        {
            grid = gridInput;

            FlagCommand = new Command(OnFlagClick, () => !HoldingFlag);
            RestartCommand = new Command(OnRestartGame);
        }

        public void StartGame()
        {
            int id = 0;
            for (int i = 0; i < BoardWidth; i++)
            {
                for (int j = 0; j < BoardHeight; j++)
                {
                    //Add Cell object to Board
                    Board.Add(new Cell(id++, j, i));

                    //Add Button to Grid
                    Button playerMove;
                    grid.Children.Add(playerMove = new Button()
                    {
                        Padding = 0,
                    }, j, i);
                    playerMove.Clicked += OnPlayerMove;
                }
            }
        }
        public void OnRestartGame()
        {
            NumOfMines = 8;
            NumOfMoves = 0;
            Board = new List<Cell>();
            grid.Children.Clear();

            int id = 0;
            for (int i = 0; i < BoardWidth; i++)
            {
                for (int j = 0; j < BoardHeight; j++)
                {
                    //Add Cell object to Board
                    Board.Add(new Cell(id++, j, i));

                    //Add Button to Grid
                    Button playerMove;
                    grid.Children.Add(playerMove = new Button()
                    {
                        Padding = 0,
                    }, j, i);
                    playerMove.Clicked += OnPlayerMove;
                }
            }
            //btnFlag.IsEnabled = true;

        }
        public void OnPlayerMove(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var row = Grid.GetRow(button);
            var col = Grid.GetColumn(button);
            Cell playerCell = Board.Find(cell => cell.X == col && cell.Y == row);

            if (HoldingFlag)
            {
                button.Text = "F";
                button.TextColor = Color.Red;
                HoldingFlag = false;
                //btnFlag.IsEnabled = true;
            }
            else
            {
                RevealCell(playerCell);
            }
        }

        public void RevealCell(Cell playerCell)
        {
            playerCell.IsRevealed = true;
            var NumOfAdjacentMines = 0;

            if (NumOfMoves == 0)
            {
                GenerateMines(playerCell);
                NumOfMoves++;
            }

            if (playerCell.HasMine)
            {
                GameOver();
                return;
            }

            var AdjacentCells = new List<Cell>();
            for (int i = playerCell.Y - 1; i <= playerCell.Y + 1; i++) //for loop around the playerCell to collect into a List
            {
                for (int j = playerCell.X - 1; j <= playerCell.X + 1; j++)
                {
                    Cell cellFound = Board.Find(cell => cell.X == j && cell.Y == i);

                    if (cellFound != null && cellFound.Id != playerCell.Id)
                    {
                        AdjacentCells.Add(cellFound);
                        if (cellFound.HasMine) NumOfAdjacentMines++;
                    }

                }
            }
            if (NumOfAdjacentMines == 0)
            {
                foreach (Cell cell in AdjacentCells)
                {
                    //RemoveCell(cell.X, cell.Y, NumOfAdjacentMines);
                    if (!cell.IsRevealed) RevealCell(cell);
                }
            }
            RemoveCell(playerCell.X, playerCell.Y, NumOfAdjacentMines);


        }
        void OnFlagClick()
        {
            HoldingFlag = true;
            //btnFlag.IsEnabled = false;
        }
        void GenerateMines(Cell playerCell)
        {
            var StartingCells = new List<Cell>();
            for (int i = playerCell.X - 1; i <= playerCell.X + 1; i++) //for loop around the playerCell to collect into a List
            {
                for (int j = playerCell.Y - 1; j <= playerCell.Y + 1; j++)
                {
                    Cell cellFound = Board.Find(x => x.X == i && x.Y == j);

                    StartingCells.Add(cellFound);
                }
            }

            while (NumOfMines > 0)
            {
                foreach (Cell cell in Board)
                {
                    if (NumOfMines <= 0)
                    {
                        break;
                    }

                    if (cell.HasMine == false && !StartingCells.Contains(cell))
                    {
                        var rand = new Random();
                        if (rand.Next(101) < 20) //20% chance for each cell to have a mine
                        {
                            cell.HasMine = true;
                            NumOfMines--;
                        }
                    }
                }
            }
        }

        public void RemoveCell(int col, int row, int numOfAdjacentMines)
        {
            for (int index = grid.Children.Count - 1; index >= 0; index--)
            {
                if (Grid.GetRow(grid.Children[index]) == row && Grid.GetColumn(grid.Children[index]) == col)
                {
                    grid.Children.RemoveAt(index);

                    if (numOfAdjacentMines > 0)
                    {
                        grid.Children.Add(new Label
                        {
                            Text = Convert.ToString(numOfAdjacentMines),
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FontSize = 20,
                        }, col, row);
                    }

                }
            }
        }

        public void GameOver()
        {
            PlayerLost = true;
            //btnFlag.IsEnabled = false;

            for (int index = grid.Children.Count - 1; index >= 0; index--)
            {
                var row = Grid.GetRow(grid.Children[index]);
                var col = Grid.GetColumn(grid.Children[index]);
                Cell playerCell = Board.Find(cell => cell.X == col && cell.Y == row);
                if (playerCell.HasMine)
                {
                    grid.Children.RemoveAt(index);
                    grid.Children.Add(new Label
                    {
                        Text = "M",
                        TextColor = Color.Orange,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 20,
                    }, col, row);


                }
            }
        }
        public class Cell
        {
            public bool IsRevealed { get; set; }
            public bool IsFlagged { get; set; }
            public bool HasMine { get; set; }
            public int AdjacentMines { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Id { get; set; }

            public Cell(int id, int x, int y)
            {
                X = x;
                Y = y;
                Id = id;
            }
        }
    }
}
