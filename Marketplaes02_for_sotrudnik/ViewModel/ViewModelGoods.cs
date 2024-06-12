using Marketplaes02_for_sotrudnik.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Marketplaes02_for_sotrudnik.BD;
using System.Windows;
using System.Windows.Controls;

namespace Marketplaes02_for_sotrudnik.ViewModel
{
    public class ViewModelGoods : INotifyPropertyChanged
    {
        int ID_user;
        /// <summary>
        /// Список Good
        /// </summary>
        private IList<Goods> _Goodslist;
        public IList<Goods> Goodslist
        {
            get => _Goodslist;
            set
            {
                _Goodslist = value;
                OnPropertyChanged("Goodslist");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        }
        public ViewModelGoods()
        {
            Load();
        }

        private DateTime _date_time;
        public DateTime Date_time
        {
            get => _date_time;
            set
            {
                _date_time = value;
                OnPropertyChanged("Date_time");
            }
        }
        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async Task Load()
        {

            await Task.Run(async() =>
            {
                await LoadGoods();
                Date_time = DateTime.Now;
            });
        }
      
        /// <summary>
        /// Метод Получения товаров из БД
        /// </summary>
        /// <returns></returns>
        private async Task<bool> LoadGoods()
        {

            string
                sql = "SELECT * FROM goods";



            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            await con.GetConnectBD();
           

            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();
            Goodslist = new ObservableCollection<Goods>();
            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                Goodslist.Clear();
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {
                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)

                Goodslist.Add(new Goods()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToSingle(reader["Price"]),
                    Image = await new  FileBase().GetShareableImageLink(reader["ImageGood"].ToString()),
                    Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    V_nalichii = Convert.ToInt32(reader["V_nalichii"]),
                });

                // await Task.Delay(1000);
            }
            OnPropertyChanged("Goodslist");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }


      



    }
}
