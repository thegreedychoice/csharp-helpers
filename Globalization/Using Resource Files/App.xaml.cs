using System.Globalization;
using System.Windows;

namespace Using_Resource_Files
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CultureInfo.CurrentUICulture
                = CultureInfo.CreateSpecificCulture("sv");
        }
    }
}
