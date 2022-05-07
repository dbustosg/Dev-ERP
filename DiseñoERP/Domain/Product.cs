using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the identifier product.
        /// </summary>
        /// <value>
        /// The identifier product.
        /// </value>
        public int Id_product { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the wholesale price.
        /// </summary>
        /// <value>
        /// The wholesale price.
        /// </value>
        public double WholesalePrice { get; set; }

        /// <summary>
        /// Gets or sets the size product.
        /// </summary>
        /// <value>
        /// The size product.
        /// </value>
        public SizeProducts SizeProduct { get; set; }

        /// <summary>
        /// Gets or sets the material product.
        /// </summary>
        /// <value>
        /// The material product.
        /// </value>
        public MaterialProducts MaterialProduct { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public Manage.ProductManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
            manage = new Manage.ProductManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Product(int id)
        {
            this.Id_product = id;
            manage = new Manage.ProductManage();
        }

        /// <summary>
        /// Reads all products.
        /// </summary>
        public void readAll()
        {
            manage.readAll();
        }

        /// <summary>
        /// Reads the product.
        /// </summary>
        public void readProduct()
        {
            manage.readProduct(this);
        }

        /// <summary>
        /// Reads the specific product.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificProduct(String campo, String busqueda)
        {
            manage.readSpecificProduct(campo, busqueda);
        }

        /// <summary>
        /// Inserts the product.
        /// </summary>
        public void insertProduct()
        {
            manage.insertProduct(this);
        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        public void deleteProduct()
        {
            manage.deleteProduct(this);
        }

        /// <summary>
        /// Modifies the product.
        /// </summary>
        public void modifyProduct()
        {
            manage.modifyProduct(this);
        }
    }
}
