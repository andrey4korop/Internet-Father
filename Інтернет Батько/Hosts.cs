using System;
using System.Collections.Generic;
using System.IO;


namespace Інтернет_Батько
{
    public class Hosts
    {
        Http http = Http.getInstance();
        TrayIcon trayicon = TrayIcon.getInstance();
        private string ip = "127.0.0.1";
        private string httprequest;
        
        private string path = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";
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
            httprequest = http.HttpData("ver_black", "", Config.cookie);
            if ((httprequest != Config.blackVer)||(black.Count==0))
            {
                Config.blackVer = httprequest;
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
                    if (File.ReadAllLines(path).GetLength(0) != black.Count)
                    {
                        File.WriteAllLines(path, black.ToArray());
                    }
                }
                catch
                {
                    trayicon.trayicon.ShowBalloonTip(1000, "Інтернет Батько", "Проблема з записом файла", System.Windows.Forms.ToolTipIcon.Error);
                }
            
            //black.Clear();
        }

        public void DeleteHosts()
        {   
            black.Clear();
            black.Add("127.0.0.1 localhost");
            try
            {
                File.WriteAllLines(path, black.ToArray());
            }
            catch
            {
                trayicon.trayicon.ShowBalloonTip(1000, "Інтернет Батько", "Проблема з записом файла", System.Windows.Forms.ToolTipIcon.Error);
            }
            black.Clear();
        }
    }
}
