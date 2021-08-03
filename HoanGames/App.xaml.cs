using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoanGames
{
    public partial class App : Application
    {
        string dbPath => FileAccessHelper.GetLocalFilePath("people.db3");
        public static PlayerRepository PlayerRepo { get; private set; }

        public App()
        {
            InitializeComponent();

            PlayerRepo = new PlayerRepository(dbPath);

            var nav = new NavigationPage(new MainPage());

            MainPage = nav;
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
