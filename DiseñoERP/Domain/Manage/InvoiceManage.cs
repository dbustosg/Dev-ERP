using LITTLERP.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain.Manage
{
    /// <summary>
    /// Manage of invoice
    /// </summary>
    public class InvoiceManage
    {
        /// <summary>
        /// The list invoices
        /// </summary>
        public List<Invoice> listInvoices;

        /// <summary>
        /// The t invoices
        /// </summary>
        public DataTable tInvoices;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceManage"/> class.
        /// </summary>
        public InvoiceManage()
        {
            listInvoices = new List<Invoice>();
            tInvoices = new DataTable();
        }

        /// <summary>
        /// Reads all invoices.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select id_invoice from INVOICE where deleted=0 order by fecha DESC", "invoice");

            DataTable table = data.Tables["invoice"];

            Invoice invoice;

            foreach (DataRow row in table.Rows)
            {
                invoice = new Invoice(Convert.ToInt32(row["ID_INVOICE"]));
                readInvoice(invoice);
                listInvoices.Add(invoice);
            }
        }

        /// <summary>
        /// Reads the invoice.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        public void readInvoice(Invoice invoice)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select * from INVOICE where id_invoice = "+invoice.idInvoice, "invoice");

            DataTable table = data.Tables["invoice"];
            DataRow row = table.Rows[0];

            invoice.date = Convert.ToDateTime(row["FECHA"]);
            invoice.accounted = Convert.ToInt32(row["ACCOUNTED"]);
        }

        /// <summary>
        /// Reads the invoice by order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public Invoice readInvoiceByOrder(Order order)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select REF_INVOICE from ORDER_INVOICE where ref_order = " + order.idOrder, "order_invoice");

            DataTable table = data.Tables["order_invoice"];
            DataRow row = table.Rows[0];

            Invoice invoice = new Invoice();
            invoice.idInvoice = Convert.ToInt32(row["REF_INVOICE"]);
            
            readInvoice(invoice);

            return invoice;
        }

        /// <summary>
        /// Reads the specific invoice.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificInvoice(String campo, String busqueda)
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select id_invoice from INVOICE where deleted=0 and " + campo + " like '%" + busqueda + "%' order by fecha DESC", "invoice");

            DataTable table = data.Tables["invoice"];

            Invoice aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Invoice(Convert.ToInt32(row["ID_INVOICE"]));
                readInvoice(aux);
                listInvoices.Add(aux);
            }
        }
        /// <summary>
        /// Reads the information invoice to crystal report.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="invoice">The invoice.</param>
        public void readInfoInvoice_ToCrystalReport(Order order, Invoice invoice)
        {
            invoice = readInvoiceByOrder(order);

            tInvoices.Columns.Add("IdInvoice", Type.GetType("System.String"));
            tInvoices.Columns.Add("DateTime", Type.GetType("System.String"));

            tInvoices.Rows.Add(new Object[] {invoice.idInvoice, invoice.date});
        }

        /// <summary>
        /// Inserts the invoice.
        /// </summary>
        public void insertInvoice()
        {
            ConnectOracle connect = new ConnectOracle();

            String id = Convert.ToString(DateTime.Today.Year) + Convert.ToString(countId() + 1);

            string SQL = "INSERT INTO INVOICE VALUES (" + id + ",SYSDATE,0,0)";

            connect.setData(SQL);
        }

        /// <summary>
        /// Updates the accounted.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        public void updateAccounted(Invoice invoice)
        {
            ConnectOracle connect = new ConnectOracle();

            string SQL = "UPDATE INVOICE SET ACCOUNTED = 1 WHERE ID_INVOICE = " + invoice.idInvoice;

            connect.setData(SQL);
        }

        /// <summary>
        /// Delete invoice
        /// </summary>
        /// <param name="invoice"></param>
        public void deleteInvoice(Invoice invoice)
        {
            ConnectOracle connect = new ConnectOracle();

            string SQL = "UPDATE INVOICE SET DELETED = 1 WHERE ID_INVOICE = " + invoice.idInvoice;

            connect.setData(SQL);
        }

        /// <summary>
        /// Counts the identifier.
        /// </summary>
        /// <returns></returns>
        public int countId()
        {
            ConnectOracle connect = new ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(ID_INVOICE)", "INVOICE", ""));

            return count;
        }

        /// <summary>
        /// Checks the before invoice accounted.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <returns></returns>
        public Boolean checkBeforeInvoiceAccounted(Invoice invoice)
        { 
            ConnectOracle connect = new ConnectOracle();
            Boolean accounted = false;

            int count = Convert.ToInt32(connect.DLookUp("count(ID_INVOICE)", "INVOICE", "ACCOUNTED = 1 AND ID_INVOICE = "+(invoice.idInvoice-1)));

            String id = Convert.ToString(DateTime.Today.Year) + Convert.ToString(1);

            if (count == 1 || invoice.idInvoice == Convert.ToInt32(id))
                accounted = true;

            return accounted;
        }

        // Order - Invoice        

        /// <summary>
        /// Inserts the order invoice.
        /// </summary>
        /// <param name="order">The order.</param>
        public void insertOrderInvoice(Order order)
        {
            ConnectOracle connect = new ConnectOracle();

            int maximum = Convert.ToInt32("0" + connect.DLookUp("max(id_order_invoice)", "ORDER_INVOICE", "")) + 1;
            String idInvoice = Convert.ToString(DateTime.Today.Year) + Convert.ToString(countId());

            string SQL = "INSERT INTO ORDER_INVOICE VALUES (" + maximum +","+order.idOrder+","+idInvoice +")";

            connect.setData(SQL);
        }

        /// <summary>
        /// Searches the identifier order.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <returns></returns>
        public int searchIdOrder(Invoice invoice)
        {
            ConnectOracle connect = new ConnectOracle();

            int id = Convert.ToInt32(connect.DLookUp("REF_ORDER", "ORDER_INVOICE", "REF_INVOICE = " + invoice.idInvoice));

            return id;
        }
    }
}
