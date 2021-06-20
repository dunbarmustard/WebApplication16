using System;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class Retail

    {

        public Guid bookid { get; set; }
        public string bookname { get; set; }

        public Int32 cost { get; set; }
        public string isbn { get; set; }

        public Int32 stockcount { get; set; }
        public Guid supplierid { get; set; }

        //  public string RequestId { get; set; }

        //  public List<SupplierRetail> results;


        //  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);



    }


    public class RetailResults
    {
        public List<Retail> qResult = new List<Retail>(); //{ get; set; }

    }


}
