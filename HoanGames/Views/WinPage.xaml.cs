
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoanGames.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WinPage : ContentPage
    {
        public WinPage()
        {
            InitializeComponent();

            btnBack.Clicked += (s, e) => Navigation.PopModalAsync();
        }
    }
}