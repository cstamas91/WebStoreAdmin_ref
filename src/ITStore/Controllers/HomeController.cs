using System;
using System.Linq;
using System.Web.Mvc;
using ITStore.Models;
using System.Web;
using System.Web.Routing;
using PagedList;

namespace ITStore.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home   
        public ActionResult Index(Int32? categoryID, String sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ManufacturerSortParam = String.IsNullOrEmpty(sortOrder) ? "manufacturer_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CategoryID = categoryID;
            ViewBag.SortOrder = sortOrder;

            var products = _entities.Product.Include("Category").Where(p => (categoryID == null || categoryID == 0) ? true : p.CategoryID == categoryID).AsEnumerable();
            products = products.Where(p => p.IsActive || p.Stock > 0);

            switch(sortOrder)
            {
                case "manufacturer_desc":
                    products = products.OrderByDescending(p => p.Model);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Model);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View("Index", products.ToPagedList(pageNumber, pageSize));

        }
    }
}