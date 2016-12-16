using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ninja_Safe_Internet
{
    class TimerHost
    {
        private Http http = new Http();
        private TrayIcon trayicon = TrayIcon.getInstance();
        private static TimerHost instance;

        public static TimerHost getInstance()
        {
            if (instance == null)
                instance = new TimerHost();
            return instance;
        }
        Hosts hosts = Hosts.getInstance();
        public DispatcherTimer Timer;

        private void Tick(object sender, EventArgs e)
        {
            if (http.HttpData("license", Config.key, Config.cookie) == "license_yes")
            {
                hosts.SaveHosts();
                Config.license = "license_yes";
            }
            if (http.HttpData("license", Config.key, Config.cookie) == "license_no")
            {
                SetTimer(false);
                hosts.DeleteHosts();
                trayicon.trayicon.ShowBalloonTip(500, "Ninja Sefe Internet", "Термін дії ліцензії закінчився.", System.Windows.Forms.ToolTipIcon.Warning);
                Config.license = "license_no";
            }
            if (http.HttpData("license", Config.key, Config.cookie) == "cookie_no")
            {
                SetTimer(false);
                hosts.DeleteHosts();
                trayicon.trayicon.ShowBalloonTip(500, "Ninja Sefe Internet", "Цей ключ вже використовується на іншому комп'ютері.", System.Windows.Forms.ToolTipIcon.Warning);
                Config.license = "license_no";
            }
            if (http.HttpData("license", Config.key, Config.cookie) == "")
            {
                SetTimer(false);
            }
            
        }
        public TimerHost()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Tick);
            Timer.Interval = new TimeSpan(0, 0, 30);


        }

        public void SetTimer(bool par)
        {
            if (par)
                Timer.Start();

            else
            {
                Timer.Stop();
            }
        }
    }
}
