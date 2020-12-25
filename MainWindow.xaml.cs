using System;
using System.Threading.Tasks;
using System.Windows;
using ChaseLabs.CLUpdate;
using System.Windows.Threading;
using System.IO;

namespace ModSkinUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //helping with multiThreaded tasks
        Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public MainWindow()
        {
            InitializeComponent();
            GetDownloadLink();
            Update();
        }

        string GetDownloadLink()
        {
            HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.Load("http://leagueskin.net/p/download-mod-skin-2020-chn");

            var downloadButton = htmlDocument.DocumentNode.SelectSingleNode("//a[@id='link_download3']");
            string buttonURL = downloadButton.Attributes["href"].Value;
            //Console.WriteLine(value: buttonURL);

            return buttonURL;
        }

        void Update()
        {
            Task.Run(() => 
            {
                dispatcher.Invoke(() => 
                {
                    status.Content = "Checking for Updates...";
                }, DispatcherPriority.Normal);

                string url = GetDownloadLink();
                //Console.WriteLine(value: url);

                //string update_path = @"C:\Users\riZ\Desktop\MOD_LOL\update";
                //string application_path = @"C:\Users\riZ\Desktop\MOD_LOL\";
                string update_path = Directory.GetParent(Environment.CurrentDirectory).ToString() + "\\update";
                string application_path = Directory.GetParent(Environment.CurrentDirectory).ToString();

                var update = Updater.Init(url, update_path, application_path, null, false);

                //UpdateManager.Update(null, null, null, url, update_path, application_path, null, false);

                //if (UpdateManager.CheckForUpdate())
                //{

                //}

                string[] files = Directory.GetFiles(Directory.GetParent(Environment.CurrentDirectory).ToString());
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                        Console.WriteLine($"{file} is deleted.");
                    }
                }
                

                update.Download();
                dispatcher.Invoke(() =>
                {
                    status.Content = "Downloading Update...";
                }, DispatcherPriority.Normal);

                update.Unzip();
                dispatcher.Invoke(() =>
                {
                    status.Content = "Unziping Update...";
                }, DispatcherPriority.Normal);

                update.CleanUp();
                dispatcher.Invoke(() =>
                {
                    status.Content = "Finishing Up...";
                }, DispatcherPriority.Normal);

                System.Threading.Thread.Sleep(1000);
                Environment.Exit(0);
                //using (var client = new System.Net.WebClient())
                //{
                //    client.DownloadFile(url, @"C:\Users\riZ\Desktop\MOD_LOL\mod.zip");

                //    dispatcher.Invoke(() =>
                //    {
                //        status.Content = "Downloading Update...";
                //    }, DispatcherPriority.Normal);

                //    client.Dispose();

                //    dispatcher.Invoke(() =>
                //    {
                //        status.Content = "Finishing Up...";
                //    }, DispatcherPriority.Normal);
                //}
            });
        }
    }
}
