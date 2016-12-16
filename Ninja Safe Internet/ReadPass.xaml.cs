using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Ninja_Safe_Internet
{
    /// <summary>
    /// Логика взаимодействия для ReadPass.xaml
    /// </summary>
    public partial class ReadPass : Window
    {

       /* private static ReadPass instance;

        public static ReadPass getInstance()
        {
            if (instance == null)
                instance = new ReadPass();
            return instance;
        }*/
        public ReadPass()
        {
            InitializeComponent();
        }

        public static string action;

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Config.password == passRead.Text)
            {
                switch (action)
                {
                    case "open":
                        Application.Current.MainWindow.Visibility = Visibility.Visible;
                        Close();
                            break;
                    case "close":

                        foreach (Process winProc in Process.GetProcesses())
                        {
                            if (winProc.ProcessName == "ChekApp")
                            {

                                Process aaa = Process.GetProcessById(winProc.Id);
                                aaa.Kill();
                                /*ProcessStartInfo psi = new ProcessStartInfo("taskkill", @"/f /im tor.exe ");
                                Process.Start(psi);*/

                            }

                        }

                        //Process proc = new Process();
                        //Запускаем Блокнто
                        /*ProcessStartInfo psi = new ProcessStartInfo("taskkill", @"/f /im chekapp.exe ");
                        Process.Start(psi); */

                        HideProcess("HideProcess", false);

                        Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Невірний пароль", "Помилка");
            }
        }
    }
}
