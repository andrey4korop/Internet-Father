using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Інтернет_Батько
{
    public partial class ReadPass : Form
    {

        public ReadPass()
        {
            InitializeComponent();
            
            
    }
        //public string action;
        private void ReadPass_Load(object sender, EventArgs e)
        {

        }
        public MainForm mainform = MainForm.getInstance();
        private void button1_Click(object sender, EventArgs e)
        {
            if (Config.password == passRead.Text)
            {
                switch (MainForm.action)
                {
                    case "open":
                        //mainform.WindowState = FormWindowState.Normal;
                        //MainForm.ShowWindow(MainForm.FindWindow(null, mainform.Text), MainForm.SW_SHOWNORMAL);
                        //Application.Current.MainWindow.Visibility = Visibility.Visible;
                        Close();
                        mainform.Show();
                        
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

                        // HideProcess("HideProcess", false);
                        //
                        Environment.Exit(0);
                        //mainform.HideProcess("HideProcess", false);
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
