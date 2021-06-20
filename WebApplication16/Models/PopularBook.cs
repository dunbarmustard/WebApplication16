using System;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class PopularBook

    {

        public Guid bookid { get; set; }

        public Int32 orderQuant { get; set; }



   


    }


    public class PopularBookResults
    {
        public List<PopularBook> qResult = new List<PopularBook>(); //{ get; set; }

    }


}
