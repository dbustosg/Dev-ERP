using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LITTLERP.Domain.Manage
{
    /// <summary>
    /// Manage of user
    /// </summary>
    public class UserManage
    {
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        public List<User> list { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManage"/> class.
        /// </summary>
        public UserManage()
        {
            list = new List<User>();
        }

        /// <summary>
        /// Reads the identifier user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public int readIdUser(User user)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("iduser", "USERS", "NAME = '" + user.name + "'"));

            return count;
        }

        /// <summary>
        /// Reads all users.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select iduser from USERS where deleted=0 order by iduser","users");

            DataTable table = data.Tables["users"];

            User aux;

            foreach(DataRow row in table.Rows)
            {
                aux = new User(Convert.ToInt32(row["IDUSER"]));
                readUser(aux);
                list.Add(aux);
            }
        }

        /// <summary>
        /// Reads the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void readUser(User user)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();
            data = connect.getData("select * from USERS where iduser="+user.id_user, "users");

            DataTable table = data.Tables["users"];
            DataRow row = table.Rows[0];
            user.name = Convert.ToString(row["NAME"]);
        }

        /// <summary>
        /// Reads the specific user.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificUser(String campo, String busqueda)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select iduser from USERS where deleted=0 and " + campo + " like '%" + busqueda + "%' order by iduser", "users");

            DataTable table = data.Tables["users"];

            User aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new User(Convert.ToInt32(row["IDUSER"]));
                readUser(aux);
                list.Add(aux);
            }
        }

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void insertUser(User user)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int maximum = Convert.ToInt32("0" + connect.DLookUp("max(iduser)", "USERS", ""))+1; //PARA SOLUCIONAR EL PROBLEMA DE INICIO DE TABLA
            string name = user.name;
            string password = user.password;

            string SQL = "INSERT INTO USERS VALUES ("+maximum+",'"+name+"','"+useful.GetSHA256(password)+"',0)"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Checks the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Boolean checkUser(User user)
        { 
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(iduser)", "USERS", "NAME = '" + user.name + "' and password = '" + user.password + "'" + " and deleted = 0"));

            return count != 0;
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void deleteUser (User user)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();
            connect.setData("UPDATE USERS SET deleted=1 WHERE iduser=" + user.id_user); //ES UN BORRADO LÓGICO
        }

        /// <summary>
        /// Modifies the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void modifyUser(User user)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            string SQL = "UPDATE USERS SET PASSWORD='"+ useful.GetSHA256(user.password)+"' WHERE NAME='"+ user.name+"'"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Modifies the name user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void modifyNameUser(User user)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            string SQL = "UPDATE USERS SET NAME='" + user.name + "' WHERE IDUSER=" + user.id_user; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Inserts the user role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        public void insertUserRole(User user, Role role)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int maximum = Convert.ToInt32("0" + connect.DLookUp("max(iduser_role)", "USERS_ROLES", "")) + 1; //PARA SOLUCIONAR EL PROBLEMA DE INICIO DE TABLA
            int idUser = user.id_user;
            int idRole = role.id_role;

            string SQL = "INSERT INTO USERS_ROLES VALUES (" + idUser + "," + idRole + "," + maximum + ")";

            connect.setData(SQL);
        }

        /// <summary>
        /// Deletes the user role.
        /// </summary>
        /// <param name="user">The user.</param>
        public void deleteUserRole(User user)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            string SQL = "DELETE FROM USERS_ROLES WHERE iduser = " + user.id_user; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Checks the user role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Boolean checkUserRole(User user, Role role)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(iduser_role)", "USERS_ROLES", "IDUSER = " + user.id_user + " and IDROLE = " + role.id_role));

            return count != 0;
        }

        /// <summary>
        /// Reads the permissions.
        /// </summary>
        /// <param name="user">The user.</param>
        public void readPermissions(User user)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();
            data = connect.getData("SELECT distinct * FROM PERMISSIONS P, roles_permissions RP, roles R, users_roles UR, users U WHERE p.idpermission=rp.idpermission " +
                "AND rp.idrole=r.idrole AND r.idrole = ur.idrole AND ur.iduser = u.iduser AND u.iduser = " + user.id_user, "permissions");

            DataTable table = data.Tables["permissions"];

            foreach (DataRow row in table.Rows)
            {
                user.listPermissions.Add(Convert.ToInt32(row["IDPERMISSION"]));
            }
        }
    }
}
