using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HoanGames.ViewModels
{
    public class WinViewModel
    {
        public Command BackCommand { get; }
        private readonly INavigation Navigation;
        public WinViewModel(INavigation navigation)
        {
            Navigation = navigation;
            BackCommand = new Command(GoBack);
        }
        public void GoBack()
        {
            Navigation.PopModalAsync();
        }
    }
}
