using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ninja_Safe_Internet
{
    public class Http
    {
        private string uri;
        TrayIcon trayicon = TrayIcon.getInstance();
        private static Http instance;

        public static Http getInstance()
        {
            if (instance == null)
                instance = new Http();
            return instance;
        }

        public Http()
        {
           // this.uri = "http://46.101.224.13/ninja.php?id=black";
        }
        public List<string> HttpLoadBlack()
        {
            List<string> ret = new List<string>();
            try
            {

                //this.uri = "http://46.101.224.13/ninja.php?id=black";
                this.uri = "http://localhost/ninja.php?id=black";
                HttpWebRequest request = WebRequest.Create(this.uri) as HttpWebRequest;
                request.Method = "GET";
                request.UserAgent = "NinjaSafeInternet";
                HttpWebResponse respouse = request.GetResponse() as HttpWebResponse;

                string enc = "utf-8";
                string ct = respouse.Headers["Content-Type"];
                int n = ct.IndexOf("charset =");
                if (n >= 0) enc = ct.Substring(n + 8);

               
                StreamReader stream = new StreamReader(respouse.GetResponseStream(), Encoding.GetEncoding(enc));
                while (!stream.EndOfStream)
                {
                    ret.Add((string)stream.ReadLine());
                }

                stream.Close();
                return ret;
            }
            catch 
            {
                TimerCheckInet timercheckinet = TimerCheckInet.getInstance();
                timercheckinet.SetTimer(true);
                trayicon.trayicon.ShowBalloonTip(1000, "Ninja Safe Internet", "Відсутнє з'єднання з Інтернетом", System.Windows.Forms.ToolTipIcon.Error);
                return ret;
            }
        }



        public string HttpData(string par, string val, string cookie)
        {
            string ret="";
            try
            {
                this.uri = "http://localhost/ninja.php?id=" + par + "&val=" + val + "&cookie=" + cookie;
                HttpWebRequest request = WebRequest.Create(this.uri) as HttpWebRequest;
                request.Method = "GET";
                request.UserAgent = "NinjaSafeInternet";
                HttpWebResponse respouse = request.GetResponse() as HttpWebResponse;

                string enc = "utf-8";
                string ct = respouse.Headers["Content-Type"];
                int n = ct.IndexOf("charset =");
                if (n >= 0) enc = ct.Substring(n + 8);

                StreamReader stream = new StreamReader(respouse.GetResponseStream(), Encoding.GetEncoding(enc));

                
                    ret = (string)stream.ReadLine();
                stream.Close();
                return ret;
            }
            catch 
            {
                TimerCheckInet timercheckinet = TimerCheckInet.getInstance();
                timercheckinet.SetTimer(true);
                trayicon.trayicon.ShowBalloonTip(1000, "Ninja Safe Internet", "Відсутнє з'єднання з Інтернетом", System.Windows.Forms.ToolTipIcon.Error);
                return ret;
            }
            
        }

        public static string HttpCheckInet(string par, string val, string cookie)
        {
            string ret = "";
            try
            {
                string uri = "http://localhost/ninja.php?id=" + par + "&val=" + val + "&cookie=" + cookie;
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = "GET";
                request.UserAgent = "NinjaSafeInternet";
                HttpWebResponse respouse = request.GetResponse() as HttpWebResponse;

                string enc = "utf-8";
                string ct = respouse.Headers["Content-Type"];
                int n = ct.IndexOf("charset =");
                if (n >= 0) enc = ct.Substring(n + 8);

                StreamReader stream = new StreamReader(respouse.GetResponseStream(), Encoding.GetEncoding(enc));


                ret = (string)stream.ReadLine();
                stream.Close();
                return ret;
            }
            catch 
            {
                //trayicon.trayicon.ShowBalloonTip(1000, "Ninja Safe Internet", "Відсутнє з'єднання з Інтернетом", System.Windows.Forms.ToolTipIcon.Error);
                return ret;
            }

        }


    }
}
