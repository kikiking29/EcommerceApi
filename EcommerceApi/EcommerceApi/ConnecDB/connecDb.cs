using EcommerceApi.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace EcommerceApi.ConnecDB
{
    public class ConnecDb
    {
        string connectionstring = "Server=localhost;Database=hauntshop;Uid=root;Pwd=1234;";
        public string connectDb()
        {
            string connectionString = "Server=localhost;Database=hauntshop;Uid=root;Pwd=1234;";
            return connectionString;
        }

        public String Setdata(string Sql)
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            connection.Open();
            MySqlCommand comm = new MySqlCommand(Sql, connection);
            String result = Convert.ToString(comm.ExecuteNonQuery());
            connection.Close();
            return result;
        }

        public DataSet Selectdata(string Sql)
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            connection.Open();
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            connection.Close();
            return ds;
        }

        public DataSet Selectitem(string Sql)
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            connection.Open();
            if (Sql != null)
            {
                var db = "SELECT * FROM users WHERE TRUE";
                MySqlDataAdapter dap = new MySqlDataAdapter(db + Sql + ";", connection);
                dap.Fill(ds);
            }
            connection.Close();
            return ds;
        }


        public int CheckIduser(UserDto data)
        {
            int id = 0;
            PasswordModels passwrd = new PasswordModels();
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            connection.Open();
            string Sql = "SELECT id_users FROM users where u_usersname='" + data.Username + "'AND u_password=CONCAT('*', UPPER(SHA1(UNHEX(SHA1('" + data.Password + "')))));";
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                id = int.Parse(dr["id_users"].ToString());

            }
            connection.Close();
            return id;
        }
        public string getRole(UserDto data)
        {
            int id = 0;
            PasswordModels passwrd = new PasswordModels();
            MySqlConnection connection = new MySqlConnection(connectionstring);
            DataSet ds = new DataSet();
            connection.Open();
            string Sql = "SELECT u_role FROM users where u_usersname='" + data.Username + "'AND u_password=CONCAT('*', UPPER(SHA1(UNHEX(SHA1('" + data.Password + "')))));";
            MySqlDataAdapter dap = new MySqlDataAdapter(Sql, connection);
            dap.Fill(ds);
            string u_role = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                u_role = dr["u_role"].ToString();

            }
            connection.Close();
            return u_role;
        }
    }
}
