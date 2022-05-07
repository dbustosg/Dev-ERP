using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Class of zip codes
    /// </summary>
    public class ZipCode
    {
        /// <summary>
        /// Gets or sets the identifier zip code.
        /// </summary>
        /// <value>
        /// The identifier zip code.
        public int id_zipCode { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value> 
        public String zipCode { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public LITTLERP.Domain.Manage.CustomerManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipCode"/> class.
        /// </summary>
        public ZipCode()
        {
            manage = new LITTLERP.Domain.Manage.CustomerManage();
        }
    }
}
