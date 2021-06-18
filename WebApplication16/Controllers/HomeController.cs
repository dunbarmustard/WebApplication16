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


            //Query the database
            var rs = session.Execute("SELECT * FROM tutorialkeyspace.tutorialtable;");

            //Setup a list of objects. Each object will store the contents of each row and column from the query
           SupplierRetailResults results = new SupplierRetailResults();
          //  SupplierRetail supplier = new  SupplierRetail  ();
            //Grab the row values from each column in results from query
            foreach (var row in rs)
            {
                var email = row.GetValue<String>("email");
                var name = row.GetValue<String>("name");
                SupplierRetail supplier = new SupplierRetail();
                supplier.email = email;
                supplier.name = name;
                results.qResult.Add(supplier);

                //Add the objects to the list now
           //     items.Add(supplier);



                //We need to do one more query for the actual SUPPLIER table in order to link each id with the name of the supplier. Do the query in a loop for each unique ID from previous results in list

       

            }




            return View(results);
        }



     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
