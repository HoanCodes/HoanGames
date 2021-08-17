using Xamarin.Forms;

namespace HoanGames
{
    public partial class App : Application
    {
        private static string DbPath => FileAccessHelper.GetLocalFilePath("people.db3");
        public static PlayerRepository PlayerRepo { get; private set; }
        public static GameRepository GameRepo { get; private set; }
        public NavigationPage Nav { get; set; }

        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();

            InitializeComponent();

            PlayerRepo = new PlayerRepository(DbPath);

            GameRepo = new GameRepository(DbPath);

            Nav = new NavigationPage(new MainPage());

            MainPage = Nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
