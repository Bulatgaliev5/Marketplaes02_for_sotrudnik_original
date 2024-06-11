using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Marketplaes02_for_sotrudnik.BD
{
    public class FileBase
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;

     //   ftp://37.18.74.116:21/
        public FileBase()
        {
            _host = "37.18.74.116";
            _port = 21;
            _username = "p101_f_ilnar";
            _password = "Qwerty123";
        }

        public FileBase(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }


        public async Task UploadFileAsync(string localPath, string remotePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/" + remotePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_username, _password);

                byte[] buffer = new byte[1024];
                FileStream localStream = File.OpenRead(localPath);

                Stream requestStream = await request.GetRequestStreamAsync();

                int bytesRead;
                while ((bytesRead = await localStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await requestStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
            catch (WebException ex)
            {

                MessageBox.Show("Ошибка: " + ex.Message);
            }


        }


        public async Task DownloadFileAsync(string remotePath, string localPath)
        {
            FtpWebRequest
                request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/" + remotePath);

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);
            FtpWebResponse
                response = (FtpWebResponse)await request.GetResponseAsync();
            Stream
                responseStream = response.GetResponseStream();
            FileStream
                localStream = File.OpenWrite(localPath);
            byte[]
                buffer = new byte[1024];
            long
                totalBytesDownloaded = 0;
            int
                bytesRead;
            System.Timers.Timer timer = new System.Timers.Timer(100); // Интервал обновления в миллисекундах
            timer.Start();

            while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                localStream.Write(buffer, 0, bytesRead);
                totalBytesDownloaded += bytesRead;
            }

            timer.Stop();
            localStream.Close();
        }
        public async Task<BitmapImage> LoadImageFromFtpAsync(string remotePath)
        {
            FtpWebRequest
                request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/Bulat_files/" + remotePath);

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);

            BitmapImage
                bitmapImage = new BitmapImage();

            try
            {
                FtpWebResponse
                    response = (FtpWebResponse)await request.GetResponseAsync();
                Stream
                    responseStream = response.GetResponseStream();
                MemoryStream
                    memoryStream = new MemoryStream();

                await responseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при загрузке изображения: " + ex.Message);
                return null;
            }

            return bitmapImage;
        }
        public async Task<string> GetShareableImageLink(string remotePath)
        {
            string
                shareLink = "ftp://" + _username + ":" + _password + "@" + _host + ":" + _port + "/Bulat_files/" + WebUtility.UrlEncode(remotePath);

             return shareLink;
        }
    }
}
