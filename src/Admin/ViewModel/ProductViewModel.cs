using ITStore.Admin.Model;
using ITStore.Data;
using System;
using System.Collections.ObjectModel;

namespace ITStore.Admin.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private IITStoreModel _model;
        private ObservableCollection<ProductDTO> _products;
        private ProductDTO _currentProduct;
        private Boolean _isLoaded;
        private Int32 _selectedIndex;

        public ObservableCollection<ProductDTO> Products
        {
            get { return _products; }
            private set
            {
                if(_products != value)
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProductDTO CurrentProduct
        {
            get { return _currentProduct; }
            private set
            {
                if(_currentProduct != value)
                {
                    _currentProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if(_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 SelectedIndex
        {
            get { return _selectedIndex; }
            private set
            {
                if(_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();

                    if (_selectedIndex >= 0 && _selectedIndex < _products.Count)
                        CurrentProduct = new ProductDTO
                        {

                            ID = _products[_selectedIndex].ID,
                            Manufacturer = _products[_selectedIndex].Manufacturer,
                            Model = _products[_selectedIndex].Model,
                            CategoryID = _products[_selectedIndex].CategoryID,
                            Descr = _products[_selectedIndex].Descr,
                            Price = _products[_selectedIndex].Price,
                            PriceWithTax = _products[_selectedIndex].PriceWithTax,
                            Stock = _products[_selectedIndex].Stock,
                            IsActive = _products[_selectedIndex].IsActive
                        };
                }
            }
        }

        public DelegateCommand AddProductCommand { get; private set; }
        public DelegateCommand DeleteProductCommand { get; private set; }

        public DelegateCommand ModifyProductCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler<MessageEventArgs> MessageApplication;
        public event EventHandler ExitApplication;

        public ProductViewModel(IITStoreModel model)
        {
            _model = model;
            _isLoaded = false;
            _selectedIndex = -1;

            AddProductCommand = new DelegateCommand(param =>
            {
                _model.AddProduct(CurrentProduct);
                Products.Add(CurrentProduct);
                CurrentProduct = new ProductDTO();

            });

            DeleteProductCommand = new DelegateCommand(param =>
            {
                _model.DeleteProduct(CurrentProduct.ID);
                Products.RemoveAt(SelectedIndex);
            });

            LoadCommand = new DelegateCommand(async (param) =>
            {
                try
                {
                    await _model.LoadAsync();
                    Products = new ObservableCollection<ProductDTO>(_model.Products);
                    SelectedIndex = -1;
                    IsLoaded = true;
                }
                catch
                {
                    OnMessageApplication("A betötés sikertelen!");
                }
            });

            SaveCommand = new DelegateCommand(async (param) =>
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

            ModifyProductCommand = new DelegateCommand(param =>
            {
                Int32 index = SelectedIndex;

                Products.RemoveAt(index);
                _model.ModifyProduct(CurrentProduct);
                Products.Insert(index, _model.Products[index]);
            });
        }
        private void OnMessageApplication(String message)
        {
            if (MessageApplication != null)
                MessageApplication(this, new MessageEventArgs { Message = message });
        }

    }
}
