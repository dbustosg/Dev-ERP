using LITTLERP.Domain;
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

namespace DiseñoERP.View
{
    /// <summary>
    /// Lógica de interacción para InsertUser.xaml
    /// </summary>
    public partial class InsertUser : Window
    {
        /// <summary>
        /// The roles
        /// </summary>
        private List<Role> roles = new List<Role>();
        /// <summary>
        /// The user
        /// </summary>
        private User user;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertUser"/> class.
        /// </summary>
        /// <param name="userApp">The user application.</param>
        public InsertUser(User userApp)
        {
            InitializeComponent();

            this.user = userApp;

            fillListBox();
        }

        /// <summary>
        /// Fills the ListBox.
        /// </summary>
        private void fillListBox()
        {
            Role role = new Role();
            role.readAll();

            foreach (Role roleItem in role.manage.list)
                if (roleItem.id_role != 1 && (roleItem.id_role != 2 || !user.checkUserRole(roleItem)))
                    lb_roles.Items.Add(roleItem);
        }

        /// <summary>
        /// Checks the name user.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Boolean checkNameUser(String name)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(iduser)", "USERS", "NAME = '" + name + "'"));

            return count > 0;
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!checkNameUser(txtUser.Text))
            {
                LITTLERP.Domain.User user = new LITTLERP.Domain.User();
                user.name = txtUser.Text;
                user.password = "1234"; //Default password

                if (roles.Count > 0)
                {
                    user.insert(); //Se crea antes, porque tiene que existir para poder insertar sus roles
                    user.id_user = user.readIdUser(); //Obtenemos el id del usuario, ya que se le asigna al insertarlo, no al crear el objeto

                    foreach (Role role in roles)
                        user.insertUserRole(role);

                }
                else
                    MessageBox.Show("Error! Any role has been selected!");

                MessageBox.Show("User has been inserted successfully!");
            }
            else if (checkNameUser(txtUser.Text))
                MessageBox.Show("Error! The name already exists!");

            this.Close();
        }

        /// <summary>
        /// Handles the 1 event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lb_roles.Items.Clear();
            lb_rolesAssigned.Items.Clear();
            txtUser.Text = "";

            fillListBox();
        }

        /// <summary>
        /// Handles the Click event of the btnAdd_Role control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Role_Click(object sender, RoutedEventArgs e)
        {
            if (lb_roles.SelectedItem != null)
            {
                Role r = lb_roles.SelectedItem as Role;
                roles.Add(r);

                lb_rolesAssigned.Items.Add(lb_roles.SelectedItem);
                lb_roles.Items.Remove(lb_roles.SelectedItem);

            }
            else
            {
                MessageBox.Show("Any role has been selected!");
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDelete_Role control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDelete_Role_Click(object sender, RoutedEventArgs e)
        {
            if (lb_rolesAssigned.SelectedItem != null)
            {
                Role r = lb_rolesAssigned.SelectedItem as Role;
                roles.Remove(r);
                lb_roles.Items.Add(lb_rolesAssigned.SelectedItem);
                lb_rolesAssigned.Items.Remove(lb_rolesAssigned.SelectedItem);
            }
            else
            {
                MessageBox.Show("Any role has been selected!");
            }
        }
    }
}
