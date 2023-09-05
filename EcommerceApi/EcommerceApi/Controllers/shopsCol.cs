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

    public class shopsCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Shops")]
        public List<shopsModels> Getshopsdataall()
        {

            List<shopsModels> shopss = new List<shopsModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM shops;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    shopsModels shops = new shopsModels()
                    {
                        id_shops = int.Parse(dr["id_shops"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        sh_name = dr["sh_name"].ToString(),
                        sh_type = dr["sh_type"].ToString(),
                        sh_description = dr["sh_description"].ToString(),
                    };
                    shopss.Add(shops);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return shopss;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Shops/{id}")]
        public shopsModels Getbyshopsid(int id)
        {
            shopsModels shops = new shopsModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM shops WHERE id_shops='" + id+ "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    shops = new shopsModels()
                    {
                        id_shops = int.Parse(dr["id_shops"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        sh_name = dr["sh_name"].ToString(),
                        sh_type = dr["sh_type"].ToString(),
                        sh_description = dr["sh_description"].ToString(),

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return shops;
        }


        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPost]
        [Route("Shops")]
        public newshopsModels Newshops(newshopsModels data)
        {
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string setAddress = "INSERT into shops set id_users=@id_users,sh_name=@sh_name,sh_type=@sh_type,sh_description=@sh_description;";
                    MySqlCommand commads = new MySqlCommand(setAddress, connection);
                    commads.Parameters.AddWithValue("@id_users", data.id_users);
                    commads.Parameters.AddWithValue("@sh_name", data.sh_name);
                    commads.Parameters.AddWithValue("@sh_type", data.sh_type);
                    commads.Parameters.AddWithValue("@sh_description", data.sh_description);
                    commads.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newshopsModels
            {
                id_users = data.id_users,
                sh_name = data.sh_name,
                sh_type = data.sh_type,
                sh_description = data.sh_description
            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Shops/{id}")]
        public shopsModels Updateshops(shopsModels data)
        {
            ConnecDb conn = new ConnecDb();
            try
            {
                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE shops SET id_users=@id_users,sh_name=@sh_name,sh_type=@sh_type,sh_description=@sh_description WHERE id_shops=@id_shops;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_shops", data.id_shops);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@sh_name", data.sh_name);
                    comm.Parameters.AddWithValue("@sh_type",data.sh_type);
                    comm.Parameters.AddWithValue("@sh_description", data.sh_description);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new shopsModels
            {
                id_shops = data.id_shops,
                id_users = data.id_users,
                sh_name = data.sh_name,
                sh_type = data.sh_type,
                sh_description = data.sh_description           

            };

        }


       
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("Shops/{id}")]
        public void Deleteshops(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE shops  WHERE id_shops={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}