using DiseñoERP.Domain;
using DiseñoERP.Domain.Manage;
using DiseñoERP.Persistence;
using DiseñoERP.View;
using LITTLERP;
using LITTLERP.Domain;
using LITTLERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;


namespace DiseñoERP.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The user application
        /// </summary>
        private static User userApp;
        /// <summary>
        /// The interruptor BTN save
        /// </summary>
        private Boolean interruptorBtnSave;
        /// <summary>
        /// The interruptor orders
        /// </summary>
        private int interruptorOrders;
        /// <summary>
        /// The customer selected
        /// </summary>
        public Customer customerSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public MainWindow (User user)
        {
            InitializeComponent();

            userApp = user;
            interruptorOrders = 2;
            
            //ITEM -> USERS
            //Comprobamos si tiene el permiso "Show user"

            if (userApp.id_user == 1 || userApp.listPermissions.Contains(3))
            {
                User aux = new User();
                aux.readAll(); //Cargamos los valores de la base de datos en List
                dgvUsers.ItemsSource = aux.manage.list; //se le añade la lista al dataGrid
                fillComboBoxSearchUser();

                //Comprobamos permisos para habilitar botones
                checkModify();
                checkDelete();
                checkAddUser();
            }
            else
                tbi_User.IsEnabled = false;


            //ITEM -> PROFILE
            txtUser.IsEnabled = false;
            txtUserID.IsEnabled = false;
            txtUser.Text = userApp.name;
            txtUserID.Text = Convert.ToString(userApp.id_user);

            fillListsBoxRoles();

            //ITEM -> customers

            if (userApp.id_user == 1 || userApp.listPermissions.Contains(7))
            {
                Customer customer = new Customer();
                customer.readAll();
                dgv_customers.ItemsSource = customer.manage.listCustomer;
                bloquearCampos();
                fillComboBoxSearchBy();

                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(5))
                    btnAddCustomer.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(6))
                    btnModify_Customer.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(8))
                    btnDeleteCustomer.IsEnabled = false;
            }
            else
                tbi_Customer.IsEnabled = false;


            //ITEM -> products

            if (userApp.id_user == 1 || userApp.listPermissions.Contains(11))
            {
                bloquearEsclavoProduct();
                fillComboBoxSearchBy_Product();

                Product product = new Product();
                product.readAll();
                dgv_products.ItemsSource = product.manage.listProducts;

                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(9))
                    btnAddProduct.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(10))
                    btnModifyProduct.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(12))
                    btnDelete_Product.IsEnabled = false;
            }
            else
                tbi_Products.IsEnabled = false;
            

            //ITEM -> supliers
            Suplier suplier = new Suplier();
            suplier.readAll();
            dgv_Supliers.ItemsSource = suplier.manage.list;
            bloquearEsclavoSuplier();

            //ITEM -> orders
            if (userApp.id_user == 1 || userApp.listPermissions.Contains(15))
            {
                Order order = new Order();
                order.readAll();
                dgv_orders.ItemsSource = order.manage.orderList;

                fillComboBoxSearchBy_Order();
                bloquearSlaveOrders();

                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(13))
                    btnAddOrders.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(14))
                    btnModifyOrders.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(16))
                    btnDelete_Orders.IsEnabled = false;
            }
            else
                tbi_Orders.IsEnabled=false;


            //ITEM -> Invoices
            if (userApp.id_user == 1 || userApp.listPermissions.Contains(15))
            {
                Invoice invoice = new Invoice();
                invoice.readAll();
                dgv_invoices.ItemsSource = invoice.manage.listInvoices;
                fillComboBoxSearchBy_Invoice();

                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(18))
                    btnAddInvoice.IsEnabled = false;
                if (userApp.id_user != 1 && !userApp.listPermissions.Contains(19))
                    btnDeleteInvoices.IsEnabled = false;
            } 
            else
                tbi_Invoices.IsEnabled=false;   

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="interruptor">The interruptor.</param>
        public MainWindow(int interruptor)
        {
            InitializeComponent();

            interruptorOrders = interruptor;
            

            if (interruptorOrders == 0)
            {
                customerSelected = new Customer();

                tbi_Customer.IsSelected = true;
                tbi_Home.IsEnabled = false;
                tbi_Orders.IsEnabled = false;
                tbi_Products.IsEnabled = false;
                tbi_Supliers.IsEnabled = false;
                tbi_User.IsEnabled = false;
                Profile.IsEnabled = false;
                tbi_Invoices.IsEnabled = false;

                Customer customer = new Customer();
                customer.readAll();
                dgv_customers.ItemsSource = customer.manage.listCustomer;
                bloquearCampos();
                fillComboBoxSearchBy();

               
                btnAddCustomer.IsEnabled = false;
                btnModify_Customer.IsEnabled = false;
                btnDeleteCustomer.IsEnabled = false;
            }
            else if (interruptorOrders == 1)
            {
                tbi_Customer.IsEnabled = false;
                tbi_Home.IsEnabled = false;
                tbi_Orders.IsEnabled = false;
                tbi_Products.IsSelected = true;
                tbi_Supliers.IsEnabled = false;
                tbi_User.IsEnabled = false;
                Profile.IsEnabled = false;

                bloquearEsclavoProduct();
                fillComboBoxSearchBy_Product();

                Product product = new Product();
                product.readAll();
                dgv_products.ItemsSource = product.manage.listProducts;

                btnAddProduct.IsEnabled = false;
                btnModifyProduct.IsEnabled = false;
                btnDelete_Product.IsEnabled = false;
            }
        }

        /*
         * Canal de notificaciones
         */
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: System.Windows.Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 30);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
        });

      
        /// <summary>
        /// Handles the Click event of the btn_LogOut control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btn_LogOut_Click (object sender, RoutedEventArgs e)
            {
                App.Current.Shutdown();
            }


        //ITEM -> USERS
        //Control de los 3 botones        
        /// <summary>
        /// Checks the modify.
        /// </summary>
        public void checkModify()
        {
            if (!userApp.listPermissions.Contains(2) && userApp.id_user != 1)
                btnModify.IsEnabled = false;
        }

        /// <summary>
        /// Checks the delete.
        /// </summary>
        public void checkDelete()
        {
            if (!userApp.listPermissions.Contains(4) && userApp.id_user != 1)
                btnDelete.IsEnabled = false;
        }

        /// <summary>
        /// Checks the add user.
        /// </summary>
        public void checkAddUser()
        {
            if (!userApp.listPermissions.Contains(1) && userApp.id_user != 1)
                btnAdd.IsEnabled = false;
        }
      
        /// <summary>
        /// Sets the name and the date.
        /// </summary>
        /// <param name="name">The name.</param>
        public void setNameDate(String name)
        {
            txtNameUser.Text = name;
            txtDateUser.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Fills the user data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillUserData(object sender, RoutedEventArgs e)
        {
            if (dgvUsers.SelectedItems.Count > 0)
            {
                User user = (User)dgvUsers.SelectedItem;

                tb_id_user.Text = user.readIdUser().ToString();
                tb_name_user.Text = user.name.ToString();

                fillListsBoxRoles_ItemSelected();
            }
        }

        /// <summary>
        /// Fills the lists box roles item selected.
        /// </summary>
        private void fillListsBoxRoles_ItemSelected()
        {
            lb_roles_users.Items.Clear();

            if(dgvUsers.SelectedItems.Count > 0)
            {
                User user = (User)dgvUsers.SelectedItem;

                Role role = new Role();
                role.readAll();

                foreach (Role roleItem in role.manage.list)
                    if (user.checkUserRole(roleItem))
                        lb_roles_users.Items.Add(roleItem);
            }  
        }

        /// <summary>
        /// Fills the ComboBox search user.
        /// </summary>
        private void fillComboBoxSearchUser()
        {
            List<string> list = new List<string>();
            list.Add("IDUSER");
            list.Add("Name");

            cbSearch_User.ItemsSource = list;
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            InsertUser insertUser = new InsertUser(userApp);
            insertUser.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnModify control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsers.SelectedItems.Count > 0)
            {
                User data = (User)dgvUsers.SelectedItem;

                if (userApp.id_user == 1) // Si es superAdmin puede modificar a cualquier usuario
                {
                    if (userApp.id_user != data.id_user)
                    {
                        ModifyUser modifyUser = new ModifyUser(data.id_user, userApp.id_user);
                        modifyUser.Show();
                    }
                    else
                        System.Windows.MessageBox.Show("Root can't be modify");
                }
                else
                {
                    if (userApp.id_user == data.id_user)
                        System.Windows.MessageBox.Show("You can't modify yourself!");
                    else if ((checkPermission(data, 1) || checkPermission(data, 2))) //Comprobamos si el usuario seleccionado es admin o superAdmin
                        System.Windows.MessageBox.Show("An admin can't modify another admin or superadmin!");
                    else
                    {
                        ModifyUser modifyUser = new ModifyUser(data.id_user, userApp.id_user);
                        modifyUser.Show();
                    }
                }
            }
            else
                System.Windows.MessageBox.Show("You must select at least one row");
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<User> data = new List<User>();
            User userDelete = new User();
            int indice = 0;

            if (dgvUsers.SelectedItems.Count > 0)
            {
                data = (List<User>)dgvUsers.ItemsSource;

                for (int i = 0; i < dgvUsers.SelectedItems.Count; i++)
                {
                    if (checkPermission(userApp, 1)) // Si es superAdmin puede eliminar a cualquier usuario
                    {
                        indice = dgvUsers.Items.IndexOf(dgvUsers.SelectedItems[i]);
                        data.RemoveAt(indice);
                        User row = (User)dgvUsers.SelectedItems[i];
                        row.delete();
                    }
                    else
                    {
                        indice = dgvUsers.Items.IndexOf(dgvUsers.SelectedItems[i]);
                        userDelete = data.ElementAt(indice);

                        if (userApp.id_user == userDelete.id_user)
                            System.Windows.MessageBox.Show("You can't delete yourself!");
                        else if (checkPermission(userDelete, 1) || checkPermission(userDelete, 2)) //Comprobamos si el usuario seleccionado es admin o superAdmin
                            System.Windows.MessageBox.Show("An admin can't delete another admin or superadmin!");
                        else
                        {
                            data.RemoveAt(indice);
                            User row = (User)dgvUsers.SelectedItems[i];
                            row.delete();
                        }
                    }

                }

                dgvUsers.ItemsSource = null; //Lo vaciamos para que no esten los de antes + los de ahora
                dgvUsers.ItemsSource = data; //Lo llenamos otra vez con los datos
            }
            else
                System.Windows.MessageBox.Show("You must select at least one row");

        }

        /// <summary>
        /// Handles the Click event of the btnUpdateWindows control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnUpdateWindows_Click(object sender, RoutedEventArgs e)
        {
            User aux = new User();
            aux.readAll(); //Cargamos los valores de la base de datos en List
            dgvUsers.ItemsSource = aux.manage.list; //se le añade la lista al dataGrid
        }

        /// <summary>
        /// BTNs the search user.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearchUser(object sender, RoutedEventArgs e)
        {
            String campo = cbSearch_User.SelectedItem as String;
            String busqueda = tbSearchUser.Text;

            if (campo != null || busqueda != "Search...")
            {
                dgvUsers.ItemsSource = null;

                User user = new User();
                user.readSpecificUser(campo, busqueda);
                dgvUsers.ItemsSource = user.manage.list;
            }
            else
                System.Windows.MessageBox.Show("Wrong fields!");
        }

        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        private Boolean checkPermission(User user, int role)
        {
            Role rol = new Role();
            rol.id_role = role;

            return user.checkUserRole(rol);
        }


        //ITEM -> PROFILE
        
        /// <summary>
        /// Handles the Click event of the btnChangePassword control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (pwdOldPassword.Password != "")
            {
                userApp.password = useful.GetSHA256(pwdOldPassword.Password);

                if (userApp.check())
                {
                    if (pwdNewPassword.Password.Equals(pwdRepeatNewPassword.Password))
                    {
                        userApp.password = pwdNewPassword.Password;
                        userApp.modifyUser();

                        notifier.ShowSuccess("Password has been changed! Log in again, please.");
                    }
                    else
                        notifier.ShowWarning("Error: Passwords do not match!");
                }
                else
                    notifier.ShowError("Error: incorrect password!");
            }

            if (txtNewUserName.Text != "")
            {
                User user = new User();
                user.name = txtNewUserName.Text;
                user.id_user = userApp.id_user;

                if (user.readIdUser() > 0)
                    notifier.ShowError("This username already exists");
                else
                {
                    userApp.manage.modifyNameUser(user);
                    notifier.ShowSuccess("Username has been changed! Log in again, please.");
                }
            }
        }

        /// <summary>
        /// Fills the lists box roles.
        /// </summary>
        private void fillListsBoxRoles()
        {
            Role role = new Role();
            role.readAll();

            foreach (Role roleItem in role.manage.list)
                if (userApp.checkUserRole(roleItem))
                    lb_roles_profile.Items.Add(roleItem);        
        }

        //ITEM -> CUSTOMERS

        private void fillCustomerTable(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            String[] arrayNIF = { "11921079H", "24406153W", "07083860K", "85914263V", "88187228F", "89905737W" };
            String[] arrayName = { "Jesus", "Andres", "Sonia", "Alba", "Jorge", "Luis" };
            String[] arraySurname = { "Bustos", "Guzman", "Soria", "Madrid", "Cordoba", "Perez" };
            String[] arrayAddress = { "C/Baja", "C/Altagracia", "C/Galdos", "C/Toledo", "C/Real", "C/Calatrava" };
            int[] arrayPhone = { 645123789, 620159487, 630485179, 752031448, 695283007, 612784159 };
            String[] arrayZipCode = { "13730", "13420", "13001", "13003", "13004", "13740" };
            String[] arrayEmail = { "zollezouwigu-6742@yopmail.com", "breivuyalloihe-7657@yopmail.com", "bebreuddowiga-8604@yopmail.com", "nubreddagoucoi-5931@yopmail.com", "coimmuquipudou-9341@yopmail.com", "loufurazuxa-8754@yopmail.com" };

            Customer c;
            ZipCode zipCode;
            try
            {
                for (int i = 0; i < 10000; i++)
                {
                    c = new Customer();
                    zipCode = new ZipCode();

                    c.name = arrayName[rnd.Next(0, 6)];
                    c.surname = arraySurname[rnd.Next(0, 6)];
                    c.address = arrayAddress[rnd.Next(0, 6)];
                    c.phone = arrayPhone[rnd.Next(0, 6)];
                    c.email = arrayEmail[rnd.Next(0, 6)];
                    c.NIF = arrayNIF[rnd.Next(0, 6)];

                    zipCode.zipCode = arrayZipCode[rnd.Next(0, 6)];
                    c.zipcode = zipCode;

                    c.insertCustomer();
                }
            } catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Empty fields
        /// </summary>
        private void vaciarCampos()
        {
            tb_NIF.Text = "";
            tb_nameCustomer.Text = "";
            tb_surname.Text = "";
            tb_email.Text = "";
            tb_phone.Text = "";
            tb_address.Text = "";
            tb_ZipCode.Text = "";
            cboRegions.ItemsSource = null;
            cboStates.ItemsSource = null;
            cboCities.ItemsSource = null;
            cboZipCodes.ItemsSource = null;
        }

        /// <summary>
        /// Block fields
        /// </summary>
        private void bloquearCampos()
        {
            tb_NIF.IsEnabled             = false;
            tb_nameCustomer.IsEnabled    = false;
            tb_surname.IsEnabled         = false;
            tb_phone.IsEnabled           = false;
            tb_email.IsEnabled           = false;
            tb_address.IsEnabled         = false;
            tb_ZipCode.IsEnabled         = false;
            cboRegions.IsEnabled         = false;
            cboStates.IsEnabled          = false;
            cboCities.IsEnabled          = false;
            cboZipCodes.IsEnabled        = false;
            btnSearch_ZipCode.IsEnabled  = false;
            btnSave_Customers.IsEnabled  = false;
            btnChange_Location.IsEnabled = false;
        }

        /// <summary>
        /// Activate fields
        /// </summary>
        private void activarCampos()
        {
            tb_NIF.IsEnabled             = true;
            tb_nameCustomer.IsEnabled    = true;
            tb_surname.IsEnabled         = true;
            tb_phone.IsEnabled           = true;
            tb_email.IsEnabled           = true;
            tb_address.IsEnabled         = true;
            tb_ZipCode.IsEnabled         = true;
            cboRegions.IsEnabled         = true;
            cboStates.IsEnabled          = true;
            cboCities.IsEnabled          = true;
            cboZipCodes.IsEnabled        = true;
            btnSearch_ZipCode.IsEnabled  = true;
            btnSave_Customers.IsEnabled  = true;
            btnChange_Location.IsEnabled = true;
        }

        /// <summary>
        /// Fills the customer data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillCustomerData(object sender, RoutedEventArgs e)
        {
            if (dgv_customers.SelectedItems.Count > 0)
            {
                bloquearCampos();

                Customer customer = dgv_customers.SelectedItem as Customer;

                tb_NIF.Text = customer.NIF;
                tb_NIF.IsEnabled = false;
                tb_nameCustomer.Text = customer.name;
                tb_surname.Text = customer.surname;
                tb_email.Text = customer.email;
                tb_phone.Text = customer.phone.ToString();
                tb_address.Text = customer.address;

                CustomerManage customerManage = new CustomerManage();

                List<LITTLERP.Domain.Region> region = new List<LITTLERP.Domain.Region>();
                region.Add(customerManage.getRegionWithRef(customer.ref_zip_code.ToString()));

                List<State> state = new List<State>();
                state.Add(customerManage.getStateWithRef(customer.ref_zip_code.ToString()));

                List<City> city = new List<City>();
                city.Add(customerManage.getCityWithRef(customer.ref_zip_code.ToString()));

                List<ZipCode> zipCode = new List<ZipCode>();
                zipCode.Add(customerManage.getZipCodeWithRef(customer.ref_zip_code.ToString()));

                cboRegions.ItemsSource = region;
                cboRegions.SelectedIndex = 0;

                cboStates.ItemsSource = state;
                cboStates.SelectedIndex = 0;

                cboCities.ItemsSource = city;
                cboCities.SelectedIndex = 0;

                cboZipCodes.ItemsSource = zipCode;
                cboZipCodes.SelectedIndex = 0;

            }
        }

        // Buttons

        /// <summary>
        /// Handles the Customer event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Customer(object sender, RoutedEventArgs e)
        {
            activarCampos();
            vaciarCampos();
            fillComboRegions();
        }

        /// <summary>
        /// Handles the Customer event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSave_Customer(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            
            customer.NIF = tb_NIF.Text;
            customer.name = tb_nameCustomer.Text;
            customer.surname = tb_surname.Text;
            customer.email = tb_email.Text;
            customer.phone = Convert.ToInt32(tb_phone.Text);
            customer.address = tb_address.Text;
            customer.zipcode = cboZipCodes.SelectedItem as ZipCode;
            

            if (tb_NIF.IsEnabled == false)
            {
                customer.modifyCustomer();
                System.Windows.MessageBox.Show("The customer has been modified!");
                btnModify_Customer.IsEnabled = false;
                bloquearCampos();
            }
            else if(customer.checkCustomer())
                System.Windows.MessageBox.Show("The customer already exists!");
            else
            {
                customer.insertCustomer();
                vaciarCampos();
                System.Windows.MessageBox.Show("The customer has been inserted!");
                bloquearCampos();
            }   
        }

        /// <summary>
        /// Handles the Customer event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDelete_Customer(object sender, RoutedEventArgs e)
        {
            if(dgv_customers.SelectedIndex != -1)
            {
                Customer customer = dgv_customers.SelectedItem as Customer;
                customer.deleteCustomer();
                System.Windows.MessageBox.Show("The customer has been deleted!");
            }
            else
            {
                System.Windows.MessageBox.Show("You must selected any customer!");
            }
        }

        /// <summary>
        /// Handles the Customer event of the click_btnModify control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void click_btnModify_Customer(object sender, RoutedEventArgs e)
        {
            if (dgv_customers.SelectedIndex != -1)
            {
                activarCampos();
                tb_NIF.IsEnabled = false;
            }
            else
                System.Windows.MessageBox.Show("You must selected any customer!");
        }

        /// <summary>
        /// Handles the Click event of the btnUpdateWindowsCustomer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnUpdateWindowsCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer aux = new Customer();
            aux.readAll(); //Cargamos los valores de la base de datos en List
            dgv_customers.ItemsSource = aux.manage.listCustomer; //se le añade la lista al dataGrid
        }

        /// <summary>
        /// BTNs the change location.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnChangeLocation(object sender, RoutedEventArgs e)
        {
            cboRegions.ItemsSource = null;
            cboStates.ItemsSource = null;
            cboCities.ItemsSource = null;
            cboZipCodes.ItemsSource = null;
            fillComboRegions();
        }

        /// <summary>
        /// BTNs the search customer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearchCustomer(object sender, RoutedEventArgs e)
        {
            String campo = cb_SearchBy.SelectedItem as String;
            String busqueda = tb_SearchCustomer.Text;

            if (campo != null || busqueda != "Search...")
            {
                dgv_customers.ItemsSource = null;

                Customer customer = new Customer();
                customer.readSpecificCustomer(campo,busqueda);
                dgv_customers.ItemsSource = customer.manage.listCustomer;
            }
            else
                System.Windows.MessageBox.Show("Wrong fields!");
        }

        /// <summary>
        /// Selects the customer order.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void selectCustomerOrder(object sender, RoutedEventArgs e)
        {
            if (interruptorOrders == 0)
            {
                customerSelected = dgv_customers.SelectedItem as Customer;
                Singleton.Instance.insertOrder.customerOrder = customerSelected;
                Singleton.Instance.insertOrder.fillComboBoxCustomer();
                Close();
            }
        }

        // Combo box        
        /// <summary>
        /// Fills the ComboBox search by.
        /// </summary>
        private void fillComboBoxSearchBy()
        {
            List<string> list = new List<string>();
            list.Add("NIF");
            list.Add("Name");
            list.Add("Surname");

            cb_SearchBy.ItemsSource = list;
        }

        /// <summary>
        /// Fills the combo regions.
        /// </summary>
        private void fillComboRegions()
        {
            CustomerManage customer = new CustomerManage();
            customer.readAllRegions();

            cboRegions.ItemsSource = customer.listRegion;
            cboRegions.SelectedIndex = 0;
        }

        /// <summary>
        /// Fills the combo states.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillComboStates(object sender, RoutedEventArgs e)
        {
            if(cboRegions.SelectedIndex != -1)
            {
                LITTLERP.Domain.Region region = cboRegions.SelectedItem as LITTLERP.Domain.Region;

                CustomerManage customer = new CustomerManage();
                customer.readStates(region.id_region);

                cboStates.ItemsSource = customer.listStates;
            }
        }

        /// <summary>
        /// Fills the combo cities.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillComboCities(object sender, RoutedEventArgs e)
        {
            if(cboStates.SelectedIndex != -1)
            {
                State state = cboStates.SelectedItem as State;

                CustomerManage customer = new CustomerManage();
                customer.readCities(state.id_state);

                cboCities.ItemsSource = customer.listCities;
            }
        }

        /// <summary>
        /// Fills the combo zip code.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillComboZipCode(object sender, RoutedEventArgs e)
        {
            if(cboCities.SelectedIndex != -1)
            {
                State state = cboStates.SelectedItem as State;
                City city = cboCities.SelectedItem as City;

                CustomerManage customer = new CustomerManage();
                customer.readZipCodes(city.id_city, state.id_state);

                cboZipCodes.ItemsSource = customer.listZipCode;

                cboZipCodes.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSearchZipCode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearchZipCode_Click(object sender, RoutedEventArgs e)
        {
            if (!tb_ZipCode.Text.Equals("") || tb_ZipCode == null)
            {
                CustomerManage customer = new CustomerManage();
                if (customer.checkZipCode(tb_ZipCode.Text))
                {
                    Customer c = customer.fillWithZipCode(tb_ZipCode.Text);

                    List<LITTLERP.Domain.Region> region = new List<LITTLERP.Domain.Region>(); 
                    region.Add(c.region);

                    List<State> state = new List<State>();
                    state.Add(c.state);

                    List<City> city = new List<City>();
                    city.Add(c.city);

                    List<ZipCode> zipCode = new List<ZipCode>();
                    zipCode.Add(c.zipcode);

                    cboRegions.ItemsSource = region;
                    cboRegions.SelectedIndex = 0;

                    cboStates.ItemsSource = state;
                    cboStates.SelectedIndex = 0;

                    cboCities.ItemsSource = city;
                    cboCities.SelectedIndex = 0;

                    cboZipCodes.ItemsSource = zipCode;
                    cboZipCodes.SelectedIndex = 0;
                }
                else
                    System.Windows.MessageBox.Show("Error: ZIP Code not exists");
            }
            else
                System.Windows.MessageBox.Show("Error: ZIP Code empty");
        }

        // SUPLIERS
        
        /// <summary>
        /// Fills the suplier data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillSuplierData(object sender, RoutedEventArgs e)
        {
            if (dgv_Supliers.SelectedItems.Count > 0)
            {
                Suplier suplier = dgv_Supliers.SelectedItem as Suplier;

                tb_DNI_suplier.Text = Convert.ToString(suplier.DNI);
                tb_name_suplier.Text = suplier.Name;
                tb_surname_suplier.Text= suplier.Surname;
                tb_phone_suplier.Text = Convert.ToString(suplier.Phone);
                tb_email_suplier.Text = suplier.Email;
                tb_city_suplier.Text = suplier.Location;
                tb_adress_suplier.Text = suplier.Direction;
            }
        }

        /// <summary>
        /// Block slave suplier.
        /// </summary>
        private void bloquearEsclavoSuplier()
        {
            tb_DNI_suplier.IsEnabled = false;
            tb_name_suplier.IsEnabled = false;
            tb_surname_suplier.IsEnabled = false;
            tb_phone_suplier.IsEnabled = false;
            tb_email_suplier.IsEnabled = false;
            tb_city_suplier.IsEnabled = false;
            tb_adress_suplier.IsEnabled = false;
        }

        //////////// PRODUCTS ///////////////////////

        /// <summary>
        /// Block slave product.
        /// </summary>
        private void bloquearEsclavoProduct()
        {
            tb_idProduct.IsEnabled = false;
            tb_descriptionProduct.IsEnabled = false;
            tb_WholesalePrice.IsEnabled = false;
            tb_PriceProduct.IsEnabled = false;
            cb_material.IsEnabled = false;
            cb_MessuresProduct.IsEnabled = false;
            btnSaveProduct.IsEnabled = false;
        }

        /// <summary>
        /// Activate slave product.
        /// </summary>
        private void activarEsclavoProduct()
        {
            tb_descriptionProduct.IsEnabled = true;
            tb_WholesalePrice.IsEnabled = true;
            tb_PriceProduct.IsEnabled = true;
            cb_material.IsEnabled = true;
            cb_MessuresProduct.IsEnabled = true;
            btnSaveProduct.IsEnabled = true;
        }

        /// <summary>
        /// Empty product slave fields
        /// </summary>
        private void vaciarCamposEsclavoProduct()
        {
            tb_idProduct.Text = "";
            tb_descriptionProduct.Text = "";
            tb_WholesalePrice.Text = "";
            tb_PriceProduct.Text = "";

            ProductManage productManage = new ProductManage();
            productManage.readAllMaterials();
            productManage.readAllMessures();

            cb_material.ItemsSource = productManage.listMaterialProducts;
            cb_material.SelectedIndex = 0;

            cb_MessuresProduct.ItemsSource = productManage.listSizeProducts;
            cb_MessuresProduct.SelectedIndex = 0;
        }

        /// <summary>
        /// Fills the product data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillProductData (object sender, RoutedEventArgs e)
        {

            Product product = dgv_products.SelectedItem as Product;

            if(product != null)
            {
                tb_idProduct.Text = Convert.ToString(product.Id_product);
                tb_descriptionProduct.Text = product.Description;
                tb_WholesalePrice.Text = Convert.ToString(product.WholesalePrice);
                tb_PriceProduct.Text = Convert.ToString(product.Price);

                ProductManage productManage = new ProductManage();

                List<MaterialProducts> products = new List<MaterialProducts>();
                products.Add(productManage.readMaterialProduct(product.MaterialProduct.id_material));
                cb_material.ItemsSource = products;
                cb_material.SelectedIndex = 0;

                List<SizeProducts> sizes = new List<SizeProducts>();
                sizes.Add(productManage.readSizeProduct(product.SizeProduct.id_size));
                cb_MessuresProduct.ItemsSource = sizes;
                cb_MessuresProduct.SelectedIndex = 0;

                bloquearEsclavoProduct();
            }
            
        }

        /// <summary>
        /// Fills the ComboBox search by product.
        /// </summary>
        private void fillComboBoxSearchBy_Product()
        {
            List<string> list = new List<string>();
            list.Add("IDPRODUCT");
            list.Add("DESCRIPTION");

            cb_SearchBy_Product.ItemsSource = list;
        }

        /// <summary>
        /// Handles the Click event of the btnUpdateWindowsProduct control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnUpdateWindowsProduct_Click(object sender, RoutedEventArgs e)
        {
            Product aux = new Product();
            aux.readAll(); //Cargamos los valores de la base de datos en List
            dgv_products.ItemsSource = aux.manage.listProducts; //se le añade la lista al dataGrid
        }

        /// <summary>
        /// BTNs the new product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNewProduct(object sender, RoutedEventArgs e)
        {
            interruptorBtnSave = true;
            activarEsclavoProduct();
            vaciarCamposEsclavoProduct();
        }

        /// <summary>
        /// BTNs the delete product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDeleteProduct(object sender, RoutedEventArgs e)
        {
            if(dgv_products.SelectedItem != null)
            {
                Product product = dgv_products.SelectedItem as Product;
                product.deleteProduct();
                System.Windows.MessageBox.Show("The product has been deleted!");
            }
            else
                System.Windows.MessageBox.Show("You must selected any product!");

        }

        /// <summary>
        /// Handles the ModifyProduct event of the btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btn_ModifyProduct(object sender, RoutedEventArgs e)
        {
            if (dgv_products.SelectedItem != null)
            {
                interruptorBtnSave = false;
                activarEsclavoProduct();

                ProductManage manage = new ProductManage();
                manage.readAllMaterials();
                manage.readAllMessures();

                cb_material.ItemsSource = manage.listMaterialProducts;
                
                cb_MessuresProduct.ItemsSource = manage.listSizeProducts;

                Product product = dgv_products.SelectedItem as Product;
                cb_material.SelectedIndex = product.MaterialProduct.id_material - 1;
                cb_MessuresProduct.SelectedIndex = product.SizeProduct.id_size - 1;
            }
            else
                System.Windows.MessageBox.Show("You must selected any product!");
        }

        /// <summary>
        /// Handles the SaveProduct event of the btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btn_SaveProduct (object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            
            product.Description = tb_descriptionProduct.Text;
            product.WholesalePrice = Convert.ToDouble(tb_WholesalePrice.Text.Replace(".",","));
            product.Price = Convert.ToDouble(tb_PriceProduct.Text.Replace(".", ","));

            MaterialProducts material = cb_material.SelectedItem as MaterialProducts;
            product.MaterialProduct = material;

            SizeProducts sizeProducts = cb_MessuresProduct.SelectedItem as SizeProducts;
            product.SizeProduct = sizeProducts;

            if (interruptorBtnSave)
            {
                product.insertProduct();
                System.Windows.MessageBox.Show("The product has been added!");
            }
            else
            {
                product.Id_product = Convert.ToInt32(tb_idProduct.Text);
                product.modifyProduct();
            }
                
            bloquearEsclavoProduct();
        }

        /// <summary>
        /// BTNs the search product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearchProduct(object sender, RoutedEventArgs e)
        {
            String campo = cb_SearchBy_Product.SelectedItem as String;
            String busqueda = tb_SearchByProduct.Text;

            if (campo != null || busqueda != "Search...")
            {
                dgv_products.ItemsSource = null;

                Product product = new Product();
                product.readSpecificProduct(campo, busqueda);
                dgv_products.ItemsSource = product.manage.listProducts;
            }
            else
                System.Windows.MessageBox.Show("Wrong fields!");
        }

        /// <summary>
        /// Selects the product order.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void selectProductOrder(object sender, RoutedEventArgs e)
        {
            if (interruptorOrders == 1)
            {
                Product productSelected = dgv_products.SelectedItem as Product;
                Singleton.Instance.insertOrder.fillSlaveProduct(productSelected);
                Close();
            }
        }

        //////////////////// Orders /////////////////////////

        /// <summary>
        /// Handles the Click event of the btnAddOrders control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAddOrders_Click(object sender, RoutedEventArgs e)
        {
            Singleton.interruptor = false;
            Singleton.Instance.insertOrder.interruptorModify = false;
            Singleton.interruptorInvoice = false;
            Singleton.Instance.insertOrder.Show();
            Singleton.Instance.insertOrder.user = userApp;

        }

        /// <summary>
        /// Block the orders slave.
        /// </summary>
        private void bloquearSlaveOrders()
        {
            tb_ClientOrder.IsEnabled = false;
            tb_DateOrder.IsEnabled = false;
            tb_idOrder.IsEnabled = false;
            tb_TotalOrder.IsEnabled = false;
            tb_paymentMethod.IsEnabled = false;
        }

        /// <summary>
        /// Fills the orders slave.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fillSlaveOrders(object sender, RoutedEventArgs e)
        {
            if(dgv_orders.SelectedIndex != -1)
            {
                Order order = dgv_orders.SelectedItem as Order;

                Customer customer = new Customer(order.ref_customer);
                customer.readCustomer();
                tb_ClientOrder.Text = customer.name;

                PaymentMethod paymentMethod = new PaymentMethod(order.ref_paymentMethod);
                paymentMethod.readPaymentMethod();
                tb_paymentMethod.Text = paymentMethod.method;

                tb_DateOrder.Text = Convert.ToString(order.date);
                tb_idOrder.Text = Convert.ToString(order.idOrder);
                tb_TotalOrder.Text = Convert.ToString(order.total);

                OrderProduct orderProduct = new OrderProduct();
                orderProduct.manage.readAllOrderProduct(order);
                dg_productsOrders.ItemsSource = orderProduct.manage.orderProductList;

                checkStatus(order);
            }
        }

        /// <summary>
        /// Deletes the order.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteOrder(object sender, RoutedEventArgs e)
        {
            if (dgv_orders.SelectedIndex != -1)
            {
                Order order = dgv_orders.SelectedItem as Order;
                order.deleteOrder();
                notifier.ShowSuccess("The order has been deleted!");
            }
            else
                notifier.ShowError("You must selected any order!");
        }

        /// <summary>
        /// Updates the order.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void updateOrder(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            order.readAll();
            dgv_orders.ItemsSource = order.manage.orderList;
            notifier.ShowInformation("Orders list has been updated!");
        }

        /// <summary>
        /// Modifies the order.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void modifyOrder(object sender, RoutedEventArgs e)
        {
            if (dgv_orders.SelectedIndex != -1)
            {
                Singleton.interruptor = true;

                Order order = dgv_orders.SelectedItem as Order;
                Singleton.order = order;
                Singleton.Instance.insertOrder.Show();
                Singleton.Instance.insertOrder.user = userApp;
            }
            else
                notifier.ShowError("You must selected any order!");
        }

        /// <summary>
        /// BTNs the search order.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearchOrder(object sender, RoutedEventArgs e)
        {
            String campo = cb_SearchBy_Orders.SelectedItem as String;
            String busqueda = tb_SearchByOrders.Text;

            if (campo != null || busqueda != "Search...")
            {
                dgv_orders.ItemsSource = null;

                Order order = new Order();
                order.readSpecificOrder(campo,busqueda);

                dgv_orders.ItemsSource = order.manage.orderList;
            }
            else
                System.Windows.MessageBox.Show("Wrong fields!");
        }

        /// <summary>
        /// Fills the ComboBox search by order.
        /// </summary>
        private void fillComboBoxSearchBy_Order()
        {
            List<string> list = new List<string>();
            list.Add("IDORDER");
            list.Add("REFCUSTOMER");

            cb_SearchBy_Orders.ItemsSource = list;
        }

        /// <summary>
        /// Checks the status.
        /// </summary>
        /// <param name="order">The order.</param>
        private void checkStatus(Order order)
        {
            StatusOrder status = new StatusOrder(order.idOrder);
            status.readStatusOrder();

            if (status.confirmed == 1)
            {
                btn_ConfirmedOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
                btn_ConfirmedOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
            }
            else
            {
                btn_ConfirmedOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                btn_ConfirmedOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
            }

            if (status.labeled == 1)
            {
                btn_LabeledOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
                btn_LabeledOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
            }
            else
            {
                btn_LabeledOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                btn_LabeledOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
            }
                
            if (status.sent == 1)
            {
                btn_SentOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
                btn_SentOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
            }
            else
            {
                btn_SentOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                btn_SentOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
            }
                
            if (status.invoiced == 1)
            {
                btn_InvoicedOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
                btn_InvoicedOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 205, 50));
            }
            else
            {
                btn_InvoicedOrder.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                btn_InvoicedOrder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
            }
        }

        /// <summary>
        /// Updates the confirmed status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void updateConfirmedStatus(object sender, RoutedEventArgs e)
        {
            if (dgv_orders.SelectedIndex != -1)
            {
                Order order = dgv_orders.SelectedItem as Order;
                StatusOrder status = new StatusOrder(order.idOrder);
                status.readStatusOrder();

                if (status.confirmed == 1)
                    status.updateConfirmed(0);
                else
                    status.updateConfirmed(1);

                checkStatus(order);
            }
        }

        /// <summary>
        /// Updates the labeled status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void updateLabeledStatus(object sender, RoutedEventArgs e)
        {
            if (dgv_orders.SelectedIndex != -1)
            {
                Order order = dgv_orders.SelectedItem as Order;
                StatusOrder status = new StatusOrder(order.idOrder);
                status.readStatusOrder();

                if (status.labeled == 1)
                    status.updateLabeled(0);
                else
                    status.updateLabeled(1);

                checkStatus(order);
            }
        }

        /// <summary>
        /// Updates the sent status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void updateSentStatus(object sender, RoutedEventArgs e)
        {
            if (dgv_orders.SelectedIndex != -1)
            {
                Order order = dgv_orders.SelectedItem as Order;
                StatusOrder status = new StatusOrder(order.idOrder);
                status.readStatusOrder();

                if (status.sent == 1)
                    status.updateSent(0);
                else
                    status.updateSent(1);

                checkStatus(order);
            }
        }

        /// <summary>
        /// Updates the invoiced status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void updateInvoicedStatus(object sender, RoutedEventArgs e)
        {
            if (dgv_orders.SelectedIndex != -1)
            {
                Order order = dgv_orders.SelectedItem as Order;
                StatusOrder status = new StatusOrder(order.idOrder);
                status.readStatusOrder();

                if (status.invoiced == 1)
                    status.updateInvoiced(0);
                else
                {
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure this order has been invoiced?", "Order invoiced", button);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            status.updateInvoiced(1);
                            Invoice invoice = new Invoice();
                            invoice.insertInvoice();
                            InvoiceManage manageInvoice = new InvoiceManage();
                            manageInvoice.insertOrderInvoice(order);
                            break;
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }

                    
                }
                    

                checkStatus(order);
            }
        }

        ///////////// INVOICES /////////////////

        /// <summary>
        /// BTNs the new invoice.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void btnNewInvoice(object sender, RoutedEventArgs e)
        {
            Singleton.interruptor = false;
            Singleton.interruptorInvoice = true;
            Singleton.Instance.insertOrder.interruptorModify = false;
            Singleton.Instance.insertOrder.Show();
            Singleton.Instance.insertOrder.user = userApp;
        }

        public void btnDeleteInvoice(object sender, RoutedEventArgs e)
        {
            if (dgv_invoices.SelectedIndex != -1)
            {
                Invoice invoice = dgv_invoices.SelectedItem as Invoice;
                invoice.deleteInvoice();
                invoice.manage.readAll();

                dgv_invoices.ItemsSource = invoice.manage.listInvoices;
            }
            else
                notifier.ShowError("You must selected any invoice!");
        }

        /// <summary>
        /// BTNs the update invoices.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void updateListInvoices(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice();
            invoice.manage.readAll();
            dgv_invoices.ItemsSource = invoice.manage.listInvoices;
            notifier.ShowInformation("Invoices have been updated");
        }

        private void showInfoInvoice()
        {
            if (dgv_invoices.SelectedIndex != -1)
            {
                Singleton.interruptor = true;

                Invoice invoice = dgv_invoices.SelectedItem as Invoice;

                Order order = new Order(invoice.searchIdOrder());
                order.readOrder();

                Singleton.order = order;

                Singleton.Instance.insertOrder.cbo_CustomersOrders.IsEnabled   = false;
                Singleton.Instance.insertOrder.cbo_PaymentMethod.IsEnabled     = false;
                Singleton.Instance.insertOrder.btn_SaveOrder.IsEnabled         = false;
                Singleton.Instance.insertOrder.btnDone.IsEnabled               = false;
                Singleton.Instance.insertOrder.tb_prepaidOrder.IsEnabled       = false;
                Singleton.Instance.insertOrder.btnSearchCustomer.IsEnabled     = false;
                Singleton.Instance.insertOrder.btnAddProductOrder.IsEnabled    = false;
                Singleton.Instance.insertOrder.btnDeleteProductOrder.IsEnabled = false;

                Singleton.Instance.insertOrder.Show();
                Singleton.Instance.insertOrder.user = userApp;
            }
            else
                notifier.ShowError("You must selected any invoice!");
        }

        /// <summary>
        /// Handles the MouseRightButtonDown event of the dgv_invoices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void dgv_invoices_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.ContextMenu cm = this.FindResource("cmOptionsInvoices") as System.Windows.Controls.ContextMenu;
            cm.PlacementTarget = sender as System.Windows.Controls.Button;
            cm.IsOpen = true;
        }

        /// <summary>
        /// Action of context menu for show invoice
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void cmActionShow(object sender, RoutedEventArgs e)
        {
            showInfoInvoice();
        }

        /// <summary>
        /// Action of context menu for account invoice
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void cmActionAccounted(object sender, RoutedEventArgs e)
        {

            if (userApp.id_user != 1 && !userApp.listPermissions.Contains(20))
            {
                notifier.ShowError("You don't have enough privileges!");
            }
            else
            {
                Invoice invoice = dgv_invoices.SelectedItem as Invoice;

                if (invoice.checkBeforeInvoiceAccounted())
                {
                    invoice.updateAccounted();
                    invoice.readAll();
                    dgv_invoices.ItemsSource = invoice.manage.listInvoices;
                }
                else
                    notifier.ShowError("There are previous unaccounted invoices!");
            }   
        }

        /// <summary>
        /// BTNs the search invoice.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearchInvoice(object sender, RoutedEventArgs e)
        {
            String campo = cbSearchInvoiceBy.SelectedItem as String;
            String busqueda = tbSearchInvoice.Text;

            if (campo != null || busqueda != "Search...")
            {
                dgv_invoices.ItemsSource = null;

                Invoice invoice = new Invoice();
                invoice.readSpecificInvoice(campo, busqueda);

                dgv_invoices.ItemsSource = invoice.manage.listInvoices;
            }
            else
                System.Windows.MessageBox.Show("Wrong fields!");
        }

        /// <summary>
        /// Fills the ComboBox search by product.
        /// </summary>
        private void fillComboBoxSearchBy_Invoice()
        {
            List<string> list = new List<string>();
            list.Add("ID_INVOICE");

            cbSearchInvoiceBy.ItemsSource = list;
        }

        private void printInvoice(object sender, RoutedEventArgs e)
        {
            if(dgv_invoices.SelectedIndex  != -1)
            {
                Invoice invoice = dgv_invoices.SelectedItem as Invoice;

                Order order = new Order(invoice.searchIdOrder());
                order.readOrder();

                ViewReportInvoice report = new ViewReportInvoice(order);
                report.Show();
            }
            else
                notifier.ShowError("You must selected any invoice!");
        }
    }
}