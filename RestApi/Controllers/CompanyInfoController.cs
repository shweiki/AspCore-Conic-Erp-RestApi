﻿
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[Authorize]

public class CompanyInfoController : Controller
{
    private readonly IApplicationDbContext DB;
    public CompanyInfoController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("CompanyInfo/EditCompanyInfo")]
    [HttpPost]
    public IActionResult EditCompanyInfo(CompanyInfo collection)
    {
        if (collection != null)
        {
            CompanyInfo companyInfo = DB.CompanyInfo.Where(x => x.Id == 1).SingleOrDefault();
            if (companyInfo == null)
                DB.CompanyInfo.Add(collection);
            else
            {
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
        var companyInfo = DB.CompanyInfo.Where(x => x.Id == 1).SingleOrDefault();
        if (companyInfo == null)
            return Ok();
        else
            return Ok(companyInfo);

    }
}