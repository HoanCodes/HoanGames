using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HoanGames.ViewModels;

namespace HoanGames.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinesweeperTestPage : ContentPage
    {
        public MinesweeperTestPage()
        {
            InitializeComponent();
            BindingContext = new MinesweeperTestViewModel();
        }
    }
}
