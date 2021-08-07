using Xamarin.Forms;

namespace HoanGames
{
    public partial class App : Application
    {
        string dbPath => FileAccessHelper.GetLocalFilePath("people.db3");
        public static PlayerRepository PlayerRepo { get; private set; }
        public NavigationPage Nav { get; set; }

        public App()
        {
            InitializeComponent();

            PlayerRepo = new PlayerRepository(dbPath);

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
