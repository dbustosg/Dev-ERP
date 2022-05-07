using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LITTLERP.Domain
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier user.
        /// </summary>
        /// <value>
        /// The identifier user.
        /// </value>
        public int id_user { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public String password { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public Manage.UserManage manage { get; set; }

        /// <summary>
        /// Gets or sets the list permissions.
        /// </summary>
        /// <value>
        /// The list permissions.
        /// </value>
        public List<int> listPermissions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            manage = new Manage.UserManage();
            listPermissions = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id_user">The identifier user.</param>
        public User(int id_user)
        {
            manage = new Manage.UserManage();
            this.id_user = id_user;
            listPermissions = new List<int>();
        }


        //METHODS (ENCAPSULATION)

        /// <summary>
        /// Inserts this instance.
        /// </summary>
        public void insert()
        {
            manage.insertUser(this);
        }

        /// <summary>
        /// Checks this instance.
        /// </summary>
        /// <returns></returns>
        public Boolean check()
        {
            return manage.checkUser(this);
        }

        /// <summary>
        /// Reads all users.
        /// </summary>
        public void readAll()
        {
            manage.readAll();
        }

        /// <summary>
        /// Reads the identifier user.
        /// </summary>
        /// <returns></returns>
        public int readIdUser()
        {
            return manage.readIdUser(this);
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void delete()
        {
            manage.deleteUser(this);
        }

        /// <summary>
        /// Reads the user.
        /// </summary>
        public void readUser()
        {
            manage.readUser(this);
        }

        /// <summary>
        /// Reads the specific user.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificUser(String campo, String busqueda)
        {
            manage.readSpecificUser(campo,busqueda);
        }

        /// <summary>
        /// Modifies the user.
        /// </summary>
        public void modifyUser()
        {
            manage.modifyUser(this);
        }

        /// <summary>
        /// Inserts the user role.
        /// </summary>
        /// <param name="role">The role.</param>
        public void insertUserRole(Role role)
        {
            manage.insertUserRole(this, role);
        }

        /// <summary>
        /// Deletes the user role.
        /// </summary>
        public void deleteUserRole()
        {
            manage.deleteUserRole(this);
        }

        /// <summary>
        /// Checks the user role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Boolean checkUserRole(Role role)
        {
            return manage.checkUserRole(this, role);
        }

        /// <summary>
        /// Reads the permissions.
        /// </summary>
        public void readPermissions()
        {
            manage.readPermissions(this);
        }
    }
}
