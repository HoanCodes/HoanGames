using System;
using HoanGames.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoanGames.Views

{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinesweeperPage : ContentPage
    {
        public MinesweeperViewModel Game { get; set; }
        public MinesweeperPage(char difficulty, int width = 0, int height = 0, int numOfMines = 0)
        {
            InitializeComponent();

            //Sends grid object to the ViewModel
            Game = new MinesweeperViewModel(grid);
            BindingContext = Game;

            //Can reduce the amount of code-behind later
            Game.CreateBoard();
            Game.StartGame(difficulty, width, height, numOfMines);

            //Subscription to allow the Modal to pop up on GameWon event
            Game.GameWon += WinSubsciber;
        }
        async void WinSubsciber(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new WinPage()).ConfigureAwait(false);
        }
    }
}

//C# for adding Grid if need be
/*
 
            var button = (Button)sender;
            var row = Grid.GetRow(button);
            var grid = button.Parent as Grid;
            //assuming the image is in column 1
            var image = grid.Children.Where(c => Grid.GetRow(c) == row && Grid.GetColumn(c)==1);


            Grid grid = new Grid
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
                }
            };
            */
