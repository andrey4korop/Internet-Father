using System;
using System.Windows.Forms;

namespace Інтернет_Батько
{
    public partial class SetPass : Form
    {
        private static SetPass instance;
        public static SetPass getInstance()
        {
            if (instance == null)
                instance = new SetPass();
            return instance;
        }
        private SetPass()
        {
            InitializeComponent();
        }
        public MainForm mainform = MainForm.getInstance();
        private void button1_Click(object sender, EventArgs e)
        {
            if (Config.cookie == "")
            {
                Random ran = new Random();
                string cookie = "";
                for (byte i = 0; i < 20; i++)
                {
                    cookie += Convert.ToString(Convert.ToChar(ran.Next(65, 90)));

                }
                Config.cookie = cookie;

            }
            if (textpass.Text == "")
                MessageBox.Show("Пустий пароль. Введіть пароль.", "Помилка");
            else
            {
                Config.password = textpass.Text;
               // mainform.WindowState = FormWindowState.Normal;
                mainform.Show();
                Close();
            }
        }

        private void SetPass_Activated(object sender, EventArgs e)
        {
            MainForm.winopen = true;
        }

        private void SetPass_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void SetPass_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MainForm.winopen = false;
            if (Config.password == "")
            {
                //Close();
                e.Cancel = true;
            }
            else
            {
                
                mainform.Show();
               // mainform.WindowState = FormWindowState.Normal;
                //e.Cancel = false;
            }
        }
    }
}
