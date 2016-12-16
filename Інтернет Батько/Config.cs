using Microsoft.Win32;

namespace Інтернет_Батько
{
    public class Config
    {
        private static string keyName = "HKEY_CURRENT_USER\\NitanTest";

        public static string license
        { 
            get { return getval("licence"); }
            set { setval( "licence", value); }
        }
        public static string host
        {
            get { return getvalhost("host"); }
            set { setvalhost("host", value); }
        }
        public static string cookie
        {
            get { return getval( "cookie"); }
            set { setval( "cookie", value); }
        }
        public static string blackVer
        {
            get { return getval("black_ver"); }
            set { setval( "black_ver", value); }
        }
        public static string password
        {
            get { return getval( "password"); }
            set { setval( "password", value); }
        }
        public static string key
        {
            get { return getval( "key"); }
            set { setval( "key", value); }
        }

        public static string getval(string keyval)
        {
            string ret = "";
            try
            {
                ret = (string)Registry.GetValue(keyName, keyval, null);
                if (ret == null)
                {
                    Registry.SetValue(keyName, keyval, "");
                    ret = (string)Registry.GetValue(keyName, keyval, null);
                }
            }
            catch
            {
                Registry.SetValue(keyName, keyval, "");
            }
            return ret;
            
        }

        public static void setval(string keyval, string val)
        {
            
            try
            {
                Registry.SetValue(keyName, keyval, val);
            }
            catch
            {
                Registry.SetValue(keyName, keyval, "");
            }
           
        }


        ////////////////////////////////////////////////
        public static string getvalhost(string keyval)
        {
            string ret = "";
            try
            {
                ret = (string)Registry.GetValue(keyName, keyval, null);
                if (ret == null)
                {
                    Registry.SetValue(keyName, keyval, "138.68.98.242");
                    ret = (string)Registry.GetValue(keyName, keyval, null);
                }
            }
            catch
            {
                Registry.SetValue(keyName, keyval, "138.68.98.242");
            }
            return ret;

        }

        public static void setvalhost(string keyval, string val)
        {

            try
            {
                Registry.SetValue(keyName, keyval, val);
            }
            catch
            {
                Registry.SetValue(keyName, keyval, "138.68.98.242");
            }

        }




    }
}
