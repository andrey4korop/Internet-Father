using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ninja_Safe_Internet;

namespace Ninja_Safe_Internet
{
    /// <summary>
    /// Логика взаимодействия для SetPassword.xaml
    /// </summary>
    public partial class SetPassword : Window
    {
        private static SetPassword instance;

        public static SetPassword getInstance()
        {
            if (instance == null)
                instance = new SetPassword();
            return instance;
        }


        public SetPassword()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Config.cookie == null)
            {
                Random ran = new Random();
                string cookie = "";
                for (byte i = 0; i < 20; i++)
                {
                    cookie += Convert.ToString(Convert.ToChar(ran.Next(65, 90)));
                    
                }
                Config.cookie = cookie;

            }
            if (textpass.Text == "")
                MessageBox.Show("Пустий пароль. Введіть пароль.","Помилка");
            else
            {
                Config.password = textpass.Text;
                
                Close();
            }
        }
        

        private void Windows_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.winopen = false;
            if (Config.password == null)
            {
                e.Cancel = true;
            }
            else e.Cancel = false;

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MainWindow.winopen = true;

        }

      
    }
}
