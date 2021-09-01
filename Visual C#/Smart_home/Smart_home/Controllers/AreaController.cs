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
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Smart_home_db"].ConnectionString);
        SqlCommand command = new SqlCommand();
        public DataTable Sql()
        {
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        // GET: api/Area
        public HttpResponseMessage Get()
        {

            command = new SqlCommand
            {
                CommandText = "select * from Areas",
                Connection = con
            };
            DataTable dt_area = Sql();


            command = new SqlCommand
            {
                CommandText = "select Devices.id,Devices.name,Type.name as type,status from Devices " +
                        "inner join Type " +
                        "on type_id = Type.id ",
                Connection = con
            };
            DataTable dt_devices = Sql();
            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = row.Field<int>("id"),
                home_id = row.Field<int>("home_id"),
                name = row.Field<string>("name"),
                status = row.Field<bool>("status"),
                devices = dt_devices
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // GET: api/Area/5
        public HttpResponseMessage Get(int id)
        {
            command = new SqlCommand
            {
                CommandText = "select * from Areas where id = @id",
                Connection = con
            };
            command.Parameters.AddWithValue("@id", id);
            DataTable dt_area = Sql();


            command = new SqlCommand
            {
                CommandText = "select Devices.id,Devices.name,Type.name as type,status from Devices " +
                    "inner join Type " +
                    "on type_id = Type.id " +
                    "where area_id = @id",
                Connection = con
            };
            command.Parameters.AddWithValue("@id", id);
            DataTable dt_device = Sql();

            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = row.Field<int>("id"),
                home_id = row.Field<int>("home_id"),
                name = row.Field<string>("name"),
                status = row.Field<bool>("status"),
                devices = dt_device
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // POST: api/Area
        public HttpResponseMessage Post(AreaDTO area)
        {
            command = new SqlCommand
            {
                CommandText = "insert into Areas values(@home_id,@name,@status)",
                Connection = con
            };
            command.Parameters.AddWithValue("@home_id", area.home_id);
            command.Parameters.AddWithValue("@name", area.name);
            command.Parameters.AddWithValue("@status", area.status);
            Sql();


            command = new SqlCommand
            {
                CommandText = "select * from Areas where id = (select max(id) from Areas)",
                Connection = con
            };
            DataTable dt_area = Sql();


            command = new SqlCommand
            {
                CommandText = "select Devices.id,Devices.name,Type.name as type,status from Devices " +
                    "inner join Type " +
                    "on type_id = Type.id " +
                    "where area_id = (select max(id) from Areas)",
                Connection = con
            };
            DataTable dt_devices = Sql();

            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = row.Field<int>("id"),
                home_id = row.Field<int>("home_id"),
                name = row.Field<string>("name"),
                status = row.Field<bool>("status"),
                devices = dt_devices
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // PUT: api/Area/5
        public HttpResponseMessage Put(int id, AreaDTO area )
        {

            command = new SqlCommand
            {
                CommandText = "update Areas set " +
                    $"home_id = @home_id," +
                    $"name = @name," +
                    $"status = status " +
                    $"where id = @id",
                Connection = con
            };
            command.Parameters.AddWithValue("@home_id", area.home_id);
            command.Parameters.AddWithValue("@name", area.name);
            command.Parameters.AddWithValue("@status", area.status);
            command.Parameters.AddWithValue("@id", id);
            Sql();

            command = new SqlCommand
            {
                CommandText = "select * from Areas where id = @id",
                Connection = con
            };
            command.Parameters.AddWithValue("id", id);
            DataTable dt_area = Sql();

            command = new SqlCommand
            {
                CommandText = "select Devices.id,Devices.name,Type.name as type,status from Devices " +
                    "inner join Type " +
                    "on type_id = Type.id " +
                    "where area_id = @id",
                Connection = con
            };
            command.Parameters.AddWithValue("@id", id);
            DataTable dt_device = Sql();


            IList<AreaInfoDTO> items = dt_area.AsEnumerable().Select(row =>
            new AreaInfoDTO
            {
                id = row.Field<int>("id"),
                home_id = row.Field<int>("home_id"),
                name = row.Field<string>("name"),
                status = row.Field<bool>("status"),
                devices = dt_device
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // DELETE: api/Area/5
        public void Delete(int id)
        {
        }
    }
}
