using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using AspCore_Conic_Erp_RestApi.Controllers;
using Entities; 
using Microsoft.AspNetCore.Authorization;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class CompanyInfoController : Controller
    {
        private ConicErpContext DB;
        public CompanyInfoController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        [Route("CompanyInfo/EditCompanyInfo")]
        [HttpPost]
        public IActionResult EditCompanyInfo(CompanyInfo collection)
        {
            if (collection != null)
            {
                CompanyInfo companyInfo = DB.CompanyInfos.Where(x => x.Id == 1).SingleOrDefault();
                if (companyInfo == null)
                    DB.Add(collection);
                else {
                    companyInfo.Name = collection.Name;
                    companyInfo.NickName = collection.NickName;
                    companyInfo.Logo = collection.Logo;
                    companyInfo.BusinessDescription = collection.BusinessDescription;
                    companyInfo.RateNumber = collection.RateNumber;
                    companyInfo.Address = collection.Address;
                    companyInfo.PhoneNumber1 = collection.PhoneNumber1;
                    companyInfo.PhoneNumber2 = collection.PhoneNumber2;
                    companyInfo.Fax = collection.Fax;
                    companyInfo.Email = collection.Email;
                    companyInfo.Website = collection.Website;
                }

                // TODO: Add update logic here
                DB.SaveChanges();
              
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [Route("CompanyInfo/GetCompanyInfo")]
        [HttpGet]
        public IActionResult GetCompanyInfo()
        {
            var companyInfo = DB.CompanyInfos.Where(x => x.Id == 1).SingleOrDefault();
                if(companyInfo == null)
                    return Ok();
                else
                    return Ok(companyInfo);

        }
    }
}