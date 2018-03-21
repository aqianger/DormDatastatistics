using DormData;
using DormWebApi.CommUtil;
using DormWebApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DormWebApi.Controllers
{
    [EnableCors(origins: "https://esquel.sharepoint.com,http://localhost:3000", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        HotelDataTestEntities entities = new HotelDataTestEntities();
        WebApiResult result = new WebApiResult();
        // GET api/values
        public IEnumerable<string> Get()
        {
            LogHelper.WriteLog(this.GetType(), "test");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        /// <summary>
        /// 接收从RSS传来的Hotel列表数据,post http://host/api/Values/SetHotel
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetHotel(dynamic value)
        {
            var s = value as JArray;

            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        Hotel dm = entities.Hotel.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new Hotel();
                            dm.ID = id;
                            entities.Hotel.Add(dm);
                        }
                        else
                        {

                            if (dm.Modified.HasValue && dm.Modified.Value == mdTime)
                            {
                                continue;
                            }
                        }
                        dm.dbedited = DateTime.Now;
                        dm.Address = s[i]["Address"].ToString();
                        if (s[i]["CancellationTime"].Type == JTokenType.Integer)
                        {
                            dm.CancellationTime = (int)s[i]["CancellationTime"];
                        }
                        if (s[i]["CurrentPrice"].Type == JTokenType.Integer || s[i]["CurrentPrice"].Type == JTokenType.Float)
                        {
                            dm.CurrentPrice = decimal.Parse(s[i]["CurrentPrice"].ToString());
                        }
                        if (s[i]["FactoryCodeId"].Type == JTokenType.Integer)
                        {
                            dm.FactoryCodeId = (int)s[i]["FactoryCodeId"];
                        }
                        if (s[i]["GradeId"].Type == JTokenType.Integer)
                        {
                            dm.GradeId = (int)s[i]["GradeId"];
                        }
                        if (s[i]["HighSeasonPrice"].Type == JTokenType.Integer || s[i]["HighSeasonPrice"].Type == JTokenType.Float)
                        {
                            dm.HighSeasonPrice = decimal.Parse(s[i]["HighSeasonPrice"].ToString());
                        }
                        if (s[i]["HotelLocalAddress"].Type == JTokenType.String)
                        {
                            dm.HotelLocalAddress = s[i]["HotelLocalAddress"].ToString();
                        }
                        if (s[i]["HotelLocalName"].Type == JTokenType.String)
                        {
                            dm.HotelLocalName = s[i]["HotelLocalName"].ToString();
                        }
                        dm.IsDorm = bool.Parse(s[i]["IsDorm"].ToString());
                        if (s[i]["LowSeasonPrice"].Type == JTokenType.Integer || s[i]["LowSeasonPrice"].Type == JTokenType.Float)
                        {
                            dm.LowSeasonPrice = decimal.Parse(s[i]["LowSeasonPrice"].ToString());
                        }
                        dm.Modified = mdTime;
                        dm.PhoneNumber = s[i]["PhoneNumber"].ToString();
                        if (s[i]["PublicHolidayPrice"].Type == JTokenType.Integer || s[i]["PublicHolidayPrice"].Type == JTokenType.Float)
                        {
                            dm.PublicHolidayPrice = decimal.Parse(s[i]["PublicHolidayPrice"].ToString());
                        }
                        dm.Remarks = s[i]["Remarks"].ToString();
                        dm.RoomDescription = s[i]["RoomDescription"].ToString();
                        dm.Title = s[i]["Title"].ToString();


                        try
                        {
                            successed += entities.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
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
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }

        /// <summary>
        /// 接收从RSS传来的DormRoomStatus列表数据,post http://host/api/Values/SetDormRoomStatus
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetDormRoomStatus(dynamic value)
        {
            var s = value as JArray;


            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        DormRoomStatus dm = entities.DormRoomStatus.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new DormRoomStatus();
                            dm.ID = id;
                            entities.DormRoomStatus.Add(dm);
                        }
                        else if (dm.Modified == mdTime)
                        {
                            continue;
                        }
                        dm.dbedited = DateTime.Now;
                        if (s[i]["CompleteDate"].Type == JTokenType.Date)
                        {
                            dm.CompleteDate = DateTime.Parse(s[i]["CompleteDate"].ToString());
                        }
                        dm.DormIDId = (int)s[i]["DormIDId"];
                        dm.Modified = mdTime;
                        if (s[i]["RecordDate"].Type == JTokenType.Date)
                        {
                            dm.RecordDate = DateTime.Parse(s[i]["RecordDate"].ToString());
                        }

                        dm.Remark = s[i]["Remark"].ToString();
                        dm.Status = s[i]["Status"].ToString();
                        dm.Title = s[i]["Title"].ToString();



                        try
                        {
                            successed += entities.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
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
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        // POST api/values
        /// <summary>
        /// 接收从RSS传来的Dorm列表数据,post http://host/api/Values/SetDorm
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetDorm(dynamic value)
        {
            var s = value as JArray;


            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        Dorm dm = entities.Dorm.FirstOrDefault(d => d.ID == id);
                        DateTime edTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new Dorm();
                            dm.ID = id;
                            entities.Dorm.Add(dm);
                        }
                        else
                        {
                            if (dm.Modified == edTime)
                            {
                                continue;
                            }
                        }
                        dm.dbedited = DateTime.Now;
                        dm.DormCodeId = int.Parse(s[i]["DormCodeId"].ToString());
                        dm.DormCodeText = s[i]["DormCodeText"].ToString();
                        if (s[i]["FactoryCodeId"] != null)
                        {
                            dm.FactoryCodeId = int.Parse(s[i]["FactoryCodeId"].ToString());
                        }
                        dm.Modified = edTime;
                        dm.Price = decimal.Parse(s[i]["Price"].ToString());
                        dm.Title = s[i]["Title"].ToString();

                        try
                        {
                            successed += entities.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
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
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的Booking列表数据,post http://host/api/Values/SetBooking
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetBooking(dynamic value)
        {

            var s = value as JArray;


            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = int.Parse(s[i]["ID"].ToString());
                    if (id > 0)
                    {
                        Booking booking = entities.Booking.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (booking == null)
                        {
                            booking = new Booking();
                            booking.ID = id;
                            entities.Booking.Add(booking);
                        }
                        else
                        {
                            if (booking.Modified == mdTime)
                            {
                                continue;
                            }
                        }
                        booking.AdminRemarks = s[i]["AdminRemarks"].ToString();
                        booking.AllGuestName = s[i]["AllGuestName"].ToString();
                        booking.AuthorId = int.Parse(s[i]["AuthorId"].ToString());
                        booking.CheckinDate = DateTime.Parse(s[i]["CheckinDate"].ToString());
                        booking.CheckoutDate = DateTime.Parse(s[i]["CheckoutDate"].ToString());
                        booking.BookingPurposeCodeId = (int)s[i]["BookingPurposeCodeId"];
                        if (s[i]["IsDorm"] != null)
                        {
                            booking.IsDorm = bool.Parse(s[i]["IsDorm"].ToString());
                        }
                        if (s[i]["ChargeValueCalculatedDate"] != null && s[i]["ChargeValueCalculatedDate"].ToString().Length > 8)
                        {
                            booking.ChargeValueCalculatedDate = DateTime.Parse(s[i]["ChargeValueCalculatedDate"].ToString());
                        }

                        booking.dbedited = DateTime.Now;
                        if (s[i]["EditorId"] != null)
                        {
                            booking.EditorId = (int)s[i]["EditorId"];
                        }
                        booking.FactoryCodeId = (int)s[i]["FactoryCodeId"];
                        booking.IsUpdate = bool.Parse(s[i]["IsUpdate"].ToString());
                        booking.Modified = mdTime;
                        booking.NumberOfRooms = (int)s[i]["NumberOfRooms"];
                        booking.Remarks = s[i]["Remarks"].ToString();
                        booking.StaffEmails = s[i]["StaffEmails"].ToString();
                        booking.RequestorDepartmentCode = s[i]["RequestorDepartmentCode"].ToString();
                        booking.RequestorFactoryCode = s[i]["RequestorFactoryCode"].ToString();
                        booking.RequestorName = s[i]["RequestorName"].ToString();

                        booking.Title = s[i]["Title"].ToString();
                        booking.StatusForDetailsUsed = s[i]["StatusForDetailsUsed"].ToString();
                        if (s[i]["StaffGrade"] != null)
                        {

                            booking.StaffGrade = (int)s[i]["StaffGrade"] > 0 ? true : false;
                        }
                        booking.Status = s[i]["Status"].ToString();
                        try
                        {
                            successed += entities.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
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
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的BookingDetails列表数据,post http://host/api/Values/SetBookingDetails
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetBookingDetails(dynamic value)
        {
            var s = value as JArray;
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        BookingDetails bkdt = entities.BookingDetails.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (bkdt == null)
                        {
                            bkdt = new BookingDetails();
                            bkdt.ID = id;
                            entities.BookingDetails.Add(bkdt);
                        }
                        else
                        {
                            if (bkdt.Modified.HasValue && bkdt.Modified.Value == mdTime)
                            {
                                continue;
                            }
                        }
                        if (s[i]["ActualCheckinDate"] != null && s[i]["ActualCheckinDate"].ToString().Length > 8)
                        {
                            bkdt.ActualCheckinDate = DateTime.Parse(s[i]["ActualCheckinDate"].ToString());
                        }

                        if (s[i]["ActualCheckoutDate"] != null && s[i]["ActualCheckoutDate"].ToString().Length > 8)
                        {
                            bkdt.ActualCheckoutDate = DateTime.Parse(s[i]["ActualCheckoutDate"].ToString());
                        }

                        if (s[i]["Alloc_x0020_Flag"] != null && s[i]["Alloc_x0020_Flag"].ToString().Length > 0)
                        {
                            bool k = false;
                            if (bool.TryParse(s[i]["Alloc_x0020_Flag"].ToString(), out k))
                            {
                                bkdt.Alloc_x0020_Flag = k;
                            }
                        }

                        if (s[i]["CancelFlag"] != null && s[i]["CancelFlag"].ToString().Length > 0)
                        {
                            bool k = false;
                            if (bool.TryParse(s[i]["CancelFlag"].ToString(), out k))
                            {
                                bkdt.CancelFlag = k;
                            }
                        }
                        if (s[i]["CheckinDate"] != null && s[i]["CheckinDate"].ToString().Length > 0)
                        {
                            bkdt.CheckinDate = DateTime.Parse(s[i]["CheckinDate"].ToString());
                        }
                        if (s[i]["CheckoutDate"] != null && s[i]["CheckoutDate"].ToString().Length > 0)
                        {
                            bkdt.CheckoutDate = DateTime.Parse(s[i]["CheckoutDate"].ToString());
                        }
                        if (s[i]["AuthorId"] != null)
                        {
                            bkdt.AuthorId = (int)s[i]["AuthorId"];
                        }
                        if (s[i]["CancelFlag"] != null)
                        {
                            bkdt.CancelFlag = ((JObject)s[i]).Value<bool>("CancelFlag");
                        }
                        bkdt.CancelSettled = s[i]["CancelSettled"].ToString();
                        bkdt.ChargedCancel = bool.Parse(s[i]["ChargedCancel"].ToString());
                        if (s[i]["ChargeTypeId"].Type == JTokenType.Integer)
                        {
                            bkdt.ChargeTypeId = ((JObject)s[i]).Value<int>("ChargeTypeId");
                        }
                        if (s[i]["ChargeValue"].Type == JTokenType.Integer || s[i]["ChargeValue"].Type == JTokenType.Float)
                        {
                            bkdt.ChargeValue = decimal.Parse(s[i]["ChargeValue"].ToString());
                        }
                        if (s[i]["CostCenterCodeId"].Type == JTokenType.Integer)
                        {
                            bkdt.CostCenterCodeId = (int)s[i]["CostCenterCodeId"];
                        }
                        bkdt.Created = DateTime.Parse(s[i]["Created"].ToString());
                        bkdt.dbedited = DateTime.Now;
                        /* 与CostCenterCodeId重复了
                        if (s[i]["DepartmentCodeId"].HasValues)
                        {
                            bkdt.DepartmentCodeId = (int)s[i]["DepartmentCodeId"];
                        }
                        */
                        bkdt.DetailFactoryCodeId = (int)s[i]["DetailFactoryCodeId"];
                        bkdt.DepartmentName = s[i]["DepartmentName"].ToString();
                        /* 与DetailFactoryCodeId重复了
                        if (s[i]["FactoryCode2Id"].Type==JTokenType.Integer)
                        {
                            bkdt.FactoryCode2Id = (int)s[i]["FactoryCode2Id"];
                        }*/

                        if (s[i]["FactoryCodeId"].Type == JTokenType.Integer)
                        {
                            bkdt.FactoryCodeId = (int)s[i]["FactoryCodeId"];
                        }
                        bkdt.Gender = s[i]["Gender"].ToString();
                        if (s[i]["Guest"] != null)
                        {
                            bkdt.Guest = bool.Parse(s[i]["Guest"].ToString());
                        }
                        bkdt.IsDorm = bool.Parse(s[i]["IsDorm"].ToString());
                        bkdt.Modified = mdTime;
                        if (s[i]["NumberOfRoomNights"].Type == JTokenType.Integer || s[i]["NumberOfRoomNights"].Type == JTokenType.Float)
                        {
                            bkdt.NumberOfRoomNights = (byte)decimal.Parse(s[i]["NumberOfRoomNights"].ToString());
                        }

                        bkdt.OfficeEmail = s[i]["OfficeEmail"].ToString();
                        if (s[i]["OfficeNameId"].HasValues)
                        {
                            bkdt.OfficeNameId = (int)s[i]["OfficeNameId"];
                        }
                        bkdt.OfficeNameText = s[i]["OfficeNameText"].ToString();
                        bkdt.ReferenceNumberId = (int)s[i]["ReferenceNumberId"];
                        bkdt.Remarks = s[i]["Remarks"].ToString();
                        bkdt.StaffDepartment = s[i]["StaffDepartment"].ToString();
                        bkdt.StaffTitle = s[i]["StaffTitle"].ToString();
                        if (s[i]["SubFactoryId"].Type == JTokenType.Integer)
                        {
                            bkdt.SubFactoryId = (int)s[i]["SubFactoryId"];
                        }
                        bkdt.Title = s[i]["Title"].ToString();
                        bkdt.TravelDocumentName = s[i]["TravelDocumentName"].ToString();
                        bkdt.TravelType = s[i]["TravelType"].ToString();


                        try
                        {
                            successed += entities.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
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
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}\n{3}", i, s[i], ex.Message, ex.StackTrace);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的Factory列表数据,post http://host/api/Values/SetFactory
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetFactory(dynamic value)
        {
            var s = value as JArray;

            if (s == null)
            {
                result.seterror(-1, "没有获取到参数");
                return result;
            }
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        Factory dm = entities.Factory.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new Factory();
                            dm.ID = id;
                            entities.Factory.Add(dm);
                        }
                        else if (dm.Modified == mdTime)
                        {
                            continue;
                        }

                        dm.dbedited = DateTime.Now;
                        dm.City = s[i]["City"].ToString();
                        dm.Country = s[i]["Country"].ToString();
                        dm.Currency = s[i]["Currency"].ToString();
                        dm.DestinationCode = s[i]["DestinationCode"].ToString();
                        if (s[i]["IsDestination"] != null)
                        {
                            dm.IsDestination = bool.Parse(s[i]["IsDestination"].ToString());
                        }
                        dm.Language = s[i]["Language"].ToString();

                        dm.Modified = mdTime;

                        dm.PostCode = s[i]["PostCode"].ToString();
                        dm.TimeZone = s[i]["TimeZone"].ToString();
                        dm.Title = s[i]["Title"].ToString();


                        successed += entities.SaveChanges();
                    }

                }
                string msg = string.Format("成功导入{0}条数据。", successed);
                result.seterror(successed, msg);
                LogHelper.WriteLog(this.GetType(), msg, LogLevel.Info);
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的FactoryCode列表数据,post http://host/api/Values/SetFactoryCode
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetFactoryCode(dynamic value)
        {
            var s = value as JArray;


            if (s == null)
            {
                result.seterror(-1, "没有获取到参数");
                return result;
            }
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        FactoryCode dm = entities.FactoryCode.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new FactoryCode();
                            dm.ID = id;
                            entities.FactoryCode.Add(dm);
                        }
                        else if (dm.Modified == mdTime)
                        {
                            continue;
                        }
                        dm.dbedited = DateTime.Now;
                        dm.Title = s[i]["Title"].ToString();


                        dm.Modified = mdTime;




                        successed += entities.SaveChanges();
                    }

                }
                string msg = string.Format("成功导入{0}条数据。", successed);
                result.seterror(successed, msg);
                LogHelper.WriteLog(this.GetType(), msg, LogLevel.Info);
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的BookingPurpose列表数据,post http://host/api/Values/SetBookingPurpose
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetBookingPurpose(dynamic value)
        {
            var s = value as JArray;


            if (s == null)
            {
                result.seterror(-1, "没有获取到参数");
                return result;
            }
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        BookingPurpose dm = entities.BookingPurpose.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new BookingPurpose();
                            dm.ID = id;
                            entities.BookingPurpose.Add(dm);
                        }
                        else
                        {
                            if (dm.Modified == mdTime)
                            {
                                continue;
                            }
                        }
                        dm.dbedited = DateTime.Now;
                        dm.Title = s[i]["Title"].ToString();
                        dm.NACode = s[i]["NACode"].ToString();


                        dm.Modified = mdTime;




                        successed += entities.SaveChanges();
                    }

                }
                string msg = string.Format("成功导入{0}条数据。", successed);
                result.seterror(successed, msg);
                LogHelper.WriteLog(this.GetType(), msg, LogLevel.Info);
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的CostCenter列表数据,post http://host/api/Values/SetCostCenter
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetCostCenter(dynamic value)
        {
            var s = value as JArray;


            if (s == null)
            {
                result.seterror(-1, "没有获取到参数");
                return result;
            }
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        CostCenter dm = entities.CostCenter.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new CostCenter();
                            dm.ID = id;
                            entities.CostCenter.Add(dm);
                        }
                        else
                        {
                            if (dm.Modified == mdTime)
                            {
                                continue;
                            }
                        }

                        dm.dbedited = DateTime.Now;
                        dm.Title = s[i]["Title"].ToString();
                        dm.DepartmentCode = s[i]["DepartmentCode"].ToString();
                        dm.DepartmentCode_x002d_CH = s[i]["DepartmentCode_x002d_CH"].ToString();
                        dm.EditorId = (int)s[i]["EditorId"];
                        dm.FactoryCodeId = (int)s[i]["FactoryCodeId"];
                        dm.SubFactory = s[i]["SubFactory"].ToString();


                        dm.Modified = mdTime;




                        successed += entities.SaveChanges();
                    }

                }
                string msg = string.Format("成功导入{0}条数据。", successed);
                result.seterror(successed, msg);
                LogHelper.WriteLog(this.GetType(), msg, LogLevel.Info);
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的MyList列表数据,post http://host/api/Values/SetMyList 此暂时方法没有用到
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetMyList(dynamic value)
        {
            var s = value as JArray;


            if (s == null)
            {
                result.seterror(-1, "没有获取到参数");
                return result;
            }
            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {
                        MyList dm = entities.MyList.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dm == null)
                        {
                            dm = new MyList();
                            dm.ID = id;
                            entities.MyList.Add(dm);
                        }
                        else if (dm.Modified == mdTime)
                        {
                            continue;
                        }

                        dm.dbedited = DateTime.Now;
                        dm.Title = s[i]["Title"].ToString();
                        dm.OData__x0079_jb6 = s[i]["OData__x0079_jb6"].ToString();

                        dm.Modified = mdTime;

                        dm.shgk = s[i]["shgk"].ToString();



                        successed += entities.SaveChanges();
                    }

                }
                string msg = string.Format("成功导入{0}条数据。", successed);
                result.seterror(successed, msg);
                LogHelper.WriteLog(this.GetType(), msg, LogLevel.Info);
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 接收从RSS传来的DormAllocation列表数据,post http://host/api/Values/DormAllocation
        /// </summary>
        /// <param name="value">传入json数组[{ID:xxxx....},{ID:xxxx....}]</param>
        /// <returns></returns>
        public WebApiResult SetDormAllocation(dynamic value)
        {
            var s = value as JArray;


            int count = s.Count;
            LogHelper.WriteLog(this.GetType(), string.Format("获取到了{0}条数据", count), LogLevel.Info);
            int i = 0;
            int successed = 0;
            try
            {
                for (; i < count; i++)
                {

                    int id = (int)s[i]["ID"];
                    if (id > 0)
                    {


                        DormAllocation dat = entities.DormAllocation.FirstOrDefault(d => d.ID == id);
                        DateTime mdTime = DateTime.Parse(s[i]["Modified"].ToString());
                        if (dat == null)
                        {
                            dat = new DormAllocation();
                            dat.ID = id;

                            entities.DormAllocation.Add(dat);
                        }
                        else if (dat.Modified == mdTime)
                        {
                            continue;
                        }
                        dat.Modified = mdTime;
                        if (s[i]["BookingDetailsIDId"].Type == JTokenType.Integer)
                        {
                            dat.BookingDetailsIDId = (int)s[i]["BookingDetailsIDId"];
                        }
                        if (s[i]["CheckinStaffId"].Type == JTokenType.Integer)
                        {
                            dat.CheckinStaffId = (int)s[i]["CheckinStaffId"];
                        }

                        dat.dbedited = DateTime.Now;
                        /*
                         * DormCodeId 来自Hotel.ID,没必要
                        if (s[i]["DormCodeId"].Type== JTokenType.Integer)
                        {
                            dat.DormCodeId = (int)s[i]["DormCodeId"];
                        }
                        */

                        if (s[i]["DormNumberId"].Type == JTokenType.Integer)
                        {
                            dat.DormNumberId = (int)s[i]["DormNumberId"];
                            if (dat.DormNumberId > 0 && (!dat.Price.HasValue || dat.Price.Value == 0))
                            {
                                dat.Price = entities.Dorm.Single(x => x.ID == dat.DormNumberId).Price;
                            }
                        }
                        if (s[i]["FactoryCodeId"].Type == JTokenType.Integer)
                        {
                            dat.FactoryCodeId = (int)s[i]["FactoryCodeId"];
                        }

                        dat.OfficeName = s[i]["OfficeName"].ToString();
                        dat.ReferenceNumberId = (int)s[i]["ReferenceNumberId"];




                        successed += entities.SaveChanges();
                    }

                }
                LogHelper.WriteLog(this.GetType(), string.Format("成功导入{0}条数据。", successed), LogLevel.Info);
            }
            catch (Exception ex)
            {
                result.errmsg = string.Format("第{0}条数据保存失败,数据ID为{1}：{2}", i, s[i], ex.Message);
                result.code = -1;
                LogHelper.WriteLog(this.GetType(), result.errmsg, LogLevel.Error);
                LogHelper.WriteLog(this.GetType(), ((JObject)s[i]).ToString(), LogLevel.Info);
            }

            return result;
        }
        /// <summary>
        /// 统计某个时间段内所有Dorm区域的入住人数
        /// </summary>
        /// <param name="startTime">统计开始时间</param>
        /// <param name="endTime">统计结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public WebApiResult SetDormInDates(string startTime = "", string endTime = "")
        {
            DateTime startDate = DateTime.Parse("2017-01-01");
            DateTime endDate = DateTime.Today.AddDays(1);
            DateTime MaxDate = DateTime.Today.AddYears(-1);
            DateTime MinDate = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                DateTime.TryParse(startTime, out startDate);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                DateTime.TryParse(endTime, out endDate);
            }


            entities.DormInDates.RemoveRange(
                entities.DormInDates.Where(x => x.Date <= startDate && x.Date < endDate));
            entities.SaveChanges();
            List<DormAllocation> das = entities.DormAllocation.Where(
              x => x.DormNumberId.Value > 0 && entities.BookingDetails.Any(y => y.ID == x.BookingDetailsIDId.Value &&
                  ((y.ActualCheckinDate.Value >= startDate && y.ActualCheckinDate.Value < endDate) ||
                 (y.ActualCheckinDate.Value < startDate && startDate < y.ActualCheckoutDate.Value)))
              ).ToList();
            /*
             ((y.ActualCheckinDate.Value <= startDate && y.ActualCheckinDate.Value < endDate) ||
                 (y.ActualCheckinDate.Value < startDate && startDate < y.ActualCheckoutDate.Value))
             */
            List<int> bdids = das.Select(x => x.BookingDetailsIDId ?? 0).Distinct<int>().ToList();
            if (das.Count > 0)
            {
                List<BookingDetails> bdlist = entities.BookingDetails.Where(x => bdids.Contains(x.ID)).ToList();
                // List<DormInDates> didlist = new List<DormInDates>();
                int Added = 0;
                foreach (DormAllocation dac in das)
                {
                    BookingDetails bdl = bdlist.Single(x => x.ID == dac.BookingDetailsIDId);
                    DateTime sDt = bdl.ActualCheckinDate.Value.Date > startDate ? bdl.ActualCheckinDate.Value.Date : startDate;
                    DateTime eDt = bdl.ActualCheckoutDate.Value.Date > endDate ? endDate : bdl.ActualCheckoutDate.Value.Date;
                    for (DateTime dt1 = sDt; dt1.Date < eDt; dt1 = dt1.AddDays(1))
                    {

                        if (!entities.DormInDates.Any(d => d.Date == dt1 && d.DormNumerId == dac.DormNumberId.Value))
                        {
                            DormInDates Did = new DormInDates();
                            Did.DAID = bdl.ID;
                            Did.Date = dt1;
                            Did.DormCodeID = entities.Dorm.Single(x => x.ID == dac.DormNumberId).DormCodeId;
                            Did.DormNumerId = dac.DormNumberId.Value;
                            Did.dbedited = DateTime.Now;
                            //didlist.Add(Did);
                            Added++;
                            if (MaxDate < dt1)
                            {
                                MaxDate = dt1;
                            }
                            if (MinDate > dt1)
                            {
                                MinDate = dt1;
                            }
                            entities.DormInDates.Add(Did);
                            result.code = entities.SaveChanges();
                        }

                    }
                }
                if (Added > 0)
                {

                    result.code = entities.SaveChanges();
                    if (MaxDate < endDate)
                    {
                        MaxDate = endDate;
                    }
                    if (MinDate <= MaxDate)
                    {
                        LogHelper.WriteLog(this.GetType(), "统计{0}-{1}之间的入住数据", LogLevel.Debug);
                        entities.StatisticData(MinDate, MaxDate);
                    }

                }
            }
            return result;
        }
        /// <summary>
        /// 调用存储过程，此方法暂时没用到
        /// </summary>
        /// <param name="param">传入带起止时间的Json参数</param>
        /// <returns></returns>
        public WebApiResult SetStatisticsByDay(JObject param)
        {
            DateTime start = DateTime.Parse("2017-05-01");
            DateTime end = DateTime.Today;
            if (param["startDate"].Type != JTokenType.Null)
            {
                start = DateTime.Parse(param["startDate"].ToString());
            }
            if (param["endDate"].Type != JTokenType.Null)
            {
                end = DateTime.Parse(param["endDate"].ToString());
            }

            entities.StatisticData(start, end);
            return result;
        }
        /// <summary>
        /// 获取Dorm区域ID和Title列表
        /// </summary>
        /// <returns></returns>
        public WebApiResult Dorms()
        {
            result.param1 = entities.Hotel.Where(x => x.IsDorm == true).Select(x => new
            {
                x.ID,
                x.Title
            }).ToList();
            return result;
        }
        /// <summary>
        /// 提供日历图数据，传入起止时间
        /// </summary>
        /// <param name="param">含起止时间的json对象</param>
        /// <returns></returns>
        public WebApiResult CdEvents(dynamic param)
        {
            int C_dormcode = 0;
            try
            {
                C_dormcode = param.dormcode;
            }
            catch { }

            try
            {
                DateTime start = param.start;

                DateTime end = param.end;
                var q_Dorms = entities.Hotel.Where(x => x.IsDorm && (C_dormcode == 0 || x.ID == C_dormcode)).Select(x => new { x.ID, x.Title });
                var q_Stats = entities.StatisticsByDay.Where(x => x.start >= start && x.start <= end);
                var q_list = q_Dorms.GroupJoin(q_Stats, x => x.ID, y => y.DormCodeId, (x, y) => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    Stats = y
                }).ToList();
                List<CalendarEventItem> list = new List<CalendarEventItem>();
                var Dcounts = entities.Dorm.Where(x => (C_dormcode == 0 || x.DormCodeId == C_dormcode)).GroupBy(x => x.DormCodeId).Select(x => new
                {
                    DormCodeId = x.Key,
                    Dorms = x.Count()
                });
                decimal rate = 0.0m;
                int Dmnum = 0;
                foreach (var q in q_list)
                {
                    foreach (var j in q.Stats)
                    {
                        CalendarEventItem Cei = new CalendarEventItem();
                        Cei.id = q.ID;
                        Cei.start = j.start;
                        //日入住率 = 当日已入住的房间数 / (该Dorm的总房间数 - 该Dorm当日正在维护中的房间数)
                        Dmnum = j.roomnum - (j.maintenance ?? 0);

                        if (Dmnum > 0)
                        {
                            rate = ((decimal)j.guestnum / Dmnum) * 100;
                        }
                        Cei.rate = rate;
                        Cei.title = string.Concat(q.Title, " ", j.guestnum, "(入住率 ", string.Format("{0:F1}%", rate), ")");
                        list.Add(Cei);
                    }

                }
                if (list.Count > 0 && C_dormcode == 0)
                {
                    DateTime maxDt = list.Max(x => x.start);
                    DateTime minDt = list.Min(x => x.start);
                    if (!entities.Hotel.Any(x => x.Title.Contains("杨梅客房")))
                    {
                        for (DateTime st = start; st <= end; st = st.AddDays(1))
                        {
                            if (st > maxDt)
                            {
                                break;
                            }
                            if (st >= minDt)
                            {
                                CalendarEventItem Cei = new CalendarEventItem();
                                Cei.id = 9999;
                                Cei.start = st;
                                Cei.rate = 0;
                                Cei.title = "GET 杨梅客房 0(入住率 0.0%)";
                                list.Add(Cei);
                            }
                        }
                    }
                }
                result.param1 = list;

                return result;
            }
            catch (Exception ex)
            {
                result.seterror(-1, ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 获取指定日期，指定Dorm区域的入住人员信息列表
        /// </summary>
        /// <param name="param">含DormCode、start的json对象</param>
        /// <returns></returns>
        public WebApiResult DormBookingDetails(dynamic param)
        {
            int DormCode = 0;
            try
            {
                DormCode = param.DormCode;
            }
            catch (Exception ex)
            {
                result.seterror(-1, "参数错误");
                return result;
            }
            string strDate = "";
            DateTime Date = DateTime.Parse("1901-1-1");
            try
            {
                strDate = param.start;
                if (DateTime.TryParse(strDate, out Date))
                {
                    if (Date < new DateTime(2017, 1, 1))
                    {
                        result.seterror(-1, "日期无效，必须大于2017-01-01日");
                        return result;
                    }
                }
                else
                {
                    result.seterror(-1, "日期参数错误");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.seterror(-1, "日期参数错误");
                return result;
            }

            string Sql = string.Format(@"select distinct C.Title as ReferenceNumber,
'{0}' as [Date],A.StaffTitle,A.DepartmentName as StaffDepartment,
ActualCheckinDate as CheckinDate,ActualCheckoutDate as CheckoutDate,
A.OfficeNameText as OfficeName,
A.Gender,B.* from (select * from [dbo].[BookingDetails] where IsDorm=1 and CancelFlag=0 and ActualCheckinDate<='{0}' and
ActualCheckoutDate>'{0}') A left join Booking C on A.ReferenceNumberId=C.ID
left join (select BookingDetailsIDId,Dorm.DormCodeText,Dorm.ID,Dorm.DormCodeId ,Dorm.ID as DormNumber from [dbo].[DormAllocation] left join 
Dorm on DormAllocation.DormNumberId=Dorm.ID ) B on A.ID=B.[BookingDetailsIDId] where B.DormCodeId={1}", Date.ToString("yyyy-MM-dd"), DormCode);

            List<DormListItem> list = entities.Database.SqlQuery<DormListItem>(Sql, new object[] { }).ToList();
            result.param1 = list;

            return result;

        }

        public WebApiResult DormTrend(dynamic param)
        {
            string Ym = "";
            try
            {
                Ym = param.yearmonth;
            }
            catch (Exception ex)
            {
                result.seterror(-1, "参数错误");
                return result;
            }
            if (Ym.Length != 6 && !Regex.IsMatch(Ym, @"^(([1-9]\d{3})|(0\d{2}[1-9]))(0[1-9]|1[0-2])$"))
            {
                result.seterror(-1, "参数错误");
                return result;
            }
            int Year = int.Parse(Ym.Substring(0, 4));
            int Month = int.Parse(Ym.Substring(4));
            JArray DormCodes = new JArray();
            try
            {
                DormCodes = param.dormcodes as JArray;
            }
            catch
            {
                DormCodes = new JArray();
            }
            List<int> DormCodeids = new List<int>();
            foreach (var j in DormCodes)
            {
                DormCodeids.Add((int)j);
            }
            var q = entities.StatisticsByDay.Where(x => x.start.Year == Year && x.start.Month == Month);
            if (DormCodes.Count > 0)
            {
                q = q.Where(x => DormCodeids.Contains(x.DormCodeId));
            }
            var list = q.GroupJoin(entities.Hotel,
               x => x.DormCodeId, y => y.ID, (x, y) => new
               {
                   start = x.start,
                   x.guestnum,
                   x.DormCodeId,
                   Title = y.Select(a => a.Title).FirstOrDefault()
               }).OrderBy(x => x.start).ThenBy(x => x.DormCodeId).ToList();
            ArrayList source = new ArrayList();
            ArrayList Item = new ArrayList();
            // ArrayList DataItems = new ArrayList();
            Item.Add("DormCode");
            foreach (int d in DormCodeids)
            {
                Item.Add(list.First(x => x.DormCodeId == d).Title);

            }
            source.Add(Item);
            bool HasDate = false;
            foreach (var d in list)
            {
                HasDate = false;
                int i = source.Count - 1;
                if (i > 0)
                {
                    if (((ArrayList)source[i])[0].ToString() == d.start.ToString("yyyy-MM-dd"))
                    {
                        HasDate = true;

                    }
                }
                if (!HasDate)
                {
                    ArrayList Ditem = new ArrayList();
                    Ditem.Add(d.start.ToString("yyyy-MM-dd"));
                    Ditem.Add(d.guestnum);
                    source.Add(Ditem);
                }
                else
                {
                    ((ArrayList)source[i]).Add(d.guestnum);
                }
            }

            result.param1 = source;
            return result;


        }

        /// <summary>
        /// 按月统计各Dorm区域入住人数
        /// </summary>
        /// <param name="param">传入'yyyyMM'格式的年月，含开始年月和结束年月</param>
        /// <returns></returns>
        public WebApiResult DormTrendByMonth(dynamic param)
        {
            string Ym_start = "";
            string Ym_end = DateTime.Today.ToString("yyyyMM");
            try
            {
                Ym_start = param.ymstart;
            }
            catch (Exception ex)
            {
                result.seterror(-1, "参数错误");
                return result;
            }
            try
            {
                Ym_end = param.ymend;
            }
            catch (Exception ex)
            {
                Ym_end = DateTime.Today.ToString("yyyyMM");
            }
            if (Ym_start.Length != 6 && !Regex.IsMatch(Ym_start, @"^(([1-9]\d{3})|(0\d{2}[1-9]))(0[1-9]|1[0-2])$"))
            {
                result.seterror(-1, "参数开始时间错误");
                return result;
            }
            if (Ym_end.Length != 6 && !Regex.IsMatch(Ym_end, @"^(([1-9]\d{3})|(0\d{2}[1-9]))(0[1-9]|1[0-2])$"))
            {
                result.seterror(-1, "参数结束时间错误");
                return result;
            }
            int YearMonth_start = int.Parse(Ym_start.Substring(0, 4)) * 100 + int.Parse(Ym_start.Substring(4));

            int YearMonth_end = int.Parse(Ym_end.Substring(0, 4)) * 100 + int.Parse(Ym_end.Substring(4));

            JArray DormCodes = new JArray();
            try
            {
                DormCodes = param.dormcodes as JArray;
            }
            catch
            {
                DormCodes = new JArray();
            }
            List<int> DormCodeids = new List<int>();
            foreach (var j in DormCodes)
            {
                DormCodeids.Add((int)j);
            }
            var q = entities.StatisticsByDay.Where(x => x.start.Year * 100 + x.start.Month >= YearMonth_start && x.start.Year * 100 + x.start.Month <= YearMonth_end);
            if (DormCodes.Count > 0)
            {
                q = q.Where(x => DormCodeids.Contains(x.DormCodeId));
            }
            var list = q.GroupJoin(entities.Hotel,
               x => x.DormCodeId, y => y.ID, (x, y) => new
               {
                   YM = x.start.Year * 100 + x.start.Month,
                   x.guestnum,
                   x.DormCodeId,
                   Title = y.Select(a => a.Title).FirstOrDefault()
               }).GroupBy(x => new { x.YM, x.DormCodeId, x.Title }).Select(
                x => new
                {
                    YM = x.Key.YM.ToString(),
                    DormCodeId = x.Key.DormCodeId,
                    Title = x.Key.Title,
                    guestnum = x.Sum(a => a.guestnum)
                }
                ).OrderBy(x => x.YM).ThenBy(x => x.DormCodeId).ToList();
            ArrayList source = new ArrayList();
            ArrayList Item = new ArrayList();
            // ArrayList DataItems = new ArrayList();
            Item.Add("DormCode");
            foreach (int d in DormCodeids)
            {
                Item.Add(list.First(x => x.DormCodeId == d).Title);

            }
            source.Add(Item);
            bool HasDate = false;
            foreach (var d in list)
            {
                HasDate = false;
                int i = source.Count - 1;
                if (i > 0)
                {
                    if (((ArrayList)source[i])[0].ToString() == d.YM)
                    {
                        HasDate = true;

                    }
                }
                if (!HasDate)
                {
                    ArrayList Ditem = new ArrayList();
                    Ditem.Add(d.YM);
                    Ditem.Add(d.guestnum);
                    source.Add(Ditem);
                }
                else
                {
                    ((ArrayList)source[i]).Add(d.guestnum);
                }
            }

            result.param1 = source;
            return result;

        }
        

    }
}
