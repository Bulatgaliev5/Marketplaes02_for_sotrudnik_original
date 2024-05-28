using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Marketplaes02_for_sotrudnik.BD
{
    public class ConnectBD
    {
        // Строка подключения
        private static readonly string txt =
        "Server=37.18.74.116;" +
        "Port=3309;" +
        "Database=dp_bulat_base;" +
        "UserID=g_bulat;" +
        "Password=bulat.123;" +
        "CharacterSet=utf8mb4;" +
        "ConvertZeroDatetime=True;" +
        "AllowZeroDatetime=True; Allow User Variables = True";

        private readonly MySqlConnection con = new MySqlConnection(txt);

        /// <summary>
        /// Метод синхронного подключения к БД с объекта подключения 
        /// </summary>
        /// <returns></returns>
        public async Task GetConnectBD()
        {
                try
                {
                    await con.OpenAsync();
                }
                catch (MySqlException ex)
                {
                         MessageBox.Show("Не удалось подключиться к серверу. Попробуйте попозже или же сообщите администратору: " + ex.Message);

                }

        }
        /// <summary>
        /// Метод синхронного отключения от БД с объекта подключения
        /// </summary>
        /// <returns></returns>
        public async Task GetCloseBD() => await con.CloseAsync();

        /// <summary>
        /// Метод возвращения объекта подключения
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnBD() => con;
    }
}
