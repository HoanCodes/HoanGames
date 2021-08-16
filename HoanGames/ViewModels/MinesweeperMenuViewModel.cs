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
        public Command<string> DifficultyCommand { get; }
        
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
            DifficultyCommand = new Command<string>(SetDifficulty);
        }
        public void SetDifficulty(string difficulty) //Not sure why this would not work
        {
            switch (int.Parse(difficulty))
            {
                case 0: //Easy
                    {
                        SetGame(6, 6, 6);
                        break;
                    }
                case 1: //Medium
                    {
                        SetGame(8, 8, 12);
                        break;
                    }
                case 2: //Hard
                    {
                        SetGame(10, 10, 20);
                        break;
                    }
                case 3: //Custom
                    {
                        SetGame(WidthEntry, HeightEntry, MineEntry);
                        break;
                    }
            }
        }
        public async void SetGame(int width, int height, int mines)
        {
            Game GameSession = await App.GameRepo.GetGame();
            if (GameSession != null)
            {
                await App.GameRepo.UpdateGame(width, height, mines);
            }
            else
            {
                await App.GameRepo.AddGame(width, height, mines);
            }

            await Navigation.PushAsync(new MinesweeperPage()).ConfigureAwait(false);
        }
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}