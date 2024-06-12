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
using System.Net.NetworkInformation;

namespace Marketplaes02_for_sotrudnik.ViewModel
{
    public class ViewModelOrder : INotifyPropertyChanged
    {
        private string _Status;

        public string Status
        {
            get => _Status;
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
        public ViewModelOrder(string status)
        {

            Load(status);
        }
        private async Task CheckStatus(string status)
        {
            if (status == "Все заказы")
            {
                Status = "Все заказы";
            }
            else if (status == "Ожидают обработки")
            {
                Status = "Заказ принят";
            }
            else if (status == "Доставляются")
            {
                Status = "Заказ в пути";
            }
            else if (status == "Архив")
            {
                Status = "Заказ доставлен";
            }
            await LoadMyOrders();
        }
        public async void Load(string status)
        {
           await CheckStatus(status);
            
            Date_time = DateTime.Now;
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
        private IList<All_MyOrder> _all_MyOrder;
        public IList<All_MyOrder> All_MyOrder
        {
            get => _all_MyOrder;
            set
            {
                _all_MyOrder = value;
                OnPropertyChanged("All_MyOrder");
            }
        }
        int UserID;
        private IList<MyOrder_items> _MyOrder_itemslist;
        public IList<MyOrder_items> MyOrder_itemslist
        {
            get => _MyOrder_itemslist;
            set
            {
                _MyOrder_itemslist = value;
                OnPropertyChanged("MyOrder_itemslist");
            }
        }

        private IList<MyOrders> _MyOrderslist;
        public IList<MyOrders> MyOrderslist
        {
            get => _MyOrderslist;
            set
            {
                _MyOrderslist = value;
                OnPropertyChanged("MyOrderslist");
            }
        }


        private async Task<bool> LoadMyOrders()
        {
            string
                     sql;

            if (Status== "Все заказы")
            {
                sql = "SELECT o.ID_order, o.Order_date, o.Total_Count,o.Total_Price_with_discount, o.Status, " +
                        "o.Track_number, " +
                        "u.Login AS User_Login, u.Name AS User_Name, o.Adres_Dostavki " +
                        "FROM orders o JOIN users u ON o.ID_user = u.ID";
            }
            else
            {
               
                  sql = "SELECT o.ID_order, o.Order_date, o.Total_Count,o.Total_Price_with_discount, o.Status, " +
                  "o.Track_number, " +
                  "u.Login AS User_Login, u.Name AS User_Name, o.Adres_Dostavki " +
                  "FROM orders o JOIN users u ON o.ID_user = u.ID " +
                  "WHERE o.Status=@Status";
            }






            ConnectBD con = new ConnectBD();


            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@Status", Status));
            await con.GetConnectBD();
            MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {

                await con.GetCloseBD();

                return false;

            }

            MyOrderslist = new ObservableCollection<MyOrders>();

            while (await reader.ReadAsync())
            {

                MyOrderslist.Add(new MyOrders()
                {
                    ID_order = Convert.ToInt32(reader["ID_order"]),
                    Total_Count = Convert.ToInt32(reader["Total_Count"]),
                    User_Nickname = Convert.ToString(reader["User_Login"]),
                    User_Name = Convert.ToString(reader["User_Name"]),
                    Adres_Dostavki = Convert.ToString(reader["Adres_Dostavki"]),
                    Order_date = Convert.ToDateTime(reader["Order_date"]),
                    Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                    Track_number = Convert.ToString(reader["Track_number"]),
                    Status = Convert.ToString(reader["Status"]),

                });

            }

            OnPropertyChanged("MyOrderslist");
            await con.GetCloseBD();

            return true;


        }

        private async Task<bool> LoadMyOrder_items()
        {

            string
                  sql = "SELECT u_i.ID_order_item AS ID_order_item, o.Status AS Status_order_item, o.ID_order, o.Order_date, u_i.Total_Count,u_i.Total_Price_with_discount, " +
                  "g.Name AS Goods_Name, g.ID_goods AS Goods_ID, g.ImageGood AS Goods_Image, u.ID AS User_ID, o.Adres_Dostavki, " +
                  "u.Name AS User_Name, u.Number_phone AS User_Number_phone, o.Track_number, u.Login " +
                  "FROM orders o " +
                  "JOIN order_items u_i ON u_i.ID_order = o.ID_order " +
                  "JOIN goods g ON u_i.ID_goods = g.ID_goods " +
                  "JOIN users u ON o.ID_user = u.ID " +
                  "ORDER BY u_i.ID_order_item DESC";

            ConnectBD con = new ConnectBD();


            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());



            await con.GetConnectBD();
            MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {

                await con.GetCloseBD();

                return false;

            }

            MyOrder_itemslist = new ObservableCollection<MyOrder_items>();

            while (await reader.ReadAsync())
            {

                MyOrder_itemslist.Add(new MyOrder_items()
                {
                    ID_Order_items = Convert.ToInt32(reader["ID_order_item"]),

                    Image = Convert.ToString(reader["Goods_Image"]),
                    Name = Convert.ToString(reader["Goods_Name"]),
                    Total_Count = Convert.ToInt32(reader["Total_Count"]),
                    User_Nickname = Convert.ToString(reader["Login"]),
                    Order_date = Convert.ToDateTime(reader["Order_date"]),
                    Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                    Status = Convert.ToString(reader["Status_order_item"]),
                    ID_order = Convert.ToInt32(reader["ID_order"]),
                    Track_number = Convert.ToString(reader["Track_number"]),
                });

            }

            OnPropertyChanged("MyOrder_itemslist");

            await con.GetCloseBD();

            return true;


        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
    }
}
