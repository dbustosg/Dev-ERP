using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LITTLERP.Persistence;

namespace DiseñoERP.Domain.Manage
{
    /// <summary>
    /// Manage of order
    /// </summary>
    public class OrderManage
    {
        /// <summary>
        /// Gets or sets the order list.
        /// </summary>
        /// <value>
        /// The order list.
        /// </value>
        public List<Order> orderList { get; set; }

        /// <summary>
        /// Gets or sets the payment methods list.
        /// </summary>
        /// <value>
        /// The payment methods list.
        /// </value>
        public List<PaymentMethod> paymentMethodsList { get; set; }

        /// <summary>
        /// Gets or sets the order product list.
        /// </summary>
        /// <value>
        /// The order product list.
        /// </value>
        public List<OrderProduct> orderProductList { get; set; }

        /// <summary>
        /// Gets or sets the order generic list.
        /// </summary>
        /// <value>
        /// The order generic list.
        /// </value>
        public List<OrderGeneric> orderGenericList { get; set; }

        /// <summary>
        /// Gets or sets the t order product.
        /// </summary>
        /// <value>
        /// The t order product.
        /// </value>
        public DataTable tOrderProduct { get; set; }
        /// <summary>
        /// Gets or sets the t order.
        /// </summary>
        /// <value>
        /// The t order.
        /// </value>
        public DataTable tOrder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderManage"/> class.
        /// </summary>
        public OrderManage()
        {
            orderList = new List<Order>();
            paymentMethodsList = new List<PaymentMethod>();
            orderGenericList = new List<OrderGeneric>();
            orderProductList = new List<OrderProduct>();
            tOrderProduct = new DataTable();
            tOrder = new DataTable();   
        }

        /// <summary>
        /// Reads all orders.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select idorder from ORDERS where deleted=0 order by idorder", "orders");

            DataTable table = data.Tables["orders"];

            Order aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Order(Convert.ToInt32(row["IDORDER"]));
                readOrder(aux);
                orderList.Add(aux);
            }
        }

        /// <summary>
        /// Reads the order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readOrder(Order order)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select * from ORDERS O, ORDERS_STATUS OS where O.IDORDER = OS.IDORDER AND  O.idorder =" + order.idOrder, "orders");

            DataTable table = data.Tables["orders"];
            DataRow row = table.Rows[0];

            order.status = new StatusOrder(order.idOrder);

            order.idOrder = Convert.ToInt32(row["IDORDER"]);
            order.ref_customer = Convert.ToInt32(row["REFCUSTOMER"]);
            order.ref_user = Convert.ToInt32(row["REFUSER"]);
            order.ref_paymentMethod = Convert.ToInt32(row["REFPAYMENTMETHOD"]);
            order.date = Convert.ToString(row["DATETIME"]);
            order.total = Convert.ToDouble(row["TOTAL"]);
            order.prepaid = Convert.ToDouble(row["PREPAID"]);
            order.status.confirmed = Convert.ToInt32(row["CONFIRMED"]);
            order.status.labeled = Convert.ToInt32(row["LABELED"]);
            order.status.sent = Convert.ToInt32(row["SENT"]);
            order.status.invoiced = Convert.ToInt32(row["INVOICED"]);

            checkOrderStatus(order.status);
        }

        /// <summary>
        /// Reads the specific order.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificOrder(String campo, String busqueda)
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select idorder from ORDERS where deleted=0 and " + campo + " like '%" + busqueda + "%' order by idorder", "orders");

            DataTable table = data.Tables["orders"];

            Order aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Order(Convert.ToInt32(row["IDORDER"]));
                readOrder(aux);
                orderList.Add(aux);
            }
        }

        /// <summary>
        /// Reads the information order to crystal report.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readInfoOrder_ToCrystalReport(Order order)
        {
            order.readOrder();
            PaymentMethod paymentMethod = new PaymentMethod(order.ref_paymentMethod);
            paymentMethod.readPaymentMethod();

            tOrder.Columns.Add("PaymentMethod", Type.GetType("System.String"));
            tOrder.Columns.Add("Total", Type.GetType("System.Double"));
            tOrder.Columns.Add("Tax", Type.GetType("System.Double"));
            tOrder.Columns.Add("TotalWithTax", Type.GetType("System.Double"));

            tOrder.Rows.Add(paymentMethod.method, order.total, order.total*0.21, order.total*1.21);
        }

        /// <summary>
        /// Reads the order by invoice.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <param name="order">The order.</param>
        public void readOrderByInvoice(Invoice invoice, Order order)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select REF_ORDER from ORDER_INVOICE where ref_invoice = " + invoice.idInvoice, "order_invoice");

            DataTable table = data.Tables["order_invoice"];
            DataRow row = table.Rows[0];

            order.idOrder = Convert.ToInt32(row["REF_ORDER"]);
            order.readOrder();
        }

        /// <summary>
        /// Reads the product of order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readProductOfOrder(Order order)
        {
            readProductGeneric(order);
            readProductNormal(order);
        }

        /// <summary>
        /// Inserts the order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void insertOrder(Order order)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            String total = Convert.ToString(order.total);
            total = total.Replace(",", ".");

            String prepaid = Convert.ToString(order.prepaid);
            prepaid = prepaid.Replace(",", ".");

            string SQL = "INSERT INTO ORDERS VALUES (" + order.idOrder + "," + order.ref_customer + "," + order.ref_user + ",SYSDATE," + order.ref_paymentMethod + "," + total + "," + prepaid + ",0)"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Deletes the order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void deleteOrder(Order order)
        {
            ConnectOracle connect = new ConnectOracle();
            connect.setData("UPDATE ORDERS SET deleted=1 WHERE idorder =" + order.idOrder); //ES UN BORRADO LÓGICO
        }

        /// <summary>
        /// Deletes the no logic order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void deleteNoLogicOrder(Order order)
        {
            ConnectOracle connect = new ConnectOracle();
            connect.setData("DELETE FROM ORDERS WHERE idorder =" + order.idOrder); //Borra order para volver a crearlo en el modify, asi como los productos del pedido
        }

        /// <summary>
        /// Counts the identifier.
        /// </summary>
        /// <returns></returns>
        public int countId()
        {
            ConnectOracle connect = new ConnectOracle();

            int count = Convert.ToInt32(connect.DLookUp("count(idorder)", "ORDERS", ""));

            return count;
        }

        /////////// Status        
        /// <summary>
        /// Reads the status order.
        /// </summary>
        /// <param name="status">The status.</param>
        public void readStatusOrder(StatusOrder status)
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select * from ORDERS_STATUS where idorder = " + status.idOrderStatus, "orders_status");

            DataTable table = data.Tables["orders_status"];

            DataRow row = table.Rows[0];

            status.confirmed = Convert.ToInt32(row["CONFIRMED"]);
            status.labeled = Convert.ToInt32(row["LABELED"]);
            status.sent = Convert.ToInt32(row["SENT"]);
            status.invoiced = Convert.ToInt32(row["INVOICED"]);
        }

        /// <summary>
        /// Inserts the status order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void insertStatusOrder(Order order)
        {
            ConnectOracle connect = new ConnectOracle();

            String SQL = null;

            if (order.ref_paymentMethod == 2)
                SQL = "INSERT INTO ORDERS_STATUS VALUES (" + order.idOrder + ",1,0,0,0)";
            else
                SQL = "INSERT INTO ORDERS_STATUS VALUES (" + order.idOrder + ",0,0,0,0)";

            connect.setData(SQL);
        }

        /// <summary>
        /// Updates the confirmed.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="option">The option.</param>
        public void updateConfirmed(StatusOrder status, int option)
        {
            ConnectOracle connect = new ConnectOracle();

            string SQL = "UPDATE ORDERS_STATUS SET CONFIRMED = " + option + " WHERE IDORDER = " + status.idOrderStatus;

            connect.setData(SQL);
        }

        /// <summary>
        /// Updates the labeled.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="option">The option.</param>
        public void updateLabeled(StatusOrder status, int option)
        {
            ConnectOracle connect = new ConnectOracle();

            string SQL = "UPDATE ORDERS_STATUS SET LABELED = " + option + " WHERE IDORDER = " + status.idOrderStatus;

            connect.setData(SQL);
        }

        /// <summary>
        /// Updates the sent.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="option">The option.</param>
        public void updateSent(StatusOrder status, int option)
        {
            ConnectOracle connect = new ConnectOracle();

            string SQL = "UPDATE ORDERS_STATUS SET SENT = " + option + " WHERE IDORDER = " + status.idOrderStatus;

            connect.setData(SQL);
        }

        /// <summary>
        /// Updates the invoiced.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="option">The option.</param>
        public void updateInvoiced(StatusOrder status, int option)
        {
            ConnectOracle connect = new ConnectOracle();

            string SQL = "UPDATE ORDERS_STATUS SET INVOICED = " + option + " WHERE IDORDER = " + status.idOrderStatus;

            connect.setData(SQL);
        }

        /// <summary>
        /// Checks the order status.
        /// </summary>
        /// <param name="status">The status.</param>
        private void checkOrderStatus(StatusOrder status)
        {
            readStatusOrder(status);

            if (status.confirmed == 1)
                status.nameStatus = "Confirmed";
            if (status.labeled == 1)
                status.nameStatus = "Labeled";
            if (status.sent == 1)
                status.nameStatus = "Sent";
            if (status.invoiced == 1)
                status.nameStatus = "Invoiced";
        }

        //////////// payment method

        /// <summary>
        /// Reads all payment method.
        /// </summary>
        public void readAllPaymentMethod()
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select idpaymentmethod, paymentmethod from PAYMENTMETHODS", "paymentmethods");

            DataTable table = data.Tables["paymentmethods"];

            PaymentMethod aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new PaymentMethod();
                aux.idMethod = Convert.ToInt32(row["IDPAYMENTMETHOD"]);
                aux.method = Convert.ToString(row["PAYMENTMETHOD"]);

                paymentMethodsList.Add(aux);
            }
        }

        /// <summary>
        /// Reads the payment method.
        /// </summary>
        /// <param name="paymentMethod">The payment method.</param>
        public void readPaymentMethod(PaymentMethod paymentMethod)
        {
            DataSet data = new DataSet();
            ConnectOracle connect = new ConnectOracle();

            data = connect.getData("select idpaymentmethod, paymentmethod from PAYMENTMETHODS where idpaymentmethod = " + paymentMethod.idMethod, "paymentmethods");

            DataTable table = data.Tables["paymentmethods"];
            ;
            DataRow row = table.Rows[0];

            paymentMethod.idMethod = Convert.ToInt32(row["IDPAYMENTMETHOD"]);
            paymentMethod.method = Convert.ToString(row["PAYMENTMETHOD"]);
        }


        //////////// ORDER PRODUCTS /////////////

        /// <summary>
        /// Reads the product normal.
        /// </summary>
        /// <param name="order">The order.</param>
        private void readProductNormal(Order order)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            //data = connect.getData("select * from ORDERSPRODUCTS where idorderproduct =" + orderProduct.idOrderProduct, "ordersproduct");
            data = connect.getData("SELECT * FROM ORDERSPRODUCTS where reforder =" + order.idOrder, "ordersproduct");

            DataTable table = data.Tables["ordersproduct"];
            OrderProduct aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new OrderProduct();
                aux.idOrderProduct = Convert.ToInt32(row["IDORDERPRODUCT"]);
                readOrderProduct(aux);
                orderProductList.Add(aux);
            }
        }

        /// <summary>
        /// Inserts the order product.
        /// </summary>
        /// <param name="orderProduct">The order product.</param>
        public void insertOrderProduct(OrderProduct orderProduct)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            int maximumNormalProducts = Convert.ToInt32("0" + connect.DLookUp("max(idorderproduct)", "ORDERSPRODUCTS", "")) + 1; //PARA SOLUCIONAR EL PROBLEMA DE INICIO DE TABLA
            int maximumGenericProducts = Convert.ToInt32("0" + connect.DLookUp("max(idorderproduct)", "ORDERS_GENERICS", "")) + 1;

            String price = Convert.ToString(orderProduct.priceofsale);
            price = price.Replace(",", ".");

            string SQL = null;

            if (orderProduct.refProduct != -1)
                SQL = "INSERT INTO ORDERSPRODUCTS VALUES (" + maximumNormalProducts + "," + orderProduct.refOrder + "," + orderProduct.refProduct + "," + orderProduct.amount + "," + price + ")"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING
            else if (orderProduct.refProduct == -1)
                SQL = "INSERT INTO ORDERS_GENERICS VALUES (" + orderProduct.refOrder + ",'" + orderProduct.description + "'," + orderProduct.amount + "," + price + "," + maximumGenericProducts + ")";

            connect.setData(SQL);
        }

        /// <summary>
        /// Reads all order product.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readAllOrderProduct(Order order)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            //data = connect.getData("select * from ORDERSPRODUCTS where idorderproduct =" + orderProduct.idOrderProduct, "ordersproduct");
            data = connect.getData("SELECT IDORDERPRODUCT, DESCRIPTION,REFORDER,AMOUNT,PRICESALE FROM ORDERS_GENERICS where reforder = " + order.idOrder +
                "UNION " +
                "SELECT IDORDERPRODUCT,(SELECT DESCRIPTION FROM PRODUCTS WHERE IDPRODUCT = OP.REFPRODUCT), " +
                    "REFORDER, AMOUNT, PRICESALE FROM ORDERSPRODUCTS OP where reforder =" + order.idOrder, "ordersproduct");

            DataTable table = data.Tables["ordersproduct"];
            OrderProduct aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new OrderProduct();
                aux.idOrderProduct = Convert.ToInt32(row["IDORDERPRODUCT"]);
                aux.refOrder = Convert.ToInt32(row["REFORDER"]);
                //orderProduct.refProduct = Convert.ToInt32(row["REFPRODUCT"]);
                aux.description = Convert.ToString(row["DESCRIPTION"]);
                aux.amount = Convert.ToInt32(row["AMOUNT"]);
                aux.priceofsale = Convert.ToDouble(row["PRICESALE"]);
                orderProductList.Add(aux);
            }
        }

        /// <summary>
        /// Reads all order product crystal report.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readAllOrderProduct_CrystalReport(Order order)
        {
            readAllOrderProduct(order);

            tOrderProduct.Columns.Add("Description",Type.GetType("System.String"));
            tOrderProduct.Columns.Add("PriceOfSale", Type.GetType("System.Double"));
            tOrderProduct.Columns.Add("Amount", Type.GetType("System.Int32"));
            tOrderProduct.Columns.Add("Total", Type.GetType("System.Double"));

            foreach (OrderProduct op in orderProductList)
            {
                tOrderProduct.Rows.Add(new Object[] {op.description, op.priceofsale, op.amount, op.amount*op.priceofsale });
            }
        }

        /// <summary>
        /// Reads the order product.
        /// </summary>
        /// <param name="orderProduct">The order product.</param>
        public void readOrderProduct(OrderProduct orderProduct)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            //data = connect.getData("select * from ORDERSPRODUCTS where idorderproduct =" + orderProduct.idOrderProduct, "ordersproduct");
            data = connect.getData("SELECT(SELECT DESCRIPTION FROM PRODUCTS WHERE IDPRODUCT = OP.REFPRODUCT) DESCRIPTION, " +
                    "REFORDER, AMOUNT, PRICESALE,REFPRODUCT FROM ORDERSPRODUCTS OP where idorderproduct =" + orderProduct.idOrderProduct, "ordersproduct");

            DataTable table = data.Tables["ordersproduct"];
            DataRow row = table.Rows[0];

            orderProduct.refOrder = Convert.ToInt32(row["REFORDER"]);
            orderProduct.refProduct = Convert.ToInt32(row["REFPRODUCT"]);
            orderProduct.description = Convert.ToString(row["DESCRIPTION"]);
            orderProduct.amount = Convert.ToInt32(row["AMOUNT"]);
            orderProduct.priceofsale = Convert.ToDouble(row["PRICESALE"]);
        }


        //////////// ORDER PRODUCTS GENERIC /////////////

        /// <summary>
        /// Inserts the order product generic.
        /// </summary>
        /// <param name="orderGeneric">The order generic.</param>
        public void insertOrderProductGeneric(OrderGeneric orderGeneric)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            int maximum = Convert.ToInt32("0" + connect.DLookUp("max(id)", "ORDERS_GENERICS", "")) + 1; //PARA SOLUCIONAR EL PROBLEMA DE INICIO DE TABLA

            String price = Convert.ToString(orderGeneric.Price);
            price = price.Replace(",", ".");

            string SQL = "INSERT INTO ORDERS_GENERICS VALUES (" + orderGeneric.OrderId + ",'" + orderGeneric.Description + "'," + orderGeneric.Amount + "," + orderGeneric.Price + "," + maximum + ")"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Reads the product generic.
        /// </summary>
        /// <param name="order">The order.</param>
        private void readProductGeneric(Order order)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            //data = connect.getData("select * from ORDERSPRODUCTS where idorderproduct =" + orderProduct.idOrderProduct, "ordersproduct");
            data = connect.getData("SELECT * FROM ORDERS_GENERICS where reforder =" + order.idOrder, "orders_generics");

            DataTable table = data.Tables["orders_generics"];
            OrderProduct aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new OrderProduct();
                aux.idOrderProduct = Convert.ToInt32(row["IDORDERPRODUCT"]);
                aux.refOrder = Convert.ToInt32(row["REFORDER"]);
                aux.refProduct = -1;
                aux.description = Convert.ToString(row["DESCRIPTION"]);
                aux.amount = Convert.ToInt32(row["AMOUNT"]);
                aux.priceofsale = Convert.ToDouble(row["PRICESALE"]);
                orderProductList.Add(aux);
            }
        }
    }
}