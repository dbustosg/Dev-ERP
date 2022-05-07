using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// States
    /// </summary>
    public class State
    {
        /// <summary>
        /// Gets or sets the state of the identifier.
        /// </summary>
        /// <value>
        /// The state of the identifier.
        /// </value>
        public int id_state { get; set; }

        /// <summary>
        /// Gets or sets the state of the name.
        /// </summary>
        /// <value>
        /// The state of the name.
        /// </value>
        public String nameState { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public LITTLERP.Domain.Manage.CustomerManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        public State()
        {
            manage = new LITTLERP.Domain.Manage.CustomerManage();
        }
    }
}
