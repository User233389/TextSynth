using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextSynth
{
    /// <summary>
    /// SettingsDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
            
        }

        public void UIBlurEffect()
        {
            BlurEffect blurEffect = new BlurEffect();
            SettingTabControl.Effect = blurEffect;
            StatusBar1.Effect = blurEffect;
        }

        public void CancelUIBlurEffect()
        {
            BlurEffect blurEffect = new BlurEffect();
            SettingTabControl.Effect = null;
            StatusBar1.Effect = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UIBlurEffect();
            AppResetFluentDialog appResetFluentDialog = new AppResetFluentDialog();
            appResetFluentDialog.Owner = this;
            appResetFluentDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            appResetFluentDialog.ShowDialog();
            CancelUIBlurEffect();
        }

        public string Settings_FontName {  get; set; }
        public string Settings_FontSize {  get; set; }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            //既定の音声設定
            if(DefaltVoiceRadioButton.IsChecked == true)
            {
                Properties.Settings.Default.Settings_Voice = "None";
            }
            else if (MaleVoiceRadioButton.IsChecked == true)
            {
                Properties.Settings.Default.Settings_Voice = "Male";
            }
            else if (FemaleRadioButton.IsChecked == true)
            {
                Properties.Settings.Default.Settings_Voice = "Female";
            }
            else if(NuturalRadionButton.IsChecked == true)
            {
                Properties.Settings.Default.Settings_Voice = "Nutural";
            }

            //起動時に録音を開始する設定
            if (StartOfRecoradingChrckBox.IsChecked == true)
            {
                Properties.Settings.Default.Setting_StartRecord = true;
            }
            else if (StartOfRecoradingChrckBox.IsChecked == false)
            {
                Properties.Settings.Default.Setting_StartRecord = false;
            }

            //フォント設定
            Properties.Settings.Default.FontName = Settings_FontName;

            if(float.TryParse(Settings_FontName, out float ftSize ))
            {
                Properties.Settings.Default.Float_FontSize = ftSize;
                Properties.Settings.Default.String_FontSize = Settings_FontSize;
            }

            Properties.Settings.Default.Save();

            this.DialogResult = true;

        }

        private void CancellButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FontNameLabel.Content = "フォント名:"+Properties.Settings.Default.FontName;
            FontSizeLabel.Content = "フォントサイズ:"+Properties.Settings.Default.String_FontSize;

            if (Properties.Settings.Default.Settings_Voice == "None")
            {
                DefaltVoiceRadioButton.IsChecked = true;
            }
            else if (Properties.Settings.Default.Settings_Voice == "Male")
            {
                MaleVoiceRadioButton.IsChecked = true;
            }
            else if (Properties.Settings.Default.Settings_Voice == "Female")
            {
                FemaleRadioButton.IsChecked = true;
            }
            else if (Properties.Settings.Default.Settings_Voice == "Nutural")
            {
                NuturalRadionButton.IsChecked = true;
            }

            if (Properties.Settings.Default.Setting_StartRecord == true)
            {
                StartOfRecoradingChrckBox.IsChecked = true;
            }
            else if (Properties.Settings.Default.Setting_StartRecord == false)
            {
                StartOfRecoradingChrckBox.IsChecked = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UIBlurEffect();
            FluentFontDialog fluentFontDialog = new FluentFontDialog();
            fluentFontDialog.ShowDialog();
            if(fluentFontDialog.DialogResult == true)
            {
                FontNameLabel.Content = "フォント名:" + fluentFontDialog.fontFamily1;
                FontSizeLabel.Content = "フォントサイズ:" + fluentFontDialog.fontSize1;
                Settings_FontName = fluentFontDialog.fontFamily1.Source;
                Settings_FontSize = fluentFontDialog.fontSize1.ToString();
            }
            CancelUIBlurEffect();
        }
    }
}
