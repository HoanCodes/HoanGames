
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoanGames.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinesweeperMenu : ContentPage
    {
        public MinesweeperMenu()
        {
            InitializeComponent();
            btnEasy.Clicked += (s, e) =>
            {
                Navigation.PopModalAsync();
                Navigation.PushAsync(new MinesweeperPage('e')); //Easy (6x6, 6 mines)
            };
            btnMedium.Clicked += (s, e) =>
            {
                Navigation.PopModalAsync();
                Navigation.PushAsync(new MinesweeperPage('m')); //Medium (7x7, 10 mines)
            };
            btnHard.Clicked += (s, e) =>
            {
                Navigation.PopModalAsync();
                Navigation.PushAsync(new MinesweeperPage('h')); //Hard (8x8, 16 mines)
            };
            btnCustom.Clicked += (s, e) =>
            {
                //quick check if any entries are empty
                int w, h, m = 0;
                try
                {
                    w = int.Parse(width.Text);
                    h = int.Parse(height.Text);
                    m = int.Parse(numOfMines.Text);
                }
                catch
                {
                    statusMessage.Text = "Please enter valid numbers!";
                    return;
                }

                statusMessage.Text = "";
                Navigation.PopModalAsync();
                Navigation.PushAsync(new MinesweeperPage('c', w, h, m));
            };
        }
    }
}