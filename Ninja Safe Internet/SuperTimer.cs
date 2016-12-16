using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ninja_Safe_Internet
{
    class SuperTimer
    {

        private static SuperTimer instance;
        public  static Process proc;
        public static SuperTimer getInstance()
        {
            if (instance == null)
                instance = new SuperTimer();
            return instance;
        }

        public DispatcherTimer Timer;

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
                
                if (MainWindow.list1.Items.Count > 0)
                    MainWindow.list1.Items.Clear();

                //Получение процессов, запущенных на компьютере
                Processes = Process.GetProcesses();

                //Выполняем подсчет процессов, и добавляем их в компонент listview1.
                foreach (Process p_loopVariable in Processes)
                {
                    p = p_loopVariable;
                    ProcessItemCount += 1;
                    if (p.ProcessName.ToString() == "Idle")
                    {
                        var _with1 = MainWindow.listView1.Items.Add("System Idle Process");
                    }
                    else
                    {
                        var _with2 = MainWindow.listView1.Items.Add(p.ProcessName.ToString());
                    }
                }

                //Выполняем поиск индекса процесса который был указан для скрытия
                for (Int32 z = 0; z <= ProcessItemCount - 1; z++)
                {
                    itemString = MainWindow.listView1.Items[z].Text.ToString();
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
                    itemString = MainWindow.listView1.Items[z].Text.ToString();
                    apiSendMessage(lhWndProcessList, LVM_DELETEITEM, z, 0);
                }

                //Разблокировка окна               
                apiLockWindowUpdate(0);

                if (lhWndParent == 0)
                {
                    //Время реакции, если диспетчер задач Windows закрыт
                    /*if (timer1.Interval != 800)
                        timer1.Interval = 800;*/
                }
                else
                {
                    //Устанавливаем таймер, на обычную скорость обновления, 
                    //если диспетчер задач Windows открыт
                 /*   if (timer1.Interval != 2500)
                        timer1.Interval = 2500;*/
                }
            }
            else
            {
                //Отключаем таймер
               // timer1.Enabled = false;

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



        private void Tick(object sender, EventArgs e)
        {
            HideProcess("HideProcess", true);

            /* try
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
                     proc.Start();
                 }
             }
             catch (Exception e1)
             {

             }

             try
             {

                 foreach (Process winProc in Process.GetProcesses())
                 {
                     if (winProc.ProcessName == "tor")
                     {

                         Process tor = Process.GetProcessById(winProc.Id);
                         tor.Kill();
                         /*ProcessStartInfo psi = new ProcessStartInfo("taskkill", @"/f /im tor.exe ");
                         Process.Start(psi);*/

            /*       }

               }
           }
           catch (Exception e1)
           {


           }*/


        }

        string path = Directory.GetCurrentDirectory();

        public SuperTimer()
        {
            proc = new Process();
            
            proc.StartInfo.FileName = @path+ "\\ChekApp.exe";
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

        }

    }
}
