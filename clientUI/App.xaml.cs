using ClientUI;

namespace clientUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();

            var registrationPage = new RegistrationPage();

            MainPage = new NavigationPage(registrationPage);
        }
    }
}
