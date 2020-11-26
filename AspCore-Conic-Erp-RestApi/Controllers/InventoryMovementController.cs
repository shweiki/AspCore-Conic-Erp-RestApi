using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]

    public class InventoryMovementController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
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
      
    }
}
