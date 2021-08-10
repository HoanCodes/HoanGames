﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using HoanGames.Views;
using HoanGames.Models;

namespace HoanGames.ViewModels
{
    public class MinesweeperViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Cell> Board { get; set; } = new ObservableCollection<Cell>();
        private bool GameFinished { get; set; }
        private bool _holdingFlag;
        public bool HoldingFlag
        {
            get => _holdingFlag;
            set
            {
                _holdingFlag = value;

                FlagCommand.ChangeCanExecute(); //disable button until (HoldingFlag == false)
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private int _numOfFlags;
        public int NumOfFlags
        {
            get => _numOfFlags;
            set
            {
                _numOfFlags = value;
                OnPropertyChanged();
            }
        }

        private int _boardWidth = 6;
        public int BoardWidth
        {
            get => _boardWidth;
            set
            {
                _boardWidth = value;
                OnPropertyChanged();
            }
        }

        private int _boardHeight = 6;
        public int BoardHeight
        {
            get => _boardHeight;
            set
            {
                _boardHeight = value;
                OnPropertyChanged();
            }
        }

        private double _layoutWidth;
        public double LayoutWidth
        {
            get => _layoutWidth;
            set
            {
                _layoutWidth = value;
                OnPropertyChanged();
            }
        }
        public double LayoutHeight { get; set; }
        public int NumOfMines { get; set; }
        public int NumOfMoves { get; set; } = 0;
        public Command RevealCellCommand { get; }
        public Command RestartCommand { get; }
        public Command FlagCommand { get; }

        private readonly INavigation Navigation;
        public MinesweeperViewModel(INavigation navigation)
        {
            Navigation = navigation;
            RevealCellCommand = new Command<int>(RevealCell);
            RestartCommand = new Command(StartGame);
            FlagCommand = new Command(GetFlag, () => !HoldingFlag);

            GetGameSession();
            StartGame();
        }
        public void StartGame()
        {
            IsBusy = true;
            NumOfMoves = 0;
            GameFinished = false;
            Board.Clear();

            double cellHeight = Application.Current.MainPage.Height * 2 / 3 / BoardHeight;
            LayoutWidth = cellHeight * BoardWidth;

            int id = 0;
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    Board.Add(new Cell(id++, j, i)
                    {
                        Height = cellHeight,
                        Width = cellHeight
                    });
                }
            }
            IsBusy = false;
        }
        public void RevealCell(int cellId)
        {
            Cell playerCell = Board.FirstOrDefault(cell => cell.Id == cellId);

            if (HoldingFlag)
            {
                playerCell.IsFlagged = !playerCell.IsFlagged;
                HoldingFlag = false;

                /*
                var numOfFlags = 0;
                foreach (Cell cell in Board)
                {
                    if (cell.IsFlagged)
                    {
                        numOfFlags++;
                    }
                }
                NumOfFlags = numOfFlags;
                */

                return;
            }

            playerCell.IsRevealed = true;
            var AdjacentCells = new List<Cell>();
            var NumOfAdjacentMines = 0;

            if (NumOfMoves++ == 0)
            {
                GenerateMines(playerCell);
            }

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
                    Cell cellFound = Board.FirstOrDefault(cell => cell.X == j && cell.Y == i);

                    if (cellFound != null && cellFound.Id != playerCell.Id)
                    {
                        AdjacentCells.Add(cellFound);
                        if (cellFound.HasMine) NumOfAdjacentMines++;
                    }
                }
            }

            //Only call RevealCell on adjacent cells if there are no adjacent cells
            if (NumOfAdjacentMines == 0)
            {
                foreach (Cell cell in AdjacentCells)
                {
                    if (!cell.IsRevealed && !GameFinished)
                    {
                        RevealCell(cell.Id);
                    }
                }
            }
            else
            {
                playerCell.CellText = NumOfAdjacentMines.ToString();
            }

            //Win condition check
            if (NumOfMoves == (BoardWidth * BoardHeight) - NumOfMines)
            {
                GameWon();
            }
        }
        public void GenerateMines(Cell playerCell)
        {
            //Makes sure there are no mines around the starting cell
            var StartingCells = new List<Cell>();
            for (int i = playerCell.X - 1; i <= playerCell.X + 1; i++)
            {
                for (int j = playerCell.Y - 1; j <= playerCell.Y + 1; j++)
                {
                    Cell cellFound = Board.FirstOrDefault(x => x.X == i && x.Y == j);
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

            //Set up mines
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
        public void GetFlag()
        {
            HoldingFlag = true;
        }
        public async void GetGameSession()
        {
            Game GameSession = await App.GameRepo.GetGame().ConfigureAwait(false);

            BoardWidth = GameSession.BoardWidth;
            BoardHeight = GameSession.BoardHeight;
            NumOfMines = GameSession.BoardMines;
        }
        public async void GameWon()
        {
            if (GameFinished)
            {
                return;
            }
            GameFinished = true;
            await Navigation.PushModalAsync(new WinPage()).ConfigureAwait(false);
            foreach (Cell cell in Board)
            {
                cell.IsEnabled = false;
                if (cell.HasMine)
                {
                    cell.BgColor = Color.Green;
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
                    cell.CellText = "X";
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
        private bool _isRevealed;
        public bool IsRevealed
        {
            get => _isRevealed;
            set
            {
                _isRevealed = value;

                if (_isRevealed)
                {
                    //Disable cell
                    CellText = "";
                    BgColor = Color.White;
                    IsEnabled = false;
                }
            }
        }
        public bool _isFlagged;
        public bool IsFlagged
        {
            get => _isFlagged;
            set
            {
                _isFlagged = value;
                CellText = _isFlagged ? "F" : "";
            }
        }
        public bool HasMine { get; set; }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        private string _cellText;
        public string CellText
        {
            get => _cellText;
            set
            {
                _cellText = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private Color _bgColor = Color.LightGray;
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
            get => _id;
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
