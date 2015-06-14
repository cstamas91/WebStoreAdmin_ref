using System;
using ITStore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ITStore.Admin.Model
{
    public class ITStoreModel : IITStoreModel
    {
        private enum DataFlag
        {
            Created,
            CreatedAndUpdated,
            Updated,
            Deleted
        }

        private List<ProductDTO> _products;
        private Dictionary<ProductDTO, DataFlag> _flags;
        private List<CategoryDTO> _categories;
        private List<PurchaseDTO> _orders;
        private Dictionary<Int32,List<ProductForOrderDTO>> _selectedOrderProducts;
        private HttpClient _client;

        public ITStoreModel(String baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
        }

        public IDictionary<Int32,List<ProductForOrderDTO>> SelectedOrderProducts
        {
            get { return _selectedOrderProducts; }
        }

        public IList<PurchaseDTO> Orders
        {
            get { return _orders; }
        }

        public IList<CategoryDTO> Categories
        {
            get { return _categories; }
        }

        public IList<ProductDTO> Products
        {
            get { return _products; }
        }

        public void AddProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (_products.Contains(product))
                throw new ModelException();

            //product.ID = _products.Max(p => p.ID) + 1;
            _flags.Add(product, DataFlag.Created);
        }

        public void ModifyProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            ProductDTO productToModify = _products.FirstOrDefault(p => p.ID == product.ID);

            if (productToModify == null)
                throw new ModelException();

            productToModify.CategoryID = product.CategoryID;
            productToModify.Descr = product.Descr;
            productToModify.Manufacturer = product.Manufacturer;
            productToModify.Model = product.Model;
            productToModify.Price = product.Price;
            productToModify.PriceWithTax = product.PriceWithTax;
            productToModify.Stock = product.Stock;
            productToModify.IsActive = product.IsActive;

            if (_flags.ContainsKey(productToModify) && (_flags[productToModify] == DataFlag.Created || _flags[productToModify] == DataFlag.CreatedAndUpdated))
                _flags[productToModify] = DataFlag.CreatedAndUpdated;
            else
                _flags[productToModify] = DataFlag.Updated;
        }

        public void DeleteProduct(Int32 id)
        {
            ProductDTO productToDelete = _products.FirstOrDefault(p => p.ID == id);
            if (productToDelete == null)
                throw new ModelException();

            if (_flags.ContainsKey(productToDelete) && (_flags[productToDelete] == DataFlag.Created || _flags[productToDelete] == DataFlag.CreatedAndUpdated))
                _flags.Remove(productToDelete);
            else
                _flags[productToDelete] = DataFlag.Deleted;

            _products.Remove(productToDelete);
        }

        public async Task LoadOrdersAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/purchase/");
            if(response.IsSuccessStatusCode)
            {
                //String asd = (await response.Content.ReadAsStringAsync());
                _orders = (await response.Content.ReadAsAsync<IEnumerable<PurchaseDTO>>()).ToList();
                _selectedOrderProducts = new Dictionary<int, List<ProductForOrderDTO>>();
                foreach (PurchaseDTO purchase in _orders)
                {
                    List<ProductForOrderDTO> currentProductList = new List<ProductForOrderDTO>();
                    HttpResponseMessage productResponse = await _client.GetAsync("api/purchase/" + purchase.ID);
                    if (productResponse.IsSuccessStatusCode)
                    {
                        List<ProductForOrderDTO> temp = (await productResponse.Content.ReadAsAsync<IEnumerable<ProductForOrderDTO>>()).ToList();
                        _selectedOrderProducts.Add(purchase.ID, temp);
                    }
                }
            }
            else
            {
                throw new ModelException();
            }
        }

        public async Task LoadProductsForCategory(Int32 categoryID)
        {
            HttpResponseMessage response = await _client.GetAsync("api/products/" + categoryID);
            if(response.IsSuccessStatusCode)
            {
                _products = (await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>()).ToList();
            }
            else
            {
                throw new ModelException();
            }
        }

        public async Task LoadAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/products/");
            if (response.IsSuccessStatusCode)
            {
                _products = (await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>()).ToList();
            }
            else
            {
                throw new ModelException();
            }

            response = await _client.GetAsync("api/category/");
            if (response.IsSuccessStatusCode)
            {
                _categories = (await response.Content.ReadAsAsync<IEnumerable<CategoryDTO>>()).ToList();
            }
            else
            {
                throw new ModelException();
            }

            _flags = new Dictionary<ProductDTO, DataFlag>();
        }

        public async Task SaveAsync()
        {
            List<ProductDTO> productsToSave = _flags.Keys.ToList();

            foreach (ProductDTO product in productsToSave)
            {
                HttpResponseMessage response = null;

                switch (_flags[product])
                {
                    case DataFlag.Created:
                    case DataFlag.CreatedAndUpdated:
                        product.ID = 0;
                        response = await _client.PostAsJsonAsync("api/products/", product);
                        break;
                    case DataFlag.Deleted:
                        response = await _client.DeleteAsync("api/products/" + product.ID);
                        break;
                    case DataFlag.Updated:
                        response = await _client.PutAsJsonAsync("api/products/", product);
                        break;
                }

                if (response != null && !response.IsSuccessStatusCode)
                    throw new ModelException();

                if (response.StatusCode == HttpStatusCode.Created)
                    product.ID = (await response.Content.ReadAsAsync<ProductDTO>()).ID;

                _flags.Remove(product);
            }
        }

        public Boolean CheckOrder(Int32 orderID) 
        {

            foreach(ProductForOrderDTO p in _selectedOrderProducts[orderID])
            {
                if (p.Quantity > p.Product.Stock)
                    return false;
            }
            return true;
        }

        public async Task UpdateOrder(Int32 orderID)
        {

            HttpResponseMessage response = await _client.PutAsJsonAsync("api/purchase", orderID);
            if (response != null && !response.IsSuccessStatusCode)
                throw new ModelException();
            List<ProductForOrderDTO> productList = _selectedOrderProducts[orderID].ToList();
            foreach (ProductForOrderDTO dto in productList)
            {
                ProductDTO productDTO = dto.Product;
                productDTO.Stock -= dto.Quantity;

                response = await _client.PutAsJsonAsync("api/products", productDTO);
                if (response != null && !response.IsSuccessStatusCode)
                    throw new ModelException();
            }

        }

        public event EventHandler<ProductEventArgs> ProductChanged;

        public event EventHandler<OrderEventArgs> OrderChanged;
    }

}
