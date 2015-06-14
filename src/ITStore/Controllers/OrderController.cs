using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITStore.Models;

namespace ITStore.Controllers
{
    public class OrderController : BaseController
    {

        public ActionResult Error(String message)
        {
            return View("Error", message);
        }
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult Index([Bind(Prefix="Item2")] CheckoutModel checkout)
        {
            if (!ModelState.IsValid)
                return View("Cart");

            if(checkout == null)
            {
                return RedirectToAction("Cart", "Order");
            }

            //Lekérjük a kosár tartalmát:
            var cart = HttpContext.Session["ShoppingCart"] as ShoppingCart;
            //Konstruálunk egy listát a kosár tartalmából:
            ProductList products = new ProductList();
            foreach (int pID in cart.Items.Keys)
            {
                Product product = _entities.Product.Include("Category").Where(p => p.ID == pID).FirstOrDefault();
                product.Quantity = cart.Items[pID];
                products.Add(product);
            }

            //TODO: product adatok ellenőrzése! és azok frissítése!
            try
            {
                _entities.Purchases.Add(new Purchases
                {
                    Name = checkout.CustomerName,
                    Phone = checkout.CustomerPhoneNumber,
                    Addr = checkout.CustomerAddress,
                    Email = checkout.CustomerEmail,
                    Completion = false,
                    Datum = DateTime.Now.Date
                });
                _entities.SaveChanges();
                int pID = _entities.Purchases.Max(p => p.ID);
                foreach(Product product in products)
                {
                    _entities.PurchaseToProduct.Add(new PurchaseToProduct
                    {
                       PurchaseID = pID,
                       ProductID = product.ID,
                       Quantity = product.Quantity
                    });
                    _entities.SaveChanges();
                }
                (HttpContext.Session["ShoppingCart"] as ShoppingCart).ClearProducts();
                return View("OrderComplete");
            }
            catch
            {
                return RedirectToAction("Error", "Hiba történt a rendelés megerősítésekor, a kosár ki lett ürítve.");
            }
            
        }

        public ActionResult ClearCart()
        {
            (HttpContext.Session["ShoppingCart"] as ShoppingCart).ClearProducts();
            return RedirectToAction("Cart");
        }

        public ActionResult AddToCart(Int32? productID, Int32? quantity, Int32? categoryID, String sortOrder, Int32? page){
            (HttpContext.Session["ShoppingCart"] as ShoppingCart).AddProduct((int)productID, (int)quantity);
            return RedirectToAction("Index", "Home", new { categoryID = categoryID, sortOrder = sortOrder, page = page});
        }

        public ActionResult ChangeQuantity(Int32? productID, Int32? quantity)
        {
            if (quantity > _entities.Product.FirstOrDefault(p => p.ID == productID).Stock)
                return View("Error", "Nem lehet teljesíteni a kérést, nincs ennyi eszköz raktáron");
            (HttpContext.Session["ShoppingCart"] as ShoppingCart).ChangeQuantity((int)productID, (int)quantity);
            return RedirectToAction("Cart");
        }

        public ActionResult Cart()
        {
            var cart = HttpContext.Session["ShoppingCart"] as ShoppingCart;
            ProductList products = new ProductList();
            foreach(int pID in cart.Items.Keys)
            {
                Product product = _entities.Product.Include("Category").Where(p => p.ID == pID).FirstOrDefault();
                product.Quantity = cart.Items[pID];
                products.Add(product);
            }
            return View("Cart", new Tuple<ProductList, CheckoutModel>(products, new CheckoutModel()));
        }
    }
}