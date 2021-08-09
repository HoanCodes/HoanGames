using HoanGames.Views;
using System.ComponentModel;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class MinesweeperMenuViewModel : INotifyPropertyChanged
    {
        public int _widthEntry = 10;
        public int _heightEntry = 10;
        public int _mineEntry = 20;
        public Command EasyCommand { get; }
        public Command MediumCommand { get; }
        public Command HardCommand { get; }
        public Command CustomCommand { get; }
        public int WidthEntry
        {
            get
            {
                return _widthEntry;
            }
            set
            {
                _widthEntry = value;
                OnPropertyChanged(nameof(WidthEntry));
            }
        }
        public int HeightEntry
        {
            get
            {
                return _heightEntry;
            }
            set
            {
                _heightEntry = value;
                OnPropertyChanged(nameof(HeightEntry));
            }
        }
        public int MineEntry
        {
            get
            {
                return _mineEntry;
            }
            set
            {
                _mineEntry = value;
                OnPropertyChanged(nameof(MineEntry));
            }
        }
        public string StatusMessage { get; }
        private readonly INavigation Navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        public MinesweeperMenuViewModel(INavigation navigation)
        {
            Navigation = navigation;
            EasyCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('e')).ConfigureAwait(false));
            MediumCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('m')).ConfigureAwait(false));
            HardCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('h')).ConfigureAwait(false));
            CustomCommand = new Command(async () => await Navigation.PushAsync(new MinesweeperPage('c', WidthEntry, HeightEntry, MineEntry)).ConfigureAwait(false));
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}