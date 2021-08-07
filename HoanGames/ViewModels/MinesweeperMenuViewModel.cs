using HoanGames.Views;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    class MinesweeperMenuViewModel
    {
        public Command EasyCommand { get; }
        public Command MediumCommand { get; }
        public Command HardCommand { get; }
        public Command CustomCommand { get; }
        public int WidthEntry { get; }
        public int HeightEntry { get; }
        public int MineEntry { get; }
        private INavigation _navigation;

        public MinesweeperMenuViewModel()
        {
            EasyCommand = new Command(async () =>
            {
                await _navigation.PushAsync(new MinesweeperPage('e'));
            });
        }
    }
}
