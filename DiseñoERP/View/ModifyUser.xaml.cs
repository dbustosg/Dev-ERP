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
    /// Lógica de interacción para ModifyUser.xaml
    /// </summary>
    public partial class ModifyUser : Window
    {
        /// <summary>
        /// The user
        /// </summary>
        private static User user;
        /// <summary>
        /// The identifier user
        /// </summary>
        private static int idUser;
        /// <summary>
        /// The roles
        /// </summary>
        private List<Role> roles = new List<Role>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyUser"/> class.
        /// </summary>
        /// <param name="idUserModified">The identifier user modified.</param>
        /// <param name="idUserLog">The identifier user log.</param>
        public ModifyUser(int idUserModified, int idUserLog)
        {
            InitializeComponent();

            //Cogemos el id del usuario que está al mando
            idUser = idUserLog;

            //Inicializamos el usuario a modificar
            user = new User();
            user.id_user = idUserModified;
            user.readUser();

            txtUser.Text = user.name; //Asignamos el nombre
            txtUser.IsEnabled = false; //Para no poder cambiar el nombre

            fillListsBoxRoles(); //Rellenamos los listBox

        }

        /// <summary>
        /// Fills the lists box roles.
        /// </summary>
        private void fillListsBoxRoles()
        {
            Role role = new Role();
            role.readAll();

            foreach (Role roleItem in role.manage.list)
            {
                if (roleItem.id_role != 1)
                {
                    if (roleItem.id_role != 2 || idUser == 1)
                    {
                        if (!user.checkUserRole(roleItem)) // Si no tiene asignado ese rol lo ponemos en el listBox izquierdo
                            lb_roles.Items.Add(roleItem);
                        else // Si tiene asignado ese rol lo ponemos en el listBox derecho
                        {
                            lb_rolesAssigned.Items.Add(roleItem);
                            roles.Add(roleItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            user.deleteUserRole(); //Borramos todos los roles que tuviese asignado anteriormente

            //Insertamos los roles que estén actualmente en la lista de roles
            foreach (Role role in roles)
                user.insertUserRole(role);

            MessageBox.Show("User has been modified successfully!");
            this.Close(); //Cerramos la pestaña

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
        /// Handles the 1 event of the btnDelete_Role_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDelete_Role_Click_1(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            lb_roles.Items.Clear();
            lb_rolesAssigned.Items.Clear();

            fillListsBoxRoles(); //Se reinician los ListBox
        }
    }
}
