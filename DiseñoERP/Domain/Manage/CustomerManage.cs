using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DiseñoERP.Domain;
using LITTLERP.Persistence;

namespace LITTLERP.Domain.Manage
{
    /// <summary>
    /// Manage of customer
    /// </summary>
    public class CustomerManage
    {
        /// <summary>
        /// Gets or sets the list region.
        /// </summary>
        /// <value>
        /// The list region.
        /// </value>
        public List<Region> listRegion { get; set; }

        /// <summary>
        /// Gets or sets the list customer.
        /// </summary>
        /// <value>
        /// The list customer.
        /// </value>
        public List<Customer> listCustomer { get; set; }

        /// <summary>
        /// Gets or sets the list states.
        /// </summary>
        /// <value>
        /// The list states.
        /// </value>
        public List<State> listStates { get; set; }

        /// <summary>
        /// Gets or sets the list cities.
        /// </summary>
        /// <value>
        /// The list cities.
        /// </value>
        public List<City> listCities { get; set; }

        /// <summary>
        /// Gets or sets the list zip code.
        /// </summary>
        /// <value>
        /// The list zip code.
        /// </value>
        public List<ZipCode> listZipCode { get; set; }

        /// <summary>
        /// Gets or sets the tcustomers.
        /// </summary>
        /// <value>
        /// The tcustomers.
        /// </value>
        public DataTable tcustomers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManage"/> class.
        /// </summary>
        public CustomerManage()
        {
            listCustomer = new List<Customer>();
            listRegion = new List<Region>();
            listStates = new List<State>();
            listCities = new List<City>();
            listZipCode = new List<ZipCode>();
            tcustomers = new DataTable();
        }

        /// <summary>
        /// Reads all customers.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select idcustomer from CUSTOMERS where deleted=0 order by idcustomer", "customers");

            DataTable table = data.Tables["customers"];

            Customer aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Customer(Convert.ToInt32(row["IDCUSTOMER"]));
                readCustomer(aux);
                listCustomer.Add(aux);
            }
        }

        /// <summary>
        /// Reads the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void readCustomer(Customer customer)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();
            data = connect.getData("select * from CUSTOMERS where idcustomer =" + customer.idCustomer, "customers");

            DataTable table = data.Tables["customers"];
            DataRow row = table.Rows[0];
            customer.idCustomer = Convert.ToInt32(row["IDCUSTOMER"]);
            customer.NIF = Convert.ToString(row["NIF"]);
            customer.name = Convert.ToString(row["NAME"]);
            customer.surname = Convert.ToString(row["SURNAME"]);
            customer.phone = Convert.ToInt32(row["PHONE"]);
            customer.email = Convert.ToString(row["EMAIL"]);
            customer.address = Convert.ToString(row["ADDRESS"]);
            customer.ref_zip_code = Convert.ToInt32(row["REFZIPCODESCITIES"]);
        }

        /// <summary>
        /// Reads the specific customer.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificCustomer(String campo, String busqueda)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select idcustomer from CUSTOMERS where deleted=0 and "+campo+" like '%"+busqueda+"%' order by idcustomer", "customers");

            DataTable table = data.Tables["customers"];

            Customer aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Customer(Convert.ToInt32(row["IDCUSTOMER"]));
                readCustomer(aux);
                listCustomer.Add(aux);
            }
        }

        /// <summary>
        /// Reads the customer by order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readCustomerByOrder(Order order)
        {
            DataSet data = new DataSet();
            ConnectOracle Search = new ConnectOracle();

            data = Search.getData("SELECT DISTINCT C.NIF, C.NAME, C.SURNAME, C.ADDRESS, CT.CITY, s.state, C.PHONE " +
                "FROM CUSTOMERS C, ORDERS O, CITIES CT, REGIONS R, STATES S, ZIPCODES ZC, ZIPCODESCITIES ZCC " +
                "WHERE c.idcustomer = o.refcustomer AND c.refzipcodescities = zcc.idzipcodescities" +
                " AND zcc.idzipcodescities = zc.idzipcode AND zcc.refcity = ct.idcity" +
                " AND zcc.refstate = s.idstate AND o.idorder = "+order.idOrder, "customers");

            DataTable tmp = data.Tables["customers"];

            tcustomers.Columns.Add("NIF", Type.GetType("System.String"));
            tcustomers.Columns.Add("NAME", Type.GetType("System.String"));
            tcustomers.Columns.Add("SURNAME", Type.GetType("System.String"));
            tcustomers.Columns.Add("ADDRESS", Type.GetType("System.String"));
            tcustomers.Columns.Add("CITY", Type.GetType("System.String"));
            tcustomers.Columns.Add("STATE", Type.GetType("System.String"));
            tcustomers.Columns.Add("PHONE", Type.GetType("System.String"));

            // Data from the database
            foreach (DataRow row in tmp.Rows)
            {
                tcustomers.Rows.Add(new Object[] { row["NIF"], row["NAME"], row["SURNAME"], row["ADDRESS"], row["CITY"], row["STATE"], row["PHONE"]});
            }
        }

        /// <summary>
        /// Checks the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public Boolean checkCustomer(Customer customer)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(idcustomer)", "CUSTOMERS", "NIF = '" + customer.NIF + "'" + " and deleted = 0"));

            return count != 0;
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void insertCustomer(Customer customer)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int maximum = Convert.ToInt32("0" + connect.DLookUp("max(idcustomer)", "CUSTOMERS", "")) + 1; //PARA SOLUCIONAR EL PROBLEMA DE INICIO DE TABLA
            string nif = customer.NIF;
            string name = customer.name;
            string surname = customer.surname;
            string phone = customer.phone.ToString();
            string email = customer.email;
            string address = customer.address;
            string ref_zipcodes_cities = getRefWithZipCode(customer.zipcode.zipCode);

            string SQL = "INSERT INTO CUSTOMERS VALUES (" + maximum + ",'" + name + "','" + surname + "','"+address+"',"+phone+",'"+email+"',0,"+ref_zipcodes_cities+",'"+nif+"')"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void deleteCustomer(Customer customer)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();
            connect.setData("UPDATE CUSTOMERS SET deleted=1 WHERE idcustomer=" + customer.idCustomer); //ES UN BORRADO LÓGICO
        }

        /// <summary>
        /// Modifies the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void modifyCustomer(Customer customer)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            string SQL = "UPDATE CUSTOMERS SET NAME='" + customer.name + "', SURNAME='" + customer.surname + "', ADDRESS = '" + customer.address+
                "', PHONE = " + customer.phone +", EMAIL = '" + customer.email + "', REFZIPCODESCITIES = '"+ getRefWithZipCode(customer.zipcode.zipCode) + "' WHERE NIF='" + customer.NIF + "'"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }


        //////////////////////// REGIONS ///////////////////////////        
        /// <summary>
        /// Reads all regions.
        /// </summary>
        public void readAllRegions()
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select idregion, region from REGIONS R", "regions");

            DataTable table = data.Tables["regions"];

            Region aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Region();
                aux.nameRegion = Convert.ToString(row["REGION"]);
                aux.id_region = Convert.ToInt32(row["IDREGION"]);

                listRegion.Add(aux);
            }
        }

        //////////////////////// STATES ///////////////////////////        
        /// <summary>
        /// Reads the states.
        /// </summary>
        /// <param name="idRegion">The identifier region.</param>
        public void readStates(int idRegion)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select idstate, state from STATES where refregion = " + idRegion, "states");

            DataTable table = data.Tables["states"];

            State aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new State();
                aux.nameState = Convert.ToString(row["STATE"]);
                aux.id_state = Convert.ToInt32(row["IDSTATE"]);

                listStates.Add(aux);
            }
        }

        //////////////////////// CITIES ///////////////////////////        
        /// <summary>
        /// Reads the cities.
        /// </summary>
        /// <param name="idState">State of the identifier.</param>
        public void readCities(int idState)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select distinct c.idcity, C.CITY from CITIES C, ZIPCODESCITIES ZC, STATES S where" +
                " c.idcity = zc.refcity AND zc.refstate = s.idstate " +
                "AND s.idstate = " + idState, "cities");

            DataTable table = data.Tables["cities"];

            City aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new City();
                aux.nameCity = Convert.ToString(row["CITY"]);
                aux.id_city = Convert.ToInt32(row["IDCITY"]);

                listCities.Add(aux);
            }
        }

        //////////////////////// ZIP CODES ///////////////////////        
        /// <summary>
        /// Checks the zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        public Boolean checkZipCode(String ZipCode)
        {
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(idzipcode)", "ZIPCODES", "zipcode = '"+ZipCode+"'"));

            return count != 0;
        }

        /// <summary>
        /// Reads the zip codes.
        /// </summary>
        /// <param name="idCity">The identifier city.</param>
        /// <param name="idState">State of the identifier.</param>
        public void readZipCodes(int idCity, int idState)
        {
            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select z.idzipcode, z.zipcode from zipcodes z, zipcodescities zc" +
                " where z.idzipcode = zc.refzipcode and zc.refcity = '" + idCity.ToString()+"' and zc.refstate = '" + idState.ToString()+"'", "zipcodes");

            DataTable table = data.Tables["zipcodes"];

            ZipCode aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new ZipCode();
                aux.zipCode = Convert.ToString(row["ZIPCODE"]);
                aux.id_zipCode = Convert.ToInt32(row["IDZIPCODE"]);

                listZipCode.Add(aux);
            }
        }

        /// <summary>
        /// Fills the with zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        public Customer fillWithZipCode(String ZipCode)
        {
            Customer customer = new Customer();

            customer.region = getRegionWithZipCode(ZipCode);
            customer.state = getStateWithZipCode(ZipCode);
            customer.city = getCityWithZipCode(ZipCode);
            customer.zipcode = getZipCodeWithZipCode(ZipCode);

            return customer;
        }


        //Obtener datos con el codigo postal        
        /// <summary>
        /// Gets the region with zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        private Region getRegionWithZipCode(String ZipCode)
        {
            Region region = new Region();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select r.idregion, r.region from states s, regions r, zipcodes z, zipcodescities zc " +
                "where zc.refstate = s.idstate and zc.refzipcode = z.idzipcode and s.refregion = r.idregion " +
                "and z.zipcode = '" + ZipCode + "'", "regions");

            DataTable table = data.Tables["regions"];

            foreach (DataRow row in table.Rows)
            {
                region.id_region = Convert.ToInt32(row["IDREGION"]);
                region.nameRegion = Convert.ToString(row["REGION"]);
            }

            return region;
        }

        /// <summary>
        /// Gets the state with zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        private State getStateWithZipCode(String ZipCode)
        {
            State state = new State();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select s.idstate,s.state from states s, zipcodes z, zipcodescities zc " +
                "where zc.refstate = s.idstate and zc.refzipcode = z.idzipcode " +
                "and z.zipcode = '" + ZipCode + "'", "states");

            DataTable table = data.Tables["states"];

            foreach (DataRow row in table.Rows)
            {
                state.id_state  = Convert.ToInt32(row["IDSTATE"]);
                state.nameState = Convert.ToString(row["STATE"]);
            }

            return state;
        }

        /// <summary>
        /// Gets the city with zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        private City getCityWithZipCode(String ZipCode)
        {
            City city = new City();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select c.idcity, c.city from cities c,zipcodes z, zipcodescities zc " +
                "where zc.refcity = c.idcity and zc.refzipcode = z.idzipcode " +
                "and z.zipcode = '" + ZipCode + "'", "cities");

            DataTable table = data.Tables["cities"];

            foreach (DataRow row in table.Rows)
            {
                city.id_city = Convert.ToInt32(row["IDCITY"]);
                city.nameCity = Convert.ToString(row["CITY"]);
            }

            return city;
        }

        /// <summary>
        /// Gets the zip code with zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        private ZipCode getZipCodeWithZipCode(String ZipCode)
        {
            ZipCode zipCode = new ZipCode();

            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            zipCode.id_zipCode = Convert.ToInt32(connect.DLookUp("idzipcode", "ZIPCODES", "zipcode = '" + ZipCode + "'"));
            zipCode.zipCode = ZipCode;

            return zipCode;
        }

        /// <summary>
        /// Gets the reference with zip code.
        /// </summary>
        /// <param name="ZipCode">The zip code.</param>
        /// <returns></returns>
        private String getRefWithZipCode(String ZipCode)
        {
            String referencia = null;

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select zc.refzipcode from zipcodes z, zipcodescities zc where zc.refzipcode = z.idzipcode " +
                "and z.zipcode = '" + ZipCode + "'", "zipcodescities");

            DataTable table = data.Tables["zipcodescities"];

            foreach (DataRow row in table.Rows)
            {
                referencia = Convert.ToString(row["REFZIPCODE"]);
            }

            return referencia;
        }


        //Obtener datos con la referencia almacenada en customer        
        /// <summary>
        /// Gets the region with reference.
        /// </summary>
        /// <param name="refZipCode">The reference zip code.</param>
        /// <returns></returns>
        public Region getRegionWithRef(String refZipCode)
        {
            Region region = new Region();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select r.idregion, r.region from states s, regions r, customers c, zipcodescities zc " +
                "where zc.refstate = s.idstate and zc.refzipcode = c.refzipcodescities and s.refregion = r.idregion " +
                "and c.refzipcodescities = '" + refZipCode + "'", "regions");

            DataTable table = data.Tables["regions"];

            foreach (DataRow row in table.Rows)
            {
                region.id_region = Convert.ToInt32(row["IDREGION"]);
                region.nameRegion = Convert.ToString(row["REGION"]);
            }

            return region;
        }

        /// <summary>
        /// Gets the state with reference.
        /// </summary>
        /// <param name="refZipCode">The reference zip code.</param>
        /// <returns></returns>
        public State getStateWithRef(String refZipCode)
        {
            State state = new State();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select s.idstate, s.state from states s, customers c, zipcodescities zc " +
                "where zc.refstate = s.idstate and zc.refzipcode = c.refzipcodescities " +
                "and c.refzipcodescities = '" + refZipCode + "'", "states");

            DataTable table = data.Tables["states"];

            foreach (DataRow row in table.Rows)
            {
                state.id_state = Convert.ToInt32(row["IDSTATE"]);
                state.nameState = Convert.ToString(row["STATE"]);
            }

            return state;
        }

        /// <summary>
        /// Gets the city with reference.
        /// </summary>
        /// <param name="refZipCode">The reference zip code.</param>
        /// <returns></returns>
        public City getCityWithRef(String refZipCode)
        {
            City city = new City();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select ct.idcity, ct.city from cities ct, customers c, zipcodescities zc " +
                "where zc.refcity = ct.idcity and zc.refzipcode = c.refzipcodescities " +
                "and c.refzipcodescities = '" + refZipCode + "'", "cities");

            DataTable table = data.Tables["cities"];

            foreach (DataRow row in table.Rows)
            {
                city.id_city = Convert.ToInt32(row["IDCITY"]);
                city.nameCity = Convert.ToString(row["CITY"]);
            }

            return city;
        }

        /// <summary>
        /// Gets the zip code with reference.
        /// </summary>
        /// <param name="refZipCode">The reference zip code.</param>
        /// <returns></returns>
        public ZipCode getZipCodeWithRef(String refZipCode)
        {
            
            ZipCode zipCode = new ZipCode();

            DataSet data = new DataSet();
            Persistence.ConnectOracle connect = new Persistence.ConnectOracle();

            data = connect.getData("select z.idzipcode,z.zipcode from zipcodes z, customers c, zipcodescities zc " +
                "where zc.refzipcode = z.idzipcode and zc.refzipcode = c.refzipcodescities " +
                "and c.refzipcodescities = '" + refZipCode + "'", "zipcodes");

            DataTable table = data.Tables["zipcodes"];

            foreach (DataRow row in table.Rows)
            {
                zipCode.id_zipCode = Convert.ToInt32(row["IDZIPCODE"]);
                zipCode.zipCode = Convert.ToString(row["ZIPCODE"]);
            }

            return zipCode;
        }
    }
}
