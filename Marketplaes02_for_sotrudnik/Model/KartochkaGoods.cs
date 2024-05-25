using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class KartochkaGoods : INotifyPropertyChanged
    {
        private int _Discount;
        private float _Price_with_discount;
        /// <summary>
        /// Поле ID_goods
        /// </summary>
        private int _ID_goods;
        /// <summary>
        /// Поле Название
        /// </summary>
        private string _Name;
        /// <summary>
        /// Поле Цена
        /// </summary>
        private float _Price;

        /// <summary>
        /// Поле Изображение 
        /// </summary>
        private string _Image;

        private int _V_nalichii;
        public int V_nalichii
        {
            get => _V_nalichii;
            set
            {
                _V_nalichii = value;
                OnPropertyChanged("V_nalichii");

            }
        }

        private string _ImageIsbrannoe;
        public string ImageIsbrannoe
        {
            get => _ImageIsbrannoe;
            set
            {
                _ImageIsbrannoe = value;
                OnPropertyChanged("ImageIsbrannoe");

            }
        }
        private string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
                LoadPrice_with_discount();
            }
        }
        public void LoadPrice_with_discount()
        {
            Price_with_discount = Price - (Price * Discount / 100);
            OnPropertyChanged("Price_with_discount");
        }
        /// <summary>
        /// Свойсва ID_goods
        /// </summary>
        /// 
        public int Discount
        {
            get => _Discount;
            set
            {
                if (value <= 100)
                {
                    _Discount = value;
                    OnPropertyChanged("Discount");
                    LoadPrice_with_discount();

                }


            }
        }
        public int ID_goods
        {
            get => _ID_goods;
            set
            {
                _ID_goods = value;
                OnPropertyChanged("ID_goods");
            }
        }
        /// <summary>
        /// Свойсва Название
        /// </summary>
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
        /// Свойства цена
        /// </summary>
        public float Price
        {
            get => _Price;
            set
            {
                _Price = value;
                OnPropertyChanged("Price");
                LoadPrice_with_discount();
            }
        }
        public float Price_with_discount
        {
            get => _Price_with_discount;
            set
            {
                _Price_with_discount = value;
                OnPropertyChanged("Price_with_discount");

            }
        }
        /// <summary>
        /// Свойства Изображение товара
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
    }
}
