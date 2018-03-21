using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormWebApi.Models
{
    public class WebApiResult
    {
        public int code { get; set; }
        public string errmsg { get; set; }
        public object param1 { get; set; }

        public object param2 { get; set; }

        public void seterror(int code,string errmsg)
        {
            this.code = code;
            this.errmsg = errmsg;
        }
    }
}