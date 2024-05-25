
using Google.Apis.Drive.v3.Data;
using Marketplaes02_for_sotrudnik.BD;
using Marketplaes02_for_sotrudnik.Model;
using Microsoft.Win32;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.IO;

using System.Windows;
using System.Windows.Input;
using YandexDisk.Client.Protocol;

namespace Marketplaes02_for_sotrudnik.ViewModel
{
    public class ViewModelAddGoods : AddGoods 
    {
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

        private int _id_goods_image;
        public int id_goods_image
        {
            get => _id_goods_image;
            set
            {
                _id_goods_image = value;
                OnPropertyChanged("id_goods_image");
            }
        }

        

        public ViewModelAddGoods()
        {
            Load_id_kategoriyaList();
            imagesList =
            [
                new ImageFile()
                {
                    linkimage = "/Icons/no_photo.jpg",
                    IsViviblevalueFileUpload = Visibility.Collapsed
                },
                 new ImageFile()
                {
                    linkimage = "/Icons/no_photo.jpg",
                    IsViviblevalueFileUpload = Visibility.Collapsed
                },
                 new ImageFile()
                {
                    linkimage = "/Icons/no_photo.jpg",
                    IsViviblevalueFileUpload = Visibility.Collapsed
                },
                 new ImageFile()
                {
                    linkimage = "/Icons/no_photo.jpg",
                    IsViviblevalueFileUpload = Visibility.Collapsed
                },
            ];


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

        public async Task<bool> SQLAddGood()
        {

            ConnectBD con = new ConnectBD();
                string sql = "INSERT INTO `goods` (`Name`, `Price`, `id_kategoriya`, `Description`, `Discount`, `V_nalichii`) VALUES (@Name, @Price, @id_kategoriya, @Description, @Discount, @V_nalichii);";
                MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
                cmd.Parameters.Add(new MySqlParameter("@Name", Name));
                cmd.Parameters.Add(new MySqlParameter("@Price", Price));
                cmd.Parameters.Add(new MySqlParameter("@id_kategoriya", SelectedKategoriya.id_kategoriya));
                cmd.Parameters.Add(new MySqlParameter("@Description", Description));
                cmd.Parameters.Add(new MySqlParameter("@Discount", Discount));
                cmd.Parameters.Add(new MySqlParameter("@V_nalichii", V_nalichii));
            await con.GetConnectBD();
                cmd.ExecuteNonQuery();
                await con.GetCloseBD();
                return true;
        }
        private bool IsValidText(object control)
        {

            return string.IsNullOrEmpty(Convert.ToString(control)) || Convert.ToString(control).Length < 1 || Convert.ToString(control) =="0";
        }
        private bool IsValidIList(IList<ImageFile> control)
        {

            for (int i = 0; i < control.Count; i++)
            {
                if (control[i].linkimage=="/Icons/no_photo.jpg")
                {
                    return false;
                }
            }

            return true;
        }
        private void CloseWindow(Window window)
        {
            
            if (window != null)
            {
                window.Close();
            }
        }




        private ICommand _AddImageCommand;
        public ICommand AddImageCommand
        {
            get
            {
                if (_AddImageCommand == null)
                {
                    _AddImageCommand = new ActionCommand(AddImage);
                }
                return _AddImageCommand;
            }

        }

        private ICommand _AddGoodsCommand;
        public ICommand AddGoodsCommand
        {
            get
            {
                if (_AddGoodsCommand == null)
                {
                    _AddGoodsCommand = new ActionCommand(AddAddGoods);
                }
                return _AddGoodsCommand;
            }

        }
        public async void AddAddGoods(object win)
        {
            Window window = (Window)win;

            if (!IsValidText(Name) && !IsValidText(Price) &&
            !IsValidText(SelectedKategoriya) && !IsValidText(Description)
              && !IsValidText(Discount) && !IsValidText(V_nalichii))
            {


                var result = await SQLAddGood();
                if (result)
                {
                    await Load_goods();
                    if (IsValidIList(imagesList))
                    {
                        Upload();
                        MessageBox.Show("Успех", "Информация", MessageBoxButton.OK
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
        public async void AddImage(object item)
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

        private async void Upload()
        {
            for (int i = 0; i < imagesList.Count; i++)
            {
              await SQLAddImage(i);
            }
        }
        private int _UploadfFile;
        public int UploadfFile
        {
            get => _UploadfFile;
            set
            {
                _UploadfFile = value;
                OnPropertyChanged("UploadfFile");
            }
        }


        public async Task<bool> UploadImage(string FilePath, int imageIndex)
        {
            imagesList[imageIndex].IsViviblevalueFileUpload =  Visibility.Visible;
            imagesList[imageIndex].valueFileUpload = 20;
             ////https://oauth.yandex.ru/authorize?response_type=token&client_id=<идентификатор_приложения>
             ////See https://tech.yandex.ru/oauth/
             //var Tokenapi = new DiskHttpApi("y0_AgAAAAAhuR-GAAvCBgAAAAEEZT7EAADinuaiQQJNYZODkddcTxQMEi9Zfw");
            imagesList[imageIndex].valueFileUpload = 40;
            //var link = await Tokenapi.Files.GetUploadLinkAsync("/Диплом_Файлы/" + Path.GetFileName(FilePath), overwrite: true);
            //var fs = File.OpenRead(FilePath);
            imagesList[imageIndex].valueFileUpload = 60;
            //await Tokenapi.Files.UploadAsync(link, fs);
            imagesList[imageIndex].valueFileUpload = 80;

            //await Task.Delay(5000);
            //var testfolder = await Tokenapi.MetaInfo.GetInfoAsync(new ResourceRequest
            //{
            //    Path = "/Диплом_Файлы",
            //    Limit = 1,
            //    Sort = "-created",


            //});
         
            imagesList[imageIndex].valueFileUpload = 100;
            //var linkfile = await Tokenapi.Files.GetDownloadLinkAsync(testfolder.Embedded.Items[0].Path);
            //imagesList[imageIndex].linkimage = linkfile.Href;

            //var folderURL = testfolder.PublicUrl;
            //string linkfile = folderURL + "/" + testfolder.Embedded.Items[0].Name;

            //var linkfile = testfolder.Embedded.Items[0].Preview;
            //imagesList[imageIndex].linkimage = linkfile;


     

            //var testfolder = await Tokenapi.MetaInfo.GetInfoAsync(new ResourceRequest
            //{
            //    Path = "/Диплом_Файлы",
            //    Limit = 1,
            //    Sort = "-created"
                
                
            //});
            imagesList[imageIndex].valueFileUpload = 100;
            //var linkfile = await Tokenapi.Files.GetDownloadLinkAsync(testfolder.Embedded.Items[0].Path);
            //imagesList[imageIndex].linkimage = linkfile.Href;

            FileBase fileBase = new FileBase();
            fileBase.UploadFile(FilePath, "Bulat_files/" + Path.GetFileName(FilePath));

            await fileBase.DownloadFileAsync("Bulat_files/" + Path.GetFileName(FilePath), "C:\\Users\\Admin\\source\\" +
                Path.GetFileName(FilePath));

            //  imagesList[imageIndex].linkimage = ;



            imagesList[imageIndex].IsViviblevalueFileUpload = Visibility.Collapsed;
            OnPropertyChanged("imagesList");
            return true;

        }


        private async Task Load_goods()
        {
            string
            sql = "SELECT * FROM goods ORDER BY ID_goods DESC LIMIT 1";

            ConnectBD
            con = new ConnectBD();

            MySqlCommand
            cmd = new MySqlCommand(sql, con.GetConnBD());

            await con.GetConnectBD();

            MySqlDataReader
                reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                
                await con.GetCloseBD();
                return;
            }
            
            while (await reader.ReadAsync())
            {
                id_goods_image = Convert.ToInt32(reader["ID_goods"]);

            }
            OnPropertyChanged("id_goods_image");
            await con.GetCloseBD();
            return;

        }

        public async Task<bool> SQLAddImage(int i)
        {

            ConnectBD con = new ConnectBD();
            string sql = "INSERT INTO `imagesgoods` (`ID_goods`, `Image`) VALUES (@ID_goods, @Image)";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", id_goods_image));
            cmd.Parameters.Add(new MySqlParameter("@Image", imagesList[i]));
            await con.GetConnectBD();
            cmd.ExecuteNonQuery();
            await con.GetCloseBD();
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
    }
}
