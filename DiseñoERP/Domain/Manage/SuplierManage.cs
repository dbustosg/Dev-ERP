using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiseñoERP.Domain.Manage
{
    class SuplierManage
    {
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        public List<Suplier> list { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuplierManage"/> class.
        /// </summary>
        public SuplierManage()
        {
            list = new List<Suplier>();
        }

        /// <summary>
        /// Reads all supliers.
        /// </summary>
        public void readAll()
        {
            String json = System.IO.File.ReadAllText(@"Supliers.json");
            list = JsonConvert.DeserializeObject<List<Suplier>>(json);
        }
    }
}
