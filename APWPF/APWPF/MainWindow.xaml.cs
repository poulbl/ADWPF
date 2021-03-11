using System;
using System.Collections.Generic;
using System.DirectoryServices;
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
        }

        private void OpenUserDetails(object sender, SelectionChangedEventArgs e)
        {
            ADStream stream = new ADStream(ip, user, password);
            Dictionary<string, List<string>> userdata = stream.GetAllDataFromUser(users.SelectedItem.ToString());
            UserDetails userDetails = new UserDetails(userdata[$"{users.SelectedItem}"]);
            int i = 0;
            userDetails.Show();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> usersList = new List<string>();

            ADStream stream = new ADStream(ip, user, password);

            if (Session.GetbLoggedIn())
            {
                result.Text = "";
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

        }
        private void BtnPrint_Click(object sender, RoutedEventArgs e)

        {
            string strFile = result.Text;

            Printer trying = new Printer(strFile);
            trying.Show();

        }
    }
}
