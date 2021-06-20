using System;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class SupplierRetail

    {


        public Guid supplierid { get; set; }
        public string companyname { get; set; }

        public string address { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string licenseno { get; set; }

        public string phone { get; set; }


        public Guid bookid { get; set; }
        public string bookname { get; set; }

        public string isbn { get; set; }
        public Int32 cost { get; set; }

        public Int32 stockcount { get; set; }




        //  public string RequestId { get; set; }

        //  public List<SupplierRetail> results;


        //  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);



    }


    public class SupplierRetailResults
    {
        public List<SupplierRetail> qResult = new List<SupplierRetail>(); //{ get; set; }

    }


}
