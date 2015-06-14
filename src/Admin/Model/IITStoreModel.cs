using System;
using System.Collections.Generic;
using ITStore.Data;
using System.Threading.Tasks;

namespace ITStore.Admin.Model
{
    public interface IITStoreModel
    {
        IList<CategoryDTO> Categories { get; }

        IList<ProductDTO> Products { get; }

        IList<PurchaseDTO> Orders { get; }

        IDictionary<Int32,List<ProductForOrderDTO>> SelectedOrderProducts { get; }

        event EventHandler<ProductEventArgs> ProductChanged;

        event EventHandler<OrderEventArgs> OrderChanged;


        void AddProduct(ProductDTO product);

        void ModifyProduct(ProductDTO product);

        void DeleteProduct(Int32 id);

        Boolean CheckOrder(Int32 orderID);

        Task UpdateOrder(Int32 orderID);

        Task LoadOrdersAsync();
        Task LoadProductsForCategory(Int32 categoryID);
        Task LoadAsync();

        Task SaveAsync();
    }
}
