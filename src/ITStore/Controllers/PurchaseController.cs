using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using ITStore.Data;
using ITStore.Models;

namespace ITStore.Models.Controllers
{
    public class PurchaseController : ApiController
    {
        private ITStoreEntities _entities;

        public PurchaseController()
        {
            _entities = new ITStoreEntities();
        }

        public PurchaseController(ITStoreEntities entities)
        {
            _entities = entities;
        }

        ~PurchaseController()
        {
            Dispose(false);
        }

        public IHttpActionResult GetPurchase(Int32 id)
        {
            Purchases purchase = _entities.Purchases.FirstOrDefault(p => p.ID == id);
            try
            {
                return Ok(purchase.PurchaseToProduct.ToList().Select(p => new ProductForOrderDTO
                {
                    Product = new ProductDTO
                    {
                        ID = p.Product.ID,
                        Category = new CategoryDTO
                        {
                            ID = p.Product.Category.ID,
                            Name = p.Product.Category.Name
                        },
                        Model = p.Product.Model,
                        Manufacturer = p.Product.Manufacturer,
                        Descr = p.Product.Descr,
                        Price = p.Product.Price,
                        PriceWithTax = p.Product.PriceWithTax,
                        CategoryID = p.Product.CategoryID,
                        Stock = p.Product.Stock,
                        IsActive = p.Product.IsActive
                    },
                    Quantity = p.Quantity
                }));
            }
            catch
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult GetPurchases()
        {
            try
            {
                return Ok(_entities.Purchases.ToList().Select(purchase => new PurchaseDTO
                {
                    ID = purchase.ID,
                    Name = purchase.Name,
                    Addr = purchase.Addr,
                    Email = purchase.Email,
                    Phone = purchase.Phone,
                    Completion = purchase.Completion,
                    Datum = purchase.Datum
                }));
            }
            catch
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult PutPurchase([FromBody] Int32 orderID)
        {
            try
            {
                Purchases purchase = _entities.Purchases.FirstOrDefault(p => p.ID == orderID);

                if (purchase == null)
                    return NotFound();

                purchase.Completion = true;

                _entities.SaveChanges();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _entities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}