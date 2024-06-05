using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class ImageFile: INotifyPropertyChanged
    {
        private int _ID_goods;
        public int ID_goods
        {
            get => _ID_goods;
            set
            {
                _ID_goods = value;
                OnPropertyChanged("ID_goods");
            }
        }

        private int _ImageID;
        public int ImageID
        {
            get => _ImageID;
            set
            {
                _ImageID = value;
                OnPropertyChanged("ImageID");
            }
        }

        private string _linkimage;
        public string linkimage
        {
            get => _linkimage;
            set
            {
                _linkimage = value;
                OnPropertyChanged("linkimage");
            }
        }

        private string _linkimageBD;
        public string linkimageBD
        {
            get => _linkimageBD;
            set
            {
                _linkimageBD = value;
                OnPropertyChanged("linkimageBD");
            }
        }

        private int _valueFileUpload;
        public int valueFileUpload
        {
            get => _valueFileUpload;
            set
            {
                _valueFileUpload = value;
                OnPropertyChanged("valueFileUpload");
            }
        }

        private Visibility _IsViviblevalueFileUpload;
        public Visibility IsViviblevalueFileUpload
        {
            get => _IsViviblevalueFileUpload;
            set
            {
                _IsViviblevalueFileUpload = value;
                OnPropertyChanged("IsViviblevalueFileUpload");
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
