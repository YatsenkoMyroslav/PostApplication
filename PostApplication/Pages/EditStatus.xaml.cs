using Microsoft.VisualBasic;
using PostApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PostApplication.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditStatus.xaml
    /// </summary>
    public partial class EditStatus : Page
    {
        POSTEntities entities;
        IEnumerable<PostOffice> offices;
        IEnumerable<SendingStatus> statuses;
        IEnumerable<SendingType> sendingTypesEntities;
        IEnumerable<TransportType> transportTypesEntities;
        int estimatedPrice, recievePost;
        Sending sending;

        public EditStatus()
        {
            entities = new POSTEntities();
            offices = entities.PostOffice.ToArray();
            sendingTypesEntities = entities.SendingType.ToArray().OrderBy(s => s.Id);
            transportTypesEntities = entities.TransportType.ToArray().OrderBy(t => t.Id);
            statuses = entities.SendingStatus.ToArray().OrderBy(s => s.Id);
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(244, 236, 226));
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {

            if (!LoggedWorker.IsLoggedIn())
            {
                MessageBox.Show("To edit a delivery please log in", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                GoBack(null, null);
            }
            else
            {

                foreach (var type in sendingTypesEntities)
                {
                    sendingType.Items.Add(type.TypeName);
                }

                foreach (var type in transportTypesEntities)
                {
                    transportType.Items.Add(type.TypeName);
                }

                foreach (var status in statuses)
                {
                    Status.Items.Add(status.Status);
                }

                Status.SelectedIndex = 0;
                sendingType.SelectedIndex = 0;
                transportType.SelectedIndex = 0;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            entities.Dispose();
            MenuPage menuPage = new MenuPage();
            NavigationService.Navigate(menuPage);
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            String searchField = SearchField.Text;

            sending = GetData(searchField);

            User sentBy = entities.User.FirstOrDefault(u => u.Id == sending.SentById);

            senderName.Text = sentBy.Name;
            senderLastname.Text = sentBy.Lastname;

            User sentTo = entities.User.FirstOrDefault(u => u.Id == sending.SentToId);

            recipientName.Text = sentTo.Name;
            recipientLastname.Text = sentTo.Lastname;

            estimatedValue.Text = sending.ValuePrice.ToString();

            toPost.Text = sending.SentToOfficeId.ToString();
            
            Status.SelectedIndex = sending.SendingStatusId-1;

            transportType.SelectedIndex = sending.TransportTypeId-1;

            sendingType.SelectedIndex=sending.SendingTypeId - 1;

            description.Text = sending.Description;

        }

        private Sending GetData(String TTH)
        {
            return entities.Sending.FirstOrDefault(s => s.Id.ToString()==TTH);
        }
        private bool Validate()
        {
            if (String.IsNullOrEmpty(estimatedValue.Text) || String.IsNullOrEmpty(toPost.Text))
            {
                infoText.Text = "Error. Enter sending data.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
            else if (!(Int32.TryParse(estimatedValue.Text, out estimatedPrice) && Int32.TryParse(toPost.Text, out recievePost)))
            {
                infoText.Text = "Error. Check data correction and try againg.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
            else if (!(offices.Select(o => o.Id).Contains(recievePost)))
            {
                infoText.Text = "Error. Check data correction and try againg.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
           
            return true;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                sending.SendingStatusId = Status.SelectedIndex+1;
                sending.Description = description.Text;
                sending.ValuePrice = estimatedPrice;

                sending.SendingTypeId = sendingType.SelectedIndex + 1;
                sending.TransportTypeId = transportType.SelectedIndex + 1;
                sending.SentToOfficeId = recievePost;
                
                entities.SaveChanges();

                entities.Dispose();

                GoBack(null, null);
            }
        }
    }
}