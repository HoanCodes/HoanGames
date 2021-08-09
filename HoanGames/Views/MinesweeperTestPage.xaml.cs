using HoanGames.ViewModels;

namespace HoanGames.Views
{
    
    public partial class MinesweeperTestPage
    {
        public MinesweeperTestPage()
        {
            InitializeComponent();
            BindingContext = new MinesweeperTestViewModel(Navigation);
        }
    }
}
