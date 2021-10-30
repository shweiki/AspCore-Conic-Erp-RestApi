using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities; 

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class EditorsUserController : Controller
    {
        private ConicErpContext DB;
        public EditorsUserController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
        // GET: EditorsUsers
        [Route("EditorsUser/Get")]
        [HttpGet]
        public IActionResult Get()
        {
            var EditorsUsers = DB.EditorsUsers.Select(x => new
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
                EditorsUser Editor = DB.EditorsUsers.Where(x => x.Id == collection.Id).SingleOrDefault();
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
                    DB.EditorsUsers.Add(collection);
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
}




