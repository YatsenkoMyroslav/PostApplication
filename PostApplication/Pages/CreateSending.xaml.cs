using Microsoft.VisualBasic;
using PostApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PostApplication.Pages
{
    
    public partial class CreateSending : Page
    {
        POSTEntities entities;
        IEnumerable<PostOffice> offices;
        IEnumerable<SendingType> sendingTypesEntities;
        IEnumerable<TransportType> transportTypesEntities;

        int lengthValue, weightValue, heightValue, widthValue, volume, price, estimatedPrice, sentPost, recievePost;

        public CreateSending()
        {
            entities = new POSTEntities();
            offices = entities.PostOffice.ToArray();
            sendingTypesEntities = entities.SendingType.ToArray().OrderBy(s => s.Id);
            transportTypesEntities = entities.TransportType.ToArray().OrderBy(t => t.Id);
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(244, 236, 226));
        }

        private void Onload(object sender, RoutedEventArgs e) {
            
            if (!LoggedWorker.IsLoggedIn())
            {
                MessageBox.Show("To create a new delivery please log in", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                GoBack(null, null);
            }
            else {

                foreach (var type in sendingTypesEntities)
                {
                    sendingType.Items.Add(type.TypeName);
                }

                foreach (var type in transportTypesEntities)
                {
                    transportType.Items.Add(type.TypeName);
                }

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

        private void Calculate(object sender, RoutedEventArgs e)
        {
            if (Validate()) {
                infoText.Foreground = new SolidColorBrush(Colors.Black);

                volume = (int)Math.Round(widthValue * lengthValue * heightValue / Math.Pow(100, 3));

                price = Math.Max(volume * 250, weightValue) * DeliveryPrice.PriceForKG;

                infoText.Text = $"Delivery price: {price} grn.";
            }
        }

        private bool Validate()
        {

            if (String.IsNullOrEmpty(length.Text) || String.IsNullOrEmpty(width.Text) || String.IsNullOrEmpty(height.Text) ||
                String.IsNullOrEmpty(weight.Text) || String.IsNullOrEmpty(estimatedValue.Text)
                || String.IsNullOrEmpty(fromPost.Text) || String.IsNullOrEmpty(toPost.Text))
            {
                infoText.Text = "Error. Enter sending data.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
            else if (!(Int32.TryParse(estimatedValue.Text, out estimatedPrice) && Int32.TryParse(fromPost.Text, out sentPost) &&
                Int32.TryParse(toPost.Text, out recievePost) && Int32.TryParse(weight.Text, out weightValue) &&
                Int32.TryParse(height.Text, out heightValue) && Int32.TryParse(width.Text, out widthValue) &&
                Int32.TryParse(length.Text, out lengthValue))) {
                infoText.Text = "Error. Check data correction and try againg.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
            else if (!(offices.Select(o => o.Id).Contains(sentPost) && offices.Select(o => o.Id).Contains(recievePost))) {
                infoText.Text = "Error. Check data correction and try againg.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
            else if (weightValue <= 0 || lengthValue<=0 || heightValue<=0 || estimatedPrice<=0 || widthValue <= 0)
            {
                infoText.Text = "Error. Check data correction and try againg.";
                infoText.Foreground = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }

                return true;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                Calculate(null, null);

                Sending sending = new Sending();
                
                sending.SendingStatusId = entities.SendingStatus.ToArray().FirstOrDefault(st => st.Id == 1).Id;
                sending.Weight = weightValue;
                sending.Volume = volume;
                sending.Description = description.Text;
                sending.ValuePrice = estimatedPrice;
                sending.PriceForSending = price;
                sending.CreatedBy = LoggedWorker.GetInstance().Id;
                
                int sentById = entities.User.ToArray().FirstOrDefault(u => u.Name == senderName.Text.Trim() && u.Lastname == senderLastname.Text.Trim()).Id;
                
                if(sentById == 0) {
                    User user = new User();
                    user.Name = senderName.Text;
                    user.Lastname = senderLastname.Text;

                    String phoneNumber;
                    do
                    {
                        phoneNumber = Interaction.InputBox("Enter sender phonenumber (380xxxxxxxxx)", "Add phonenumber", "", 300, 300);
                    } while(entities.User.FirstOrDefault(u => u.PhoneNumber==phoneNumber) != null || phoneNumber.Length!=12);

                    user.PhoneNumber = phoneNumber;
                    user.Id = entities.User.Count()+1;
                    sentById = user.Id;

                    entities.User.Add(user);
                    entities.SaveChanges();
                }
                sending.SentById = sentById;

                int sentToId = entities.User.ToArray().FirstOrDefault(u => u.Name == recipientName.Text.Trim() && u.Lastname == recipientLastname.Text.Trim()).Id;

                if (sentToId == 0)
                {
                    User user = new User();
                    user.Name = senderName.Text;
                    user.Lastname = senderLastname.Text;

                    String phoneNumber;
                    do
                    {
                        phoneNumber = Interaction.InputBox("Enter recepient phonenumber (380xxxxxxxxx)", "Add phonenumber", "", 300, 300);
                    } while (entities.User.FirstOrDefault(u => u.PhoneNumber == phoneNumber) != null || phoneNumber.Length != 12);

                    user.PhoneNumber = phoneNumber;
                    user.Id = entities.User.Count() + 1;
                    sentToId = user.Id;

                    entities.User.Add(user);
                    entities.SaveChanges();
                }
                sending.SentToId = sentToId;

                sending.SendingTypeId = sendingType.SelectedIndex+1;
                sending.TransportTypeId = transportType.SelectedIndex+1;
                sending.SentFromOfficeId = sentPost;
                sending.SentToOfficeId= recievePost;
                sending.DateOfSending= DateTime.Now;

                entities.Sending.Add(sending);
                
                entities.SaveChanges();

                entities.Dispose();

                GoBack(null, null);
            }
        }
    }
}
