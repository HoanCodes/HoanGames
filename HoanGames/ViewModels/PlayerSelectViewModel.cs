using HoanGames.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class PlayerSelectViewModel : INotifyPropertyChanged
    {
        private string _nameEntry;
        public string NameEntry
        {
            get
            {
                return _nameEntry;
            }
            set
            {
                _nameEntry = value;

                OnPropertyChanged();
            }
        }
        private List<Player> _playerList;
        public List<Player> PlayerList
        {
            get
            {
                return _playerList;
            }
            set
            {
                _playerList = value;

                OnPropertyChanged();
            }
        }

        public Command AddPlayerCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public PlayerSelectViewModel()
        {
            AddPlayerCommand = new Command(OnAddPlayer);

            PlayerList = new List<Player>();
            GetPlayersList();
        }

        public async void OnAddPlayer()
        {
            await App.PlayerRepo.AddPlayer(NameEntry).ConfigureAwait(false);
            NameEntry = "";
            GetPlayersList();
        }
        public async void GetPlayersList()
        {
            PlayerList = await App.PlayerRepo.GetAllPlayers().ConfigureAwait(false);
        }
    }
}
