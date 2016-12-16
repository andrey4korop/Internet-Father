using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
using System.Collections.Generic;

namespace Інтернет_Батько
{
    public partial class MainForm : Form
    {

        string path = Directory.GetCurrentDirectory();

        const Int32 WM_COMMAND = 273;//0x111;
        const Int32 MF_ENABLED = 0;//0x0;
        const Int32 MF_GRAYED = 1;//0x1;
        const Int32 LVM_FIRST = 4096;//0x1000;
        const Int32 LVM_DELETEITEM = (LVM_FIRST + 8);
        const Int32 LVM_SORTITEMS = (LVM_FIRST + 48);

        [DllImport("user32", EntryPoint = "FindWindowA",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiFindWindow(string lpClassName,
                                                  string lpWindowName);

        [DllImport("user32", EntryPoint = "FindWindowExA",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiFindWindowEx(Int32 hWnd1,
                                                   Int32 hWnd2,
                                                   string lpsz1,
                                                   string lpsz2);

        [DllImport("user32", EntryPoint = "EnableWindow",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern bool apiEnableWindow(Int32 hwnd,
                                                   Int32 fEnable);

        [DllImport("user32", EntryPoint = "GetMenu",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiGetMenu(Int32 hwnd);

        [DllImport("user32", EntryPoint = "GetSubMenu",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiGetSubMenu(Int32 hMenu,
                                                  Int32 nPos);

        [DllImport("user32", EntryPoint = "GetMenuItemID",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiGetMenuItemID(Int32 hMenu,
                                                     Int32 nPos);

        [DllImport("user32", EntryPoint = "EnableMenuItem",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiEnableMenuItem(Int32 hMenu,
                                                      Int32 wIDEnableItem,
                                                      Int32 wEnable);

        [DllImport("user32", EntryPoint = "SendMessageA",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiSendMessage(Int32 hWnd,
                                                   Int32 wMsg,
                                                   Int32 wParam,
                                                   Int32 lParam);

        [DllImport("user32", EntryPoint = "GetDesktopWindow",
                             CharSet = CharSet.Ansi,
                             SetLastError = true,
                             ExactSpelling = true)]
        private static extern Int32 apiGetDesktopWindow();

        [DllImport("user32", EntryPoint = "LockWindowUpdate",
                            CharSet = CharSet.Ansi,
                            SetLastError = true,
                            ExactSpelling = true)]
        private static extern Int32 apiLockWindowUpdate(Int32 hwndLock);

        private object HideProcess(string pName, bool pHide = true)
        {
            //Получение идентификатор диспетчера задач
            Int32 lhWndParent = apiFindWindow(null,
                "Диспетчер задач Windows");//"Windows Task Manager");  
            Int32 lhWndDialog = 0;
            Int32 lhWndProcessList = 0;
            Int32 lhWndProcessHeader = 0;

            //Идентификатор меню диспетчера задач
            Int32 hMenu = apiGetMenu(lhWndParent);

            //Идентификатор подменю меню "Вид"
            Int32 hSubMenu = apiGetSubMenu(hMenu, 2);

            //Идентификатор подменю Вид -> Скорость обновления
            Int32 hSubSubMenu = apiGetSubMenu(hSubMenu, 1);

            //Идентификатор меню Вид -> Обновить
            Int32 hId1 = apiGetMenuItemID(hSubMenu, 0);

            //Идентификатор подменю Вид -> Скорость обновления->Высокая
            Int32 hId2 = apiGetMenuItemID(hSubSubMenu, 0);

            //Идентификатор подменю Вид -> Скорость обновления->Обычная
            Int32 hId3 = apiGetMenuItemID(hSubSubMenu, 1);

            //Идентификатор подменю Вид -> Скорость обновления->Низкая
            Int32 hId4 = apiGetMenuItemID(hSubSubMenu, 2);

            //Идентификатор подменю Вид -> Скорость обновления->Приостановить
            Int32 hId5 = apiGetMenuItemID(hSubSubMenu, 3);

            if (pHide == true)
            {
                Int32 ProcessItemCount = default(Int32);
                Int32 ProcessItemIndex = default(Int32);
                string itemString = null;
                Process p = new Process();
                Process[] Processes = null;

                //Loop through all seven child windows,
                //for handles to the listviews, buttons, and header
                for (Int32 i = 1; i <= 7; i++)
                {
                    lhWndDialog = apiFindWindowEx(lhWndParent, lhWndDialog, null, null);
                    if (lhWndProcessList == 0)
                        //lhWndProcessList = apiFindWindowEx(lhWndDialog, 0, "SysListView32", "Processes");
                        lhWndProcessList = apiFindWindowEx(lhWndDialog, 0,
                                           "SysListView32", "Процессы");
                    if (lhWndProcessHeader == 0)
                        lhWndProcessHeader = apiFindWindowEx(lhWndProcessList, 0,
                                           "SysHeader32", null);
                }

                //Выполняем остановку обновления списка процессов
                //Кнопка Вид-> Скорость обновления-> "Приостановить" 
                apiSendMessage(lhWndParent, WM_COMMAND, hId5, 0);

                //Выполняем блокировку кнопки обновления списка процессов 
                //Вид-> Обновить
                apiEnableMenuItem(hMenu, hId1, MF_GRAYED);

                //Выполняем блокировку кнопки, устанавливающую 
                //высокую скорость обновления списка процессов 
                //Кнопка Вид-> Скорость обновления-> "Высокая" 
                apiEnableMenuItem(hMenu, hId2, MF_GRAYED);

                //Выполняем блокировку кнопки, устанавливающую
                //обычную скорость обновления списка процессов 
                //Кнопка Вид-> Скорость обновления-> "Обычная" 
                apiEnableMenuItem(hMenu, hId3, MF_GRAYED);

                //Выполняем блокировку кнопки, устанавливающую
                //низкую скорость обновления списка процессов 
                //Кнопка Вид-> Скорость обновления-> "Низкая" 
                apiEnableMenuItem(hMenu, hId4, MF_GRAYED);

                //Выполняем блокировку кнопки, обновления списка процессов 
                //Кнопка Вид-> "Обновить" 
                apiEnableMenuItem(hMenu, hId5, MF_GRAYED);

                //Во вкладке процессы, запрещаем сортировку по заголовку столбца
                apiEnableWindow(lhWndProcessHeader, 0);

                //Если в компоненте listView1, число элементов больше чем ноль
                //то выполняем его очистку
                if (this.listView1.Items.Count > 0)
                    this.listView1.Items.Clear();

                //Получение процессов, запущенных на компьютере
                Processes = Process.GetProcesses();

                //Выполняем подсчет процессов, и добавляем их в компонент listview1.
                foreach (Process p_loopVariable in Processes)
                {
                    p = p_loopVariable;
                    ProcessItemCount += 1;
                    if (p.ProcessName.ToString() == "Idle")
                    {
                        var _with1 = this.listView1.Items.Add("System Idle Process");
                    }
                    else
                    {
                        var _with2 = this.listView1.Items.Add(p.ProcessName.ToString());
                    }
                }

                //Выполняем поиск индекса процесса который был указан для скрытия
                for (Int32 z = 0; z <= ProcessItemCount - 1; z++)
                {
                    itemString = listView1.Items[z].Text.ToString();
                    if (itemString == pName)
                        ProcessItemIndex = z;
                }

                //Фиксируем окно диспетчера задач Windows
                //для устранения мигания
                apiLockWindowUpdate(lhWndProcessList);

                //Выполняем команду Вид-> Обновить
                apiSendMessage(lhWndParent, WM_COMMAND, hId1, 0);

                //Сортируем процессы по алфавиту
                apiSendMessage(lhWndProcessList, LVM_SORTITEMS, 0, 0);//null);

                //Удаляем процесс 
                apiSendMessage(lhWndProcessList, LVM_DELETEITEM, ProcessItemIndex, 0);

                //Для страховки, дополнительно проходимся по списку
                //и удаляем доступные процессы
                for (Int32 z = 0; z <= ProcessItemCount - 1; z++)
                {
                    itemString = listView1.Items[z].Text.ToString();
                    apiSendMessage(lhWndProcessList, LVM_DELETEITEM, z, 0);
                }

                //Разблокировка окна               
                apiLockWindowUpdate(0);

                if (lhWndParent == 0)
                {
                    //Время реакции, если диспетчер задач Windows закрыт
                    if (timer1.Interval != 800)
                        timer1.Interval = 800;
                }
                else
                {
                    //Устанавливаем таймер, на обычную скорость обновления, 
                    //если диспетчер задач Windows открыт
                    if (timer1.Interval != 2500)
                        timer1.Interval = 2500;
                }
            }
            else
            {
                //Отключаем таймер
                timer1.Enabled = false;

                for (Int32 i = 1; i <= 7; i++)
                {
                    lhWndDialog = apiFindWindowEx(lhWndParent, lhWndDialog, null, null);
                    if (lhWndProcessList == 0)
                        lhWndProcessList = apiFindWindowEx(lhWndDialog, 0, "SysListView32", "Процессы");
                    if (lhWndProcessHeader == 0)
                        lhWndProcessHeader = apiFindWindowEx(lhWndProcessList, 0, "SysHeader32", null);
                }

                //Разблокировка меню настроек меню Вид-> Скорость обновления

                //Кнопка обновить сейчас
                apiEnableMenuItem(hMenu, hId1, MF_ENABLED);

                //Кнопка Вид-> Скорость обновления-> "Высокая" скорость обновления
                apiEnableMenuItem(hMenu, hId2, MF_ENABLED);

                //Кнопка Вид-> Скорость обновления-> "Обычная" скорость обновления
                apiEnableMenuItem(hMenu, hId3, MF_ENABLED);

                //Кнопка Вид-> Скорость обновления-> "Низкая" скорость обновления
                apiEnableMenuItem(hMenu, hId4, MF_ENABLED);

                //Кнопка Вид-> Скорость обновления-> "Приостановить" скорость обновления
                apiEnableMenuItem(hMenu, hId5, MF_ENABLED);

                //Устанавливаем обычную скорость обновления 
                //Вид-> Скорость обновления ->Обычная
                apiSendMessage(lhWndParent, WM_COMMAND, hId3, 0);

                //Выполняем обновление списка процессов 
                //Вид-> Обновить
                apiSendMessage(lhWndParent, WM_COMMAND, hId1, 0);

                //Включаем сортировку по заголовку столбца, во вкладке "Процессы"
                apiEnableWindow(lhWndProcessHeader, 1);
            }
            return true;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// 


        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetKernelObjectSecurity(IntPtr Handle, int securityInformation, [Out] byte[] pSecurityDescriptor,
uint nLength, out uint lpnLengthNeeded);

        public static RawSecurityDescriptor GetProcessSecurityDescriptor(IntPtr processHandle)
        {
            const int DACL_SECURITY_INFORMATION = 0x00000004;
            byte[] psd = new byte[0];
            uint bufSizeNeeded;
            // Call with 0 size to obtain the actual size needed in bufSizeNeeded
            GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, psd, 0, out bufSizeNeeded);
            if (bufSizeNeeded < 0 || bufSizeNeeded > short.MaxValue)
                throw new Win32Exception();
            // Allocate the required bytes and obtain the DACL
            if (!GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION,
            psd = new byte[bufSizeNeeded], bufSizeNeeded, out bufSizeNeeded))
                throw new Win32Exception();
            // Use the RawSecurityDescriptor class from System.Security.AccessControl to parse the bytes:
            return new RawSecurityDescriptor(psd, 0);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool SetKernelObjectSecurity(IntPtr Handle, int securityInformation, [In] byte[] pSecurityDescriptor);

        public static void SetProcessSecurityDescriptor(IntPtr processHandle, RawSecurityDescriptor dacl)
        {
            const int DACL_SECURITY_INFORMATION = 0x00000004;
            byte[] rawsd = new byte[dacl.BinaryLength];
            dacl.GetBinaryForm(rawsd, 0);
            if (!SetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, rawsd))
                throw new Win32Exception();
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [Flags]
        public enum ProcessAccessRights
        {
            PROCESS_CREATE_PROCESS = 0x0080, //  Required to create a process.
            PROCESS_CREATE_THREAD = 0x0002, //  Required to create a thread.
            PROCESS_DUP_HANDLE = 0x0040, // Required to duplicate a handle using DuplicateHandle.
            PROCESS_QUERY_INFORMATION = 0x0400, //  Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken, GetExitCodeProcess, GetPriorityClass, and IsProcessInJob).
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000, //  Required to retrieve certain information about a process (see QueryFullProcessImageName). A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION. Windows Server 2003 and Windows XP/2000:  This access right is not supported.
            PROCESS_SET_INFORMATION = 0x0200, //    Required to set certain information about a process, such as its priority class (see SetPriorityClass).
            PROCESS_SET_QUOTA = 0x0100, //  Required to set memory limits using SetProcessWorkingSetSize.
            PROCESS_SUSPEND_RESUME = 0x0800, // Required to suspend or resume a process.
            PROCESS_TERMINATE = 0x0001, //  Required to terminate a process using TerminateProcess.
            PROCESS_VM_OPERATION = 0x0008, //   Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
            PROCESS_VM_READ = 0x0010, //    Required to read memory in a process using ReadProcessMemory.
            PROCESS_VM_WRITE = 0x0020, //   Required to write to memory in a process using WriteProcessMemory.
            DELETE = 0x00010000, // Required to delete the object.
            READ_CONTROL = 0x00020000, //   Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            SYNCHRONIZE = 0x00100000, //    The right to use the object for synchronization. This enables a thread to wait until the object is in the signaled state.
            WRITE_DAC = 0x00040000, //  Required to modify the DACL in the security descriptor for the object.
            WRITE_OWNER = 0x00080000, //    Required to change the owner in the security descriptor for the object.
            STANDARD_RIGHTS_REQUIRED = 0x000f0000,
            PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF),//    All possible access rights for a process object.
        }
        public void block()
        {
            // Get the current process handle
            IntPtr hProcess = GetCurrentProcess();
            // Read the DACL
            var dacl = GetProcessSecurityDescriptor(hProcess);
            // Insert the new ACE
            dacl.DiscretionaryAcl.InsertAce(
            0,
            new CommonAce(
            AceFlags.None,
            AceQualifier.AccessDenied,
            (int)ProcessAccessRights.PROCESS_ALL_ACCESS,
            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            false,
            null)
            );
            // Save the DACL
            SetProcessSecurityDescriptor(hProcess, dacl);
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>



        public static string action;
        Hosts hosts = Hosts.getInstance();
        Http http = Http.getInstance();
        TrayIcon trayicon = TrayIcon.getInstance();
        public static MainForm instance;
        public List<string> blackProg;

        public static MainForm getInstance()
        {
            if (instance == null)
                instance = new MainForm();
            return instance;
        }


        private MainForm()
        {
            InitializeComponent();

            block();
            TextKey.Text = Config.key;
            //Устанавливаем имя формы            
            //Text = "HideProcess";

            //Скрываем программу с панели задач Windows
            this.ShowInTaskbar = false;

            //Устанавливаем интервал срабатывания таймера
            timer1.Interval = 700;

            //Запускаем таймер
            timer1.Enabled = true;

            //Выставляем у listView настройку, 
            //что каждый элемент отображается в отдельной строке.
            listView1.View = View.Details;

            //Устанавливаем заголовок над столбцом процессов
            listView1.Columns.Add("Процессы", -2, HorizontalAlignment.Left);

            //Устанавливаем сортировку по возрастанию
            listView1.Sorting = SortOrder.Ascending;
            //WindowState = FormWindowState.Minimized;
            //Скрываем форму от пользователя
            //  Hide();


            blackProg = http.HttpLoadBlackApp();



            
        }

        private void buttonKey_Click(object sender, EventArgs e)
        {
            if (TextKey.Text == "")
            {
                MessageBox.Show("Ви не ввели код авторизації", "Помилка");
                hosts.DeleteHosts();
            }
            else
            {

                switch (http.HttpData("key", TextKey.Text, Config.cookie))
                {
                    case "key_yes":
                        Config.license = "license_yes";
                        Config.key = TextKey.Text;
                        TimerHost.Enabled = true;
                        //timerHost.SetTimer(true);
                        //hosts.SaveHosts();
                        Hide();
                        MessageBox.Show("Код авторизації введено вірно. Всім доброго дня!");
                        trayicon.trayicon.ShowBalloonTip(500, "Інтернет Батько", "Защита включена", System.Windows.Forms.ToolTipIcon.Info);
                        break;
                    case "key_no":
                        Config.key = TextKey.Text;
                        Config.license = "license_no";
                        hosts.DeleteHosts();
                        MessageBox.Show("Код авторизації введено не вірно. Введіть ключ ще раз", "Помилка");
                        break;
                    case "cookie_no":
                        Config.key = TextKey.Text;
                        Config.license = "license_no";
                        hosts.DeleteHosts();
                        MessageBox.Show("Цей ключ вже використовується на іншому комп'ютері.", "Помилка");
                        break;
                    default:
                        Config.key = TextKey.Text;
                        Config.license = "license_no";
                        trayicon.trayicon.ShowBalloonTip(1000, "Інтернет Батько", "Відсутнє з'єднання з Інтернетом. Зайдіть пізніше", System.Windows.Forms.ToolTipIcon.Error);
                        Hide();
                        break;
                }
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Process winProc in Process.GetProcesses())
                {

                    foreach (string it in blackProg)
                    {
                        if (winProc.ProcessName == it)
                        {

                            Process tor = Process.GetProcessById(winProc.Id);
                            tor.Kill();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                bool flag = false;
                foreach (Process winProc in Process.GetProcesses())
                {
                    if (winProc.ProcessName == "ChekApp")
                    {
                        flag = true;
                    }

                }
                if (!flag)
                {
                    //создаем новый процесс
                    Process proc = new Process();
                    //Запускаем Блокнто
                    proc.StartInfo.FileName = @path + "\\ChekApp.exe";
                    proc.Start();
                }

            }
            catch (Exception)
            {
            }
        }

    

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this.Hide();
            //ShowWindow(FindWindow(null, this.Text),SW_HIDE);
            Hide();
           // WindowState = FormWindowState.Normal;
            e.Cancel = true;
            
            HideProcess("HideProcess", false);

        }

        private void TimerHost_Tick(object sender, EventArgs e)
        {
            switch (http.HttpData("license", Config.key, Config.cookie))
            {
                case "license_yes":
                    hosts.SaveHosts();
                    Config.license = "license_yes";
                    break;
                case "license_no":
                    TimerHost.Enabled = false;
                    hosts.DeleteHosts();
                    trayicon.trayicon.ShowBalloonTip(500, "Інтернет Батько", "Термін дії ліцензії закінчився.", System.Windows.Forms.ToolTipIcon.Warning);
                    Config.license = "license_no";
                    break;
                case "cookie_no":
                    TimerHost.Enabled = false;
                    //SetTimer(false);
                    hosts.DeleteHosts();
                    trayicon.trayicon.ShowBalloonTip(500, "Інтернет Батько", "Цей ключ вже використовується на іншому комп'ютері.", System.Windows.Forms.ToolTipIcon.Warning);
                    Config.license = "license_no";
                    break;
                default:
                    TimerHost.Enabled = false;
                    //SetTimer(false);
                    break;
            }
        }

        public static bool winopen = false;

        
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Http.HttpCheckInet("ver_black", "", Config.cookie) != "")
            {
                TimerCheckInet.Enabled = false;
                //SetTimer(false);
                TimerHost.Enabled = true;
                //timerhost.SetTimer(true);
            }
        }
    }
}
