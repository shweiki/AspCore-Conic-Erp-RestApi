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
                private ConicErpContext DB;
                        private readonly IUnitOfWork UW;

        public MenuItemController(ConicErpContext dbcontext,IUnitOfWork unitOfWork)
        {
            DB = dbcontext;
                        UW = unitOfWork;

        }

        [Route("MenuItem/GetMenuItem")]
        [HttpGet]
        public IActionResult GetMenuItem()
        {
            var MenuItems = DB.MenuItems.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.IsPrime,
                x.Status,
            }).ToList();

            return Ok(MenuItems);
        }

        [Route("MenuItem/GetActiveMenuItem")]
        [HttpGet]
        public IActionResult GetActiveMenuItem()
        {
            
            var MenuItems = DB.MenuItems.Where(x => x.Status == 0).Select(x => new { value = x.Name, label = x.Name }).ToList();
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
                    DB.MenuItems.Add(collection);
                    DB.SaveChanges();
                    return Ok(collection);

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