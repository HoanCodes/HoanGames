using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    class MinesweeperTestViewModel : INotifyPropertyChanged
    {
        private List<Cell> _board = new List<Cell>();
        public List<Cell> Board
        {
            get 
            {
                return _board;
            }
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }
        bool GameFinished { get; set; }
        public int BoardHeight { get; set; } = 6;
        public int BoardWidth { get; set; } = 6;
        public int NumOfMines { get; set; } = 4;
        public int NumOfMoves { get; set; } = 0;
        public Command RevealCellCommand { get; }
        public MinesweeperTestViewModel()
        {
            RevealCellCommand = new Command<int>(RevealCell);

            //Create List
            int id = 0;
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    //Add Cell object to Board
                    Board.Add(new Cell(id++, j, i));
                }
            }
        }

        public void RevealCell(int cellId)
        {
            Cell playerCell = Board.Find(cell => cell.Id == cellId);
            var AdjacentCells = new List<Cell>();
            var NumOfAdjacentMines = 0;

            playerCell.IsRevealed = true;

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
            if (NumOfAdjacentMines == 0)
            {
                foreach (Cell cell in AdjacentCells)
                {
                    if (!cell.IsRevealed && !GameFinished) RevealCell(cell.Id);
                }
            }
            else
            {
                playerCell.HasAdjacentMines = true;
                playerCell.AdjacentMines = NumOfAdjacentMines;
            }
            playerCell.IsVisible = false;
            if (NumOfMoves == (BoardWidth * BoardHeight) - NumOfMines)
            {
                //OnGameWon();
                return;
            }
        }
        public void GenerateMines(Cell playerCell)
        {
            var StartingCells = new List<Cell>();

            for (int i = playerCell.X - 1; i <= playerCell.X + 1; i++) //for loop around the playerCell to collect into a List
            {
                for (int j = playerCell.Y - 1; j <= playerCell.Y + 1; j++)
                {
                    Cell cellFound = Board.Find(x => x.X == i && x.Y == j);
                    if (cellFound != null)
                    {
                        StartingCells.Add(cellFound);
                    }
                }
            }
            //Check if there is enough space for the requested number of mines, if not, lower NumOfMines.
            if (NumOfMines > (BoardHeight * BoardWidth) - StartingCells.Count)
            {
                NumOfMines = (BoardHeight * BoardWidth) - StartingCells.Count;
            }
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

                            //TEST
                            //cell.IsVisible = false;
                        }
                    }
                }
            }

        }
        public void GameOver()
        {
            GameFinished = true;
            foreach (Cell cell in Board)
            {
                cell.IsEnabled = false;
                if (cell.HasMine)
                {
                    cell.BgColor = Color.Red;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Cell : INotifyPropertyChanged
    {
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public bool HasMine { get; set; }
        private int _adjacentMines;
        public int AdjacentMines
        {
            get
            {
                return _adjacentMines;
            }
            set
            {
                _adjacentMines = value;
                OnPropertyChanged();
            }
        }
        private bool _hasAdjacentMines;
        public bool HasAdjacentMines
        {
            get
            {
                return _hasAdjacentMines;
            }
            set
            {
                _hasAdjacentMines = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool _isVisible = true;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
        private Color _bgColor;
        public Color BgColor
        {
            get
            {
                return _bgColor;
            }
            set
            {
                _bgColor = value;
                OnPropertyChanged();
            }
        }
        private int _id;
        public int Id 
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
