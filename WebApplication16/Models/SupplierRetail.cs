using System;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class SupplierRetail

    {

        public string email { get; set; }
        public string name { get; set; }

      //  public string RequestId { get; set; }

      //  public List<SupplierRetail> results;


      //  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);



    }


    public class SupplierRetailResults
    {
        public List<SupplierRetail> qResult = new List<SupplierRetail>(); //{ get; set; }

    }


}
