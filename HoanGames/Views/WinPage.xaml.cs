using HoanGames.ViewModels;

namespace HoanGames.Views
{
    public partial class WinPage
    {
        public WinPage()
        {
            InitializeComponent();
            BindingContext = new WinViewModel(Navigation);
        }
    }
}