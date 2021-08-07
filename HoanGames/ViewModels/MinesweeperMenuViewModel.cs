using HoanGames.Views;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class MinesweeperMenuViewModel
    {
        public Command EasyCommand { get; }
        public Command MediumCommand { get; }
        public Command HardCommand { get; }
        public Command CustomCommand { get; }
        public int WidthEntry { get; }
        public int HeightEntry { get; }
        public int MineEntry { get; }
        private readonly INavigation Navigation;
        public MinesweeperMenuViewModel(INavigation navigation)
        {
            Navigation = navigation;
            EasyCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('e')).ConfigureAwait(false));
            MediumCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('m')).ConfigureAwait(false));
            HardCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('h')).ConfigureAwait(false));
            CustomCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('c')).ConfigureAwait(false));
        }
    }
}
