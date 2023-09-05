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

    public class ordersCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Orders")]
        public List<ordersModels> Getordersdataall()
        {

            List<ordersModels> orderss = new List<ordersModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM orders;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ordersModels orders = new ordersModels()
                    {
                        id_orders = int.Parse(dr["id_orders"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        o_total = int.Parse(dr["o_total"].ToString()),
                        o_datetime = DateTime.Parse(dr["o_datetime"].ToString()),
                        o_status = dr["o_status"].ToString(),
                    };
                    orderss.Add(orders);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orderss;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Orders/{id}")]
        public ordersModels Getbyordersid(int id)
        {
            ordersModels orders = new ordersModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM orders WHERE id_orders='" + id+ "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    orders = new ordersModels()
                    {
                        id_orders = int.Parse(dr["id_orders"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        o_total = int.Parse(dr["o_total"].ToString()),
                        o_datetime = DateTime.Parse(dr["o_datetime"].ToString()),
                        o_status = dr["o_status"].ToString(),

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orders;
        }


        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPost]
        [Route("Orders")]
        public newordersModels Neworders(newordersModels data)
        {
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string setAddress = "INSERT into orders set id_users=@id_users,o_total=@o_total,o_datetime=@o_datetime,o_status=@o_status;";
                    MySqlCommand commads = new MySqlCommand(setAddress, connection);
                    commads.Parameters.AddWithValue("@id_users", data.id_users);
                    commads.Parameters.AddWithValue("@o_total", data.o_total);
                    commads.Parameters.AddWithValue("@o_datetime",DateTime.Now);
                    commads.Parameters.AddWithValue("@o_status", data.o_status);
                    commads.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newordersModels
            {
                id_users = data.id_users,
                o_total = data.o_total,
                o_datetime = data.o_datetime,
                o_status = data.o_status
            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Orders/{id}")]
        public ordersModels Updateorders(ordersModels data)
        {
            ConnecDb conn = new ConnecDb();
            try
            {
                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE orders SET id_users=@id_users,o_total=@o_total,o_datetime=@o_datetime,o_status=@o_status WHERE id_orders=@id_orders;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_orders", data.id_orders);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@o_total", data.o_total);
                    comm.Parameters.AddWithValue("@o_datetime",DateTime.Now);
                    comm.Parameters.AddWithValue("@o_status", data.o_status);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new ordersModels
            {
                id_orders = data.id_orders,
                id_users = data.id_users,
                o_total = data.o_total,
                o_datetime = data.o_datetime,
                o_status = data.o_status           

            };

        }


       
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("Orders/{id}")]
        public void Deleteorders(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE orders  WHERE id_orders={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}