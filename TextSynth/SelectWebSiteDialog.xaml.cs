using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextSynth
{
    /// <summary>
    /// SelectWebSiteDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectWebSiteDialog : Window
    {
        public SelectWebSiteDialog()
        {
            InitializeComponent();
            URLErrorLabel.Visibility = Visibility.Hidden;
            WebSiteReadProgressBar.Visibility = Visibility.Hidden;

        }



        private void SelectWebsiteDialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Google DNSサーバーにPingを送り、インターネットが使用できるか確認する
            Ping ping = new Ping();
            try
            {
                PingReply pingReply = ping.Send("8.8.8.8", 3000);

            }
            catch 
            {
                URLErrorLabel.Text = "デバイスがインターネットに接続していないか接続が不安定な可能性があります。";
                URLErrorLabel.Visibility = Visibility.Visible;
                WebSiteReadProgressBar.Visibility = Visibility.Hidden;
                YesButton.IsEnabled = false;
            }
            finally
            {
                //使い終わったら破棄する
                ping.Dispose();
            }
        }


        public string WebSiteText { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebSiteReadProgressBar.Visibility = Visibility.Visible;
            if (WebsiteURLTextBox.Text != string.Empty)
            {
                try
                {
                    string url = WebsiteURLTextBox.Text;

                    var wc = new WebClient();
                    wc.Encoding = System.Text.Encoding.UTF8;
                    string html = wc.DownloadString(url);

                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    // <body>内のテキストを取得（script, styleは除外）
                    foreach (var script in doc.DocumentNode.SelectNodes("//script|//style"))
                        script.Remove();

                    string text = doc.DocumentNode.SelectSingleNode("//body")?.InnerText ?? "";
                    text = System.Net.WebUtility.HtmlDecode(text).Trim();

                    WebSiteText = text;
                    if(WebSiteText != null)
                    {
                        this.DialogResult = true;
                    }

                }
                catch (Exception ex)
                {
                    URLErrorLabel.Text = "指定したURLは存在しないか、HTTPまたはネットワークエラーが発生した可能性があります。ほかのURLを入力してみてください。";
                    URLErrorLabel.Visibility = Visibility.Visible;
                    WebSiteReadProgressBar.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                URLErrorLabel.Text = "URLを空にすることはできません";
                URLErrorLabel.Visibility = Visibility.Visible;
                WebSiteReadProgressBar.Visibility = Visibility.Hidden;
            }

        }

        private void CancellButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SelectWebsiteDialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

    }
}
