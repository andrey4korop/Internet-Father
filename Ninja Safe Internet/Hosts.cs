using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninja_Safe_Internet
{
   public class Hosts
    {
        Http http = Http.getInstance();
        TrayIcon trayicon = TrayIcon.getInstance();
        private string ip = "46.101.224.13";
        public List<string> black = new List<string>();

        private static Hosts instance;

        public static Hosts getInstance()
        {
            if (instance == null)
                instance = new Hosts();
            return instance;
        }
        public void GetBlack()
        {
            if ((Convert.ToInt16(http.HttpData("ver_black","",Config.cookie)) > Convert.ToInt16(Config.blackVer))||(black.Count==0))
            {
                Config.blackVer = http.HttpData("ver_black", "", Config.cookie);
                black.Clear();
                List<string> temp = http.HttpLoadBlack();
                black.Add("127.0.0.1 localhost");
                foreach (string it in temp)
                {
                    black.Add(ip + " " + it);
                }
            }
            
        }

        public void SaveHosts()
        {

            GetBlack();
            
            
                try
                {
                    if (File.ReadAllLines(@"C:\Windows\System32\drivers\etc\hosts").Count() != black.Count)
                        File.WriteAllLines(@"C:\Windows\System32\drivers\etc\hosts", black);
                }
                catch
                {
                    trayicon.trayicon.ShowBalloonTip(1000, "Ninja Safe Internet", "Проблема з записом файла", System.Windows.Forms.ToolTipIcon.Error);
                }
            
            //black.Clear();
        }

        public void DeleteHosts()
        {   
            black.Clear();
            black.Add("127.0.0.1 localhost");
            try
            {
                File.WriteAllLines(@"C:\Windows\System32\drivers\etc\hosts", black);
            }
            catch
            {
                trayicon.trayicon.ShowBalloonTip(1000, "Ninja Safe Internet", "Проблема з записом файла", System.Windows.Forms.ToolTipIcon.Error);
            }
            black.Clear();
        }
    }
}
