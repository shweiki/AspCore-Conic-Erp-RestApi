using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;


namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class InventoryItemController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();

        [Route("InventoryItem/Create")]
        [HttpPost]
        public IActionResult Create(InventoryItem collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.Status = 0;
                    DB.InventoryItems.Add(collection);
                    DB.SaveChanges();
                    Oprationsy Opx = DB.Oprationsys.Where(d => d.Status == collection.Status && d.TableName == "InventoryItem").SingleOrDefault();
                    OprationsysController Op = new OprationsysController();
                    if (Op.ChangeStatus(collection.Id, Opx.Id, "<!" + collection.Id + "!>"))
                    {
                        return Ok(true);
                    }
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        [Route("InventoryItem/Edit")]
        [HttpPost]
        public IActionResult Edit(InventoryItem collection)
        {
            if (ModelState.IsValid)
            {
                InventoryItem InventoryItem = DB.InventoryItems.Where(x => x.Id == collection.Id).SingleOrDefault();
                InventoryItem.Name = collection.Name;
                InventoryItem.Description = collection.Description;
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
            return Ok(false);
        }

        [Route("InventoryItem/GetInventoryItem")]
        [HttpGet]
        public IActionResult GetInventoryItem()
        {
            var InventoryItems = (from x in DB.InventoryItems.ToList()
                             select new
                             {
                                 x.Id,
                                 x.Name,
                                 x.Description,
                                 Items =(from i in DB.InventoryMovements.Where(o => o.InventoryItemId == x.Id && o.Status == 0).ToList()
                                         group i by i.ItemsId into g
                                         select new
                                         {
                                             Item = (from a in DB.Items.ToList()
                                                     where (a.Id == g.Key) 
                                                     select new
                                                     {
                                                         a.Id,
                                                       a.Name,
                                                       a.Barcode
                                                     }).FirstOrDefault(),
                                             QtyIn = g.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                                             QtyOut = g.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                                         }).ToList(),
                          
                             });
            return Ok(InventoryItems);
        }

        [Route("InventoryItem/GetActiveInventory")]
        [HttpGet]
        public IActionResult GetActiveInventory()
        {
            var InventoryItems = DB.InventoryItems.Where(x => x.Status == 0).Select(x => new { value = x.Id, label = x.Name }).ToList();
                return Ok(InventoryItems);

        }

    }
}
