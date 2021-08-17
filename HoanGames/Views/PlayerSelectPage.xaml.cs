using HoanGames.ViewModels;

namespace HoanGames.Views
{
    public partial class PlayerSelectPage
    {
        public PlayerSelectPage()
        {
            InitializeComponent();
            BindingContext = new PlayerSelectViewModel();
        }
    }
}