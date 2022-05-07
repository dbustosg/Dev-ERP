using DiseñoERP.Domain;
using DiseñoERP.Domain.Manage;
using DiseñoERP.Persistence;
using LITTLERP.Domain;
using LITTLERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;


namespace DiseñoERP.View
{
    /// <summary>
    /// Lógica de interacción para InsertOrder.xaml
    /// </summary>
    public partial class InsertOrder : Window
    {
        /// <summary>
        /// The customer order
        /// </summary>
        public Customer customerOrder;
        /// <summary>
        /// The product
        /// </summary>
        public Product product;
        /// <summary>
        /// The user
        /// </summary>
        public User user;
        /// <summary>
        /// The order
        /// </summary>
        public Order order;
        /// <summary>
        /// The main customer
        /// </summary>
        MainWindow mainCustomer;

        /// <summary>
        /// The order products list
        /// </summary>
        public List<OrderProduct> orderProductsList;
        /// <summary>
        /// The order generics list
        /// </summary>
        public List<OrderGeneric> orderGenericsList;
        /// <summary>
        /// The customer list
        /// </summary>
        public List<Customer> customerList;
        /// <summary>
        /// The payment method list
        /// </summary>
        public List<PaymentMethod> paymentMethodList;
        /// <summary>
        /// The interruptor tipo producto
        /// </summary>
        public Boolean interruptorTipoProducto;
        /// <summary>
        /// The interruptor modify
        /// </summary>
        public Boolean interruptorModify;
        /// <summary>
        /// The interruptor invoice
        /// </summary>
        public Boolean interruptorInvoice;
        /// <summary>
        /// The counter total
        /// </summary>
        private Double counterTotal;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertOrder"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="interruptor">if set to <c>true</c> [interruptor].</param>
        public InsertOrder(User user, Boolean interruptor)
        {
            InitializeComponent();

            this.user = user;
            counterTotal = 0;
            interruptorModify = false;
            interruptorInvoice = interruptor;

            orderProductsList = new List<OrderProduct>();
            orderGenericsList = new List<OrderGeneric>();
            
            CustomerManage customerManage = new CustomerManage();
            customerManage.readAll();
            cbo_CustomersOrders.ItemsSource = customerManage.listCustomer;

            OrderManage manage = new OrderManage();
            manage.readAllPaymentMethod();
            cbo_PaymentMethod.ItemsSource = manage.paymentMethodsList;

            String id;

            if (!interruptorInvoice)
            {
                int idOrder = LITTLERP.useful.fechaFormatoCientifico();
                id = idOrder.ToString() + (manage.countId() + 1).ToString();
            }
            else
            {
                lblTittleOrder.Content = "New invoice";
                InvoiceManage invoiceManage = new InvoiceManage();
                id = Convert.ToString(DateTime.Today.Year) + Convert.ToString(invoiceManage.countId() + 1);
            }
            
            tb_idOrder.Text = id;
            tb_idOrder.IsEnabled = false;

            btnDone.IsEnabled = false;
            
            bloquearEsclavo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertOrder"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="order">The order.</param>
        public InsertOrder(User user, Order order)
        {
            InitializeComponent();

            interruptorModify = true;
            this.user = user;
            orderProductsList = new List<OrderProduct>();
            customerList = new List<Customer>();
            paymentMethodList = new List<PaymentMethod>();

            //Cambiamos el título
            lblTittleOrder.Content = "Modify order";

            //Añadimos el id
            tb_idOrder.Text = Convert.ToString(order.idOrder);
            tb_idOrder.IsEnabled = false;

            //Añadimos el prepaid
            tb_prepaidOrder.Text = Convert.ToString(order.prepaid);

            //Añadimos el cliente a la ventana
            Customer customer = new Customer();
            customer.idCustomer = order.ref_customer;
            customer.readCustomer();
            customerList.Add(customer);
            cbo_CustomersOrders.ItemsSource = customerList;
            cbo_CustomersOrders.SelectedIndex = 0;

            //Añadimos el metodo de pago y seleccionamos el que tiene asignado
            OrderManage manage = new OrderManage();
            manage.readAllPaymentMethod();
            cbo_PaymentMethod.ItemsSource = manage.paymentMethodsList;
            cbo_PaymentMethod.SelectedIndex = order.ref_paymentMethod-1;

            Order o = new Order();
            o.idOrder = order.idOrder;
            o.manage.readProductOfOrder(o);
            dgv_product.ItemsSource = o.manage.orderProductList;
            orderProductsList = o.manage.orderProductList;

            bloquearEsclavo();
        }

        //Canal de notificaciones
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 20,
                offsetY: 20);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        /// <summary>
        /// Selects the customer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void selectCustomer(object sender, RoutedEventArgs e)
        {
            mainCustomer = new MainWindow(0);
            mainCustomer.Show();     
        }

        /// <summary>
        /// Selects the cb customer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void selectCbCustomer(object sender, RoutedEventArgs e)
        {
            customerOrder = cbo_CustomersOrders.SelectedItem as Customer;
        }

        /// <summary>
        /// Fills the ComboBox customer.
        /// </summary>
        public void fillComboBoxCustomer()
        {
            cbo_CustomersOrders.ItemsSource = null;
            List<Customer> customer = new List<Customer>();
            customer.Add(customerOrder);
            cbo_CustomersOrders.ItemsSource = customer;
            cbo_CustomersOrders.SelectedIndex = 0;
        }

        /// <summary>
        /// Block the slave.
        /// </summary>
        public void bloquearEsclavo()
        {
            tb_descriptionOrder.IsEnabled = false;
            tb_priceOrder.IsEnabled = false;
            tb_amountPrice.IsEnabled = false;
            tb_dto.IsEnabled = false;
            cb_dto.IsEnabled = false;
            tb_priceTotalOrder.IsEnabled = false;
            btn_SaveOrder.IsEnabled = false;
        }

        /// <summary>
        /// Activate the normal product slave.
        /// </summary>
        public void activarEsclavoNormalProduct()
        {
            tb_amountPrice.IsEnabled = true;
            cb_dto.IsEnabled = true;
            btn_SaveOrder.IsEnabled = true;
            tb_descriptionOrder.IsEnabled = false;
            tb_priceOrder.IsEnabled = false;
        }

        /// <summary>
        /// Activate the generic product slave.
        /// </summary>
        public void activarEsclavoGeneralProduct()
        {
            tb_descriptionOrder.IsEnabled = true;
            tb_priceOrder.IsEnabled = true;
            tb_amountPrice.IsEnabled = true;
            btn_SaveOrder.IsEnabled = true;
            tb_dto.IsEnabled=false;
            cb_dto.IsEnabled = false;
        }

        /// <summary>
        /// Checks the dto.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void checkDto(object sender, RoutedEventArgs e)
        {
            if(tb_dto.IsEnabled)
                tb_dto.IsEnabled = false;
            else
                tb_dto.IsEnabled = true;
        }

        /// <summary>
        /// Calculars the total price.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void calcularTotalPrice (object sender, RoutedEventArgs e)
        {
            if (tb_dto.Text != "" && Regex.IsMatch(tb_dto.Text, @"^[0-9]+$"))
                tb_priceTotalOrder.Text = Convert.ToString(Convert.ToDouble(tb_priceOrder.Text) - (Convert.ToDouble(tb_priceOrder.Text) * (Convert.ToDouble(tb_dto.Text) / 100)));
        }

        /// <summary>
        /// Opens the context menu add product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void openContextMenuAddProduct(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmAddProduct") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void addProduct(object sender, RoutedEventArgs e)
        {
            interruptorTipoProducto = true;
            mainCustomer = new MainWindow(1);
            mainCustomer.ShowDialog();
        }

        /// <summary>
        /// Adds the general product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void addGeneralProduct(object sender, RoutedEventArgs e)
        {
            tb_priceOrder.Text = "";
            tb_descriptionOrder.Text = "";
            tb_amountPrice.Text = "";
            tb_dto.Text = "";
            tb_priceTotalOrder.Text = "";

            interruptorTipoProducto = false;
            activarEsclavoGeneralProduct();
        }

        /// <summary>
        /// Fills the slave product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void fillSlaveProduct(Product product)
        {
            this.product = product;

            tb_descriptionOrder.Text = product.Description;
            tb_priceOrder.Text = Convert.ToString(product.Price);
            activarEsclavoNormalProduct();
        }

        /// <summary>
        /// Handles the Save event of the btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void btn_Save(object sender, RoutedEventArgs e)
        {
            OrderProduct orderProduct = new OrderProduct();

            if (interruptorTipoProducto)
            {
                orderProduct.refProduct = this.product.Id_product;
                orderProduct.description = this.product.Description;

                int idOrder = 0;
                if (interruptorInvoice)
                {
                    Order order = new Order();
                    idOrder = LITTLERP.useful.fechaFormatoCientifico();
                    idOrder = Convert.ToInt32(idOrder.ToString() + (order.manage.countId() + 1).ToString());
                }
                else
                    idOrder = Convert.ToInt32(tb_idOrder.Text);

                orderProduct.refOrder = idOrder;    

                if (cb_dto.IsChecked == true)
                    orderProduct.priceofsale = Convert.ToDouble(tb_priceTotalOrder.Text);
                else
                    orderProduct.priceofsale = Convert.ToDouble(tb_priceOrder.Text);

                orderProduct.amount = Convert.ToInt32(tb_amountPrice.Text);

                orderProductsList.Add(orderProduct);
                dgv_product.ItemsSource = null;
                dgv_product.ItemsSource = orderProductsList;
            }
            else
            {
                orderProduct.refProduct = -1;
                orderProduct.description = tb_descriptionOrder.Text;
                orderProduct.priceofsale = Convert.ToDouble(tb_priceOrder.Text);
                orderProduct.amount = Convert.ToInt32(tb_amountPrice.Text);

                int idOrder = 0;
                if (interruptorInvoice)
                {
                    Order order = new Order();
                    idOrder = LITTLERP.useful.fechaFormatoCientifico();
                    idOrder = Convert.ToInt32(idOrder.ToString() + (order.manage.countId() + 1).ToString());
                }
                else
                    idOrder = Convert.ToInt32(tb_idOrder.Text);

                orderProduct.refOrder = idOrder;

                orderProductsList.Add(orderProduct);
                dgv_product.ItemsSource = null;
                dgv_product.ItemsSource = orderProductsList;
            }

            btnDone.IsEnabled = true;
        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteProduct(object sender, RoutedEventArgs e)
        {
            if(dgv_product.SelectedIndex != -1)
            {
                OrderProduct orderProduct = dgv_product.SelectedItem as OrderProduct;
                orderProductsList.Remove(orderProduct);
                dgv_product.ItemsSource = null;
                dgv_product.ItemsSource = orderProductsList;
            }

        }

        /// <summary>
        /// Handles the Click event of the ButtonCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Singleton.instance = null; //Para solucionar el problema de volver a abrir la ventana, ponemos Singleton a nulo, para que se pueda volver a crear
            Close();
        }

        /// <summary>
        /// Handles the Click event of the ButtonDone control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonDone_Click(object sender, RoutedEventArgs e)
        {
            if (customerOrder != null && cbo_PaymentMethod.SelectedIndex != -1)
            {
                Order order = new Order();

                if (interruptorInvoice)
                {
                    int idOrder = LITTLERP.useful.fechaFormatoCientifico();
                    order.idOrder = Convert.ToInt32(idOrder.ToString() + (order.manage.countId() + 1).ToString());
                }
                else
                    order.idOrder = Convert.ToInt32(tb_idOrder.Text);

                order.ref_customer = customerOrder.idCustomer;
                order.ref_user = user.id_user;

                PaymentMethod paymentMethod = cbo_PaymentMethod.SelectedItem as PaymentMethod;
                order.ref_paymentMethod = paymentMethod.idMethod;

                if(tb_prepaidOrder.Text == null)
                    order.prepaid = 0;
                else
                    order.prepaid = Convert.ToDouble(tb_prepaidOrder.Text);

                countTotalPrice(order.prepaid);
                order.total = counterTotal;

                if (!interruptorModify)
                {
                    order.insertOrder();

                    if (interruptorInvoice)
                    {
                        Invoice invoice = new Invoice();
                        invoice.insertInvoice();
                        invoice.manage.insertOrderInvoice(order);
                    }
                }   
                else
                {
                    order.deleteNoLogicOrder();
                    order.insertOrder();
                }

                StatusOrder status = new StatusOrder(order.idOrder);
                status.insertStatusOrder(order);

                if (interruptorInvoice)
                {
                    status.updateConfirmed(1);
                    status.updateLabeled(1);
                    status.updateSent(1);
                    status.updateInvoiced(1);
                }

                if (orderProductsList.Count > 0)
                    foreach (OrderProduct op in orderProductsList)
                        op.insertOrderProduct();

                if (interruptorInvoice)
                {
                    ViewReportInvoice report = new ViewReportInvoice(order);
                    report.Show();
                }


                Singleton.instance = null; //Para solucionar el problema de volver a abrir la ventana, ponemos Singleton a nulo, para que se pueda volver a crear
                Close();
                notifier.ShowSuccess("The order has been placed");
            }
            else
            {
                notifier.ShowError("Empty required fields!");
            }
            
        }

        /// <summary>
        /// Counts the total price.
        /// </summary>
        /// <param name="prepaid">The prepaid.</param>
        private void countTotalPrice(Double prepaid)
        {
            foreach(OrderProduct op in orderProductsList)
                counterTotal += (op.priceofsale * op.amount);

            counterTotal -= prepaid;
        }
    }
}
