using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class AddGoods : INotifyPropertyChanged
    {

        private int _Discount;
        private float _Price_with_discount;
        /// <summary>
        /// Поле ID_goods
        /// </summary>
        private int _ID_goods;

        private int _id_kategoriya;
        /// <summary>
        /// Поле Название
        /// </summary>
        private string _Name;
        private string _Description;
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
        public int id_kategoriya
        {
            get => _id_kategoriya;
            set
            {
                _id_kategoriya = value;
                OnPropertyChanged("id_kategoriya");

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
        /// <summary>
        /// Свойсва ID_goods
        /// </summary>
        /// 
        
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
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged("Description");

            }
        }
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
        public void LoadPrice_with_discount()
        {
           Price_with_discount = Price - (Price * Discount / 100);
           OnPropertyChanged("Price_with_discount");
        }
        /// <summary>
        /// Свойства Цена
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
        /// <summary>
        /// Свойства Цена со скидкой
        /// </summary>
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
