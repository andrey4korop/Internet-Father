using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ninja_Safe_Internet
{
    class TrayIcon
    {
       // Http http = Http.getInstance();

        private static TrayIcon instance;

        public static TrayIcon getInstance()
        {
            if (instance == null)
                instance = new TrayIcon();
            return instance;
        }



        public System.Windows.Forms.NotifyIcon trayicon;

        public TrayIcon()
        {
            trayicon = new System.Windows.Forms.NotifyIcon();
            trayicon.Icon = new System.Drawing.Icon(@"C:\Users\andre\OneDrive\проэкт\БезопасныйИнтернет\Ninja Safe Internet\Ninja Safe Internet\Images\icon.ico");
            trayicon.Visible = true;
            trayicon.MouseClick += new System.Windows.Forms.MouseEventHandler(trayicon_MouseClick);
            //if ( http.HttpData("key", Config.key, Config.cookie) == "key_yes")
            //{
            //    trayicon.ShowBalloonTip(500, "Ninja Sefe Internet", "Защита включена", System.Windows.Forms.ToolTipIcon.Info);
            //}
            

            System.Windows.Forms.ContextMenu trayicon_contextMenu = new System.Windows.Forms.ContextMenu();
            trayicon_contextMenu.MenuItems.Add("Відкрити", new EventHandler(open));
            trayicon_contextMenu.MenuItems.Add("Закрити", new EventHandler(close));
            trayicon.ContextMenu = trayicon_contextMenu;

        }

        private void open(object sendler, EventArgs e)
        {
            ReadPass.action = "open";
            ReadPass readpass = new ReadPass();
            //setpass.Owner = this;
            readpass.Show();
        }
        private void close(object sendler, EventArgs e)
        {
            ReadPass.action = "close";
            ReadPass readpass = new ReadPass();
            //setpass.Owner = this;
            readpass.Show();
        }

        private void trayicon_MouseClick (object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
