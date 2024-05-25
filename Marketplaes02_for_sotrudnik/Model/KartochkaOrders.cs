using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class KartochkaOrders : INotifyPropertyChanged
    {
        private string _User_Name;
        private string _User_Number_phone;
        private int _Total_Count;
        private int _Count_goods;
        public string User_Name
        {
            get => _User_Name;
            set
            {
                _User_Name = value;
                OnPropertyChanged("User_Name");
            }
        }
        public string User_Number_phone
        {
            get => _User_Number_phone;
            set
            {
                _User_Number_phone = value;
                OnPropertyChanged("User_Number_phone");
            }
        }
        public int Count_goods
        {
            get => _Count_goods;
            set
            {
                _Count_goods = value;
                OnPropertyChanged("Count_goods");

            }
        }
        private float _Total_Price_with_discount;

        public float Total_Price_with_discount
        {
            get => _Total_Price_with_discount;
            set
            {
                _Total_Price_with_discount = value;
                OnPropertyChanged("Total_Price_with_discount");

            }
        }

        public int Total_Count
        {
            get => _Total_Count;
            set
            {
                _Total_Count = value;
                OnPropertyChanged("Total_Count");

            }
        }
        private string _User_Number;
        public string User_Number
        {
            get => _User_Number;
            set
            {
                _User_Number = value;
                OnPropertyChanged("User_Number");

            }
        }

        private float _Total_Order_Price_with_discount;

        public float Total_Order_Price_with_discount
        {
            get => _Total_Order_Price_with_discount;
            set
            {
                _Total_Order_Price_with_discount = value;
                OnPropertyChanged("Total_Order_Price_with_discount");

            }
        }
        private string _track_number;

        public string Track_number
        {
            get => _track_number;
            set
            {
                _track_number = value;
                OnPropertyChanged("Track_number");
            }
        }


        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        public KartochkaOrders()
        {

        }
        private int _ID_order;

        /// <summary>
        /// Поле Название изделия
        /// </summary>
        private string _Name;
        /// <summary>
        /// Поле Изображение изделия
        /// </summary>
        private string _Image;

        /// <summary>
        /// Поле Адрес доставки
        /// </summary>
        private string _adres_Dostavki;


        private DateTime _Order_date;
        private string _User_Nickname;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        public DateTime Order_date
        {
            get => _Order_date;
            set
            {
                _Order_date = value;
                OnPropertyChanged("Order_date");
            }
        }
        public string User_Nickname
        {
            get => _User_Nickname;
            set
            {
                _User_Nickname = value;
                OnPropertyChanged("User_Nickname");
            }
        }
        public int ID_order
        {
            get => _ID_order;
            set
            {
                _ID_order = value;
                OnPropertyChanged("ID_order");
            }
        }


        public string Adres_Dostavki
        {
            get => _adres_Dostavki;
            set
            {
                _adres_Dostavki = value;
                OnPropertyChanged("Adres_Dostavki");
            }
        }

        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("Name");

            }
        }
        /// <summary>
        /// Свойства Изображение изделия
        /// </summary>
        public string Image
        {
            get => _Image;
            set
            {
                _Image = value;
                OnPropertyChanged("Image");

            }
        }
    }
}
