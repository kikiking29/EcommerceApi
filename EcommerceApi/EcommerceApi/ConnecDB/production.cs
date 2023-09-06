using EcommerceApi.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace EcommerceApi.ConnecDB
{
    public class production
    {
        string connectionstring = "Server=localhost;Database=hauntshop;Uid=root;Pwd=1234;";

        public DataSet getShopid(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            var Sql = $"SELECT id_shops FROM shops WHERE id_users={id};";
            connection.Open();
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            connection.Close();
            return ds;
        }

        public int totalpriceOrders(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            var Sql = $"SELECT SUM(od.odt_among*s.s_unitprice) AS Total FROM orderdetails as od JOIN stock as s ON od.id_stock=s.id_stock WHERE id_orders={id};";
            connection.Open();
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            int totalprice = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                totalprice = int.Parse(dr["Total"].ToString());

            }
            connection.Close();
            return totalprice;
        }

        public int totalpriceOrderdetails()
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            var Sql = $"SELECT SUM(od.odt_among*s.s_unitprice) AS Total FROM orderdetails as od JOIN stock as s ON od.id_stock=s.id_stock GROUP BY id_orderdetails;";
            connection.Open();
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            int totalprice = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                totalprice = int.Parse(dr["Total"].ToString());

            }
            connection.Close();
            return totalprice;
        }


        public string checkStatusorder(int id)
        {
            MySqlConnection connection = new MySqlConnection( connectionstring);
            DataSet ds = new DataSet();
            var sql = $"SELECT o_status FROM orders WHERE id_users={id} AND o_status='Buying';";
            connection.Open();
            MySqlDataAdapter dap = new MySqlDataAdapter(sql, connection);
            dap.Fill(ds);
            string status = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                status = dr["o_status"].ToString();

            }
            connection.Close();

            return status;
        }

        public int getUnitprice(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            connection.Open();
            var Sql = $"SELECT s_unitprice FROM stock WHERE id_stock={id};";
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            int s_unitprice = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                s_unitprice = int.Parse(dr["s_unitprice"].ToString());

            }
            connection.Close();
            return s_unitprice;
        }

    }

}
