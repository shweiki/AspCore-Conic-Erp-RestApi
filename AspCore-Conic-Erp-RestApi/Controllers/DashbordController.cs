using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class DashbordController : Controller
    {

        private ConicErpContext DB;
        public DashbordController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [Route("Dashbord/GetTotal")]
        [HttpGet]
        public IActionResult GetTotal()
        {
            string MsgCredit = "0";

            WebRequest request = WebRequest.Create(
              "http://josmsservice.com/smsonline/GetBalance.cfm?AccName=highfit&AccPass=D7!cT5!SgU0");
     
            if(request.ContentLength > 0) { 
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                string responseFromServer;
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    MsgCredit = responseFromServer;
                    response.Close();
                }
            }
            var Data = new
                {
                    Items = DB.Items.Count(),
                    Purchases = DB.PurchaseInvoices.Count(),
                    Sales = DB.SalesInvoices.Count(),
                    Clients = DB.Vendors.Where(x => x.Type == "Customer").Count(),
                    Suppliers = DB.Vendors.Where(x => x.Type == "Supplier").Count(),
                    Members = DB.Members.Count(),
                    MembersActive = DB.Members.Where(x => x.Status >= 0).Count(),
                    MsgCredit
                };
                return Ok(Data);



        }
    }
}
