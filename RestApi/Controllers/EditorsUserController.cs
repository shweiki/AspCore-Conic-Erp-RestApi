
using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace RestApi.Controllers;

[Authorize]
public class EditorsUserController : Controller
{
    private readonly IApplicationDbContext DB;
    public EditorsUserController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    // GET: EditorsUsers
    [Route("EditorsUser/Get")]
    [HttpGet]
    public IActionResult Get()
    {
        var EditorsUsers = DB.EditorsUser.Select(x => new
        {
            x.Id,
            x.Name
        }).ToList();


        return Ok(EditorsUsers);

    }


    // PUT: EditorsUsers/5
    [Route("EditorsUser/Edit")]
    [HttpPost]
    public IActionResult Edit(EditorsUser collection)
    {
        if (ModelState.IsValid)
        {
            EditorsUser Editor = DB.EditorsUser.Where(x => x.Id == collection.Id).SingleOrDefault();
            Editor.Name = collection.Name;
            try
            {
                DB.SaveChanges();
                return Ok(true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        else return Ok(false);
    }

    // POST: EditorsUsers
    [Route("EditorsUser/Create")]
    [HttpPost]
    public IActionResult Create(EditorsUser collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // TODO: Add insert logic here
                DB.EditorsUser.Add(collection);
                DB.SaveChanges();

                return Ok(true);

            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }


}




