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
    public class DeviceController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Smart_home_db"].ConnectionString);
        public DataTable sql(string command)
        {

            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        // GET: api/Device
        public HttpResponseMessage Get()
        {
            DataTable dt_device = sql("select [Devices].id,[Areas].id as area_id, [Devices].name , [Type].name as type , Devices.status from Devices " +
                "inner join Areas on Devices.area_id = Areas.id " +
                "inner join[Type] on Devices.type_id = [Type].id ");
            IList<DeviceInfoDTO> items = dt_device.AsEnumerable().Select(row =>
            new DeviceInfoDTO
            {
                area_id = Convert.ToInt32(row["area_id"]),
                name = row["name"].ToString(),
                type = row["type"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                properities = sql("select Properity.name , Device_Properity.value from Device_Properity " +
                     "inner join Properity " +
                     "on Properity.id = Device_Properity.properity_id " +
                     $"where Device_Properity.device_id = {row["id"]}").AsEnumerable().ToDictionary<DataRow, string, string>(row_properity =>
                       row_properity["name"].ToString(),
                     row_value => row_value["value"].ToString())
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // GET: api/Device/5
        public HttpResponseMessage Get(int id, string account, string password)
        {
            DataTable dt_permission = sql("select User_Permission.device_id, [User].account,[User].password from User_Permission " +
                "inner join[User] on User_Permission.user_id = [User].id " +
                $"where User_Permission.device_id = '{id}'");
            if (dt_permission.Rows.Count > 0)
            {
                DataTable dt_device = sql("select [Devices].id,[Areas].id as area_id, [Devices].name , [Type].name as type , Devices.status from Devices " +
                "inner join Areas on Devices.area_id = Areas.id " +
                "inner join[Type] on Devices.type_id = [Type].id " +
                $"where Devices.id = {id}");
                IList<DeviceInfoDTO> items = dt_device.AsEnumerable().Select(row =>
                new DeviceInfoDTO
                {
                    area_id = Convert.ToInt32(row["area_id"]),
                    name = row["name"].ToString(),
                    type = row["type"].ToString(),
                    status = Convert.ToBoolean(row["status"]),
                    properities = sql("select Properity.name , Device_Properity.value from Device_Properity " +
                         "inner join Properity " +
                         "on Properity.id = Device_Properity.properity_id " +
                         $"where Device_Properity.device_id = {row["id"]}").AsEnumerable().ToDictionary<DataRow, string, string>(row_properity =>
                           row_properity["name"].ToString(),
                         row_value => row_value["value"].ToString())
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "user permission error or device not found");
            }
        }

        // POST: api/Device
        public HttpResponseMessage Post(DeviceInfoDTO device)
        {
            DataTable dt_type = sql($"select * from [Type] where name = '{device.type}'");
            if (dt_type.Rows.Count == 0)
            {
                sql($"insert into [Type] values('{device.type}')");
                dt_type = sql($"select * from [Type] where name = '{device.type}'");
            }
            
            sql($"insert into [Devices] values('{device.area_id}','{device.name}','{dt_type.Rows[0]["id"]}','{device.status}')");
            foreach(var i in device.properities)
            {
                DataTable dt_properity = sql($"select * from [Properity] where name = '{i.Key}'");
                if (dt_properity.Rows.Count == 0)
                {
                    sql($"insert into [Properity] values('{i.Key}')");
                    dt_properity = sql($"select * from [Properity] where name = '{i.Key}'");
                }
                

                sql($"insert into [Device_Properity] values((select max(id) from Devices),'{dt_properity.Rows[0]["id"]}','{i.Value}')");
            }
            DataTable dt_device = sql("select [Devices].id,[Areas].id as area_id, [Devices].name , [Type].name as type , Devices.status from Devices " +
                "inner join Areas on Devices.area_id = Areas.id " +
                "inner join[Type] on Devices.type_id = [Type].id " +
                $"where Devices.id = (select max(id) from [Devices])");
            IList<DeviceInfoDTO> items = dt_device.AsEnumerable().Select(row =>
            new DeviceInfoDTO
            {
                area_id = Convert.ToInt32(row["area_id"]),
                name = row["name"].ToString(),
                type = row["type"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                properities = sql("select Properity.name , Device_Properity.value from Device_Properity " +
                     "inner join Properity " +
                     "on Properity.id = Device_Properity.properity_id " +
                     $"where Device_Properity.device_id = {row["id"]}").AsEnumerable().ToDictionary<DataRow, string, string>(row_properity =>
                       row_properity["name"].ToString(),
                     row_value => row_value["value"].ToString())
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        // PUT: api/Device/5
        public HttpResponseMessage Put(int id, DeviceInfoDTO device)
        {
            DataTable dt_permission = sql("select User_Permission.device_id, [User].account,[User].password from User_Permission " +
                "inner join[User] on User_Permission.user_id = [User].id " +
                $"where User_Permission.device_id = '{id}'");
            if (dt_permission.Rows.Count > 0)
            {
                DataTable dt_device = sql($"select * from [Devices] where id = {id}");
                if (dt_device.Rows.Count > 0)
                {
                    DataTable dt_type = sql($"select * from [Type] where name = '{device.type}'");
                    if (dt_type.Rows.Count == 0)
                    {
                        sql($"insert into [Type] values('{device.type}')");
                        dt_type = sql($"select * from [Type] where name = '{device.type}'");
                    }

                    sql($"update [Devices] set " +
                        $"area_id = '{device.area_id}', " +
                        $"name = '{device.name}', " +
                        $"type_id = '{dt_type.Rows[0]["id"]}', " +
                        $"status = '{device.status}' " +
                        $"where id = {id}");

                    DataTable dt_deviceProperity = sql("select Properity.name from Device_Properity " +
                            "inner join Properity " +
                            "on properity_id = Properity.id " +
                            $"where device_id = {id}");

                    foreach (var i in device.properities)
                    {
                        DataTable dt_properity = sql($"select * from [Properity] where name = '{i.Key}'");

                        bool properity_avaliable = false;
                        for (int j = 0; j < dt_deviceProperity.Rows.Count; j++)
                        {
                            if (i.Key == dt_deviceProperity.Rows[j]["name"].ToString())
                            {
                                sql($"update Device_Properity set value='{i.Value}' where properity_id = {dt_properity.Rows[0]["id"]} and device_id = {id}");
                                properity_avaliable = true;
                                break;

                            }
                        }
                        if (!properity_avaliable)
                        {
                            if (dt_properity.Rows.Count == 0)
                            {
                                sql($"insert into [Properity] values('{i.Key}')");
                                dt_properity = sql($"select * from [Properity] where name = '{i.Key}'");
                            }
                            sql($"insert into Device_Properity values('{id}','{dt_properity.Rows[0]["id"]}','{i.Value}')");
                        }

                    }


                    dt_device = sql("select [Devices].id,[Areas].id as area_id, [Devices].name , [Type].name as type , Devices.status from Devices " +
                    "inner join Areas on Devices.area_id = Areas.id " +
                    "inner join[Type] on Devices.type_id = [Type].id " +
                    $"where Devices.id = {id}");
                    IList<DeviceInfoDTO> items = dt_device.AsEnumerable().Select(row =>
                    new DeviceInfoDTO
                    {
                        area_id = Convert.ToInt32(row["area_id"]),
                        name = row["name"].ToString(),
                        type = row["type"].ToString(),
                        status = Convert.ToBoolean(row["status"]),
                        properities = sql("select Properity.name , Device_Properity.value from Device_Properity " +
                             "inner join Properity " +
                             "on Properity.id = Device_Properity.properity_id " +
                             $"where Device_Properity.device_id = {row["id"]}").AsEnumerable().ToDictionary<DataRow, string, string>(row_properity =>
                               row_properity["name"].ToString(),
                             row_value => row_value["value"].ToString())
                    }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, items);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "device not exist");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "you have no permission to edite this device");
            }
            
        }

        // DELETE: api/Device/5
        public void Delete(int id)
        {
        }
    }
}
