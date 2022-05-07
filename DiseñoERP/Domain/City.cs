using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// City
    /// </summary>
    public class City
    {
        /// <summary>
        /// Gets or sets the identifier city.
        /// </summary>
        /// <value>
        /// The identifier city.
        /// </value>
        public int id_city { get; set; }

        /// <summary>
        /// Gets or sets the name city.
        /// </summary>
        /// <value>
        /// The name city.
        /// </value>
        public String nameCity { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public LITTLERP.Domain.Manage.CustomerManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="City"/> class.
        /// </summary>
        public City()
        {
            manage = new LITTLERP.Domain.Manage.CustomerManage();
        }
    }
}
