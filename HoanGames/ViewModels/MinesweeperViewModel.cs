using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class MinesweeperViewModel
    {
        private bool _holdingFlag;
        public bool HoldingFlag
        {
            get
            {
                return _holdingFlag;
            }
            set
            {
                _holdingFlag = value;

                FlagCommand.ChangeCanExecute(); //disable button until (HoldingFlag == false)
            }
        }
        //bool IsBusy { get; set; }
        bool GameFinished { get; set; }
        int NumOfMoves { get; set; } = 0;
        int NumOfMines { get; set; } = 0;
        int BoardWidth { get; set; } = 0;
        int BoardHeight { get; set; } = 0;
        List<Cell> Board { get; set; }
        public Grid BoardGrid { get; set; }
        public Command FlagCommand { get; }
        public Command RestartCommand { get; }

        public event EventHandler<EventArgs> GameWon;

        public MinesweeperViewModel(Grid gridInput)
        {
            BoardGrid = gridInput;
            FlagCommand = new Command(OnFlagClick, () => !HoldingFlag);
            RestartCommand = new Command(OnRestartGame);
        }

        public void CreateBoard()
        {
            int id = 0;
            BoardGrid.IsEnabled = true;
            Board = new List<Cell>();

            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    //Add Cell object to Board
                    Board.Add(new Cell(id++, j, i));

                    //Add Button to Grid
                    Button playerMove;
                    BoardGrid.Children.Add(playerMove = new Button()
                    {
                        Padding = 0,
                    }, j, i);
                    playerMove.Clicked += OnPlayerMove;
                }
            }
        }

        public void CreateGrid()
        {
            BoardGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                }
            };
        }
        public void StartGame(char difficulty, int width = 0, int height = 0, int numOfMines = 0)
        {
            switch (difficulty)
            {
                case 'e':
                    BoardWidth = 6;
                    BoardHeight = 6;
                    NumOfMines = 6;
                    break;
                case 'm':
                    BoardWidth = 7;
                    BoardHeight = 7;
                    NumOfMines = 10;
                    break;
                case 'h':
                    BoardWidth = 8;
                    BoardHeight = 8;
                    NumOfMines = 16;
                    break;
                case 'c':
                    BoardWidth = (width > 10) ? 10 : width; //if number is larger than 10, set to 10
                    BoardHeight = (height > 10) ? 10 : height;
                    NumOfMines = numOfMines;
                    break;
            }
            CreateBoard();
        }
        public void OnRestartGame()
        {
            GameFinished = false;
            NumOfMoves = 0;
            BoardGrid.Children.Clear();
            CreateBoard();
        }
        public void OnPlayerMove(object sender, EventArgs e) //Cell click event
        {
            var button = (Button)sender;
            var row = Grid.GetRow(button);
            var col = Grid.GetColumn(button);
            Cell playerCell = Board.Find(cell => cell.X == col && cell.Y == row);

            if (HoldingFlag)
            {   //Place an F on the cell
                button.Text = "F";
                button.TextColor = Color.Red;
                HoldingFlag = false;
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
            }
            NumOfMoves++;

            if (playerCell.HasMine)
            {
                GameOver();
                return;
            }

            var AdjacentCells = new List<Cell>();

            //for loop around the playerCell to collect adjacent cells into a List
            for (int i = playerCell.Y - 1; i <= playerCell.Y + 1; i++)
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

            //If no adjacent mines, recursively reveal adjacent cells
            if (NumOfAdjacentMines == 0)
            {
                foreach (Cell cell in AdjacentCells)
                {
                    if (!cell.IsRevealed && !GameFinished) RevealCell(cell);
                }
            }

            //Replace Button with number of adjacent mines
            RemoveCell(playerCell.X, playerCell.Y, NumOfAdjacentMines);

            if (NumOfMoves == (BoardWidth * BoardHeight) - NumOfMines)
            {
                OnGameWon();
                return;
            }
        }

        private void OnFlagClick()
        {
            HoldingFlag = true;
        }

        private void GenerateMines(Cell playerCell)
        {
            //make sure the first cell never has adjacent mines
            var StartingCells = new List<Cell>();
            for (int i = playerCell.X - 1; i <= playerCell.X + 1; i++) //for loop around the playerCell to collect into a List
            {
                for (int j = playerCell.Y - 1; j <= playerCell.Y + 1; j++)
                {
                    Cell cellFound = Board.Find(x => x.X == i && x.Y == j);
                    if (cellFound != null) StartingCells.Add(cellFound);
                }
            }

            //Check if there is enough space for the requested number of mines, if not, lower NumOfMines.
            if (NumOfMines > (BoardHeight * BoardWidth) - StartingCells.Count) NumOfMines = (BoardHeight * BoardWidth) - StartingCells.Count;
            var mines = NumOfMines;
            while (mines > 0)
            {
                foreach (Cell cell in Board)
                {
                    if (mines <= 0)
                    {
                        break;
                    }

                    if (!cell.HasMine && !StartingCells.Contains(cell))
                    {
                        var rand = new Random();
                        if (rand.Next(101) < 20) //20% chance for each cell to have a mine
                        {
                            cell.HasMine = true;
                            mines--;
                        }
                    }
                }
            }
        }

        public void RemoveCell(int col, int row, int numOfAdjacentMines)
        {
            for (int index = BoardGrid.Children.Count - 1; index >= 0; index--)
            {
                if (Grid.GetRow(BoardGrid.Children[index]) == row && Grid.GetColumn(BoardGrid.Children[index]) == col)
                {
                    BoardGrid.Children.RemoveAt(index);

                    if (numOfAdjacentMines > 0)
                    {
                        BoardGrid.Children.Add(new Label
                        {
                            Text = Convert.ToString(numOfAdjacentMines),
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FontSize = 20,
                        }, col, row);
                    }
                    else
                    {
                        BoardGrid.Children.Add(new Label(), col, row);
                    }
                }
            }
        }

        public void GameOver()
        {
            GameFinished = true;
            BoardGrid.IsEnabled = false;

            for (int index = BoardGrid.Children.Count - 1; index >= 0; index--)
            {
                var row = Grid.GetRow(BoardGrid.Children[index]);
                var col = Grid.GetColumn(BoardGrid.Children[index]);
                Cell playerCell = Board.Find(cell => cell.X == col && cell.Y == row);
                if (playerCell.HasMine)
                {
                    BoardGrid.Children.RemoveAt(index);
                    BoardGrid.Children.Add(new Label
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
        protected virtual void OnGameWon()
        {
            if (!GameFinished)
            {
                GameFinished = true;
                BoardGrid.IsEnabled = false;
                GameWon?.Invoke(this, EventArgs.Empty);
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
