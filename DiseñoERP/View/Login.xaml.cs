using LITTLERP;
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
using System.Windows.Shapes;

namespace DiseñoERP
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            LITTLERP.Domain.User user = new LITTLERP.Domain.User();
            View.MainWindow viewGeneral = null;

            user.name = txtUser.Text;
            user.password = useful.GetSHA256(pwdPassword.Password);

            Boolean exist = user.check();

            if (exist)
            {
                user.id_user = user.readIdUser();
                user.readPermissions();
                viewGeneral = new View.MainWindow(user);
                viewGeneral.setNameDate(user.name);
                viewGeneral.Show();
                this.Close();
            }
            else
                MessageBox.Show("User not found.");
        }

        /// <summary>
        /// Handles the 1 event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtUser.Text = "";
            pwdPassword.Password = "";
        }
    }
}
