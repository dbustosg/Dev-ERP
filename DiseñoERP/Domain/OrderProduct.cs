using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Products of orders
    /// </summary>
    public class OrderProduct
    {
        /// <summary>
        /// Gets or sets the identifier order product.
        /// </summary>
        /// <value>
        /// The identifier order product.
        /// </value>
        public int idOrderProduct { get; set; }

        /// <summary>
        /// Gets or sets the reference order.
        /// </summary>
        /// <value>
        /// The reference order.
        /// </value>
        public int refOrder { get; set; }

        /// <summary>
        /// Gets or sets the reference product.
        /// </summary>
        /// <value>
        /// The reference product.
        /// </value>
        public int refProduct { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public int amount { get; set; }

        /// <summary>
        /// Gets or sets the priceofsale.
        /// </summary>
        /// <value>
        /// The priceofsale.
        /// </value>
        public double priceofsale { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public OrderManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderProduct"/> class.
        /// </summary>
        public OrderProduct()
        {
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderProduct"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public OrderProduct(int id)
        {
            this.idOrderProduct = id;
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Inserts the order product.
        /// </summary>
        public void insertOrderProduct()
        {
            manage.insertOrderProduct(this);
        }

        /// <summary>
        /// Reads all order product.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readAllOrderProduct(Order order)
        {
            this.manage.readAllOrderProduct(order);
        }

        /// <summary>
        /// Reads the order product.
        /// </summary>
        public void readOrderProduct()
        {
            this.manage.readOrderProduct(this);
        }
    }
}
