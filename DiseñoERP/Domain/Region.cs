using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LITTLERP.Domain
{
    /// <summary>
    /// Region
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Gets or sets the identifier region.
        /// </summary>
        /// <value>
        /// The identifier region.
        /// </value>
        public int id_region { get; set; }

        /// <summary>
        /// Gets or sets the name region.
        /// </summary>
        /// <value>
        /// The name region.
        /// </value>
        public String nameRegion { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public Manage.CustomerManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        public Region()
        {
            manage = new Manage.CustomerManage();
        }
    }
}
