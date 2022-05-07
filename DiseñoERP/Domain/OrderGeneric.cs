using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Generics orders
    /// </summary>
    public class OrderGeneric
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public OrderManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderGeneric"/> class.
        /// </summary>
        public OrderGeneric()
        {
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderGeneric"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public OrderGeneric(int id)
        {
            this.Id = id;
            this.manage = new OrderManage();
        }

        /// <summary>
        /// Inserts the order product generic.
        /// </summary>
        public void insertOrderProductGeneric()
        {
            this.manage.insertOrderProductGeneric(this);
        }
    }
}
