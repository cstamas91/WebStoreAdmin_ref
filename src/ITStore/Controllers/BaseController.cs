using ITStore.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ITStore.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ITStoreEntities _entities;
        public BaseController()
        {
            _entities = new ITStoreEntities();
            ViewBag.Categories = _entities.Category.ToArray();
        }
    }
}