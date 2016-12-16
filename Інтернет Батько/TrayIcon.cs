using System;
using System.IO;

namespace Інтернет_Батько
{
    class TrayIcon
    {
        // Http http = Http.getInstance();
        private MainForm mainform { get; set; }
        
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
            string path = Directory.GetCurrentDirectory();
            trayicon = new System.Windows.Forms.NotifyIcon();
            trayicon.Icon = new System.Drawing.Icon(@path+"\\icon.ico");
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
            MainForm.action = "open";
           
           ReadPass readpass = new ReadPass();
            //setpass.Owner = this;
            readpass.Show();
           
        }
        

        private void close(object sendler, EventArgs e)
        {
            MainForm.action = "close";
            // ReadPass.action = "close";
               ReadPass readpass = new ReadPass();
            //setpass.Owner = this;
            //    readpass.Show();
            readpass.Show();
        }

        private void trayicon_MouseClick (object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
