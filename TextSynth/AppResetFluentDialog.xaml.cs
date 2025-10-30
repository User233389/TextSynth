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
using System.Windows.Shapes;

namespace TextSynth
{
    /// <summary>
    /// AppResetFluentDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class AppResetFluentDialog : Window
    {
        public AppResetFluentDialog()
        {
            InitializeComponent();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //リセット
            Properties.Settings.Default.Reset();
            //一度ソフトウェアを閉じてから再起動する
            System.Windows.Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
