using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Інтернет_Батько
{
    public class Http
    {
        private string uri;
        private static Http instance;
        TrayIcon trayicon = TrayIcon.getInstance();
        private string host;

        public static Http getInstance()
        {
            if (instance == null)
                instance = new Http();
            return instance;
        }

        public Http()
        {
            // this.uri = "http://localhost/ninja.php?id=black";

            host = Config.host;

            string ret = "";
            try
            {
                this.uri = "http://"+ host + "/ninja.php?id=host";
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
                if (host != ret && ret!="")
                {
                    Config.host = ret;
                }
            }
            catch
            {
            }
            host = Config.host;
        }

        public List<string> HttpLoadBlack()
        {
            List<string> ret = new List<string>();
            try
            {

                this.uri = "http://" + host + "//ninja.php?id=black";
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
               
                
                
               
                return ret;
            }
        }

        public List<string> HttpLoadBlackApp()
        {
            List<string> ret = new List<string>();
            try
            {

                this.uri = "http://" + host + "//ninja.php?id=blackapp";
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


                ret.Add("tor");

                return ret;
            }
        }



        public string HttpData(string par, string val, string cookie)
        {
            string ret="";
            try
            {
                this.uri = "http://" + host + "/ninja.php?id=" + par + "&val=" + val + "&cookie=" + cookie;
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
                trayicon.trayicon.ShowBalloonTip(1000, "Інтернет Батько", "Відсутнє з'єднання з Інтернетом", System.Windows.Forms.ToolTipIcon.Error);
                return ret;
            }
            
        }

        public static string HttpCheckInet(string par, string val, string cookie)
        {
            string ret = "";
            try
            {
                string uri = "http://" + Config.host + "/ninja.php?id=" + par + "&val=" + val + "&cookie=" + cookie;
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = "GET";
                request.Timeout = 2000;
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
