using HoanGames.ViewModels;
using System;
using Xamarin.Forms;

namespace HoanGames.Views
{
    public partial class MinesweeperPage
    {
        public MinesweeperPage()
        {
            InitializeComponent();
            BindingContext = new MinesweeperViewModel(Navigation);
        }
    }
}
