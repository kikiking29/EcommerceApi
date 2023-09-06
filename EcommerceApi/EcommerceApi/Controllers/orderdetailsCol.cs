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

    public class orderdetailsCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Orderdetails")]
        public List<orderdetailsModels> Getorderdetailsdataall()
        {

            List<orderdetailsModels> orderdetailss = new List<orderdetailsModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM orderdetails;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    orderdetailsModels orderdetails = new orderdetailsModels()
                    {
                        id_orderdetails = int.Parse(dr["id_orderdetails"].ToString()),
                        id_orders = int.Parse(dr["id_orders"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        id_stock = int.Parse(dr["id_stock"].ToString()),
                        odt_among = int.Parse(dr["odt_among"].ToString()),
                        odt_totalprice = int.Parse(dr["odt_totalprice"].ToString()),
                        odt_datetime = DateTime.Parse(dr["odt_datetime"].ToString()),
                    };
                    orderdetailss.Add(orderdetails);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orderdetailss;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Orderdetails/{id}")]
        public orderdetailsModels Getbyorderdetailsid(int id)
        {
            orderdetailsModels orderdetails = new orderdetailsModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata($"SELECT * FROM orderdetails WHERE id_orderdetails={id};");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    orderdetails = new orderdetailsModels()
                    {
                        id_orderdetails = int.Parse(dr["id_orderdetails"].ToString()),
                        id_orders = int.Parse(dr["id_orders"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        id_stock = int.Parse(dr["id_stock"].ToString()),
                        odt_among = int.Parse(dr["odt_among"].ToString()),
                        odt_totalprice = int.Parse(dr["odt_totalprice"].ToString()),
                        odt_datetime = DateTime.Parse(dr["odt_datetime"].ToString()),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orderdetails;
        }


        [HttpPost]
        [Route("Orderdetails")]
        public neworderdetailsModels Neworderdetails(neworderdetailsModels data)
        {
            ConnecDb conn = new ConnecDb();
            production pdt = new production();
            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "INSERT into orderdetails set id_orders=@id_orders,id_users=@id_users,id_stock=@id_stock,odt_among=@odt_among,odt_totalprice=@odt_totalprice,odt_datetime=@odt_datetime;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_orders", data.id_orders);
                    comm.Parameters.AddWithValue("@id_users", User.FindFirstValue(ClaimTypes.Sid));
                    comm.Parameters.AddWithValue("@id_stock",data.id_stock);
                    comm.Parameters.AddWithValue("@odt_among", data.odt_among);
                    comm.Parameters.AddWithValue("@odt_totalprice", (data.odt_among*pdt.getUnitprice(data.id_stock)));
                    comm.Parameters.AddWithValue("@odt_datetime", DateTime.Now);
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new neworderdetailsModels
            {
                id_orders = data.id_orders,
                id_users = data.id_users,
                id_stock = data.id_stock,
                odt_among = data.odt_among,
                odt_totalprice = data.odt_totalprice,
                odt_datetime =data.odt_datetime,
            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Orderdetails/{id}")]
        public orderdetailsModels Updateorderdetails(orderdetailsModels data)
        {
            ConnecDb conn = new ConnecDb();
            try
            {
                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE orderdetails SET id_orders=@id_orders,id_users=@id_users,id_stock=@id_stock,odt_among=@odt_among,odt_totalprice=@odt_totalprice,odt_datetime=@odt_datetime WHERE id_orderdetails=@id_orderdetails;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_orderdetails", data.id_orderdetails);
                    comm.Parameters.AddWithValue("@id_orders", data.id_orders);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@id_stock",DateTime.Now);
                    comm.Parameters.AddWithValue("@odt_among", data.odt_among);
                    comm.Parameters.AddWithValue("@odt_totalprice", data.odt_totalprice);
                    comm.Parameters.AddWithValue("@odt_datetime", DateTime.Now);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new orderdetailsModels
            {
                id_orderdetails = data.id_orderdetails,
                id_orders = data.id_orders,
                id_users = data.id_users,
                id_stock = data.id_stock,
                odt_among = data.odt_among,
                odt_totalprice = data.odt_totalprice,
                odt_datetime = data.odt_datetime,

            };

        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("Orderdetails/{id}")]
        public void Deleteorderdetails(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE orderdetails  WHERE id_orderdetails={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}