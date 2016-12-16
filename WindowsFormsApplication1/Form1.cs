using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string path = Directory.GetCurrentDirectory();
        public Form1()
        {
            InitializeComponent();
            


        }

        private void Timer(object sender, EventArgs e)
        {

            try
            {
                bool flag = false;
                foreach (Process winProc in Process.GetProcesses())
                {
                    if (winProc.ProcessName == "InternetFather")
                    {
                        flag = true;

                    }

                }
                if (!flag)
                {
                    //создаем новый процесс
                    Process proc = new Process();
                    //Запускаем Блокнто
                    proc.StartInfo.FileName = @path + "\\InternetFather.exe";
                    proc.Start();
                }

            }
            catch (Exception e1)
            {
                
            }


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {

                foreach (Process winProc in Process.GetProcesses())
                {
                    if (winProc.ProcessName == "tor")
                    {

                        Process tor = Process.GetProcessById(winProc.Id);
                        tor.Kill();
                        /*ProcessStartInfo psi = new ProcessStartInfo("taskkill", @"/f /im tor.exe ");
                        Process.Start(psi);*/

                    }

                }
            }
            catch (Exception e1)
            {


            }
}
    }
}
