using Microsoft.AspNetCore.Mvc;
using EcommerceApi.ConnecDb;
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
//using userInformation.Authorization;
//using AuthorizeAttribute = userInformation.Authorization.AuthorizeAttribute;

namespace EcommerceApi.Controllers
{

    public class userInformationCol : ControllerBase
    {
        connecDb conn = new connecDb();
        public class myParam
        {
            public string name;
            public object value;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Usersinformation")]
        public List<UsersinforModels> Getusersdataall()
        {

            List<UsersinforModels> users = new List<UsersinforModels>();
            try
            {
                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM users;");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UsersinforModels user = new UsersinforModels()
                    {
                        usersId = int.Parse(dr["usersId"].ToString()),
                        username = dr["username"].ToString(),
                        password = dr["passwrd"].ToString(),
                        name = dr["name"].ToString(),
                        status = dr["status"].ToString()

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
        public UsersinforModels Getbyusersid()
        {
            UsersinforModels user = new UsersinforModels();

            try
            {


                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM users WHERE usersId='" + User.FindFirstValue(ClaimTypes.Sid) + "';");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    user = new UsersinforModels()
                    {
                        usersId = int.Parse(dr["usersId"].ToString()),
                        username = dr["username"].ToString(),
                        password = dr["passwrd"].ToString(),
                        name = dr["name"].ToString(),
                        status = dr["status"].ToString()

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("Usersinformation/privileage")]
        public List<UsersinforAndPrivileageModels> Getalluserandprivileage()
        {
            List<UsersinforAndPrivileageModels> uaps = new List<UsersinforAndPrivileageModels>();

            try
            {

                DataSet ds = new DataSet();
                ds = conn.Selectdata("SELECT * FROM users  JOIN privileage ON privileage.usersId = users.usersId;");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UsersinforAndPrivileageModels uap = new UsersinforAndPrivileageModels()
                    {
                        u_usersId = int.Parse(dr["usersId"].ToString()),
                        username = dr["username"].ToString(),
                        password = dr["passwrd"].ToString(),
                        name = dr["name"].ToString(),
                        status = dr["status"].ToString(),
                        privileageId = int.Parse(dr["privileageId"].ToString()),
                        p_usersId = int.Parse(dr["usersId"].ToString()),
                        canread = dr["canread"].ToString(),
                        caninsert = dr["caninsert"].ToString(),
                        canupdate = dr["canupdate"].ToString(),
                        candelete = dr["candelete"].ToString(),
                        candrop = dr["candrop"].ToString(),
                    };
                    uaps.Add(uap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return uaps;
        }


        //[Authorize(Roles = "Admin,SuperAdmin,Geust")]
        [HttpPost]
        [Route("UsersInformation")]
        public NewUsersinforModels Registerusers(NewUsersinforModels data)
        {
            PasswordModels pass = new PasswordModels();
            UserDto userDto = new UserDto();
            connecDb conn = new connecDb();

            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();

                    string sql = "INSERT into users set username=@username,passwrd=CONCAT('*', UPPER(SHA1(UNHEX(SHA1(@password))))),name=@name;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@username", data.username);
                    comm.Parameters.AddWithValue("@password", data.password);
                    comm.Parameters.AddWithValue("@name", data.name);
                    comm.ExecuteNonQuery();
                    connection.Close();

                    userDto.Username = data.username;
                    userDto.Password = data.password;
                    pass.usersId = conn.CheckIduser(userDto);
                    string setPrivileage = "INSERT into privileage set usersId='" + pass.usersId + "',canread='0',caninsert='0',canupdate='0',candelete='0',candrop='0';";
                    conn.Setdata(setPrivileage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new NewUsersinforModels
            {
                username = data.username,
                password = data.password,
                name = data.name,
                status = data.status,
            };
        }



        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpPut]
        [Route("UsersInformation/{id}")]
        public UsersinforModels Updateusers(UsersinforModels data)
        {
            UsersinforModels user = new UsersinforModels();
            connecDb conn = new connecDb();
            try
            {


                if (ModelState.IsValid)
                {

                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE users SET username=@username,name=@name,status=@status  WHERE usersId=@usersId ;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    comm.Parameters.AddWithValue("@usersId", User.FindFirstValue(ClaimTypes.Sid));
                    comm.Parameters.AddWithValue("@username", User.FindFirstValue(ClaimTypes.Name).ToString());
                    comm.Parameters.AddWithValue("@name", data.name);
                    comm.Parameters.AddWithValue("@status", data.status);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return new UsersinforModels
            {
                usersId = data.usersId,
                username = data.username,
                name = data.name,
                status = data.status
            };

        }


        [Authorize(Roles = "Admin,SuperAdmin,User,Geust")]
        [HttpPut]
        [Route("UsersInformation/username/password")]
        public PasswordModels Updatepassword(PasswordModels data)
        {
            PasswordModels pass = new PasswordModels();
            connecDb conn = new connecDb();
            try
            {
                if (ModelState.IsValid)
                {
                    MySqlConnection connection = new MySqlConnection(conn.connectDb());
                    connection.Open();
                    string sql = "UPDATE users SET passwrd=CONCAT('*', UPPER(SHA1(UNHEX(SHA1(@password)))))  WHERE usersId=@id AND passwrd=CONCAT('*', UPPER(SHA1(UNHEX(SHA1(@oldpassword))))) ;";
                    MySqlCommand comm = new MySqlCommand(sql, connection);
                    if (data.recheck_password != data.password)
                    {
                        return new PasswordModels { password = "Passwords are not the same." };

                    }
                    comm.Parameters.AddWithValue("@id", User.FindFirstValue(ClaimTypes.Sid));
                    comm.Parameters.AddWithValue("@oldpassword", data.old_password);
                    comm.Parameters.AddWithValue("@password", data.password);
                    comm.ExecuteNonQuery();
                    connection.Close();
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new PasswordModels { usersId = pass.usersId, password = data.password };
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        [Route("UsersInformation/status/Active/{id}")]
        public void upstatusActive(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };
                string sql = $"UPDATE users SET status='Active' WHERE usersId={uId.value};";

                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        [Route("UsersInformation/status/Inactive/{id}")]
        public void upstatusInactive(int id)
        {
            try
            {
                myParam uId = new myParam
                {
                    name = "@id",
                    value = id
                };

                string sql = $"UPDATE users SET status='Inactive' WHERE usersId={uId.value};";

                conn.Setdata(sql);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

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