using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Payment method
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Gets or sets the identifier method.
        /// </summary>
        /// <value>
        /// The identifier method.
        /// </value>
        public int idMethod { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public String method { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public Manage.OrderManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethod"/> class.
        /// </summary>
        public PaymentMethod()
        {
            manage = new Manage.OrderManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethod"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public PaymentMethod(int id)
        {
            this.idMethod = id;
            manage = new Manage.OrderManage();
        }

        /// <summary>
        /// Reads the payment method.
        /// </summary>
        public void readPaymentMethod()
        {
            this.manage.readPaymentMethod(this);
        }
    }
}
