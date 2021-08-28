using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Smart_home.DTO;

namespace Smart_home.Controllers
{
    public class HomeController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Smart_home_db"].ConnectionString);
        public DataTable sql(string command)
        {

            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        // GET: api/Home
        public HttpResponseMessage Get()
        {
            DataTable dt_home = sql($"select * from Homes");
            IList<HomeInfoDTO> items = dt_home.AsEnumerable().Select(row =>
            new HomeInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                Areas = sql($"select * from Areas where home_id = {row["id"]}").AsEnumerable().Select(row_area =>
                    new AreaSimpleDTO
                    {
                        id = Convert.ToInt32(row_area["id"]),
                        name = row_area["name"].ToString(),
                        status = Convert.ToBoolean(row_area["status"]),
                        devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                        "inner join Type " +
                        "on type_id = Type.id " +
                        $"where area_id = {row_area["id"]}")
                    }).ToList()
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // GET: api/Home/5
        public HttpResponseMessage Get(int id)
        {
            DataTable dt_home = sql($"select * from Homes where id = {id}");
            IList<HomeInfoDTO> items = dt_home.AsEnumerable().Select(row =>
            new HomeInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                Areas = sql($"select * from Areas where home_id = {row["id"]}").AsEnumerable().Select(row_area =>
                    new AreaSimpleDTO
                    {
                        id = Convert.ToInt32(row_area["id"]),
                        name = row_area["name"].ToString(),
                        status = Convert.ToBoolean(row_area["status"]),
                        devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                        "inner join Type " +
                        "on type_id = Type.id " +
                        $"where area_id = {row_area["id"]}")
                }).ToList()
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // POST: api/Home
        public HttpResponseMessage Post(HomeDTO home)
        {
            sql($"insert into Homes values('{home.name}','{home.status}')");
            DataTable dt_home = sql($"select * from Homes where id = (select max(id) from Homes)");
            IList<HomeInfoDTO> items = dt_home.AsEnumerable().Select(row =>
            new HomeInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                Areas = sql($"select * from Areas where home_id = {row["id"]}").AsEnumerable().Select(row_area =>
                    new AreaSimpleDTO
                    {
                        id = Convert.ToInt32(row_area["id"]),
                        name = row_area["name"].ToString(),
                        status = Convert.ToBoolean(row_area["status"]),
                        devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                        "inner join Type " +
                        "on type_id = Type.id " +
                        $"where area_id = {row_area["id"]}")
                    }).ToList()
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // PUT: api/Home/5
        public HttpResponseMessage Put(int id, HomeDTO home)
        {
            sql($"update Homes set " +
                $"name = '{home.name}'," +
                $"status = '{home.status}'" +
                $"where id = {id}");
            DataTable dt_home = sql($"select * from Homes where id = {id}");
            IList<HomeInfoDTO> items = dt_home.AsEnumerable().Select(row =>
            new HomeInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                Areas = sql($"select * from Areas where home_id = {row["id"]}").AsEnumerable().Select(row_area =>
                    new AreaSimpleDTO
                    {
                        id = Convert.ToInt32(row_area["id"]),
                        name = row_area["name"].ToString(),
                        status = Convert.ToBoolean(row_area["status"]),
                        devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                        "inner join Type " +
                        "on type_id = Type.id " +
                        $"where area_id = {row_area["id"]}")
                    }).ToList()
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // DELETE: api/Home/5
        public void Delete(int id)
        {
        }
    }
}
