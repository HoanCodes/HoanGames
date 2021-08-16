using HoanGames.ViewModels;

namespace HoanGames.Views
{
    public partial class MinesweeperPage
    {
        public MinesweeperPage()
        {
            InitializeComponent();
            BindingContext = new MinesweeperViewModel(Navigation);
        }
    }
}
