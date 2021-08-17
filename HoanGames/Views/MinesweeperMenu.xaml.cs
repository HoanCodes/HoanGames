using HoanGames.ViewModels;

namespace HoanGames.Views
{
    public partial class MinesweeperMenu
    {
        public MinesweeperMenu()
        {
            InitializeComponent();
            BindingContext = new MinesweeperMenuViewModel(Navigation);
        }
    }
}