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
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Smart_home_db"].ConnectionString);
        public DataTable sql(string command)
        {

            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        // GET: api/Usr
        public HttpResponseMessage Get()
        {
            DataTable dt_user = sql("select [User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                "inner join Country " +
                "on[User].country_id = Country.id " +
                "inner join Language " +
                "on Country.language_id = Language.id ");
            return Request.CreateResponse(HttpStatusCode.OK, dt_user);
        }

        // GET: api/User/5
        public HttpResponseMessage Get(int id)
        {
            DataTable dt_user = sql("select [User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                "inner join Country " +
                "on[User].country_id = Country.id " +
                "inner join Language " +
                "on Country.language_id = Language.id " +
                $"where[User].id = {id}");
            return Request.CreateResponse(HttpStatusCode.OK, dt_user);
        }

        // POST: api/User
        public HttpResponseMessage Post(UserDTO user)
        {
            DataTable dt_language = sql($"select * from [Language] where name = '{user.language}'");
            if (dt_language.Rows.Count == 0)
            {
                sql($"insert into [Language] values('{user.language}')");
            }
            DataTable dt_country = sql($"select * from [Country] where name = '{user.country}'");
            if(dt_country.Rows.Count == 0)
            {
                dt_language = sql($"select * from [Language] where name = '{user.language}'");
                sql($"insert into [Country] values('{user.country}','{dt_language.Rows[0]["id"]}')");
            }
            dt_country = sql($"select * from [Country] where name = '{user.country}'");

            DataTable dt_user = sql($"select * from [User] where account = {user.account}");
            if (dt_user.Rows.Count == 0)
            {
                sql($"insert into [User] values('{user.name}','{user.account}','{user.password}','{user.status}','{dt_country.Rows[0]["id"]}')");
                dt_user = sql("select [User].id,[User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                    "inner join Country " +
                    "on[User].country_id = Country.id " +
                    "inner join Language " +
                    "on Country.language_id = Language.id " +
                    $"where[User].id =  (select max(id) from [User])");

                return Request.CreateResponse(HttpStatusCode.OK, dt_user);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "account exist");

        }

        // PUT: api/User/5
        public HttpResponseMessage Put(int id, UserDTO user)
        {
            DataTable dt_language = sql($"select * from [Language] where name = '{user.language}'");
            if (dt_language.Rows.Count == 0)
            {
                sql($"insert into [Language] values('{user.language}')");
            }
            DataTable dt_country = sql($"select * from [Country] where name = '{user.country}'");
            if (dt_country.Rows.Count == 0)
            {
                dt_language = sql($"select * from [Language] where name = '{user.language}'");
                sql($"insert into [Country] values('{user.country}','{dt_language.Rows[0]["id"]}')");
            }
            dt_country = sql($"select * from [Country] where name = '{user.country}'");

            DataTable dt_user = sql($"select * from [User] where id = {id}");
            if (dt_user.Rows.Count != 0)
            {
                sql($"update [User] set name = '{user.name}' , account = '{user.account}' , password = '{user.password}', status = '{user.status}', country_id = '{dt_country.Rows[0]["id"]}' where id = {id}");
                dt_user = sql("select [User].id,[User].name,account,password,status ,Country.name as country,Language.name as language from [User] " +
                    "inner join Country " +
                    "on[User].country_id = Country.id " +
                    "inner join Language " +
                    "on Country.language_id = Language.id " +
                    $"where[User].id = {id}");

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
