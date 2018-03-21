using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormWebApi.Models
{
    public class DormListItem
    {
        /// <summary>
        /// booking.Title
        /// </summary>
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// 查询指定的日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// BookingDetails.OfficeNameText
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// BookingDetails.Gender
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// DormAllocation.Dorm.DormCodeText
        /// </summary>
        public int DormCode { get; set; }
        /// <summary>
        /// DormAllocation.Dorm.ID
        /// </summary>
        public int DormNumber { get; set; }
        /// <summary>
        /// BookingDetails.StaffTitle
        /// </summary>
        public string StaffTitle { get; set; }
        /// <summary>
        /// BookingDetails.Department
        /// </summary>
        public string StaffDepartment { get; set; }
        /// <summary>
        /// BookingDetails.ActualCheckinDate
        /// </summary>
        public DateTime CheckinDate { get; set; }
        /// <summary>
        /// BookingDetails.ActualCheckoutDate
        /// </summary>
        public DateTime CheckoutDate { get; set; }
    }
}