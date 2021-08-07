
using HoanGames.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoanGames.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerSelectPage : ContentPage
    {
        public PlayerSelectPage()
        {
            InitializeComponent();
            BindingContext = new PlayerSelectViewModel();
        }
    }
}