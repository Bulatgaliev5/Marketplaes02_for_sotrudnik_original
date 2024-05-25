using ActiproSoftware.Windows.Extensions;
using Marketplaes02_for_sotrudnik.BD;
using Marketplaes02_for_sotrudnik.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Marketplaes02_for_sotrudnik.ViewModel
{
    public class ViewModelKartochkaOrders : KartochkaOrders
    {

        public ViewModelKartochkaOrders(int selectorder)
        {

            SelectOrder = selectorder;
            Load();

        }
        private int _SelectOrder;
        public int SelectOrder
        {
            get => _SelectOrder;
            set
            {
                _SelectOrder = value;
                OnPropertyChanged("SelectOrder");
            }
        }
        private Visibility _vivibleTreckNumber;
        public Visibility vivibleTreckNumber
        {
            get => _vivibleTreckNumber;
            set
            {
                _vivibleTreckNumber = value;
                OnPropertyChanged("vivibleTreckNumber");
            }
        }

        private Visibility _vivibleStatus;
        public Visibility vivibleStatus
        {
            get => _vivibleStatus;
            set
            {
                _vivibleStatus = value;
                OnPropertyChanged("vivibleStatus");
            }
        }

        private Visibility _vivibleLabelTrekNumber;
        public Visibility vivibleLabelTrekNumber
        {
            get => _vivibleLabelTrekNumber;
            set
            {
                _vivibleLabelTrekNumber = value;
                OnPropertyChanged("vivibleLabelTrekNumber");
            }
        }
        private Visibility _vivibleTextboxTrekNumber;
        public Visibility vivibleTextboxTrekNumber
        {
            get => _vivibleTextboxTrekNumber;
            set
            {
                _vivibleTextboxTrekNumber = value;
                OnPropertyChanged("vivibleTextboxTrekNumber");
            }
        }
        private Visibility _vivibleUpdateBorder;
        public Visibility vivibleUpdateBorder
        {
            get => _vivibleUpdateBorder;
            set
            {
                _vivibleUpdateBorder = value;
                OnPropertyChanged("vivibleUpdateBorder");
            }
        }
        private string _NameBorberText;
        public string NameBorberText
        {
            get => _NameBorberText;
            set
            {
                _NameBorberText = value;
                OnPropertyChanged("NameBorberText");
            }
        }
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

        private IList<string> _StatusList;
        public IList<string> StatusList
        {
            get => _StatusList;
            set
            {
                _StatusList = value;
                OnPropertyChanged("StatusList");
            }
        }
        private void CheckTreckNumber()
        {
            if (Track_number == "")
            {
                vivibleTextboxTrekNumber = Visibility.Visible;
                vivibleLabelTrekNumber = Visibility.Collapsed;
            }
            else
            {
                vivibleTextboxTrekNumber = Visibility.Collapsed;
                vivibleLabelTrekNumber = Visibility.Visible;
            }

        }
        private void AddStutusList()
        {
            StatusList.Add(Status);
            StatusList.Add("Заказ в пути");
            StatusList.Add("Заказ доставлен");
            vivibleUpdateBorder = Visibility.Visible;
            NameBorberText = "Изменить статус заказа и присвоить трек номер";

            if (StatusList[0]== "Заказ в пути")
            {
                StatusList.RemoveAt(1);
                NameBorberText = "Изменить статус заказа";
            }
            else if (StatusList[0] == "Заказ доставлен")
            {
                StatusList.RemoveAt(1);
                StatusList.RemoveAt(1);
                vivibleUpdateBorder=Visibility.Collapsed;
            }
            OnPropertyChanged("StatusList");
        }
        public async void Load()
        {
           await LoadMyOrders(SelectOrder);
            await LoadMyOrder_items(SelectOrder);
            Date_time = DateTime.Now;
            if (Track_number != "")
            {
                vivibleTreckNumber = Visibility.Collapsed;
                vivibleStatus = Visibility.Visible;
            }
            else
            {
                vivibleTreckNumber = Visibility.Visible;

                vivibleStatus = Visibility.Collapsed;
            }
            AddStutusList();
            CheckTreckNumber();
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




        private async Task<bool> LoadMyOrder_items(int selectorder)
        {

            string
                  sql = "SELECT * " +
                  "FROM orders o " +
                  "JOIN order_items o_u ON o_u.ID_order = o.ID_order " +
                  "JOIN goods g ON g.ID_goods = o_u.ID_goods " +
                  "WHERE o.ID_order = @ID_order";

            ConnectBD con = new ConnectBD();


            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_order", selectorder));


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
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Image = Convert.ToString(reader["ImageGood"]),
                    Name = Convert.ToString(reader["Name"]),
                    Total_Count = Convert.ToInt32(reader["Total_Count"]),
                    Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                });

            }

            OnPropertyChanged("MyOrder_itemslist");

            await con.GetCloseBD();

            return true;


        }

        private async Task<bool> LoadMyOrders(int selectorder)
        {

            string
                  sql = "SELECT o.ID_order, o.Order_date, o.Total_Count,o.Total_Price_with_discount, o.Status, " +
                  "o.Track_number, " +
                  "u.Login AS User_Login, u.Name AS User_Name, u.Number_phone AS User_Number, o.Adres_Dostavki " +
                  "FROM orders o JOIN users u ON o.ID_user = u.ID " +
                  "WHERE ID_order=@ID_order";


            ConnectBD con = new ConnectBD();

            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_order", selectorder));
            await con.GetConnectBD();
            MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {

                await con.GetCloseBD();

                return false;

            }
            StatusList = new ObservableCollection<string>();

            while (await reader.ReadAsync())
            {
                ID_order = Convert.ToInt32(reader["ID_order"]);
                Total_Count = Convert.ToInt32(reader["Total_Count"]);
                User_Nickname = Convert.ToString(reader["User_Login"]);
                User_Name = Convert.ToString(reader["User_Name"]);
                Adres_Dostavki = Convert.ToString(reader["Adres_Dostavki"]);
                Order_date = Convert.ToDateTime(reader["Order_date"]);
                Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]);
                Track_number = Convert.ToString(reader["Track_number"]);
                Status = Convert.ToString(reader["Status"]);
                User_Number = Convert.ToString(reader["User_Number"]);

            }

            OnPropertyChanged("ID_order");
            OnPropertyChanged("Total_Count");
            OnPropertyChanged("User_Nickname");
            OnPropertyChanged("User_Name");
            OnPropertyChanged("Adres_Dostavki");
            OnPropertyChanged("Order_date");
            OnPropertyChanged("Total_Price_with_discount");
            OnPropertyChanged("Track_number");
            OnPropertyChanged("Status");
            OnPropertyChanged("User_Number");


            await con.GetCloseBD();

            return true;


        }



    }
}
