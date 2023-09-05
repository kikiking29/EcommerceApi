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

    public class addressCol : ControllerBase
    {
        ConnecDb conn = new ConnecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Address")]
        public List<addressModels> Getaddressuserdataall()
        {

            List<addressModels> adss = new List<addressModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM address;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    addressModels ads = new addressModels()
                    {
                        id_address = int.Parse(dr["id_address"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        a_province = dr["a_province"].ToString(),
                        a_district = dr["a_district"].ToString(),
                        a_subdistrict = dr["a_subdistrict"].ToString(),
                        a_postalcode = int.Parse(dr["a_postalcode"].ToString()) ,
                        a_streetname = dr["a_streetname"].ToString(),
                        a_building = dr["a_building"].ToString(),
                        a_housenumber = dr["a_housenumber"].ToString(),
                        a_alley = dr["a_alley"].ToString(),
                        a_intersection = dr["a_intersection"].ToString(),
                        a_locationurl = dr["a_locationurl"].ToString(),
                        a_details = dr["a_details"].ToString(),
                    };
                    adss.Add(ads);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return adss;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Address/{id}")]
        public addressModels Getbyaddressid(int id)
        {
            addressModels ads = new addressModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata($"SELECT * FROM address WHERE id_address={id};");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ads = new addressModels()
                    {
                        id_address = int.Parse(dr["id_address"].ToString()),
                        id_users = int.Parse(dr["id_users"].ToString()),
                        a_province = dr["a_province"].ToString(),
                        a_district = dr["a_district"].ToString(),
                        a_subdistrict = dr["a_subdistrict"].ToString(),
                        a_postalcode = int.Parse(dr["a_postalcode"].ToString()),
                        a_streetname = dr["a_streetname"].ToString(),
                        a_building = dr["a_building"].ToString(),
                        a_housenumber = dr["a_housenumber"].ToString(),
                        a_alley = dr["a_alley"].ToString(),
                        a_intersection = dr["a_intersection"].ToString(),
                        a_locationurl = dr["a_locationurl"].ToString(),
                        a_details = dr["a_details"].ToString(),

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ads;
        }


        [HttpPost]
        [Route("Address")]
        public newaddressModels Newaddress( newaddressModels data)
        {
            UserDto userDto = new UserDto();
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "INSERT into address set id_users=@id_users,a_province=@a_province,a_district=@a_district,a_subdistrict=@a_subdistrict,a_postalcode=@a_postalcode,a_streetname=@a_streetname,a_building=@a_building,a_housenumber=@a_housenumber,a_alley=@a_alley,a_intersection=@a_intersection,a_locationurl=@a_locationurl,a_details=@a_details;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@a_province", data.a_province);
                    comm.Parameters.AddWithValue("@a_district", data.a_district);
                    comm.Parameters.AddWithValue("@a_subdistrict", data.a_subdistrict);
                    comm.Parameters.AddWithValue("@a_postalcode", data.a_postalcode);
                    comm.Parameters.AddWithValue("@a_streetname", data.a_streetname);
                    comm.Parameters.AddWithValue("@a_building", data.a_building);
                    comm.Parameters.AddWithValue("@a_housenumber", data.a_housenumber);
                    comm.Parameters.AddWithValue("@a_alley", data.a_alley);
                    comm.Parameters.AddWithValue("@a_intersection", data.a_intersection);
                    comm.Parameters.AddWithValue("@a_locationurl", data.a_locationurl);
                    comm.Parameters.AddWithValue("@a_details", data.a_details);
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newaddressModels
            {
                id_users = data.id_users,
                a_province = data.a_province,
                a_district = data.a_district,
                a_subdistrict = data.a_subdistrict,
                a_postalcode = data.a_postalcode,
                a_streetname = data.a_streetname,
                a_building = data.a_building,
                a_alley = data.a_alley,
                a_intersection = data.a_intersection,
                a_locationurl = data.a_locationurl,
                a_details = data.a_details,
            };
        }

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("Address/{id}")]
        public addressModels Updateaddressusers(addressModels data)
        {
            addressModels user = new addressModels();
            ConnecDb conn = new ConnecDb();
            try
            {


                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE address SET  id_users=@id_users,a_province=@a_province,a_district=@a_district,a_subdistrict=@a_subdistrict,a_postalcode=@a_postalcode,a_streetname=@a_streetname,a_building=@a_building,a_housenumber=@a_housenumber,a_alley=@a_alley,a_intersection=@a_intersection,a_locationurl=@a_locationurl,a_details=@a_details WHERE id_address=@id_address;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_address", data.id_address);
                    comm.Parameters.AddWithValue("@id_users", data.id_users);
                    comm.Parameters.AddWithValue("@a_province", data.a_province);
                    comm.Parameters.AddWithValue("@a_district", data.a_district);
                    comm.Parameters.AddWithValue("@a_subdistrict", data.a_subdistrict);
                    comm.Parameters.AddWithValue("@a_postalcode", data.a_postalcode);
                    comm.Parameters.AddWithValue("@a_streetname", data.a_streetname);
                    comm.Parameters.AddWithValue("@a_building", data.a_building);
                    comm.Parameters.AddWithValue("@a_housenumber", data.a_housenumber);
                    comm.Parameters.AddWithValue("@a_alley", data.a_alley);
                    comm.Parameters.AddWithValue("@a_intersection", data.a_intersection);
                    comm.Parameters.AddWithValue("@a_locationurl", data.a_locationurl);
                    comm.Parameters.AddWithValue("@a_details", data.a_locationurl);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new addressModels
            {
                id_address = data.id_address,
                id_users = data.id_users,
                a_province = data.a_province,
                a_district = data.a_district,
                a_subdistrict = data.a_subdistrict,
                a_postalcode = data.a_postalcode,
                a_streetname = data.a_streetname,
                a_building = data.a_building,
                a_alley = data.a_alley,
                a_intersection = data.a_intersection,
                a_locationurl = data.a_locationurl,
                a_details = data.a_details,
            };

        }


        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpDelete]
        [Route("Address/{id}")]
        public void Deleteaddressusers(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"DELETE address  WHERE id_address={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


    }
}