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

    public class categoryCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Category")]
        public List<categoryModels> Getcategorydataall()
        {

            List<categoryModels> cats = new List<categoryModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM category;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    categoryModels cat = new categoryModels()
                    {
                        id_category = int.Parse(dr["id_category"].ToString()),

                        c_type = dr["c_type"].ToString(),
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
        [Route("Category/{id}")]
        public categoryModels Getbycategoryid(int id)
        {
            categoryModels cat = new categoryModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM address WHERE id_category='" + id+ "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cat = new categoryModels()
                    {
                        id_category = int.Parse(dr["id_category"].ToString()),

                        c_type = dr["c_type"].ToString(),

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cat;
        }


        [HttpPost]
        [Route("Category")]
        public newcategoryModels Newcategory(newcategoryModels data)
        {
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string setAddress = "INSERT into category set c_type=@c_type;";
                    MySqlCommand commads = new MySqlCommand(setAddress, connection);
                    commads.Parameters.AddWithValue("@c_type", data.c_type);
                  
                    commads.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newcategoryModels
            {
                c_type = data.c_type,
               
            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Category/{id}")]
        public categoryModels Updatecategory(categoryModels data)
        {
            categoryModels user = new categoryModels();
            ConnecDb conn = new ConnecDb();
            try
            {


                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE category SET c_type=@c_type WHERE id_category=@id_category;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_category", data.id_category);
                    comm.Parameters.AddWithValue("@c_type", data.c_type);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new categoryModels
            {
                id_category = data.id_category,
                c_type = data.c_type,
               
            };

        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("Category/{id}")]
        public void Deletecategoty(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE category  WHERE id_category={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}