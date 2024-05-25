using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class All_MyOrder :  MyOrder_items,  INotifyPropertyChanged
    {

       
            public MyOrders Order { get; set; }
            public IList<MyOrder_items> Items { get; set; }
 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
    }
}
