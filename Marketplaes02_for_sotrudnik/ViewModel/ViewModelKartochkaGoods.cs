using Marketplaes02_for_sotrudnik.BD;
using Marketplaes02_for_sotrudnik.Model;
using Microsoft.Win32;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace Marketplaes02_for_sotrudnik.ViewModel
{
    public class ViewModelKartochkaGoods : KartochkaGoods
    {
        public ViewModelKartochkaGoods(int selectID_goods)
        {

            SelectID_goods = selectID_goods;
            Load();

        }
        private int _SelectID_goods;
        public int SelectID_goods
        {
            get => _SelectID_goods;
            set
            {
                _SelectID_goods = value;
                OnPropertyChanged("SelectID_goods");
            }
        }

        private IList<Kategoriya> _kategoriyaList;

        public IList<Kategoriya> kategoriyaList
        {
            get => _kategoriyaList;
            set
            {
                _kategoriyaList = value;
                OnPropertyChanged("kategoriyaList");
            }
        }
        private Kategoriya _selectedKategoriya;
        public Kategoriya SelectedKategoriya
        {
            get => _selectedKategoriya;
            set
            {
                _selectedKategoriya = value;
                OnPropertyChanged("SelectedKategoriya");
            }
        }
        private string _selectedNameKategoriya;
        public string selectedNameKategoriya
        {
            get => _selectedNameKategoriya;
            set
            {
                _selectedNameKategoriya = value;
                OnPropertyChanged("selectedNameKategoriya");
            }
        }

        private ICommand _UpdateGoodsCommand;
        public ICommand UpdateGoodsCommand
        {
            get
            {
                if (_UpdateGoodsCommand == null)
                {
                    _UpdateGoodsCommand = new ActionCommand(UpdateGoods);
                }
                return _UpdateGoodsCommand;
            }

        }
        public async void UpdateGoods(object win)
        {
            Window window = (Window)win;

            if (!IsValidText(Name) && !IsValidText(Price)
                    && !IsValidText(Description)
                       && !IsValidText(Discount) && !IsValidText(V_nalichii))
            {


                var result = await SQLUpdateGood();
                if (result)
                {
                    if (IsValidIList(imagesList))
                    {
                        Upload();
                        MessageBox.Show("Данные сохранились", "Информация", MessageBoxButton.OK
                       , MessageBoxImage.Information);

                        CloseWindow(window);
                    }
                    else
                    {
                        MessageBox.Show("Загрузите все изображения", "Информация", MessageBoxButton.OK
                       , MessageBoxImage.Information);
                    }

                }

            }
            else
            {
                MessageBox.Show("Заполните все поля ", "Информация", MessageBoxButton.OK
                    , MessageBoxImage.Information);
            }
        }
        private ICommand _UpdateImageCommand;
        public ICommand UpdateImageCommand
        {
            get
            {
                if (_UpdateImageCommand == null)
                {
                    _UpdateImageCommand = new ActionCommand(UpdateImage);
                }
                return _UpdateImageCommand;
            }

        }
        public async void UpdateImage(object item)
        {

            int imageIndex = Convert.ToInt32(item);
            string filePath = OpenFileDialog();




            if (filePath != "/Icons/no_photo.jpg")
            {
                bool res = await UploadImage(filePath, imageIndex);

                if (res)
                {

                    MessageBox.Show("Сохранен изображения", "Информация", MessageBoxButton.OK
                       , MessageBoxImage.Information);
                }

            }
            else
            {
                MessageBox.Show("Вы не выбрали изображение", "Информация", MessageBoxButton.OK
                       , MessageBoxImage.Information);
            }
        }
        private async Task<bool> Load_id_kategoriyaList()
        {
            string
            sql = "SELECT * FROM kategoriya";

            ConnectBD
            con = new ConnectBD();

            MySqlCommand
            cmd = new MySqlCommand(sql, con.GetConnBD());

            await con.GetConnectBD();

            MySqlDataReader
                reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                kategoriyaList.Clear();
                await con.GetCloseBD();
                return false;
            }
            kategoriyaList = new ObservableCollection<Kategoriya>();
            while (await reader.ReadAsync())
            {

                kategoriyaList.Add(new Kategoriya()
                {
                    id_kategoriya = Convert.ToInt32(reader["id_kategoriya"]),
                    Name = reader["Name"].ToString(),
                });
            }
            OnPropertyChanged("kategoriyaList");
            await con.GetCloseBD();
            return true;

        }
        private IList<ImageFile> _imagesList;
        public IList<ImageFile> imagesList
        {
            get => _imagesList;
            set
            {
                _imagesList = value;
                OnPropertyChanged("imagesList");
            }
        }
        public async void Load()
        {
            await Load_id_kategoriyaList();
            await GoodsSelectSQL(SelectID_goods);
            await ImagesGoodsSelectSQL(SelectID_goods);

        }

        public async Task<bool> GoodsSelectSQL(int id)
        {
            string
              sql = "SELECT *, k.Name AS NameKategoriya FROM goods g " +
              "JOIN kategoriya k ON k.id_kategoriya = g.id_kategoriya " +
              "WHERE  ID_goods=@ID_goods";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", id));
            await conn.GetConnectBD();
            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }

            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {


                ID_goods = Convert.ToInt32(reader["ID_goods"]);
                Name = reader["Name"].ToString();
                Price = Convert.ToSingle(reader["Price"]);
                Description = reader["Description"].ToString();
                Discount = Convert.ToInt32(reader["Discount"]);
                //Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]);
                V_nalichii = Convert.ToInt32(reader["V_nalichii"]);
                selectedNameKategoriya = reader["NameKategoriya"].ToString();

                // await Task.Delay(1000);
            }
            SelectedKategoriya = kategoriyaList.FirstOrDefault(k => k.Name == selectedNameKategoriya);
            OnPropertyChanged("SelectedKategoriya");
            OnPropertyChanged("ID_goods");
            OnPropertyChanged("Name");
            OnPropertyChanged("Price");
            OnPropertyChanged("Description");
            // OnPropertyChanged("Price_with_discount");
            OnPropertyChanged("Discount");
            OnPropertyChanged("V_nalichii");
            await conn.GetCloseBD();

            return true;
        }

        public async Task<bool> ImagesGoodsSelectSQL(int id)
        {
            string
              sql = "SELECT * FROM imagesgoods WHERE  ID_goods=@id";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id));
            await conn.GetConnectBD();
            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }
            imagesList = new ObservableCollection<ImageFile>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while ((await reader.ReadAsync()))
            {

                imagesList.Add(new ImageFile()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    ImageID = Convert.ToInt32(reader["ImageID"]),
                    linkimage = reader["Image"].ToString(),
                    IsViviblevalueFileUpload = Visibility.Collapsed,
                });


            }
            OnPropertyChanged("ImagesGoodsList");

            await conn.GetCloseBD();

            return true;
        }
        private bool IsValidText(object control)
        {

            return string.IsNullOrEmpty(Convert.ToString(control)) || Convert.ToString(control).Length < 1 || Convert.ToString(control) == "0";
        }
        private bool IsValidIList(IList<ImageFile> control)
        {

            for (int i = 0; i < control.Count; i++)
            {
                if (control[i].linkimage == "/Icons/no_photo.jpg")
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> SQLUpdateGood()
        {

            ConnectBD con = new ConnectBD();
            string sql = "UPDATE `goods` SET `Name`=@Name, `Price`=@Price, `id_kategoriya`=@id_kategoriya, `Description`=@Description, `Discount`=@Discount, `V_nalichii`=@V_nalichii WHERE `ID_goods`=@ID_goods";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@Name", Name));
            cmd.Parameters.Add(new MySqlParameter("@Price", Price));
            cmd.Parameters.Add(new MySqlParameter("@id_kategoriya", SelectedKategoriya.id_kategoriya));
            cmd.Parameters.Add(new MySqlParameter("@Description", Description));
            cmd.Parameters.Add(new MySqlParameter("@Discount", Discount));
            cmd.Parameters.Add(new MySqlParameter("@V_nalichii", V_nalichii));

            cmd.Parameters.Add(new MySqlParameter("@ID_goods", SelectID_goods));
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;
        }






        private async void Upload()
        {
            for (int i = 0; i < imagesList.Count; i++)
            {
                await UpdateAddImage(i);
            }
        }

        public async Task<bool> UploadImage(string FilePath, int imageIndex)
        {
            imagesList[imageIndex].IsViviblevalueFileUpload = Visibility.Visible;
            imagesList[imageIndex].valueFileUpload = 20;
            //https://oauth.yandex.ru/authorize?response_type=token&client_id=<идентификатор_приложения>
            //See https://tech.yandex.ru/oauth/
            var Tokenapi = new DiskHttpApi("y0_AgAAAAAhuR-GAAvCBgAAAAEEZT7EAADinuaiQQJNYZODkddcTxQMEi9Zfw");
            imagesList[imageIndex].valueFileUpload = 40;
            var link = await Tokenapi.Files.GetUploadLinkAsync("/Диплом_Файлы/" + Path.GetFileName(FilePath), overwrite: true);
            var fs = File.OpenRead(FilePath);
            imagesList[imageIndex].valueFileUpload = 60;
            await Tokenapi.Files.UploadAsync(link, fs);
            imagesList[imageIndex].valueFileUpload = 80;
            var testfolder = await Tokenapi.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = "/Диплом_Файлы",
                Limit = 1,
                Sort = "-created"


            });
            imagesList[imageIndex].valueFileUpload = 100;
            var linkfile = await Tokenapi.Files.GetDownloadLinkAsync(testfolder.Embedded.Items[0].Path);
            imagesList[imageIndex].linkimage = linkfile.Href;


            imagesList[imageIndex].IsViviblevalueFileUpload = Visibility.Collapsed;
            OnPropertyChanged("imagesList");
            return true;

        }
        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.png;*.jpg;*.webp)|*.png;*.jpg;*.webp";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "/Icons/no_photo.jpg";
        }
        private void CloseWindow(Window window)
        {

            if (window != null)
            {
                window.Close();
            }
        }
        public async Task<bool> UpdateAddImage(int i)
        {

            ConnectBD con = new ConnectBD();
            string sql = "UPDATE imagesgoods SET Image=@Image WHERE ID_goods=@ID_goods AND ImageID=@ImageID";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ImageID", imagesList[i].ImageID));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", SelectID_goods));
            cmd.Parameters.Add(new MySqlParameter("@Image", imagesList[i].linkimage));
            await con.GetConnectBD();
            cmd.ExecuteNonQuery();
            await con.GetCloseBD();
            return true;
        }




    }
}
