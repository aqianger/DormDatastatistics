//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DormData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hotel
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public Nullable<int> CancellationTime { get; set; }
        public decimal CurrentPrice { get; set; }
        public Nullable<int> FactoryCodeId { get; set; }
        public Nullable<int> GradeId { get; set; }
        public Nullable<decimal> HighSeasonPrice { get; set; }
        public string HotelLocalAddress { get; set; }
        public string HotelLocalName { get; set; }
        public bool IsDorm { get; set; }
        public Nullable<decimal> LowSeasonPrice { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<decimal> PublicHolidayPrice { get; set; }
        public string Remarks { get; set; }
        public string RoomDescription { get; set; }
        public string Title { get; set; }
        public System.DateTime dbedited { get; set; }
    }
}
