using System;
using ITStore.Data;
using ITStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ITStore.Models.Controllers
{
    //[RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private ITStoreEntities _entities;

        public ProductsController()
        {
            _entities = new ITStoreEntities();
        }

        public ProductsController(ITStoreEntities entities)
        {
            _entities = entities;
        }

        ~ProductsController()
        {
            Dispose(false);
        }
        //[Route("")]
        public IHttpActionResult GetProducts()
        {
            
            try
            {
                return Ok(_entities.Product.ToList().Select(product => new ProductDTO
                {
                    ID = product.ID,
                    Category = new CategoryDTO 
                    {
                        ID = product.Category.ID,
                        Name = product.Category.Name
                    },
                    Manufacturer = product.Manufacturer,
                    Model = product.Model,
                    Descr = product.Descr,
                    Price = product.Price,
                    PriceWithTax = product.PriceWithTax,
                    CategoryID = product.CategoryID,
                    IsActive = product.IsActive,
                    Stock = product.Stock
                }));
            }
            catch
            {
                return InternalServerError();
            }
        }
        //[Route("{categoryId:int}")]
        public IHttpActionResult GetProducts(Int32 categoryId)
        {
            try
            {
                return Ok(_entities.Product.Include("Category").Where(p => p.CategoryID == categoryId).ToList().Select(product => new ProductDTO
                {
                    ID = product.ID,
                    Category = new CategoryDTO 
                    {
                        ID = product.Category.ID,
                        Name = product.Category.Name
                    },
                    Manufacturer = product.Manufacturer,
                    Model = product.Model,
                    Descr = product.Descr,
                    Price = product.Price,
                    PriceWithTax = product.PriceWithTax,
                    Stock = product.Stock,
                    IsActive = product.IsActive,
                    CategoryID = product.CategoryID
                }));
            }
            catch
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult PostProducts([FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO.ID != 0)
                {
                    Product product = _entities.Product.FirstOrDefault(p => p.ID == productDTO.ID);
                    if (product != null)
                        return Conflict();
                }
                Category categ = _entities.Category.FirstOrDefault(c => c.ID == productDTO.Category.ID);
                _entities.Product.Add(new Product
                {
                    //ID = productDTO.ID,
                    Category = categ,
                    Manufacturer = productDTO.Manufacturer,
                    Model = productDTO.Model,
                    Descr = productDTO.Descr,
                    Price = productDTO.Price,
                    Stock = productDTO.Stock,
                    IsActive = productDTO.IsActive,
                    CategoryID = categ.ID
                });

                _entities.SaveChanges();

                Product savedProduct = _entities.Product.ToList().Last();
                productDTO.ID = savedProduct.ID;

                return Created(Request.RequestUri + productDTO.ID.ToString(), productDTO);
            }
            catch
            {
                return InternalServerError();
            }
        }
        //[Route("{productDTO}")]
        public IHttpActionResult PutProducts([FromBody]  ProductDTO productDTO)
        {
            try
            {
                Product product = _entities.Product.FirstOrDefault(p => p.ID == productDTO.ID);
                Category categ = _entities.Category.FirstOrDefault(c => c.ID == productDTO.Category.ID);

                if (product == null)
                    return NotFound();
                product.Category = categ;
                product.Manufacturer = productDTO.Manufacturer;
                product.Model = productDTO.Model;
                product.Descr = productDTO.Descr;
                product.Price = productDTO.Price;
                product.Stock = productDTO.Stock;
                product.IsActive = productDTO.IsActive;
                product.CategoryID = productDTO.CategoryID;

                _entities.SaveChanges();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        //public IHttpActionResult PutProducts([FromBody] ProductForOrderDTO productForOrderDTO)
        //{
        //    try
        //    {
        //        Product product = _entities.Product.FirstOrDefault(p => p.ID == productForOrderDTO.Product.ID);
        //        if (product == null)
        //        {
        //            return NotFound();
        //        }
        //        product.Stock -= productForOrderDTO.Quantity;
        //        _entities.SaveChanges();
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return InternalServerError();
        //    }
        //}


        public IHttpActionResult DeleteProduct(Int32 id)
        {
            try
            {
                Product product = _entities.Product.FirstOrDefault(p => p.ID == id);

                if (product == null)
                    return NotFound();

                _entities.Product.Remove(product);
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
            if (disposing)
            {
                _entities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}