using PostApplication.Models;
using PostApplication.Pages;
using System;
using System.Collections.Generic;
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

namespace PostApplication
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        POSTEntities entities;
        public LoginPage()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(244, 236, 226));
            LoginButton.Foreground = new SolidColorBrush(Color.FromRgb(104, 114, 89));
            UserName.Background = new SolidColorBrush(Color.FromRgb(162, 161, 130));
            Password.Background = new SolidColorBrush(Color.FromRgb(162, 161, 130));
            entities = new POSTEntities();
        }

        private void GoBack(object sender, RoutedEventArgs e) {
            MenuPage menuPage = new MenuPage();
            NavigationService.Navigate(menuPage);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            String username = UserName.Text;
            String password = Password.Password;

            Worker worker = entities.Worker.ToArray().FirstOrDefault(w => w.UserName.ToString() == username && w.Password.ToString() == password);

            if (worker is null)
            {
                ErrorLabel.Text = "Incorrect Username or password, try again";
            }
            else {
                LoggedWorker loggedWorker = LoggedWorker.GetInstance();
                loggedWorker.Username = worker.UserName;
                loggedWorker.Id = worker.Id;
                GoBack(null, null);
            }
        }
    }
}
