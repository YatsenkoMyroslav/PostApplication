using PostApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для SendingsForUser.xaml
    /// </summary>
    public partial class SendingsForUser : Page
    {
        public static POSTEntities DataEntitiesSending { get; set; }
        ObservableCollection<FullSendingInformation> ListSending { get; set; }
        public SendingsForUser()
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(244, 236, 226));

            DataEntitiesSending = new POSTEntities();
            ListSending = new ObservableCollection<FullSendingInformation>();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MenuPage menuPage = new MenuPage();
            NavigationService.Navigate(menuPage);
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            IEnumerable<FullSendingInformation> sendings = DataEntitiesSending.FullSendingInformation.ToArray();

            var queryEmployee = sendings.OrderBy(s => s.Sender);

            foreach (var s in queryEmployee)
            {
                ListSending.Add(s);
            }

            SendingTable.ItemsSource = ListSending;
        }

        private ObservableCollection<FullSendingInformation> getData(String name)
        {
            IEnumerable<FullSendingInformation> sendings = DataEntitiesSending.FullSendingInformation.ToArray();

            sendings = sendings.Where(s => s.Sender.ToString().Contains(name) || s.Recipient.ToString().Contains(name));

            ObservableCollection<FullSendingInformation> list = new ObservableCollection<FullSendingInformation>(sendings);

            return list;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            String searchField = SearchField.Text;

            ListSending = getData(searchField);

            SendingTable.ItemsSource = ListSending;

            SendingTable.Items.Refresh();
        }
    }
}
