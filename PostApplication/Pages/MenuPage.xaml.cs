using PostApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PostApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();

            if (LoggedWorker.IsLoggedIn())
            {
                UsernameBox.Text = LoggedWorker.GetInstance().Username;
            }
            else { 
                UsernameBox.Text = "Guest";
            }

            this.Background = new SolidColorBrush(Color.FromRgb(244, 236, 226));
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("Do you really want to exit?", "Exiting...",
                                           MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (response == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
