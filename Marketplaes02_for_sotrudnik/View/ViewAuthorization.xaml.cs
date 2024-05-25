using Marketplaes02_for_sotrudnik.BD;
using Marketplaes02_for_sotrudnik.ViewModel;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewAuthorization.xaml
    /// </summary>
    public partial class ViewAuthorization : Window
    {
        public ViewAuthorization()
        {
            InitializeComponent();
           
        }


        private async Task<bool> CheckData()
        {
            if (!IsValidText(passTBox.Password) && !IsValidText(loginTBox.Text))
            {
                bool result = await CheckPass(loginTBox.Text, passTBox.Password);
                if (!result)
                {


                    return false;
                }
               

            }
            else
            {
                MessageBox.Show("Заполните все поля ", "Информация", MessageBoxButton.OK
                    , MessageBoxImage.Information);
                return false;
            }
            MainWindow main = new MainWindow(this);
            loginTBox.Text = "";
            passTBox.Password = "";
            this.Hide();
            main.Show();
            return true;
        }

        /// <summary>
        ///  Валидация свойст
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsValidText(string property)
        {
            return string.IsNullOrEmpty(property) || property.Length < 1;
        }
        private async Task<bool> CheckPass(string Login, string Pass)
        {
            ConnectBD con = new ConnectBD();

            string sql = "SELECT * FROM sotrudnik s WHERE s.Login = @login AND s.Pass = @pass";

            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@login", Login));
            cmd.Parameters.Add(new MySqlParameter("@pass", Pass));

            await con.GetConnectBD();

            MySqlDataReader readed = await cmd.ExecuteReaderAsync();
            if (!readed.HasRows)
            {
                await con.GetCloseBD();
                MessageBox.Show("Не правильный логин или пароль ", "Ошибка", MessageBoxButton.OK
                , MessageBoxImage.Error);
                return false;
            }
            while (await readed.ReadAsync())
            {
                MessageBox.Show("Успешно ", "Сообщение", MessageBoxButton.OK
                , MessageBoxImage.Information);
            }

            await con.GetCloseBD();
            return true;
        }


        private async void LoginOpenApp(object sender, MouseButtonEventArgs e)
        {
            await CheckData();
        }
    }
}
