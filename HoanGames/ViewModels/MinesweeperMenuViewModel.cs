using System.ComponentModel;
using Xamarin.Forms;
using HoanGames.Models;
using HoanGames.Views;
using System.Runtime.CompilerServices;

namespace HoanGames.ViewModels
{
    public class MinesweeperMenuViewModel : INotifyPropertyChanged
    {
        public int _widthEntry = 2;
        public int _heightEntry = 20;
        public int _mineEntry = 40;
        private string _statusMessage;
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
                OnPropertyChanged();
            }
        }
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        private readonly INavigation Navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        public MinesweeperMenuViewModel(INavigation navigation)
        {
            Navigation = navigation;

            //Temporary solution, creating separate methods was not working for some reason...
            //REMOVED: ConfigureAwait(false), because Xamarin had issues with different threads rendering the elements.
            EasyCommand = new Command(async () =>
            {
                Game GameSession = await App.GameRepo.GetGame(); //Check if there is a board record in local database
                if (GameSession != null)
                {
                    await App.GameRepo.UpdateGame(6, 6, 6); //If there is, update it with new board width, height, and number of mines
                }
                else
                {
                    await App.GameRepo.AddGame(6, 6, 6); //If not, create a new board with the desired numbers
                }

                await Navigation.PushAsync(new MinesweeperPage()); //Go to Minesweeper page to play the game
            });
            MediumCommand = new Command(async () =>
            {
                Game GameSession = await App.GameRepo.GetGame();
                if (GameSession != null)
                {
                    await App.GameRepo.UpdateGame(8, 8, 12);
                }
                else
                {
                    await App.GameRepo.AddGame(8, 8, 12);
                }

                await Navigation.PushAsync(new MinesweeperPage());
            });
            HardCommand = new Command(async () =>
            {
                Game GameSession = await App.GameRepo.GetGame();
                if (GameSession != null)
                {
                    await App.GameRepo.UpdateGame(10, 10, 20);
                }
                else
                {
                    await App.GameRepo.AddGame(10, 10, 20);
                }

                await Navigation.PushAsync(new MinesweeperPage());
            });
            CustomCommand = new Command(async () =>
            {
                Game GameSession = await App.GameRepo.GetGame();
                if (GameSession != null)
                {
                    await App.GameRepo.UpdateGame(WidthEntry, HeightEntry, MineEntry);
                }
                else
                {
                    await App.GameRepo.AddGame(WidthEntry, HeightEntry, MineEntry);
                }

                await Navigation.PushAsync(new MinesweeperPage());
            });
        }
        public async void SetEasy() //Not sure why this would not work
        {
            /*
            Game GameSession = await App.GameRepo.GetGame();
            if (GameSession != null)
            {
                await App.GameRepo.UpdateGame(6, 6);
            }
            else
            {
                await App.GameRepo.AddGame(6, 6);
            }
            */
            await Navigation.PushAsync(new MinesweeperPage()).ConfigureAwait(false);
        }
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}