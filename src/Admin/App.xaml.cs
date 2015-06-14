using System;
using System.Windows;
using ITStore.Admin.Model;
using ITStore.Admin.View;
using ITStore.Admin.ViewModel;

namespace ITStore.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IITStoreModel _model;
        private MainViewModel _viewModel;
        private MainWindow _view;
        private ProductEditorWindow _editorView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new ITStoreModel("http://localhost:49588/");
            _viewModel = new MainViewModel(_model);
            _viewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _viewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _viewModel.ProductEditingStarted += new EventHandler(MainViewModel_ProductEditingStarted);
            _viewModel.ProductEditingFinished += new EventHandler(MainViewModel_ProductEditingFinished);
            _viewModel.OrderCompletionPossible += new EventHandler<MessageEventArgs>(ViewModel_OrderPossible);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
        }

        private void MainViewModel_ProductEditingStarted(object sender, EventArgs e)
        {
            _editorView = new ProductEditorWindow();
            _editorView.DataContext = _viewModel;
            _editorView.Show();
        }

        private void MainViewModel_ProductEditingFinished(object sender, EventArgs e)
        {
            _editorView.Close();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "IT Store", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_OrderPossible(object sender, MessageEventArgs e)
        {
            if (MessageBox.Show(e.Message, "IT Store", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _viewModel.CompleteOrder();
            }
        }

        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {

            Shutdown();
        }

    }
}
