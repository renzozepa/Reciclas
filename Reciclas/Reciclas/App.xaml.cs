using Reciclas.Views;
using Syncfusion.Licensing;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Reciclas
{
    public partial class App : Application
    {
        public static string FilePath;
        public static Int32 Id_Usuario;
        public static string Token;
        public static int Para_alta = 0;
        
        public App()
        {            
            SyncfusionLicenseProvider.RegisterLicense("Mzk4MTQzQDMxMzgyZTM0MmUzMGZ4VWMrbWY5SklEZ3VVRXRmZHZydmFGR3RnUXo0RUErOWtuQ01uZFdJUUk9");
            InitializeComponent();
            MainPage = new NavigationPage(new Login());
        }
        public App(string filepath)
        {
            SyncfusionLicenseProvider.RegisterLicense("Mzk4MTQzQDMxMzgyZTM0MmUzMGZ4VWMrbWY5SklEZ3VVRXRmZHZydmFGR3RnUXo0RUErOWtuQ01uZFdJUUk9");
            InitializeComponent();
            FilePath = filepath;
            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
