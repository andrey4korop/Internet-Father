using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninja_Safe_Internet
{
    public class Config
    {
        private static string keyName = "HKEY_CURRENT_USER\\NitanTest";
        public static string license
        {
            get { return (string)Registry.GetValue(keyName, "licence", null); }
            set { Registry.SetValue(keyName, "licence", value); }
        }
        public static string cookie
        {
            get { return (string)Registry.GetValue(keyName, "cookie", null); }
            set { Registry.SetValue(keyName, "cookie", value); }
        }
        public static string blackVer
        {
            get { return (string)Registry.GetValue(keyName, "black_ver", null); }
            set { Registry.SetValue(keyName, "black_ver", value); }
        }
        public static string password
        {
            get { return (string)Registry.GetValue(keyName, "password", null); }
            set { Registry.SetValue(keyName, "password", value); }
        }
        public static  string key
        {
            get { return (string)Registry.GetValue(keyName, "key", null); }
            set { Registry.SetValue(keyName, "key", value); }
        }
        
        
    }
}
