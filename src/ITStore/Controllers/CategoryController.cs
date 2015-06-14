using ITStore.Data;
using ITStore.Models;

using System;
using System.Linq;
using System.Web.Http;

namespace Service.Controllers
{
    public class CategoryController : ApiController
    {
        private ITStoreEntities _entities;
        // GET: Category
        public CategoryController()
        {
            _entities = new ITStoreEntities();
        }

        public CategoryController(ITStoreEntities entities)
        {
            _entities = entities;
        }
        ~CategoryController() 
        {
            Dispose(false);
        }

        public IHttpActionResult GetCategories()
        {
            try
            {
                return Ok(_entities.Category.ToList().Select(category => new CategoryDTO
                {
                    ID = category.ID,
                    Name = category.Name
                }));
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