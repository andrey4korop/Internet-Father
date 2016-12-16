using System;
using System.Windows.Forms;

namespace Інтернет_Батько
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainform = MainForm.getInstance();
            Hosts hosts = Hosts.getInstance();
            Http http = Http.getInstance();
            TrayIcon trayicon = TrayIcon.getInstance();
            if (Config.cookie == "" && MainForm.winopen == false)
            {
                SetPass setpass = SetPass.getInstance();
                setpass.Show();
                //    setpass.Owner = this;
                //  setpass.Show();
                // setpass.
            }
            else
            {
                if (Config.key == "")
                {

                }
                else
                {
                    switch (http.HttpData("key", Config.key, Config.cookie))
                    {
                        case "key_yes":
                            Config.license = "license_yes";
                            mainform.TimerHost.Enabled = true;
                            //TextKey.Enabled = false;
                            //timerHost.SetTimer(true);
                            trayicon.trayicon.ShowBalloonTip(500, "Інтернет Батько", "Защита включена", System.Windows.Forms.ToolTipIcon.Info);
                            //notifyIcon1.BalloonTipText()
                            //notifyIcon1.ShowBalloonTip(50);
                            hosts.SaveHosts();
                            //ShowWindow(FindWindow(null, this.Text), SW_SHOWMINIMIZED);
                            //WindowState = FormWindowState.Minimized;

                            break;
                        case "key_no":
                            Config.license = "license_no";
                            mainform.TimerHost.Enabled = false;
                            trayicon.trayicon.ShowBalloonTip(1000, "Інтернет Батько", "Термін дії ліцензії закінчився", System.Windows.Forms.ToolTipIcon.Error);
                            //timerHost.SetTimer(false);
                            //mainform.TextKey.Enabled = true;
                            hosts.DeleteHosts();

                            break;
                        case "cookie_no":
                            Config.license = "license_no";
                            mainform.TimerHost.Enabled = false;
                            //TextKey.Enabled = true;
                            //timerHost.SetTimer(false);
                            hosts.DeleteHosts();
                            trayicon.trayicon.ShowBalloonTip(500, "Інтернет Батько", "Цей ключ вже використовується на іншому комп'ютері.", System.Windows.Forms.ToolTipIcon.Warning);
                            break;
                        default:
                            // if (Config.license == "license_yes")
                            // TextKey.Enabled = false;
                            // Hide();
                            break;
                    }
                }
            }
        
        Application.Run();
        }
    }
}
