using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using HoanGames.Views;

namespace HoanGames
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            btnChangePlayer.Clicked += (s, e) => Navigation.PushAsync(new PlayerSelectPage());
            btnMinesweeper.Clicked += (s, e) => Navigation.PushAsync(new MinesweeperModal());
        }
    }
}
