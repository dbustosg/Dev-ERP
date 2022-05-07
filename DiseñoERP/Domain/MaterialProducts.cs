using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Material products
    /// </summary>
    public class MaterialProducts
    {
        /// <summary>
        /// Gets or sets the identifier material.
        /// </summary>
        /// <value>
        /// The identifier material.
        /// </value>
        public int id_material { get; set; }

        /// <summary>
        /// Gets or sets the name material.
        /// </summary>
        /// <value>
        /// The name material.
        /// </value>
        public String nameMaterial { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public ProductManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialProducts"/> class.
        /// </summary>
        public MaterialProducts()
        {
            manage = new ProductManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialProducts"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public MaterialProducts(int id)
        {
            this.id_material = id;
            manage = new ProductManage();
        }
    }
}
