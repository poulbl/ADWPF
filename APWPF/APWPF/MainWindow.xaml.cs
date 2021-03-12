using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            users.SelectionChanged += OpenUserDetails;
        }

        // Creates window for displaying user details for print. UserDetails is not completed.
        private void OpenUserDetails(object sender, SelectionChangedEventArgs e)
        {
            ADStream stream = new ADStream(ip, user, password);
            Dictionary<string, List<string>> userdata = stream.GetAllData(users.SelectedItem.ToString());
            UserDetails userDetails = new UserDetails(userdata[$"{users.SelectedItem}"]);
            int i = 0;
            userDetails.Show();
        }

        // Event for clicking the search button.
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> usersList = new List<string>();

            ADStream stream = new ADStream(ip, user, password);

            // Get a workable string from the combobox to indicate which method we want to use.
            var comboBoxSelect = functionBox.SelectedItem.ToString();
            var comboBoxString = comboBoxSelect.Split(':');
            string actualStr = comboBoxString[1].Trim().ToLower();

            // Add users to ListBox.
            if (Session.GetbLoggedIn())
            {
                SearchResultCollection searchResult = stream.GetUsers(usernameText.Text);
                if (searchResult != null)
                {
                    for (int i = 0; i < searchResult.Count; i++)
                    {
                        usersList.Add(searchResult[i].Properties["name"][0].ToString());
                    }
                    users.ItemsSource = usersList;
                }
            }
            else
            {
                result.Text = "Ikke logged ind :(";
            }

            if (actualStr == "get all data for user")
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

            // GET ALL USERS
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

            // GET USERS GROUPS
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

            // GET SPECIFIC
            else if (actualStr == "get specific data from user")
            {
                var specific = stream.GetSpecific(usernameText.Text);
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
            // GET USER
            else if (actualStr == "get user")
            {
                if (Session.GetbLoggedIn())
                {
                var userGet = stream.GetUser();
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
