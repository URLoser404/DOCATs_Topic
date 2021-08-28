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
    public class AreaController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Smart_home_db"].ConnectionString);
        public DataTable sql(string command)
        {

            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        // GET: api/Area
        public HttpResponseMessage Get()
        {
            DataTable dt_area = sql($"select * from Areas");
            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                home_id = Convert.ToInt32(row["home_id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                "inner join Type " +
                "on type_id = Type.id ")
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // GET: api/Area/5
        public HttpResponseMessage Get(int id)
        {
            DataTable dt_area = sql($"select * from Areas where id = {id}");
            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                home_id = Convert.ToInt32(row["home_id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                "inner join Type " +
                "on type_id = Type.id " +
                $"where area_id = {id}")
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // POST: api/Area
        public HttpResponseMessage Post(AreaDTO area)
        {
            sql($"insert into Areas values('{area.home_id}','{area.name}','{area.status}')");
            DataTable dt_area = sql($"select * from Areas where id = (select max(id) from Areas)");
            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                home_id = Convert.ToInt32(row["home_id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                "inner join Type " +
                "on type_id = Type.id " +
                $"where area_id = (select max(id) from Areas)")
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // PUT: api/Area/5
        public HttpResponseMessage Put(int id, AreaDTO area )
        {
            sql($"update Areas set " +
                $"home_id = '{area.home_id}'," +
                $"name = '{area.name}'," +
                $"status = '{area.status}'" +
                $"where id = {id}");
            DataTable dt_area = sql($"select * from Areas where id = {id}");
            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = Convert.ToInt32(row["id"]),
                home_id = Convert.ToInt32(row["home_id"]),
                name = row["name"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                devices = sql("select Devices.id,Devices.name,Type.name as type,status from Devices " +
                "inner join Type " +
                "on type_id = Type.id " +
                $"where area_id = {id}")
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // DELETE: api/Area/5
        public void Delete(int id)
        {
        }
    }
}
