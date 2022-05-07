using DiseñoERP.Domain;
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
    /// Lógica de interacción para ViewReportInvoice.xaml
    /// </summary>
    public partial class ViewReportInvoice : Window
    {
        /// <summary>
        /// The order
        /// </summary>
        public Order order;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewReportInvoice"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        public ViewReportInvoice(Order order)
        {
            InitializeComponent();
            this.order = order;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            customer.manage.readCustomerByOrder(order);

            order.readInfoOrder_ToCrystalReport();
            order.readAllOrderProduct_CrystalReport();

            Invoice invoice = new Invoice();
            invoice.readInfoInvoice_ToCrystalReport(order);

            // Load the values in the Report
            CrystalReport report = new CrystalReport();
            report.Database.Tables["Customer"].SetDataSource(customer.manage.tcustomers);
            report.Database.Tables["Order"].SetDataSource(order.manage.tOrder);
            report.Database.Tables["OrderProduct"].SetDataSource(order.manage.tOrderProduct);
            report.Database.Tables["Invoice"].SetDataSource(invoice.manage.tInvoices);
            Cr.ViewerCore.ReportSource = report;
        }
    }
}
