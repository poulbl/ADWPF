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

namespace ADWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ip = "192.168.132.71";
        string user = "Administrator";
        string password = "Password1";

        public MainWindow()
        {
            InitializeComponent();
            if(!Session.GetbLoggedIn())
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
            //users.SelectionChanged += OpenUserDetails;
        }

        //private void OpenUserDetails(object sender, SelectionChangedEventArgs e)
        //{
        //    ADStream stream = new ADStream(ip, user, password);
        //    Dictionary<string, List<string>> userdata = stream.GetAllData(users.SelectedItem.ToString());
        //    UserDetails userDetails = new UserDetails(userdata[$"{users.SelectedItem}"]);
        //    int i = 0;
        //    userDetails.Show();
        //}

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {

            ADStream stream = new ADStream(ip, user, password);

            var hej = functionBox.SelectedItem.ToString();
            var hej2 = hej.Split(':');
            string actualStr = hej2[1].Trim().ToLower();


            if(actualStr == "get all data for user")
            {
                var userGetAll = stream.GetAllData(usernameText.Text);

                if (Session.GetbLoggedIn())
                {
                    StringBuilder printStr = new StringBuilder();
                    printStr.Append($"Username: {usernameText.Text}");
                    printStr.Append("\n ________________________________");
                    foreach (var item in userGetAll[usernameText.Text])
                    {
                        printStr.Append("\n");
                        printStr.Append(item);
                    }
                    result.Text = printStr.ToString();
                }
                else
                {
                    result.Text = "Ikke logged ind :(";
                }
            }
            else if (actualStr == "get all users")
            {
                var allusers = stream.GetAllUsers();

                if (Session.GetbLoggedIn())
                {
                    StringBuilder printStr = new StringBuilder();

                    foreach (var item in allusers)
                    {
                        printStr.Append("\n");
                        printStr.Append(item);
                    }
                    result.Text = printStr.ToString();
                }
                else
                {
                    result.Text = "Ikke logged ind :(";
                }

            }

            else if (actualStr == "get all groups for a user")
            {
                var uerg = stream.GetAllUsersGroups();
                if (Session.GetbLoggedIn())
                {
                    StringBuilder printStr = new StringBuilder();
                    printStr.Append($"Username: {usernameText.Text}");
                    printStr.Append("\n ________________________________");
                    foreach (var item in uerg[usernameText.Text])
                    {
                        printStr.Append("\n");
                        printStr.Append(item);
                    }
                    result.Text = printStr.ToString();
                }
                else
                {
                    result.Text = "Ikke logged ind :(";
                }
            }

            else if (actualStr == "get specific data from user")
            {
                var specific = stream.GetSpecifik(usernameText.Text);
                if (Session.GetbLoggedIn())
                {
                    StringBuilder printStr = new StringBuilder();
                    printStr.Append($"Username: {usernameText.Text}");
                    printStr.Append("\n ________________________________");
                    foreach (var item in specific[usernameText.Text])
                    {
                        printStr.Append("\n");
                        printStr.Append(item);
                    }
                    result.Text = printStr.ToString();
                }
                else
                {
                    result.Text = "Ikke logged ind :(";
                }
            }
            else if (actualStr == "get user")
            {
                if (Session.GetbLoggedIn())
                {
                var userGet = stream.GetUser(stream.GetAllUsers()[0]);
                result.Text = userGet;
                }
                else
                {
                    result.Text = "Ikke logged ind :(";
                }
            }
        }
              
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            string strFile = result.Text;

            Printer trying = new Printer(strFile);
            trying.Show();

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            editGivenName.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Visible;


        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            ADStream stream = new ADStream(ip, user, password);
            stream.UpdateUserData(usernameText.Text, editGivenName.Text);
        }

        //
    }
}
