using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ninja_Safe_Internet
{
    class TimerCheckInet
    {
        // private Http http = Http.getInstance();
        TimerHost timerhost = TimerHost.getInstance();
        private TrayIcon trayicon = TrayIcon.getInstance();
        private static TimerCheckInet instance;

        public static TimerCheckInet getInstance()
        {
            if (instance == null)
                instance = new TimerCheckInet();
            return instance;
        }
        
        public DispatcherTimer Timer;

        private void Tick(object sender, EventArgs e)
        {
            if (Http.HttpCheckInet("ver_black", "", Config.cookie) != "")
            {
                SetTimer(false);
                timerhost.SetTimer(true);
            }
              
        }
        public TimerCheckInet()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Tick);
            Timer.Interval = new TimeSpan(0, 0, 10);


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
