using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LITTLERP.Domain
{
    /// <summary>
    /// Role
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets the identifier role.
        /// </summary>
        /// <value>
        /// The identifier role.
        /// </value>
        public int id_role { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public Manage.RoleManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            manage = new Manage.RoleManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="id_role">The identifier role.</param>
        public Role(int id_role)
        {
            manage = new Manage.RoleManage();
            this.id_role = id_role;
        }

        /// <summary>
        /// Reads all roles.
        /// </summary>
        public void readAll()
        {
            manage.readAll();
        }

        /// <summary>
        /// Reads the role.
        /// </summary>
        public void readRole()
        {
            manage.readRole(this);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return description;
        }
    }
}
