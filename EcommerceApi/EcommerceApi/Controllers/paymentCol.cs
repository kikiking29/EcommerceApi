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
using static EcommerceApi.Controllers.userControllers;

namespace EcommerceApi.Controllers
{

    public class paymentCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Payment")]
        public List<paymentModels> Getpaymentdataall()
        {

            List<paymentModels> cats = new List<paymentModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM payment;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    paymentModels cat = new paymentModels()
                    {
                        id_payment = int.Parse(dr["id_payment"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        id_orders = int.Parse(dr["id_orders"].ToString()),
                        p_datetime = DateTime.Parse(dr["p_datetime"].ToString()),
                        p_status = dr["p_status"].ToString(),
                    };
                    cats.Add(cat);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cats;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Payment/{id}")]
        public paymentModels Getbypaymentid(int id)
        {
            paymentModels pay = new paymentModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM payment WHERE id_payment='" + id+ "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    pay = new paymentModels()
                    {
                        id_payment = int.Parse(dr["id_payment"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        id_orders = int.Parse(dr["id_orders"].ToString()),
                        p_datetime = DateTime.Parse(dr["p_datetime"].ToString()),
                        p_status = dr["p_status"].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pay;
        }


        [HttpPost]
        [Route("Payment")]
        public newpaymentModels Newpayment(newpaymentModels data)
        {
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "INSERT into payment set id_users=@id_users,id_orders=@id_orders,p_datetime=@p_datetime,p_status=@p_status;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@id_orders", data.id_orders);
                    comm.Parameters.AddWithValue("@p_datetime", data.p_datetime);
                    comm.Parameters.AddWithValue("@p_status", data.p_status);


                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newpaymentModels
            {
                id_users = data.id_users,
                id_orders = data.id_orders,
                p_datetime = data.p_datetime,   
                p_status = data.p_status,
            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Payment/{id}")]
        public paymentModels Updatepayment(paymentModels data)
        {
            paymentModels user = new paymentModels();
            ConnecDb conn = new ConnecDb();
            try
            {


                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE payment SET id_users=@id_users,id_orders=@id_orders,p_datetime=@p_datetime,p_status=@p_status WHERE id_payment=@id_payment;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_payment", data.id_payment);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@id_orders", data.id_orders);
                    comm.Parameters.AddWithValue("@p_datetime", data.p_datetime);
                    comm.Parameters.AddWithValue("@p_status", data.p_status);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new paymentModels
            {
                id_payment = data.id_payment,
                id_users = data.id_users,
                id_orders = data.id_orders,
                p_datetime = data.p_datetime,
                p_status = data.p_status,

            };

        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("Payment/{id}")]
        public void Deletecategoty(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE payment  WHERE id_payment={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}