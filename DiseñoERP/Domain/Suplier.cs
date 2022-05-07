using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    class Suplier
    {
        /// <summary>
        /// Gets or sets the dni.
        /// </summary>
        /// <value>
        /// The dni.
        /// </value>
        public int DNI { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public String Surname { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public String Location { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public String Direction { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public int Phone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public String Email { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public SuplierManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Suplier"/> class.
        /// </summary>
        public Suplier()
        {
            manage = new SuplierManage();
        }

        /// <summary>
        /// Reads all supliers.
        /// </summary>
        public void readAll()
        {
            manage.readAll();
        }
    }
}
