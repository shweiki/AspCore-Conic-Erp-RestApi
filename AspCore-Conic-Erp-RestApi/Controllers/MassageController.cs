using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Entities;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class MassageController : Controller
    {
                private ConicErpContext DB;
        public MassageController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [Route("Massage/GetMassageObjId")]
        [HttpGet]
        public IActionResult GetMassageObjId(string TableName , long ObjId )
        {

            var Massage = DB.Massages.Where(i => i.TableName == TableName && i.Fktable == ObjId).Select(x => new {
                x.Id,
                x.SendDate,
                x.Type,
                x.Body
            }).ToList();
              
          if(Massage != null)
            return Ok(Massage);

            return Ok(false);
        }
        [Route("Massage/Create")]
        [HttpPost]
        public IActionResult Create(Massage massage)
        {
            if (ModelState.IsValid)
            {

                massage.SendDate = new DateTime();
                    DB.Massages.Add(massage);

                    DB.SaveChanges();
                    return Ok(true);
              
            }

            return Ok(false);
        }

        public bool SendSms(string PhoneNumber , string Body) {
            WebRequest request = WebRequest.Create(
    "http://josmsservice.com/smsonline/msgservicejo.cfm?numbers=962"+PhoneNumber+",&senderid=High Fit&AccName=highfit&AccPass=D7!cT5!SgU0&msg="+Body+"&requesttimeout=5000000");
            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();
            string responseFromServer;
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }

        
            response.Close();
            return true;
        }

        [Route("Massage/CheckMassages")]
        [HttpGet]
        public IActionResult CheckMassages()
        {
            IList<Massage> Massages = DB.Massages?.Where(x=>x.Status == 0).ToList();

            Massages = Massages.GroupBy(a => a.Fktable)
             .Select(g => g.Last()).ToList();

            foreach (Massage M in Massages)
            {
              //  SendSms(M.PhoneNumber, M.Body);
                M.Status = 1;
                DB.SaveChanges();
            }

            return Ok(true);
        }

    }
}
