using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class Kategoriya : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        private int _id_kategoriya;
        public int id_kategoriya
        {
            get => _id_kategoriya;
            set
            {
                _id_kategoriya = value;
                OnPropertyChanged("id_kategoriya");
            }
        }
        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("Name");

            }
        }
    }
}
