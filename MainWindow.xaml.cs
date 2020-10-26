using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChaseLabs.CLUpdate;
using System.Windows.Threading;

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
                    status.Content = "Checking for Updates";
                }, DispatcherPriority.Normal);

                string url = GetDownloadLink();
                //Console.WriteLine(value: url);

                string update_path = "C:/Users/r i Z/Desktop/LEAGUESKIN_10.21";

                //var update = Updater.Init(url, update_path, update_path);
            });
        }
    }
}
