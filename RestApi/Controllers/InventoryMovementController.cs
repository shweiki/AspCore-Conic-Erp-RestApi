﻿using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RestApi.Controllers;

[Authorize]

public class InventoryMovementController : Controller
{
    private ConicErpContext DB;
    public InventoryMovementController(ConicErpContext dbcontext)
    {
        DB = dbcontext;
    }
    public Boolean Create(IList<InventoryMovement> RepoMoves)
    {
        if (RepoMoves != null)
        {
            var fadd = from field in RepoMoves
                       select new InventoryMovement
                       {
                           ItemsId = field.ItemsId,
                           TypeMove = field.TypeMove,
                           Qty = field.Qty,
                           SellingPrice = field.SellingPrice,
                           InventoryItemId = field.InventoryItemId
                       };
            DB.InventoryMovements.AddRange(fadd);
            DB.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }
    [Route("InventoryMovement/GetInventoryMovementsBySalesInvoiceId")]
    [HttpGet]
    public IActionResult GetInventoryMovementsBySalesInvoiceId(long? SalesInvoiceId)
    {
        var InventoryMovements = DB.InventoryMovements.Where(im => im.SalesInvoiceId == SalesInvoiceId).Select(imx => new
        {
            imx.Id,
            imx.ItemsId,
            imx.Items.Name,
            imx.InventoryItemId,
            imx.Qty,
            imx.SellingPrice,
            imx.Description
        }).ToList();


        return Ok(InventoryMovements);
    }


}
