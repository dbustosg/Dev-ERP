using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Domain.Manage
{
    /// <summary>
    /// Manage of product
    /// </summary>
    public class ProductManage
    {
        /// <summary>
        /// The list products
        /// </summary>
        public List<Product> listProducts = new List<Product>();
        /// <summary>
        /// The list size products
        /// </summary>
        public List<SizeProducts> listSizeProducts = new List<SizeProducts>();
        /// <summary>
        /// The list material products
        /// </summary>
        public List<MaterialProducts> listMaterialProducts = new List<MaterialProducts>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManage"/> class.
        /// </summary>
        public ProductManage()
        {
            listProducts = new List<Product>();
            listSizeProducts = new List<SizeProducts>();
            listMaterialProducts = new List<MaterialProducts>();
        }

        /// <summary>
        /// Reads the product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void readProduct(Product product)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select * from PRODUCTS where idproduct =" + product.Id_product, "products");

            DataTable table = data.Tables["products"];
            DataRow row = table.Rows[0];

            product.Id_product = Convert.ToInt32(row["IDPRODUCT"]);
            product.Description = Convert.ToString(row["DESCRIPTION"]);
            product.Price = Convert.ToDouble(row["PRICE"]);
            product.WholesalePrice = Convert.ToDouble(row["WHOLESALE_PRICE"]);

            product.SizeProduct = new SizeProducts(Convert.ToInt32(row["ID_SIZE"]));
            readSizeProduct(product.SizeProduct);

            product.MaterialProduct = new MaterialProducts(Convert.ToInt32(row["ID_MATERIAL"]));
            readMaterialProduct(product.MaterialProduct);
        }

        /// <summary>
        /// Reads all products.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            data = connect.getData("select idproduct from PRODUCTS where deleted=0 order by idproduct", "products");

            DataTable table = data.Tables["products"];

            Product aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Product(Convert.ToInt32(row["IDPRODUCT"]));
                readProduct(aux);
                listProducts.Add(aux);
            }
        }

        /// <summary>
        /// Reads the specific product.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="busqueda">The busqueda.</param>
        public void readSpecificProduct(String campo, String busqueda)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            data = connect.getData("select idproduct from PRODUCTS where deleted=0 and " + campo + " like '%" + busqueda + "%' order by idproduct", "products");

            DataTable table = data.Tables["products"];

            Product aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Product(Convert.ToInt32(row["IDPRODUCT"]));
                readProduct(aux);
                listProducts.Add(aux);
            }
        }

        /// <summary>
        /// Reads the size product.
        /// </summary>
        /// <param name="size">The size.</param>
        public void readSizeProduct(SizeProducts size)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select * from SIZES_PRODUCTS where id =" + size.id_size, "sizes_products");

            DataTable table = data.Tables["sizes_products"];
            DataRow row = table.Rows[0];

            size.id_size = Convert.ToInt32(row["ID"]);
            size.messures = Convert.ToString(row["MESSURES"]);
        }

        /// <summary>
        /// Reads the material product.
        /// </summary>
        /// <param name="material">The material.</param>
        public void readMaterialProduct(MaterialProducts material)
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            data = connect.getData("select * from MATERIAL_PRODUCTS where id =" + material.id_material, "material_products");

            DataTable table = data.Tables["material_products"];
            DataRow row = table.Rows[0];

            material.id_material = Convert.ToInt32(row["ID"]);
            material.nameMaterial = Convert.ToString(row["MATERIAL"]);
        }

        /// <summary>
        /// Inserts the product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void insertProduct(Product product)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            int maximum = Convert.ToInt32("0" + connect.DLookUp("max(idproduct)", "PRODUCTS", "")) + 1; //PARA SOLUCIONAR EL PROBLEMA DE INICIO DE TABLA
            String description = product.Description;

            String price = Convert.ToString(product.Price);
            price = price.Replace(",", ".");

            String wholesale = Convert.ToString(product.WholesalePrice);
            wholesale = wholesale.Replace(",", ".");

            int idSize = product.SizeProduct.id_size;
            int idMaterial = product.MaterialProduct.id_material;

            string SQL = "INSERT INTO PRODUCTS VALUES (" + maximum + "," + idSize + "," + idMaterial + "," + price + ",0,'" + description + "'," + wholesale + ")"; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void deleteProduct(Product product)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();
            connect.setData("UPDATE PRODUCTS SET deleted=1 WHERE idproduct=" + product.Id_product); //ES UN BORRADO LÓGICO
        }

        /// <summary>
        /// Modifies the product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void modifyProduct(Product product)
        {
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            String price = Convert.ToString(product.Price);
            price = price.Replace(",", ".");

            String wholesale = Convert.ToString(product.WholesalePrice);
            wholesale = wholesale.Replace(",", ".");

            string SQL = "UPDATE PRODUCTS SET DESCRIPTION ='" + product.Description + "', PRICE =" + price + ", WHOLESALE_PRICE = " + wholesale +
                ", ID_SIZE = " + product.SizeProduct.id_size + ", ID_MATERIAL = " + product.MaterialProduct.id_material + " WHERE IDPRODUCT=" + product.Id_product; //COMILLAS SIMPLES PARA QUE INTERPRETE LOS VALORES COMO STRING

            connect.setData(SQL);
        }

        //////////////////////// MATERIALS ///////////////////////////

        /// <summary>
        /// Reads all materials.
        /// </summary>
        public void readAllMaterials()
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            data = connect.getData("select id, material from MATERIAL_PRODUCTS", "material_products");

            DataTable table = data.Tables["material_products"];

            MaterialProducts aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new MaterialProducts();
                aux.id_material = Convert.ToInt32(row["ID"]);
                aux.nameMaterial = Convert.ToString(row["MATERIAL"]);

                listMaterialProducts.Add(aux);
            }
        }

        /// <summary>
        /// Reads the material product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public MaterialProducts readMaterialProduct(int id)
        {
            MaterialProducts materialProduct = new MaterialProducts();

            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            data = connect.getData("select id, material from MATERIAL_PRODUCTS where id = "+id, "material_products");

            DataTable table = data.Tables["material_products"];

            foreach (DataRow row in table.Rows)
            {
                materialProduct.id_material = Convert.ToInt32(row["ID"]);
                materialProduct.nameMaterial = Convert.ToString(row["MATERIAL"]);
            }

            return materialProduct;
        }

        /// <summary>
        /// Reads all messures.
        /// </summary>
        public void readAllMessures()
        {
            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            data = connect.getData("select id, messures from SIZES_PRODUCTS", "sizes_products");

            DataTable table = data.Tables["sizes_products"];

            SizeProducts aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new SizeProducts();
                aux.id_size = Convert.ToInt32(row["ID"]);
                aux.messures = Convert.ToString(row["MESSURES"]);

                listSizeProducts.Add(aux);
            }
        }

        /// <summary>
        /// Reads the size product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public SizeProducts readSizeProduct(int id)
        {
            SizeProducts sizeProducts = new SizeProducts();

            DataSet data = new DataSet();
            LITTLERP.Persistence.ConnectOracle connect = new LITTLERP.Persistence.ConnectOracle();

            data = connect.getData("select id, messures from SIZES_PRODUCTS where id = "+id, "sizes_products");

            DataTable table = data.Tables["sizes_products"];

            foreach (DataRow row in table.Rows)
            {
                sizeProducts.id_size = Convert.ToInt32(row["ID"]);
                sizeProducts.messures = Convert.ToString(row["MESSURES"]);
            }

            return sizeProducts;    
        }

    }
}
