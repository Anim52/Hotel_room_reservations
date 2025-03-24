using Database.Context;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AplicationApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var context = new SqlServerContext())
            {
                context.Database.EnsureCreated();
            }

        }
    }

}
