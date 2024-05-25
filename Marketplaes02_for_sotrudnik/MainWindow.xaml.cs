using Marketplaes02_for_sotrudnik.Model;
using Marketplaes02_for_sotrudnik.View;
using Marketplaes02_for_sotrudnik.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Marketplaes02_for_sotrudnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Window
             window;
        public MainWindow(Window win)
        {
            InitializeComponent();
            window = win;
            ViewGoods viewModelGoods = new ViewGoods();
            Frame1.NavigationService.Navigate(viewModelGoods);


        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void OpenPageOrders(object sender, MouseButtonEventArgs e)
        {
            ViewOrder viewOrder = new ViewOrder();
            Frame1.NavigationService.Navigate(viewOrder);
          
        }

        private void OpenPageGoods(object sender, MouseButtonEventArgs e)
        {
            ViewGoods viewModelGoods = new ViewGoods();
            Frame1.NavigationService.Navigate(viewModelGoods);
        }

        private void Exit(object sender, MouseButtonEventArgs e)
        {
            window.Show();
            Close();
        }

        //private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    // Ваш код здесь. Например:
        //    if (dataGrid1.SelectedItem != null)
        //    {
        //        Goods selectedGoods = (Goods)dataGrid1.SelectedItem;
        //        MessageBox.Show($"Выбран товар: {selectedGoods.Name}");
        //    }
        //}

        //private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete && dataGrid1.SelectedItems.Count > 0)
        //    {
        //        foreach (var item in dataGrid1.SelectedItems)
        //        {
        //            Goods goods = item as Goods;
        //            if (goods != null)
        //            {
        //                GoodsList.Remove(goods);
        //            }
        //        }
        //        dataGrid1.Items.Refresh();
        //    }
        //}
    }
}