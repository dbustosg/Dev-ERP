using DiseñoERP.Domain.Manage;
using LITTLERP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets the identifier order.
        /// </summary>
        /// <value>
        /// The identifier order.
        /// </value>
        public int idOrder { get; set; }

        /// <summary>
        /// Gets or sets the reference customer.
        /// </summary>
        /// <value>
        /// The reference customer.
        /// </value>
        public int ref_customer { get; set; }

        /// <summary>
        /// Gets or sets the reference user.
        /// </summary>
        /// <value>
        /// The reference user.
        /// </value>
        public int ref_user { get; set; }

        /// <summary>
        /// Gets or sets the reference payment method.
        /// </summary>
        /// <value>
        /// The reference payment method.
        /// </value>
        public int ref_paymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public String date { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public Double total { get; set; }

        /// <summary>
        /// Gets or sets the prepaid.
        /// </summary>
        /// <value>
        /// The prepaid.
        /// </value>
        public Double prepaid { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public StatusOrder status { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public OrderManage manage { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        public PaymentMethod paymentMethod { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        public Order()
        {
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Order(int id)
        {
            this.idOrder = id;
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Reads all orders.
        /// </summary>
        public void readAll()
        {
            this.manage.readAll();
        }

        /// <summary>
        /// Reads the order.
        /// </summary>
        public void readOrder()
        {
            this.manage.readOrder(this);
        }

        /// <summary>
        /// Reads the specific order.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificOrder(String campo, String busqueda)
        {
            this.manage.readSpecificOrder(campo, busqueda);
        }

        /// <summary>
        /// Reads the information order to crystal report.
        /// </summary>
        public void readInfoOrder_ToCrystalReport()
        {
            manage.readInfoOrder_ToCrystalReport(this);
        }

        /// <summary>
        /// Reads all order product crystal report.
        /// </summary>
        public void readAllOrderProduct_CrystalReport()
        {
            manage.readAllOrderProduct_CrystalReport(this); 
        }

        /// <summary>
        /// Reads the order by invoice.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        public void readOrderByInvoice(Invoice invoice)
        {
            manage.readOrderByInvoice(invoice, this);
        }

        /// <summary>
        /// Inserts the order.
        /// </summary>
        public void insertOrder()
        {
            this.manage.insertOrder(this);
        }

        /// <summary>
        /// Deletes the order.
        /// </summary>
        public void deleteOrder()
        {
            this.manage.deleteOrder(this);
        }

        /// <summary>
        /// Deletes the no logic order.
        /// </summary>
        public void deleteNoLogicOrder()
        {
            this.manage.deleteNoLogicOrder(this);
        }
    }
}
