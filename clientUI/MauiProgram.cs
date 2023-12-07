using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;

namespace clientUI
{
    public class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            return builder.Build();
        }
    }
}