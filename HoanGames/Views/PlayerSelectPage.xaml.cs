using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HoanGames.ViewModels;

namespace HoanGames.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerSelectPage : ContentPage
    {
        public PlayerSelectPage()
        {
            InitializeComponent();
            BindingContext = new PlayerSelectViewModel();
        }
    }
}