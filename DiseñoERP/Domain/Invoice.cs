using DiseñoERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain
{
    /// <summary>
    /// Invoice
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Gets or sets the identifier invoice.
        /// </summary>
        /// <value>
        /// The identifier invoice.
        /// </value>
        public int idInvoice { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime date { get; set; }

        /// <summary>
        /// Gets or sets the accounted.
        /// </summary>
        /// <value>
        /// The accounted.
        /// </value>
        public int accounted { get; set; }

        /// <summary>
        /// Gets or sets the manage.
        /// </summary>
        /// <value>
        /// The manage.
        /// </value>
        public InvoiceManage manage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> class.
        /// </summary>
        public Invoice()
        {
            manage = new InvoiceManage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> class.
        /// </summary>
        /// <param name="idInvoice">The identifier invoice.</param>
        public Invoice(int idInvoice)
        {
            this.idInvoice = idInvoice; 
            manage = new InvoiceManage();
        }

        /// <summary>
        /// Reads all invoices.
        /// </summary>
        public void readAll()
        {
            manage.readAll();
        }

        /// <summary>
        /// Reads the invoice.
        /// </summary>
        public void readInvoice()
        {
            manage.readInvoice(this);
        }

        /// <summary>
        /// Reads the specific invoice.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificInvoice(String campo, String busqueda)
        {
            this.manage.readSpecificInvoice(campo, busqueda);
        }

        /// <summary>
        /// Reads the information invoice to crystal report.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readInfoInvoice_ToCrystalReport(Order order)
        {
            manage.readInfoInvoice_ToCrystalReport(order, this);
        }

        /// <summary>
        /// Inserts the invoice.
        /// </summary>
        public void insertInvoice()
        {
            manage.insertInvoice();
        }

        /// <summary>
        /// Updates the accounted.
        /// </summary>
        public void updateAccounted()
        {
            manage.updateAccounted(this);
        }

        /// <summary>
        /// Deletes the invoice.
        /// </summary>
        public void deleteInvoice()
        {
            manage.deleteInvoice(this);
        }

        /// <summary>
        /// Counts the identifier.
        /// </summary>
        /// <returns></returns>
        public int countId()
        {
           return manage.countId();
        }

        /// <summary>
        /// Checks the before invoice accounted.
        /// </summary>
        /// <returns></returns>
        public Boolean checkBeforeInvoiceAccounted()
        {
            return manage.checkBeforeInvoiceAccounted(this);
        }

        /// <summary>
        /// Searches the identifier order.
        /// </summary>
        /// <returns></returns>
        public int searchIdOrder()
        {
            return manage.searchIdOrder(this);
        }
    }
}
