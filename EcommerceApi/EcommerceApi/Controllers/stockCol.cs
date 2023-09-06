using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Models;
using EcommerceApi.ConnecDB;
using System;
using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Text.RegularExpressions;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcommerceApi.Controllers
{

    public class stockCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Stock")]
        public List<stockModels> Getstockdataall()
        {

            List<stockModels> stocks = new List<stockModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM stock;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    stockModels stock = new stockModels()
                    {
                        id_stock = int.Parse(dr["id_stock"].ToString()),
                        id_shops = int.Parse(dr["id_shops"].ToString()),
                        id_category = int.Parse(dr["id_category"].ToString()),
                        s_name = dr["s_name"].ToString(),
                        s_among = int.Parse(dr["s_among"].ToString()),
                        s_amongall = int.Parse(dr["s_amongall"].ToString()),
                        s_unitprice = int.Parse(dr["s_unitprice"].ToString()),
                        s_description = dr["s_description"].ToString(),
                        s_status = dr["s_status"].ToString(),

                    };
                    stocks.Add(stock);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return stocks;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Stock/{id}")]
        public stockModels Getbystockid(int id)
        {
            stockModels stock = new stockModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM stock WHERE id_stock='" + id+ "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    stock = new stockModels()
                    {
                        id_stock = int.Parse(dr["id_stock"].ToString()),
                        id_shops = int.Parse(dr["id_shops"].ToString()),
                        id_category = int.Parse(dr["id_category"].ToString()),
                        s_name = dr["s_name"].ToString(),
                        s_among = int.Parse(dr["s_among"].ToString()),
                        s_amongall = int.Parse(dr["s_amongall"].ToString()),
                        s_unitprice = int.Parse(dr["s_unitprice"].ToString()),
                        s_description = dr["s_description"].ToString(),
                        s_status = dr["s_status"].ToString(),

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return stock;
        }


        [HttpPost]
        [Route("Stock")]
        public newstockModels Newstock(newstockModels data)
        {
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "INSERT into stock set id_shops=@id_shops,id_category=@id_category,s_name=@s_name,s_among=@s_among,s_amongall=@s_amongall,s_unitprice=@s_unitprice,s_description=@s_description,s_status=@s_status;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_shops", data.id_shops);
                    comm.Parameters.AddWithValue("@id_category", data.id_category);
                    comm.Parameters.AddWithValue("@s_name", data.s_name);
                    comm.Parameters.AddWithValue("@s_among", data.s_among);
                    comm.Parameters.AddWithValue("@s_amongall", data.s_amongall);
                    comm.Parameters.AddWithValue("@s_unitprice", data.s_unitprice);
                    comm.Parameters.AddWithValue("@s_description", data.s_description);
                    comm.Parameters.AddWithValue("@s_status", data.s_description);
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newstockModels
            {
                id_shops = data.id_shops,
                id_category = data.id_category,
                s_name = data.s_name,
                s_among = data.s_among,
                s_amongall = data.s_amongall,
                s_unitprice = data.s_unitprice,
                s_description = data.s_description, 

            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Stock/{id}")]
        public stockModels Updatestock(stockModels data)
        {
            ConnecDb conn = new ConnecDb();
            try
            {
                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE stock SET id_shops=@id_shops,id_category=@id_category,s_name=@s_name,s_among=@s_among,s_amongall=@s_amongall,s_unitprice=@s_unitprice,s_description=@s_description ,s_status=@s_status WHERE id_stock=@id_stock;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_stock", data.id_stock);
                    comm.Parameters.AddWithValue("@id_shops", data.id_shops);
                    comm.Parameters.AddWithValue("@id_category", data.id_category);
                    comm.Parameters.AddWithValue("@s_name", data.s_name);
                    comm.Parameters.AddWithValue("@s_among", data.s_among);
                    comm.Parameters.AddWithValue("@s_amongall", data.s_amongall);
                    comm.Parameters.AddWithValue("@s_unitprice", data.s_unitprice);
                    comm.Parameters.AddWithValue("@s_description", data.s_description);
                    comm.Parameters.AddWithValue("@s_status", data.s_description);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new stockModels
            {
                id_stock = data.id_stock,
                id_shops = data.id_shops,
                id_category = data.id_category,
                s_name = data.s_name,
                s_among = data.s_among,
                s_amongall = data.s_amongall,
                s_unitprice = data.s_unitprice,
                s_description = data.s_description,
                s_status = data.s_status,

            };

        }



        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("Stock/{id}")]
        public void Deletestock(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE stock  WHERE id_stock={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}