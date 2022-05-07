using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Status of orders
    /// </summary>
    public class StatusOrder
    {
        /// <summary>
        /// Gets or sets the identifier order status.
        /// </summary>
        /// <value>
        /// The identifier order status.
        /// </value>
        public int idOrderStatus { get; set; }

        /// <summary>
        /// Gets or sets the confirmed.
        /// </summary>
        /// <value>
        /// The confirmed.
        /// </value>
        public int confirmed { get; set; }

        /// <summary>
        /// Gets or sets the labeled.
        /// </summary>
        /// <value>
        /// The labeled.
        /// </value>
        public int labeled { get; set; }

        /// <summary>
        /// Gets or sets the sent.
        /// </summary>
        /// <value>
        /// The sent.
        /// </value>
        public int sent { get; set; }

        /// <summary>
        /// Gets or sets the invoiced.
        /// </summary>
        /// <value>
        /// The invoiced.
        /// </value>
        public int invoiced { get; set; }

        /// <summary>
        /// Gets or sets the name status.
        /// </summary>
        /// <value>
        /// The name status.
        /// </value>
        public string nameStatus { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public OrderManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusOrder"/> class.
        /// </summary>
        /// <param name="idOrderStatus">The identifier order status.</param>
        public StatusOrder(int idOrderStatus)
        {
            this.idOrderStatus = idOrderStatus;
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Reads the status order.
        /// </summary>
        public void readStatusOrder()
        {
            this.manage.readStatusOrder(this);
        }

        /// <summary>
        /// Inserts the status order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void insertStatusOrder(Order order)
        {
            this.manage.insertStatusOrder(order);
        }

        /// <summary>
        /// Updates the confirmed.
        /// </summary>
        /// <param name="option">The option.</param>
        public void updateConfirmed(int option)
        {
            this.manage.updateConfirmed(this, option);
        }

        /// <summary>
        /// Updates the labeled.
        /// </summary>
        /// <param name="option">The option.</param>
        public void updateLabeled(int option)
        {
            this.manage.updateLabeled(this, option);
        }

        /// <summary>
        /// Updates the sent.
        /// </summary>
        /// <param name="option">The option.</param>
        public void updateSent(int option)
        {
            this.manage.updateSent(this, option);
        }

        /// <summary>
        /// Updates the invoiced.
        /// </summary>
        /// <param name="option">The option.</param>
        public void updateInvoiced(int option)
        {
            this.manage.updateInvoiced(this, option);
        }
    }
}
