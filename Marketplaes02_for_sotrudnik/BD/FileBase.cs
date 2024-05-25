using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;

namespace Marketplaes02_for_sotrudnik.BD
{
    public class FileBase 
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;

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


        public void UploadFile(string localPath, string remotePath)
        {

            FtpWebRequest
                request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/" + remotePath);

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_username, _password);

            byte[]
                buffer = new byte[1024];
            FileStream
                localStream = File.OpenRead(localPath);
            Stream
                requestStream = request.GetRequestStream();
            long
                totalBytesUploaded = 0;
            int
                bytesRead;

            System.Timers.Timer
                timer = new System.Timers.Timer(100); // Интервал обновления в миллисекундах

            timer.Start();

            while ((bytesRead = localStream.Read(buffer, 0, buffer.Length)) > 0 )
            {
                requestStream.Write(buffer, 0, bytesRead);
                totalBytesUploaded += bytesRead;
            }

            timer.Stop();
            requestStream.Close();
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

            while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0 )
            {
                localStream.Write(buffer, 0, bytesRead);
                totalBytesDownloaded += bytesRead;
            }

            timer.Stop();
            localStream.Close();
        }
    }
}
