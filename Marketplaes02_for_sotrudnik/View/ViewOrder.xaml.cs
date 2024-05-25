using Marketplaes02_for_sotrudnik.BD;
using Marketplaes02_for_sotrudnik.Model;
using Marketplaes02_for_sotrudnik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Page
    {
        public ViewOrder()
        {
            
            InitializeComponent();
            tabControl.SelectionChanged += ClicckStatus;

        }
        private string Status { get; set; }
        public async void Update()
        {
             DataContext =  new ViewModelOrder(Status);

        }

        private void BorderUpdate(object sender, MouseButtonEventArgs e)
        {
            Update();
        }

        private void BorderObrabotatOrder(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border row && row.DataContext is MyOrders SelectGoods)
            {
                ViewKartochkaOrders viewKartochka = new ViewKartochkaOrders(SelectGoods.ID_order);
                this.Opacity = 0.5;
                
                viewKartochka.ShowDialog();
                viewKartochka.Focus();
                this.Opacity = 1;
                Update();
            }
        }

        private void ClicckStatus(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl) // проверяем, что источником события является TabControl
            {
                TabItem selectedTab = (TabItem)tabControl.SelectedItem;
                Status = selectedTab.Header.ToString();
                Update();
            }
        }
    }
}
