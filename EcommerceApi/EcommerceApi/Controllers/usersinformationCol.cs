using EcommerceApi.ConnecDB;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceApi.Controllers
{
    public class userControllers
    {
        ConnecDb conn = new ConnecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Usersinformation")]
        public List<usersModels> Getusersdataall()
        {

            List<usersModels> users = new List<usersModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM users;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    usersModels user = new usersModels()
                    {
                        id_users = int.Parse(dr["id_users"].ToString()),
                        u_usersname = dr["u_usersname"].ToString(),
                        u_password = dr["u_password"].ToString(),
                        u_name = dr["u_name"].ToString(),
                        u_email = dr["u_email"].ToString(),
                        u_phonenumber = dr["u_phonenumber"].ToString()

                    };
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Usersinformation/{id}")]
        public usersModels Getbyusersid()
        {
            usersModels user = new usersModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM users WHERE usersId='" + User.FindFirstValue(ClaimTypes.Sid) + "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    user = new usersModels()
                    {
                        id_users = int.Parse(dr["id_users"].ToString()),
                        u_usersname = dr["u_usersname"].ToString(),
                        u_password = dr["u_password"].ToString(),
                        u_name = dr["u_name"].ToString(),
                        u_email = dr["u_email"].ToString(),
                        u_phonenumber = dr["u_phonenumber"].ToString()

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }



        //[Authorize(Roles = "Admin,SuperAdmin,Geust")]
        [HttpPost]
        [Route("UsersInformation")]
        public newusersModels Registerusers(newusersModels data, newaddressModels dataads)
        {
            UserDto userDto = new UserDto();
            ConnecDb conn = new ConnecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();

                    string sql = "INSERT into users set u_usersname=@u_usersname,u_password=CONCAT('*', UPPER(SHA1(UNHEX(SHA1(@u_password))))),u_name=@u_name,u_email=@u_email,u_phonenumber=@u_phonenumber;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@u_usersname", data.u_usersname);
                    comm.Parameters.AddWithValue("@u_password", data.u_password);
                    comm.Parameters.AddWithValue("@u_name", data.u_name);
                    comm.Parameters.AddWithValue("@u_email", data.u_email);
                    comm.Parameters.AddWithValue("@u_phonenumber", data.u_phonenumber);
                    comm.ExecuteNonQuery();

                    PasswordModels pass = new PasswordModels();
                    userDto.Username = data.u_usersname;
                    userDto.Password = data.u_password;
                    pass.id_users = conn.CheckIduser(userDto);
                    string setAddress = "INSERT into address set id_users='" + pass.id_users + "',a_province=@a_province,a_district=@a_district,a_postalcode=@a_postalcode,a_streetname=@a_streetname,a_building=@a_building,a_housenumber=@a_housenumber,a_alley=@a_alley,a_intersection=@a_intersection,a_locationurl=@a_locationurl,a_details=@a_details;";
                    MySqlCommand commads = new MySqlCommand(setAddress, connection);
                    commads.Parameters.AddWithValue("@a_province", dataads.a_province);
                    commads.Parameters.AddWithValue("@a_district", dataads.a_district);
                    commads.Parameters.AddWithValue("@a_postalcode", dataads.a_postalcode);
                    commads.Parameters.AddWithValue("@a_streetname", dataads.a_streetname);
                    commads.Parameters.AddWithValue("@a_building", dataads.a_building);
                    commads.Parameters.AddWithValue("@a_housenumber", dataads.a_housenumber);
                    commads.Parameters.AddWithValue("@a_alley", dataads.a_alley);
                    commads.Parameters.AddWithValue("@a_intersection", dataads.a_intersection);
                    commads.Parameters.AddWithValue("@a_locationurl", dataads.a_locationurl);
                    commads.Parameters.AddWithValue("@a_details", dataads.a_locationurl);
                    commads.ExecuteNonQuery();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new newusersModels
            {
                u_usersname = data.u_usersname,
                u_password = data.u_password,
                u_name = data.u_name,
                u_email = data.u_email,
                u_phonenumber = data.u_phonenumber,
            };
        }



        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("UsersInformation/{id}")]
        public usersModels Updateusers(usersModels data)
        {
            usersModels user = new usersModels();
            ConnecDb conn = new ConnecDb();
            try
            {


                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE users SET u_usersname=@u_usersname,u_name=@u_name,u_email=@u_email,u_phonenumber=@u_phonenumber  WHERE id_users=@id_users ;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@id_users", User.FindFirstValue(ClaimTypes.Sid));
                    comm.Parameters.AddWithValue("@u_usersname", User.FindFirstValue(ClaimTypes.Name).ToString());
                    comm.Parameters.AddWithValue("@u_name", data.u_name);
                    comm.Parameters.AddWithValue("@u_email", data.u_email);
                    comm.Parameters.AddWithValue("@u_phonenumber", data.u_phonenumber);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new usersModels
            {
                id_users = data.id_users,
                u_usersname = data.u_usersname,
                u_password = data.u_password,
                u_name = data.u_name,
                u_email = data.u_email,
                u_phonenumber = data.u_phonenumber,
            };

        }


        [Authorize(Roles = "Admin,SuperAdmin,User,Geust")]
        [HttpPut]
        [Route("UsersInformation/username/password")]
        public PasswordModels Updatepassword(PasswordModels data)
        {
            PasswordModels pass = new PasswordModels();
            ConnecDb conn = new ConnecDb();
            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE users SET passwrd=CONCAT('*', UPPER(SHA1(UNHEX(SHA1(@password)))))  WHERE id_users=@id_users AND u_password=CONCAT('*',UPPER(SHA1(UNHEX(SHA1(@oldpassword))))) ;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    if (data.recheck_password != data.password)
                    {
                        return new PasswordModels { password = "Passwords are not the same." };

                    }
                    comm.Parameters.AddWithValue("@id_users", User.FindFirstValue(ClaimTypes.Sid));
                    comm.Parameters.AddWithValue("@oldpassword", data.old_password);
                    comm.Parameters.AddWithValue("@password", data.password);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new PasswordModels { id_users = pass.id_users, password = data.password };
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete]
        [Route("UsersInformation/{id}")]
        public void Deleteusers(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"UPDATE users SET status='DELETE' WHERE usersId={uId.value};";
                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }
    }
}
