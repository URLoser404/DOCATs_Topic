using Smart_home.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Smart_home.Controllers
{
    public class UserController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Smart_home_db"].ConnectionString);
        SqlCommand command = new SqlCommand();
        public DataTable Sql()
        {

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        // GET: api/Usr
        public HttpResponseMessage Get()
        {
            command = new SqlCommand
            {
                CommandText = "select [User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                    "inner join Country " +
                    "on[User].country_id = Country.id " +
                    "inner join Language " +
                    "on Country.language_id = Language.id ",
                Connection = con
            };
            DataTable dt_user = Sql();
            return Request.CreateResponse(HttpStatusCode.OK, dt_user);
        }

        // GET: api/User/5
        public HttpResponseMessage Get(int id)
        {
            command = new SqlCommand
            {
                CommandText = "select [User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                    "inner join Country " +
                    "on[User].country_id = Country.id " +
                    "inner join Language " +
                    "on Country.language_id = Language.id " +
                    $"where[User].id = @id",
                Connection = con
            };
            command.Parameters.AddWithValue("@id", id);
            DataTable dt_user = Sql();
            return Request.CreateResponse(HttpStatusCode.OK, dt_user);
        }

        // POST: api/User
        public HttpResponseMessage Post(UserDTO user)
        {
            command = new SqlCommand
            {
                CommandText = "select * from [Language] where name = @language",
                Connection = con
            };
            command.Parameters.AddWithValue("@language", user.language);
            DataTable dt_language = Sql();

            if (dt_language.Rows.Count == 0)
            {
                command = new SqlCommand
                {
                    CommandText = "insert into [Language] values(@language)",
                    Connection = con
                };
                command.Parameters.AddWithValue("@language", user.language);
                Sql();
            }

            command = new SqlCommand
            {
                CommandText = "select * from [Country] where name = @country",
                Connection = con
            };
            command.Parameters.AddWithValue("@country", user.country);
            DataTable dt_country = Sql();

            if(dt_country.Rows.Count == 0)
            {
                command = new SqlCommand
                {
                    CommandText = "select * from [Language] where name = @language",
                    Connection = con
                };
                command.Parameters.AddWithValue("@language", user.language);
                dt_language = Sql();

                command = new SqlCommand
                {
                    CommandText = "insert into [Country] values(@country,@language_id)",
                    Connection = con
                };
                command.Parameters.AddWithValue("@country", user.country);
                command.Parameters.AddWithValue("@language_id", dt_language.Rows[0]["id"]);
                Sql();
            }

            command = new SqlCommand
            {
                CommandText = "select * from [Country] where name = @country",
                Connection = con
            };
            command.Parameters.AddWithValue("@country", user.country);
            dt_country = Sql();

            command = new SqlCommand
            {
                CommandText = $"select * from [User] where account = @account",
                Connection = con
            };
            command.Parameters.AddWithValue("@account", user.account);
            DataTable dt_user = Sql();


            if (dt_user.Rows.Count == 0)
            {
                command = new SqlCommand
                {
                    CommandText = $"insert into [User] values(@name,@account,@password,@status,@country_id)",
                    Connection = con
                };
                command.Parameters.AddWithValue("@name", user.name);
                command.Parameters.AddWithValue("@account", user.account);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@status", user.status);
                command.Parameters.AddWithValue("@country_id", dt_country.Rows[0]["id"]);
                Sql();

                command = new SqlCommand
                {
                    CommandText = "select [User].id,[User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                        "inner join Country " +
                        "on[User].country_id = Country.id " +
                        "inner join Language " +
                        "on Country.language_id = Language.id " +
                        $"where[User].id =  (select max(id) from [User])",
                    Connection = con
                };
                dt_user = Sql();

                return Request.CreateResponse(HttpStatusCode.OK, dt_user);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "account exist");

        }

        // PUT: api/User/5
        public HttpResponseMessage Put(int id, UserDTO user)
        {
            command = new SqlCommand
            {
                CommandText = "select * from [Language] where name = @language",
                Connection = con
            };
            command.Parameters.AddWithValue("@language", user.language);
            DataTable dt_language = Sql();


            if (dt_language.Rows.Count == 0)
            {
                command = new SqlCommand
                {
                    CommandText = "insert into [Language] values(@language)",
                    Connection = con
                };
                command.Parameters.AddWithValue("@langauge", user.language);
                Sql();
            }

            command = new SqlCommand
            {
                CommandText = "select * from [Country] where name = @country",
                Connection = con
            };
            command.Parameters.AddWithValue("@country", user.country);
            DataTable dt_country = Sql();


            if (dt_country.Rows.Count == 0)
            {
                command = new SqlCommand
                {
                    CommandText = "select * from [Language] where name = @language",
                    Connection = con
                };
                command.Parameters.AddWithValue("@language", user.language);
                dt_language = Sql();

                command = new SqlCommand
                {
                    CommandText = "insert into [Country] values( @country, @language_id)",
                    Connection = con
                };
                command.Parameters.AddWithValue("@country", user.country);
                command.Parameters.AddWithValue("@language_id", dt_language.Rows[0]["id"]);
                Sql();
            }

            command = new SqlCommand
            {
                CommandText = "select * from [Country] where name = @country",
                Connection = con
            };
            command.Parameters.AddWithValue("@country", user.country);
            dt_country = Sql();


            command = new SqlCommand
            {
                CommandText = "select * from [User] where id = @id"
            };
            command.Parameters.AddWithValue("@id", id);
            DataTable dt_user = Sql();


            if (dt_user.Rows.Count != 0)
            {
                command = new SqlCommand
                {
                    CommandText = "update [User] set name = @name , account = @account , password = @password , status = @status , country_id = @country_id where id = @id",
                    Connection = con
                };
                command.Parameters.AddWithValue("@name", user.name);
                command.Parameters.AddWithValue("@account", user.account);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@status", user.status);
                command.Parameters.AddWithValue("@country_id", dt_country.Rows[0]["id"]);
                command.Parameters.AddWithValue("@id", id);
                Sql();

                command = new SqlCommand
                {
                    CommandText = "select [User].id,[User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                        "inner join Country " +
                        "on[User].country_id = Country.id " +
                        "inner join Language " +
                        "on Country.language_id = Language.id " +
                        "where[User].id = @id",
                    Connection = con
                };
                command.Parameters.AddWithValue("@id", id);
                dt_user = Sql();

                return Request.CreateResponse(HttpStatusCode.OK, dt_user);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "account not exist");
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
