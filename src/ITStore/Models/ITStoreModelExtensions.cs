using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;
using ITStore.Controllers;

namespace ITStore.Models
{
    public class TaxMultiplier
    {
        public const double Value = 1.27;
    }

    public partial class Product
    {
        public double PriceWithTax {get {return Price * TaxMultiplier.Value;}}
        public int Quantity { get; set; }
    }

    public class ShoppingCart
    {
        public Dictionary<int, int> Items { get; private set; }

        public ShoppingCart() 
        {
            Items = new Dictionary<int, int>();
        }

        public void SetProductQuantity(int productID, int quantity)
        {
            Items[productID] = quantity;
        }

        internal void AddProduct(int productID, int quantity)
        {

            try
            {
                Items.Add(productID, quantity);
            }
            catch (ArgumentException)
            {
                Items[productID] += quantity;
            }
        }

        internal void ChangeQuantity(int productID, int quantity)
        {
            if (quantity == 0)
                Items.Remove(productID);
            else
                Items[productID] = quantity;
        }

        internal void ClearProducts()
        {
            List<int> idList = new List<int>();
            foreach (int id in Items.Keys)
            {
                idList.Add(id);
            }
            foreach (int id in idList)
                Items.Remove(id);
        }
    }

    public class ProductList : List<Product>
    {
        public double SumTotal
        {
            get
            {
                double returnVal = 0;
                foreach (Product p in this)
                {
                    returnVal += p.Price * p.Quantity;
                }
                return returnVal;
            }
        }

        public double SumTotalWithTax
        {
            get
            {
                return SumTotal * TaxMultiplier.Value;
            }
        }
    }
}