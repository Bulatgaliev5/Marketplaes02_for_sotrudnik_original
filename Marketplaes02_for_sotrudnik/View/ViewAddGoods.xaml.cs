using Marketplaes02_for_sotrudnik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using Google.Apis.Upload;
using YandexDisk.Client.Http;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Protocol;
using System.Text.RegularExpressions;
using System.Windows.Input;


namespace Marketplaes02_for_sotrudnik.View
{
    /// <summary>
    /// Логика взаимодействия для ViewAddGoods.xaml
    /// </summary>
    public partial class ViewAddGoods : Window
    {

        public ViewAddGoods()
        {
            InitializeComponent();
            DataContext = new ViewModelAddGoods();
        }

        private void ValidNumber(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9.]+");
        }

    }
}
