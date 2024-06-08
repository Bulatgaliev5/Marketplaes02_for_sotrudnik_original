using Marketplaes02_for_sotrudnik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewKartochkaGoods.xaml
    /// </summary>
    public partial class ViewKartochkaGoods : Window
    {
        public ViewKartochkaGoods(int SelectID_goods)
        {
            InitializeComponent();
            DataContext = new ViewModelKartochkaGoods(SelectID_goods);
        }
        private void ValidNumber(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9.]+");
        }
    }
}
