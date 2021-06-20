using System;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class Supplier

    {

        public string address { get; set; }
        public string city { get; set; }

        public string companyname { get; set; }
        public string licenseno { get; set; }

        public string phone { get; set; }
        public string state { get; set; }

        public Guid supplierid { get; set; }
        //  public string RequestId { get; set; }

        //  public List<SupplierRetail> results;


        //  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);



    }


    public class SupplierResults
    {
        public List<Supplier> qResult = new List<Supplier>(); //{ get; set; }

    }


}
