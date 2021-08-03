using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;

using HoanGames.Models;

namespace HoanGames.ViewModels
{
    public class PlayerSelectViewModel
    {
        public string Name { get; set; }
        public string Entry { get; }
        public List<Player> PlayerList { get; set; } = new List<Player>();

        public Command AddPlayerCommand;

        public PlayerSelectViewModel()
        {
            AddPlayerCommand = new Command(OnAddPlayer);
            GetPlayersList();
        }

        public void OnAddPlayer()
        {
            
        }
        public async void GetPlayersList()
        {
            //PlayerList = await App.PlayerRepo.GetAllPlayers();

            PlayerList.Add(new Player() { Name = "Magnificent" });
            PlayerList.Add(new Player() { Name = "xXxDeath_SniperxXx" });
            PlayerList.Add(new Player() { Name = "Hoan" });

        }
        
    }
}
