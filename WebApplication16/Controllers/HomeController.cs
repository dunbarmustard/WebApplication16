using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication16.Models;
using Cassandra;  //This is the important one for Amazon Keyspaces
using System.Net.Security;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebApplication16.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SupplierRetail()
        {
            //Setup the initial connection
            X509Certificate2Collection certCollection = new X509Certificate2Collection();
            X509Certificate2 amazoncert = new X509Certificate2(@"wwwroot\certificate\sf-class2-root.crt");
            var userName = "example-at-356227244861";
            var pwd =  "VaHq3xOlBBbosCj4xIXD3r9XyoM4KbYIqWCg98iIgDI=";




            //    cqlsh cassandra.us - east - 1.amazonaws.com 9142 - u "jzmoolman_keyspace-at-801764440624" - p "CO2REd/dj62NmsFF8DH8v3AxEv5IP70qewGkt9FkOto="--ssl


            certCollection.Add(amazoncert);

            var awsEndpoint = "cassandra.us-east-2.amazonaws.com";

            var cluster = Cluster.Builder()
                     .AddContactPoints(awsEndpoint)
                     .WithPort(9142)
                     .WithAuthProvider(new PlainTextAuthProvider(userName, pwd))
                     .WithSSL(new SSLOptions().SetCertificateCollection(certCollection))
                     .Build();

            var session = cluster.Connect();


            //Query the SUPPLIER
            var rs = session.Execute("SELECT * FROM bookworld.supplier;");

            //Setup a list of objects. Each object will store the contents of each row and column from the query
           SupplierResults supplierList = new SupplierResults();


            //Grab the row values from each column in results from query for SUPPLIER
            foreach (var row in rs)
            {
                var address = row.GetValue<String>("address");
                var city = row.GetValue<String>("city");
                var companyname = row.GetValue<String>("companyname");
                var licenseno = row.GetValue<String>("licenseno");
                var phone = row.GetValue<String>("phone");
                var state = row.GetValue<String>("state");
                var supplierid = row.GetValue<Guid>("supplierid");

                //Setup a new Supplier object
                Supplier supplier = new Supplier();
                supplier.address = address;
                supplier.city = city;
                supplier.companyname = companyname;
                supplier.licenseno = licenseno;
                supplier.phone = phone;
                supplier.state = state;
                supplier.supplierid = supplierid;


                //Add Supplier Objects into a LIST
                supplierList.qResult.Add(supplier);
            }






            //Lets query all the data in Retail table
            rs = session.Execute("SELECT * FROM bookworld.retail;");


            //Setup a list of objects. Each object will store the contents of each row and column from the query for RETAIL
            RetailResults retailerList = new RetailResults();


            //Grab the row values from each column in results from query for SUPPLIER
            foreach (var row in rs)
            {
                var bookid = row.GetValue<Guid>("bookid");
                var bookname = row.GetValue<String>("bookname");
                var cost = row.GetValue<Int32>("cost");
                var isbn = row.GetValue<String>("isbn");
                var stockcount = row.GetValue<Int32>("stockcount");
                var supplierid = row.GetValue<Guid>("supplierid");


                Retail retailer = new Retail();
                retailer.bookid = bookid;
                retailer.bookname = bookname;
                retailer.cost = cost;
                retailer.isbn = isbn;
                retailer.stockcount = stockcount;
                retailer.supplierid = supplierid;

                retailerList.qResult.Add(retailer);
            }


            
            //List of SupplierRetail so we can loop in the html and display

            SupplierRetailResults supplierRetailerList = new SupplierRetailResults();



            //Nested loop which will go through each list Object in Retail and each list object in Supplier
            //Combine the matches if the UID matches and add to SupplierRetail List.
            //Essentially this is merging two tables together to meet Dr.Hayes Requirement.

            for (int i = 0; i < supplierList.qResult.Count;i++)
            {

                for(int j = 0; j < retailerList.qResult.Count; j++)

                {


                    //Most important statement. We only want to merge Retail and Supplier if their Supplier ID matches
                    if(supplierList.qResult[i].supplierid == retailerList.qResult[j].supplierid)
                    {

                        //This object represents the contents of the supplier and retail for entries that matches in supplier ID
                        SupplierRetail supplierRetail = new SupplierRetail();


                        //Merge all the fields into one object
                        supplierRetail.supplierid = supplierList.qResult[i].supplierid;
                        supplierRetail.companyname = supplierList.qResult[i].companyname;
                        supplierRetail.address = supplierList.qResult[i].address;
                        supplierRetail.city =  supplierList.qResult[i].city;
                        supplierRetail.state = supplierList.qResult[i].state;
                        supplierRetail.licenseno = supplierList.qResult[i].licenseno;
                        supplierRetail.phone = supplierList.qResult[i].phone;
                        supplierRetail.bookid = retailerList.qResult[j].bookid;
                        supplierRetail.bookname = retailerList.qResult[j].bookname;
                        supplierRetail.isbn = retailerList.qResult[j].isbn;
                        supplierRetail.cost = retailerList.qResult[j].cost;
                        supplierRetail.stockcount = retailerList.qResult[j].stockcount;

                        //This is a list of the SupplierRetailer Objects
                        supplierRetailerList.qResult.Add(supplierRetail);



                    }




                }




            }





            //Return the object so we can access it in HTML in SupplierRetail.cshtml.
            return View(supplierRetailerList);
        }


        public IActionResult PopularBook()
        {

            //Setup the initial connection
            X509Certificate2Collection certCollection = new X509Certificate2Collection();
            X509Certificate2 amazoncert = new X509Certificate2(@"wwwroot\certificate\sf-class2-root.crt");
            var userName = "example-at-356227244861";
            var pwd = "VaHq3xOlBBbosCj4xIXD3r9XyoM4KbYIqWCg98iIgDI=";




            certCollection.Add(amazoncert);

            var awsEndpoint = "cassandra.us-east-2.amazonaws.com";

            var cluster = Cluster.Builder()
                     .AddContactPoints(awsEndpoint)
                     .WithPort(9142)
                     .WithAuthProvider(new PlainTextAuthProvider(userName, pwd))
                     .WithSSL(new SSLOptions().SetCertificateCollection(certCollection))
                     .Build();

            var session = cluster.Connect();




            //Query the OrderDetails
            var rs = session.Execute("SELECT * FROM bookworld.orderdetails;");


            //Setup a list of objects. Each object will store the contents of each row and column from the query
            PopularBookResults popularBookList = new PopularBookResults();



            //Create a Hash Map to distinugish all unique book IDS. We will map the unique ID to its quantity
            var uniqueBook = new Dictionary<Guid, Int32>();            

            //Garther the query results and store
            foreach (var row in rs)
            {

                var bookid = row.GetValue<Guid>("bookid");
                int orderQuant = row.GetValue<Int32>("orderquantity");

                //Store Unique Books
                if (!uniqueBook.ContainsKey(bookid))
                {
                    uniqueBook.Add(bookid, 0);
                }

                                //Setup a new popularbook object
                                PopularBook popularBook = new PopularBook();

                popularBook.bookid = bookid;
                popularBook.orderQuant = orderQuant;



                //Add PopularBook Objects into a LIST (This contains the raw results of the query)
                popularBookList.qResult.Add(popularBook);



            }


            //Compare Distinct List of Books with the PopularBookObjects and put the total quantity for each one with the associated book id
            //i.e Book 1 total quantity      NO DUPLICATES 
                  //Book 2 total quantity

            
            foreach (var item in uniqueBook.ToList())
            {

                for (int j = 0; j < popularBookList.qResult.Count; j++)

                {


                    //Most important statement. We only want to merge Retail and Supplier if their Supplier ID matches
                    if (item.Key == popularBookList.qResult[j].bookid)
                    {
                        uniqueBook[item.Key] = uniqueBook[item.Key] + popularBookList.qResult[j].orderQuant;

                    }

                }


            }

            //Determine the highest quanitity amount in key
            var maxValue = uniqueBook.Values.Max();
            
            //Determine the BookID that has the highest quantity
            var targetKey = uniqueBook.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;  //Uses Linq to do a comprator. This is essentially looping through elements . 






            //Now query the Retail table and match the targetkey with the Retail ID in order to gather the specific Book Name


            //Lets query all the data in Retail table
            rs = session.Execute("SELECT * FROM bookworld.retail;");


            //Setup a list of objects. Each object will store the contents of each row and column from the query for RETAIL
            RetailResults retailerList = new RetailResults();


            //Grab the row values from each column in results from query for SUPPLIER
            foreach (var row in rs)
            {

                var bookid = row.GetValue<Guid>("bookid");
                var bookname = row.GetValue<String>("bookname");
                var cost = row.GetValue<Int32>("cost");
                var isbn = row.GetValue<String>("isbn");
                var stockcount = row.GetValue<Int32>("stockcount");
                var supplierid = row.GetValue<Guid>("supplierid");



                //This means the targeted Book with highest quantity was found
                if (bookid == targetKey)

                {
                    Retail retailer = new Retail();
                    retailer.bookid = bookid;
                    retailer.bookname = bookname;
                    retailer.cost = cost;
                    retailer.isbn = isbn;
                    retailer.stockcount = stockcount;
                    retailer.supplierid = supplierid;

                    retailerList.qResult.Add(retailer);
                }




            }









            return View(retailerList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
