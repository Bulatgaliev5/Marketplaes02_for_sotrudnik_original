using Marketplaes02_for_sotrudnik.ViewModel;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;


namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewAddGoods.xaml
    /// </summary>
    public partial class ViewAddGoods : Window
    {

        public ViewAddGoods()
        {
            InitializeComponent();
            DataContext = new ViewModelAddGoods();
        }

        private void ValidNumber(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9.]+");
        }

    }
}
