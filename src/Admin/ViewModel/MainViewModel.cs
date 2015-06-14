using ITStore.Admin.Model;
using ITStore.Data;
using System.IO;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace ITStore.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private IITStoreModel _model;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<CategoryDTO> _categories;
        private ObservableCollection<PurchaseDTO> _orders;
        private ObservableCollection<ProductForOrderDTO> _selectedOrderProducts;
        private ProductDTO _selectedProduct;
        private PurchaseDTO _selectedOrder;
        private Boolean _productIsLoaded;
        private Boolean _orderIsLoaded;
        private Int32 _selectedProductIndex;
        private Int32 _selectedOrderIndex;
        private Int32 _selectedCategoryIndex;
        private String _nameFilterString;
        private Boolean _completionFilter;
        private DateTime _dateFilter;


        public String NameFilterString
        {
            get { return _nameFilterString; }
            set
            {
                if (_nameFilterString != value)
                    _nameFilterString = value;
                Orders = new ObservableCollection<PurchaseDTO>(_model.Orders.Where(order => order.Name.Contains((_nameFilterString == null) ? "" : _nameFilterString) && order.Completion == _completionFilter && order.Datum == _dateFilter));
            }
        }

        public Boolean CompletionFilter
        {
            get { return _completionFilter; }
            set
            {
                if(_completionFilter != value)
                    _completionFilter = value;
                Orders = new ObservableCollection<PurchaseDTO>(_model.Orders.Where(order => order.Name.Contains((_nameFilterString == null) ? "" : _nameFilterString) && order.Completion == _completionFilter && order.Datum == _dateFilter));

            }
        }

        public DateTime DateFilter
        {
            get { return _dateFilter.Date; }
            set
            {
                if (_dateFilter != value)
                    _dateFilter = value;
                Orders = new ObservableCollection<PurchaseDTO>(_model.Orders.Where(order => order.Name.Contains((_nameFilterString == null) ? "" : _nameFilterString) && order.Completion == _completionFilter && order.Datum == _dateFilter));
            }
        }

        public Int32 SelectedCategoryIndex
        {
            get { return _selectedCategoryIndex; }
            private set
            {
                if(_selectedCategoryIndex != value)
                {
                    _selectedCategoryIndex = value;
                    Products = (_selectedCategoryIndex != -1) ? new ObservableCollection<ProductDTO>(_model.Products.Where(product => product.CategoryID == Categories[_selectedCategoryIndex].ID)) : new ObservableCollection<ProductDTO>(_model.Products);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ProductForOrderDTO> SelectedOrderProducts
        {
            get { return _selectedOrderProducts; }
            private set
            {
                if(_selectedOrderProducts != value)
                {
                    _selectedOrderProducts = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<ProductDTO> Products
        {
            get { return _products; }
            private set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<CategoryDTO> Categories
        {
            get { return _categories; }
            private set
            {
                if(_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<PurchaseDTO> Orders
        {
            get { return _orders; }
            private set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProductDTO EditedProduct { get; private set; }

        public ProductDTO SelectedProduct
        {
            get { return _selectedProduct; }
            private set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        public PurchaseDTO SelectedOrder
        {
            get { return _selectedOrder; }
            private set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean ProductIsLoaded
        {
            get { return _productIsLoaded; }
            private set
            {
                if(_productIsLoaded != value)
                {
                    _productIsLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean OrderIsLoaded
        {
            get { return _orderIsLoaded; }
            private set
            {
                if(_orderIsLoaded != value)
                {
                    _orderIsLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 SelectedProductIndex
        {
            get { return _selectedProductIndex; }
            private set
            {
                if (_selectedProductIndex != value)
                {
                    _selectedProductIndex = value;
                    OnPropertyChanged();

                    if (_selectedProductIndex >= 0 && _selectedProductIndex < _products.Count)
                        SelectedProduct = new ProductDTO
                        {
                            ID = _products[_selectedProductIndex].ID,
                            Manufacturer = _products[_selectedProductIndex].Manufacturer,
                            Model = _products[_selectedProductIndex].Model,
                            Descr = _products[_selectedProductIndex].Descr,
                            Price = _products[_selectedProductIndex].Price,
                            PriceWithTax = _products[_selectedProductIndex].PriceWithTax,
                            IsActive = _products[_selectedProductIndex].IsActive,
                            CategoryID = _products[_selectedProductIndex].CategoryID,
                            Stock = _products[_selectedProductIndex].Stock
                        };
                }
            }
        }

        public Int32 SelectedOrderIndex
        {
            get { return _selectedOrderIndex; }
            private set
            {
                if(_selectedOrderIndex != value)
                {
                    _selectedOrderIndex = value;
                    OnPropertyChanged();

                    if (_selectedOrderIndex >= 0 && _selectedOrderIndex < _orders.Count)
                    {
                        SelectedOrder = new PurchaseDTO
                        {
                            Addr = _orders[_selectedOrderIndex].Addr,
                            Completion = _orders[_selectedOrderIndex].Completion,
                            Datum = _orders[_selectedOrderIndex].Datum,
                            Email = _orders[_selectedOrderIndex].Email,
                            ID = _orders[_selectedOrderIndex].ID,
                            Name = _orders[_selectedOrderIndex].Name,
                            Phone = _orders[_selectedOrderIndex].Phone
                        };
                        SelectedOrderProducts = new ObservableCollection<ProductForOrderDTO>(_model.SelectedOrderProducts[_orders[_selectedOrderIndex].ID]);
                        if (SelectedOrder.Completion == false  && _model.CheckOrder(_orders[_selectedOrderIndex].ID))
                        {
                            OnOrderCompletionPossible("A rendelés teljesíthető, nyomj Igent, ha szeretnéd végrehajtani.");
                        }
                    }
                }
            }
        }

        public DelegateCommand CreateProductCommand { get; private set; }
        public DelegateCommand CancelChangesCommand { get; private set; }
        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand AddProductCommand { get; private set; }
        public DelegateCommand DeleteProductCommand { get; private set; }
        public DelegateCommand ModifyProductCommand { get; private set; }
        public DelegateCommand LoadProductCommand { get; private set; }
        public DelegateCommand LoadOrdersCommand { get; private set; }
        public DelegateCommand SaveProductCommand { get; private set; }


        public event EventHandler<MessageEventArgs> OrderCompletionPossible;
        public event EventHandler ProductEditingStarted;
        public event EventHandler ProductEditingFinished;


        public event EventHandler<MessageEventArgs> MessageApplication;
        public event EventHandler ExitApplication;

        public void CompleteOrder()
        {

            SelectedOrder.Completion = true;
            _model.UpdateOrder(SelectedOrder.ID);
            OnPropertyChanged("SelectedOrder");
        }

        public MainViewModel(IITStoreModel model)
        {
            _dateFilter = new DateTime(2015, 1, 1);
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            _model.ProductChanged += Model_ProductChanged;
            _model.OrderChanged += Model_OrderChanged;
            _productIsLoaded = false;
            _orderIsLoaded = false;

            AddProductCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();
                OnProductEditingStarted();
            });

            ModifyProductCommand = new DelegateCommand(param =>
            {
                ProductDTO selectedProduct = param as ProductDTO;

                EditedProduct = new ProductDTO
                {
                    Category = selectedProduct.Category,
                    ID = selectedProduct.ID,
                    Model = selectedProduct.Model,
                    Manufacturer = selectedProduct.Manufacturer,
                    Price = selectedProduct.Price,
                    Descr = selectedProduct.Descr,
                    Stock = selectedProduct.Stock,
                    PriceWithTax = selectedProduct.PriceWithTax,
                    CategoryID = selectedProduct.CategoryID,
                    IsActive = selectedProduct.IsActive
                };

                OnProductEditingStarted();
            });

            SaveChangesCommand = new DelegateCommand(param =>
            {
                if (String.IsNullOrEmpty(EditedProduct.Manufacturer) || String.IsNullOrEmpty(EditedProduct.Model) || String.IsNullOrEmpty(EditedProduct.Descr))
                {
                    OnMessageApplication("Valamely mező nincs kitöltve!");
                    return;
                }
                if (EditedProduct.Category == null)
                {
                    OnMessageApplication("A kategória nincs megadva!");
                    return;
                }

                if (EditedProduct.ID == 0)
                {
                    _model.AddProduct(EditedProduct);
                    //Products.Add(EditedProduct);
                }
                else
                {
                    _model.ModifyProduct(EditedProduct);
                }
                Products[_selectedProductIndex] = EditedProduct;
                OnPropertyChanged("Products");
                EditedProduct = null;

                OnProductEditingFinished();
            });

            CreateProductCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();
                OnProductEditingStarted();
            });

            CancelChangesCommand = new DelegateCommand(param =>
            {
                EditedProduct = null;
                OnProductEditingFinished();
            });

            DeleteProductCommand = new DelegateCommand(param =>
            {
                Products.Remove(param as ProductDTO);
                _model.DeleteProduct((param as ProductDTO).ID);
            });

            LoadOrdersCommand = new DelegateCommand(async (param) =>
            {
                try
                {
                    await _model.LoadOrdersAsync();
                    Orders = new ObservableCollection<PurchaseDTO>(_model.Orders);
                    SelectedOrderIndex = -1;
                    OrderIsLoaded = true;
                }
                catch
                {
                    OnMessageApplication("A betöltés sikertelen!");
                }
            });

            LoadProductCommand = new DelegateCommand(async (param) =>
            {
                try
                {
                    await _model.LoadAsync();
                    Products = new ObservableCollection<ProductDTO>(_model.Products);
                    Categories = new ObservableCollection<CategoryDTO>(_model.Categories);
                    ProductIsLoaded = true;
                }
                catch
                {
                    OnMessageApplication("A betöltés sikertelen!");
                }
            });

            SaveProductCommand = new DelegateCommand(async (param) =>
            {
                try
                {
                    await _model.SaveAsync();
                    OnMessageApplication("A mentés sikeres!");
                }
                catch
                {
                    OnMessageApplication("A mentés sikertelen!");
                }
            });

            ExitCommand = new DelegateCommand(param => OnExitApplication());

        }

        private void OnProductEditingStarted()
        {
            if (ProductEditingStarted != null)
                ProductEditingStarted(this, EventArgs.Empty);
        }

        private void OnProductEditingFinished()
        {
            if (ProductEditingFinished != null)
                ProductEditingFinished(this, EventArgs.Empty);
        }

        private void Model_ProductChanged(object sender, ProductEventArgs e)
        {
            Int32 index = Products.IndexOf(Products.FirstOrDefault(product => product.ID == e.ProductID));
            Products.RemoveAt(index);
            Products.Insert(index, _model.Products[index]);

            SelectedProduct = Products[index];
        }

        private void Model_OrderChanged(object sender, OrderEventArgs e)
        {
            Int32 index = Orders.IndexOf(Orders.FirstOrDefault(order => order.ID == e.OrderID));
            Orders.RemoveAt(index);
            Orders.Insert(index, _model.Orders[index]);

            SelectedOrder = Orders[index];
        }

        public DelegateCommand ExitCommand { get; private set; }

        private void OnMessageApplication(String message)
        {
            if (MessageApplication != null)
                MessageApplication(this, new MessageEventArgs { Message = message });
        }

        private void OnOrderCompletionPossible(String message)
        {
            if (OrderCompletionPossible != null)
                OrderCompletionPossible(this, new MessageEventArgs { Message = message });
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }
    }

}
