using HoanGames.Views;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class MainViewModel
    {
        public Command MinesweeperCommand { get; }
        public Command AboutCommand { get; }
        public Command ChangePlayerCommand { get; }
        public MainViewModel()
        {
            MinesweeperCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MinesweeperMenu());
            });
            ChangePlayerCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PlayerSelectPage());
            });
        }

    }
}
