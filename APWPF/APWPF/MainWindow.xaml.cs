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
        public MainWindow()
        {
            InitializeComponent();
            if(!Session.GetbLoggedIn())
            {
                Login loginWindow = new Login();
                loginWindow.Show();
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = "192.168.132.71";
            string user = "Administrator";
            string password = "Password1";

            ADStream stream = new ADStream(ip, user, password);

            var userGet = stream.GetUser(stream.GetAllUsers()[0]);
            if(Session.GetbLoggedIn())
            {
                result.Text = userGet;
            }
            else
            {
                result.Text = "Ikke logged ind :(";
            }
            
        }
    }
}
