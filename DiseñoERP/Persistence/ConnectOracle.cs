using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LITTLERP.Persistence
{
    /// <summary>
    /// Connect Oracle
    /// </summary>
    public class ConnectOracle
    {
          //////////////////////////////////////////////////
         ///////////////////  DRIVER //////////////////////
        //////////////////////////////////////////////////
        const String driver = "Data Source=(DESCRIPTION ="
        + "(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = LOCALHOST)(PORT = 1521)))"
        + "(CONNECT_DATA = (SERVICE_NAME = PDB18C))); "
        + "User Id=LITLERP; Password=litlerp;";
        
        ////////////////////////////////////////////////////////////

        /**
         * Method to retrieve a set of data
         * Parameter: Query
         * Parameter: Table
         */
        public DataSet getData(String query, String table)
        {
            OracleConnection objConexion;
            OracleDataAdapter objComando;
            DataSet requestQuery = new DataSet();

            objConexion = new OracleConnection(driver);
            objConexion.Open();
            objComando = new OracleDataAdapter(query, objConexion);
            objComando.Fill(requestQuery, table);
            objConexion.Close();

            return requestQuery;
        }

        /**
         * Method to insert or update data in a table
         * Parameter: Sentence 
         */
        public void setData(String sentencia)
        {
            OracleConnection objConexion;
            OracleCommand objComando;

            objConexion = new OracleConnection(driver);
            objConexion.Open();
            objComando = new OracleCommand(sentencia, objConexion);

            objComando.ExecuteNonQuery();
            objComando.Connection.Close();
        }

        /**
         * Method to retrieve only one value
         * Parameter: column
         * Parameter: Table
         * Parameter: Condition
         */
        public Object DLookUp(String columna, String tabla, String condicion)
        {
            OracleConnection objConexion;
            OracleDataAdapter objComando;
            DataSet requestQuery = new DataSet();
            Object resultado;

            objConexion = new OracleConnection(driver);
            objConexion.Open();

            if (condicion.Equals(""))
            {
                objComando = new OracleDataAdapter("Select " + columna + " from " + tabla, objConexion);
            }
            else
            {
                objComando = new OracleDataAdapter("Select " + columna + " from " + tabla + " where " + condicion, objConexion);
            }

            objComando.Fill(requestQuery);

            try
            {
                resultado = requestQuery.Tables[0].Rows[0][requestQuery.Tables[0].Columns.IndexOf(columna)];
            }
            catch (Exception a)
            {
                resultado = -1;
            }
            objConexion.Close();
            return resultado;
        }
    }
}
