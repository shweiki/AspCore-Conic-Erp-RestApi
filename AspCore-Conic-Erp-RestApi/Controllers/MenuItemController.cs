using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class MenuItemController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("MenuItem/GetMenuItem")]
        [HttpGet]
        public IActionResult GetMenuItem()
        {
            var MenuItems = (from x in DB.MenuItems.ToList()
                              select new
                              {
                                  x.Id,
                                  x.Name,
                                  x.Description,
                                  x.IsPrime,
                        
                              });

            return Ok(MenuItems);
        }

        [Route("MenuItem/GetActiveMenuItem")]
        [HttpGet]
        public IActionResult GetActiveMenuItem()
        {
            var MenuItems = DB.MenuItems.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
            return Ok(MenuItems);
        }


        [Route("MenuItem/Create")]
        [HttpPost]
        public IActionResult Create(MenuItem collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.Status = 0;
                    DB.MenuItems.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "MenuItem").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return NotFound();
                }
            }
            return NotFound();
        }

        [Route("MenuItem/Edit")]
        [HttpPost]
        public IActionResult Edit(MenuItem collection)
        {
            if (ModelState.IsValid)
            {
                MenuItem MenuItem = DB.MenuItems.Where(x => x.Id == collection.Id).SingleOrDefault();
                MenuItem.Name = collection.Name;
                MenuItem.Description = collection.Description;
                try
                {
                    DB.SaveChanges();
                    return Ok();
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return NotFound();
                }
            }
            return NotFound();
        }

    }
}