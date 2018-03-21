using DormData;
using DormWebApi.CommUtil;
using DormWebApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DormWebApi.Controllers
{
    [EnableCors(origins: "https://apc.delve.office.com", headers: "*", methods: "*")]
    public class empController : ApiController
    {
        HotelDataTestEntities entities = new HotelDataTestEntities();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }
        public WebApiResult SetEmps(dynamic value)
        {
            WebApiResult result = new WebApiResult();
            if (value == null)
            {
                result.seterror(-1, "没有获取到参数");
                return result;
            }
            var s = value as JArray;

       
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    string id =  s[i]["Id"].ToString();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        if(s[i]["OfficeLocation"] == null)
                        {
                            continue;
                        }
                        employee dm = entities.employee.FirstOrDefault(d => d.ID == id);

                        if (dm == null)
                        {
                            dm = new  employee();
                            dm.ID = id;
                            entities.employee.Add(dm);
                        }
                        dm.dbedited = DateTime.Now;
                        if(s[i]["Department"] != null)
                        {
                            dm.Department = s[i]["Department"].ToString();
                        }
                        else
                        {
                            dm.Department = "";
                        }
                        dm.Email = s[i]["Email"].ToString();
                        dm.FullName = s[i]["FullName"].ToString();
                        if (s[i]["JobTitle"] != null)
                        {
                            dm.JobTitle = s[i]["JobTitle"].ToString();
                        }
                        else
                        {
                            dm.JobTitle = "";
                        }
                        if (s[i]["OfficeLocation"] != null)
                        {
                            dm.OfficeLocation = s[i]["OfficeLocation"].ToString();
                        }
                        else
                        {
                            dm.OfficeLocation = "";
                        }
                        if (s[i]["SipAddress"] != null)
                        {
                            dm.SipAddress = s[i]["SipAddress"].ToString();
                        }
                        dm.UserName = s[i]["UserName"].ToString();
                        try
                        {
                            successed += entities.SaveChanges();
                        }
                        catch(System.Data.Entity.Validation.DbEntityValidationException ex)
                        {
                            var msg = string.Empty;
                            var errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                            foreach (var item in errors)
                            {
                                msg += item.FirstOrDefault().ErrorMessage;
                            }
                            result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], msg);
                            result.code = -1;
                            LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                            return result;
                        }
                    }

                }
                LogHelper.WriteLog(this.GetType(), string.Format("成功导入{0}条数据。", successed), LogLevel.Info);
                result.errmsg = string.Format("已导入{0}条数据。", successed);
            }
            catch (Exception ed)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ed.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}