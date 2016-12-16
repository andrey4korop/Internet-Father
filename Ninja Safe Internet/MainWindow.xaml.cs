using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;


namespace Ninja_Safe_Internet
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // Config config = new Config();
        Hosts hosts =Hosts.getInstance();
        TimerHost timerHost = TimerHost.getInstance();
        TrayIcon trayicon = TrayIcon.getInstance();
        Http http = Http.getInstance();
        public static View list1;
        public MainWindow()
        {
             InitializeComponent();

            SuperTimer timer = SuperTimer.getInstance();
            
            //Http http = new Http();
            switch (http.HttpData("key", Config.key, Config.cookie))
            {
                case "key_yes":
                    Config.license = "license_yes";
                    timerHost.SetTimer(true);
                    trayicon.trayicon.ShowBalloonTip(500, "Ninja Sefe Internet", "Защита включена", System.Windows.Forms.ToolTipIcon.Info);
                    hosts.SaveHosts();
                    Hide();
                    break;
                case "key_no":
                    Config.license = "license_no";
                    timerHost.SetTimer(false);
                    hosts.DeleteHosts();
                    break;
                case "cookie_no":
                    Config.license = "license_no";
                    timerHost.SetTimer(false);
                    hosts.DeleteHosts();
                    trayicon.trayicon.ShowBalloonTip(500, "Ninja Sefe Internet", "Цей ключ вже використовується на іншому комп'ютері.", System.Windows.Forms.ToolTipIcon.Warning);
                    break;
                default:
                    if(Config.license=="license_yes")
                    Hide();
                    break;
            }



               
        }
                
        private void buttonKey_Click(object sender, RoutedEventArgs e)
        {
            if (TextKey.Text == "")
            {
                MessageBox.Show("Ви не ввели код авторизації","Помилка");
                hosts.DeleteHosts();
            }
            else
            {

                switch (http.HttpData("key", TextKey.Text, Config.cookie))
                {
                    case "key_yes":
                        Config.license = "license_yes";
                        Config.key = TextKey.Text;
                        timerHost.SetTimer(true);
                        hosts.SaveHosts();
                        Hide();
                        MessageBox.Show("Код авторизації введено вірно. Всім доброго дня!");
                        trayicon.trayicon.ShowBalloonTip(500, "Ninja Sefe Internet", "Защита включена", System.Windows.Forms.ToolTipIcon.Info);
                        break;
                    case "key_no":
                        Config.key = TextKey.Text;
                        Config.license = "license_no";
                        hosts.DeleteHosts();
                        MessageBox.Show("Код авторизації введено не вірно. Введіть ключ ще раз", "Помилка");
                        break;
                    case "cookie_no":
                        Config.key = TextKey.Text;
                        Config.license = "license_no";
                        hosts.DeleteHosts();
                        MessageBox.Show("Цей ключ вже використовується на іншому комп'ютері.", "Помилка");
                        break;
                    default:
                        Config.key = TextKey.Text;
                        Config.license = "license_no";
                        trayicon.trayicon.ShowBalloonTip(1000, "Ninja Sefe Internet", "Відсутнє з'єднання з Інтернетом. Зайдіть пізніше", System.Windows.Forms.ToolTipIcon.Error);
                        Hide();
                        break;
                }
            }
                
        }


        private void buttonSP_Click(object sender, RoutedEventArgs e)
        {


           
        }

       
        
       public static bool winopen = false;

        private void InitLoad(object sender, EventArgs e)
        {
            if (Config.cookie == null && winopen==false)
            {
                SetPassword setpass = SetPassword.getInstance();
                setpass.Owner = this;
                setpass.Show();

                // setpass.
            }
        }

        private void Close_App(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void buttonZV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
