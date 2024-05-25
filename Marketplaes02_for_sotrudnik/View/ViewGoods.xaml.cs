using HandyControl.Tools.Extension;
using Marketplaes02_for_sotrudnik.Model;
using Marketplaes02_for_sotrudnik.ViewModel;
using Marketplaes02_for_sotrudnik.BD;
using MySqlConnector;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewGoods.xaml
    /// </summary>
    public partial class ViewGoods : Page
    {

        //List<Goods>
        //    GoodsList = new List<Goods>();
        public ViewGoods()
        {
            InitializeComponent();

            Update();

        }

        private async void Update()
        {
            DataContext = new ViewModelGoods();
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        //private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (sender is Image image && image.ContextMenu != null)
        //    {
        //        image.ContextMenu.IsOpen = true;
        //    }
        //}

        private void Add(object sender, MouseButtonEventArgs e)
        {
            ViewAddGoods viewAddGoods = new ViewAddGoods();
            this.Opacity = 0.5;
            viewAddGoods.ShowDialog();
            viewAddGoods.Focus();
            this.Opacity = 1;
            Update();
        }

        private async void MenuItemDeleteGoods(object sender, RoutedEventArgs e)
        {
           
               
        }

        private async Task<bool> GoodsDeleteSQL(int ID_goods)
        {
            string
              sql = "DELETE FROM goods WHERE ID_goods=@ID_goods";
            ConnectBD
             con = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();
            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await con.GetCloseBD();
                return true;

            }
            await con.GetCloseBD();
            return false;
        }

        private async void BorderDeleteGoods(object sender, MouseButtonEventArgs e)
        {
            if (dataGridGoods.SelectedItems.Count>0)
            {
                var selectedItems = new List<Goods>(dataGridGoods.SelectedItems.Cast<Goods>());
                if (MessageBox.Show(
                   string.Format("Вы действительно собираетесь удалить выбранные товары "), "Внимание!",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;
                foreach (var item in selectedItems)
                {

                   
                    if (!await GoodsDeleteSQL(item.ID_goods))
                    {
                        MessageBox.Show("Данный товар не удален", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                   

                }
                MessageBox.Show("Данный товар удален", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                Update();
            }
             

        }

        private void BorderUpdate(object sender, MouseButtonEventArgs e)
        {
            Update();
        }

        private void OpenKartochkaGoods(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is Goods SelectGoods)
            {
                ViewKartochkaGoods viewKartochka = new ViewKartochkaGoods(SelectGoods.ID_goods);
                this.Opacity = 0.5;
                viewKartochka.ShowDialog();
                viewKartochka.Focus();
                this.Opacity = 1;
                Update();
            }


        }
    }
}
