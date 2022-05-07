using DiseñoERP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LITTLERP.Domain
{
    /// <summary>
    /// Customer
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the identifier customer.
        /// </summary>
        /// <value>
        /// The identifier customer.
        /// </value>
        public int idCustomer { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public int phone { get; set; }

        /// <summary>
        /// Gets or sets the reference zip code.
        /// </summary>
        /// <value>
        /// The reference zip code.
        /// </value>
        public int ref_zip_code { get; set; }

        /// <summary>
        /// Gets or sets the nif.
        /// </summary>
        /// <value>
        /// The nif.
        /// </value>
        public String NIF { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public String surname { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public String address { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public String email { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public Region region { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public State state { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public City city { get; set; }

        /// <summary>
        /// Gets or sets the zipcode.
        /// </summary>
        /// <value>
        /// The zipcode.
        /// </value>
        public ZipCode zipcode { get; set; }


        /// <summary>
        /// The manage
        /// </summary>
        public Manage.CustomerManage manage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        public Customer()
        {
            manage = new Manage.CustomerManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="idCustomer">The identifier customer.</param>
        public Customer(int idCustomer)
        {
            this.idCustomer = idCustomer;
            manage = new Manage.CustomerManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="idCustomer">The identifier customer.</param>
        /// <param name="nIF">The nif.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="address">The address.</param>
        /// <param name="ref_zip_code">The reference zip code.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="email">The email.</param>
        public Customer(int idCustomer, string nIF, string name, string surname, string address, int ref_zip_code, int phone, string email) : this(idCustomer)
        {
            NIF = nIF;
            this.name = name;
            this.surname = surname;
            this.address = address;
            this.ref_zip_code = ref_zip_code;
            this.phone = phone;
            this.email = email;
        }

        /// <summary>
        /// Reads all customers.
        /// </summary>
        public void readAll()
        {
            manage.readAll();
        }

        /// <summary>
        /// Reads the customer.
        /// </summary>
        public void readCustomer()
        {
            manage.readCustomer(this);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        public void insertCustomer()
        {
            manage.insertCustomer(this);
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        public void deleteCustomer()
        {
            manage.deleteCustomer(this);
        }

        /// <summary>
        /// Modifies the customer.
        /// </summary>
        public void modifyCustomer()
        {
            manage.modifyCustomer(this);
        }

        /// <summary>
        /// Checks the customer.
        /// </summary>
        /// <returns></returns>
        public Boolean checkCustomer()
        {
            return manage.checkCustomer(this);
        }

        /// <summary>
        /// Reads the specific customer.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificCustomer(String campo, String busqueda)
        {
            manage.readSpecificCustomer(campo, busqueda);
        }
    }
}
