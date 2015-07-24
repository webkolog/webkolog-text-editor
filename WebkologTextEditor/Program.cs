using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

/*
namespace WebkologTextEditor
{
    static class Program
    {
        private static Mutex mutex = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            const string appName = "Webkolog Text Editor";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // Uygulamanın başka bir örneği zaten çalışıyor.
                // Komut satırı argümanını (dosya yolunu) ilk örneğe ilet.
                if (args.Length > 0)
                {
                    // Burada dosya yolunu ilk örneğe iletecek bir mekanizma olmalı.
                    // Örneğin, bir Windows mesajı gönderebilir veya başka bir IPC yöntemi kullanılabilir.
                    // Bu örnekte basitçe bir mesaj kutusu gösteriyoruz.
                    MessageBox.Show("Dosya açma isteği: "+args[0]+"\nUygulamanın zaten bir örneği çalışıyor.", appName);
                }
                return; // Mevcut örneği sonlandır.
            }

            // Uygulamanın ilk örneği çalışıyor.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string filePath = null;
            if (args.Length > 0)
            {
                filePath = args[0];
                // Burada filePath değişkenini Form1'e iletmeniz gerekecek.
                // Örneğin, Form1'in constructor'ına veya bir public property'sine atayabilirsiniz.
                Application.Run(new Form1(filePath)); // Dosya yolunu forma ilet
            }
            else
            {
                Application.Run(new Form1());
            }

            // Uygulama kapatılırken mutex'i serbest bırak.
            mutex.ReleaseMutex();
        }
    }
}
*/

namespace WebkologTextEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isAppWorking = false;
            Mutex m = new Mutex(true, "Webkolog Text Editor", out isAppWorking);
            if (isAppWorking)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
