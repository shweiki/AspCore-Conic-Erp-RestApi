using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class ActionLogController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        public Boolean Create(ActionLog LogOpratio)
        {
                LogOpratio.PostingDateTime = DateTime.Now;
                try
                {
                    // TODO: Add insert logic here
                    DB.ActionLogs.Add(LogOpratio);
                    DB.SaveChanges();
                    //  Console.WriteLine(collection);
                    //return Json(collection);
                    return (true);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return (false);
                }
        }

    }
}
