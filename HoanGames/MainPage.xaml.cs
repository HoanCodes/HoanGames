using HoanGames.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HoanGames
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}
