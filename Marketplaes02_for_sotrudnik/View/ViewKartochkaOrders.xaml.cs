using Marketplaes02_for_sotrudnik.BD;
using Marketplaes02_for_sotrudnik.ViewModel;
using MySqlConnector;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewKartochkaOrders.xaml
    /// </summary>
    public partial class ViewKartochkaOrders : Window
    {
        public int SelectOrder;
        public ViewKartochkaOrders(int selectorder)
        {
            SelectOrder = selectorder;
            InitializeComponent();
            DataContext = new ViewModelKartochkaOrders(selectorder);
        }


        public async Task<bool> SQLUpdateTreckNumber(int ID_Order)
        {
            string selectedValue = ComboBoxStatus.SelectedItem as string;
            if (MaskedTextBoxTreckNumber.Text == "" || TextBoxTreckNumber.Text == "")
            {
                MaskedTextBoxTreckNumber.Focusable = true;
                return false;
            }
            else if (selectedValue == "Заказ принят")
            {

                return false;
            }
            ConnectBD con = new ConnectBD();
            string sql = "UPDATE `orders` SET Status=@Status, Track_number=@Track_number WHERE `ID_order`=@ID_order";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_order", ID_Order));
            cmd.Parameters.Add(new MySqlParameter("@Status", selectedValue));
            cmd.Parameters.Add(new MySqlParameter("@Track_number", MaskedTextBoxTreckNumber.Text));
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;
        }



        private async void BorderUpdateStatus(object sender, MouseButtonEventArgs e)
        {

            bool res = await SQLUpdateTreckNumber(SelectOrder);
            if (res)
            {
                MessageBox.Show("Трек номер и статус заказа изменен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Трек номер и статус заказа не изменен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ValidNumber(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9.]+");
        }
    }
}
