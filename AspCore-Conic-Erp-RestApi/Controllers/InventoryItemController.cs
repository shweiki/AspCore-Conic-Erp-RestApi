﻿using System.Data;
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
                    DB.InventoryItems.Add(collection);
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
            var InventoryItems = DB.InventoryItems.Select(x => new {
                x.Id,
                x.Name,
                x.Description,
                Items = DB.InventoryMovements.Where(o => o.InventoryItemId == x.Id).GroupBy(s => s.ItemsId).ToList()
                /*
                .Select(a=> new{
                    Item = DB.Items.Where(i=>i.Id == a.Key).Select(ii=>new {
                        ii.Id,
                        ii.CostPrice,
                        ii.Name,
                       ii.Barcode
                    }).FirstOrDefault(), 
                    QtyIn = a.Where(d => d.TypeMove == "In").Sum(qc => qc.Qty),
                    QtyOut = a.Where(d => d.TypeMove == "Out").Sum(qc => qc.Qty)
                }).ToList()*/
            }).ToList();

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
