using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Size of products
    /// </summary>
    public class SizeProducts
    {
        /// <summary>
        /// Gets or sets the size of the identifier.
        /// </summary>
        /// <value>
        /// The size of the identifier.
        /// </value>
        public int id_size { get; set; }

        /// <summary>
        /// Gets or sets the messures.
        /// </summary>
        /// <value>
        /// The messures.
        /// </value>
        public String messures { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public ProductManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeProducts"/> class.
        /// </summary>
        public SizeProducts()
        {
            manage = new ProductManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeProducts"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public SizeProducts(int id)
        {
            this.id_size = id;
            manage = new ProductManage();
        }
    }
}
