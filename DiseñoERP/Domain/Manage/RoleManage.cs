using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LITTLERP.Domain.Manage
{
    /// <summary>
    /// Manage of role
    /// </summary>
    public class RoleManage
    {
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        public List<Role> list { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManage"/> class.
        /// </summary>
        public RoleManage()
        {
            list = new List<Role>();
        }

        /// <summary>
        /// Reads all roles.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select idrole from ROLES order by idrole", "roles");

            DataTable table = data.Tables["roles"];

            Role aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Role(Convert.ToInt32(row["IDROLE"]));
                readRole(aux);
                list.Add(aux);
            }
        }

        /// <summary>
        /// Reads the role.
        /// </summary>
        /// <param name="role">The role.</param>
        public void readRole(Role role)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();
            data = connect.getData("select * from ROLES where idrole=" + role.id_role, "roles");

            DataTable table = data.Tables["roles"];
            DataRow row = table.Rows[0];  
            role.description = Convert.ToString(row["DESCRIPTION"]);
            
        }

    }
}
