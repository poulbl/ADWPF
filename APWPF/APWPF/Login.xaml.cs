
using System.Windows;

namespace ADWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(Session.Login(username.Text, password.Text))
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }
    }
}
