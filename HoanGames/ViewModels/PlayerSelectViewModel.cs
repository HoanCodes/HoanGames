using HoanGames.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class PlayerSelectViewModel : INotifyPropertyChanged
    {
        string _nameEntry;
        List<Player> _playerList;
        public string NameEntry
        {
            get
            {
                return _nameEntry;
            }
            set
            {
                _nameEntry = value;

                OnPropertyChanged(nameof(NameEntry));
            }
        }
        public List<Player> PlayerList
        {
            get
            {
                return _playerList;
            }
            set
            {
                _playerList = value;

                OnPropertyChanged(nameof(PlayerList));
            }
        }

        public Command AddPlayerCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public PlayerSelectViewModel()
        {
            AddPlayerCommand = new Command(OnAddPlayer);

            PlayerList = new List<Player>();
            GetPlayersList();
        }

        public async void OnAddPlayer()
        {
            await App.PlayerRepo.AddPlayer(NameEntry);
            NameEntry = "";
            GetPlayersList();
        }
        public async void GetPlayersList()
        {
            PlayerList = await App.PlayerRepo.GetAllPlayers();
        }

    }
}
