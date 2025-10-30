using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace TextSynth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechSynthesizer voice = new SpeechSynthesizer();
        
        public MainWindow()
        {
            InitializeComponent();
            voice.SpeakCompleted += Voice_SpeekComplited;
            voice.SpeakProgress += Voice_SpeekProgress;
            System.Windows.Forms.Application.EnableVisualStyles();

        }

        private void Voice_SpeekComplited(object? sender, SpeakCompletedEventArgs e)
        {
            voice.Pause();
            voice.SpeakAsyncCancelAll();
            PlayAndPauseButton.Content = "\uE769";
        }

        private void Voice_SpeekProgress(object? sender, SpeakProgressEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //一時停止の場合は再生する
            if((string)PlayAndPauseButton.Content == "\uE769")
            {
                TextRange textRange = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);
                if (SelectVoiceComboBox.SelectedIndex == 0)
                {
                    voice.SelectVoiceByHints(VoiceGender.NotSet);
                }
                else if (SelectVoiceComboBox.SelectedIndex == 1)
                {
                    voice.SelectVoiceByHints(VoiceGender.Male);
                }
                else if (SelectVoiceComboBox.SelectedIndex == 2)
                {
                    voice.SelectVoiceByHints(VoiceGender.Female);
                }
                else if (SelectVoiceComboBox.SelectedIndex == 3)
                {
                    voice.SelectVoiceByHints(VoiceGender.Neutral);
                }
                voice.Resume();
                voice.SpeakAsync(textRange.Text);
                PlayAndPauseButton.Content = "\uE768";
            }
            else if ((string)PlayAndPauseButton.Content == "\uE768")
            {
                voice.Pause();
                PlayAndPauseButton.Content = "\uE769";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle != WindowStyle.None)
            {
                this.Hide();
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                this.Show();
                this.Topmost = true;
                FullScreenButton.Content = "\uE73F";

                FullScreenMenu.IsChecked = true;   
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.ThreeDBorderWindow;
                FullScreenButton.Content = "\uE740";

                this.Topmost = false;

                FullScreenMenu.IsChecked = false;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if((string)MuteButton.Content == "\uE74f")
            {
                VolumeSlider.IsEnabled = true;
                if (VolumeSlider.Value == 0)
                {
                    MuteButton.Content = "\uE74f";
                }
                else if (VolumeSlider.Value <= 20)
                {
                    MuteButton.Content = "\uE993";
                }
                else if (VolumeSlider.Value <= 50)
                {
                    MuteButton.Content = "\uE994";
                }
                else if (VolumeSlider.Value <= 70)
                {
                    MuteButton.Content = "\uE995";
                }
                else if (VolumeSlider.Value <= 100)
                {
                    MuteButton.Content = "\uE995";
                }

                if (VolumeLabel == null) return; // safe early exit during initialization
                VolumeLabel.Content = ((int)VolumeSlider.Value).ToString() + "%";

                voice.Volume = (int)VolumeSlider.Value;

                MuteMenu.IsChecked = false;
            }
            else
            {
                VolumeSlider.IsEnabled = false;
                MuteButton.Content = "\uE74f";
                VolumeLabel.Content = "0%(ミュート)";

                voice.Volume = 0;

                MuteMenu.IsChecked = true;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (VolumeSlider.Value == 0)
            {
                MuteButton.Content = "\uE74f";
            }
            else if (VolumeSlider.Value <= 20)
            {
                MuteButton.Content = "\uE993";
            }
            else if (VolumeSlider.Value <= 50)
            {
                MuteButton.Content = "\uE994";
            }
            else if (VolumeSlider.Value <= 70)
            {
                MuteButton.Content = "\uE995";
            }
            else if (VolumeSlider.Value <= 100)
            {
                MuteButton.Content = "\uE995";
            }


            if (VolumeLabel == null) return; // safe early exit during initialization
            VolumeLabel.Content = ((int)VolumeSlider.Value).ToString() + "%";

            voice.Volume = (int)VolumeSlider.Value;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (VoiceSpeedLabel == null) return;
            {
                VoiceSpeedLabel.Content = ((int)VoiceSpeekSpeedSlider.Value).ToString();
                if ((int)VoiceSpeekSpeedSlider.Value ==0)
                {
                    VoiceSpeedLabel.Content = "0(デフォルト)";
                    VoiceSpeedIcon.Content = "\uEC49";
                }
                if (VoiceSpeekSpeedSlider.Value <= -1)
                {
                    VoiceSpeedIcon.Content = "\uEC48";
                }
                else if (VoiceSpeekSpeedSlider.Value >= 1)
                {
                    VoiceSpeedIcon.Content = "\uEC4A";
                }



            }

            voice.Rate = (int)VoiceSpeekSpeedSlider.Value;

        }


        private void WindowTopMostButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)WindowTopMostButton.Content == "\uE840")
            {
                this.Topmost = true;
                WindowTopMostButton.Content = "\uE841";
                WindowTopMostMenu.IsChecked = true;
            }
            else
            {
                this.Topmost = false;
                WindowTopMostButton.Content = "\uE840";
                WindowTopMostMenu.IsChecked = false;

            }
            
        }

        public void UIBlurEffect()
        {
            BlurEffect blurEffect = new BlurEffect();
            rtb1.Effect = blurEffect;
            MenuBar.Effect = blurEffect;
            MediaStatusBar.Effect = blurEffect;
            WindowSettingsStatusBar.Effect = blurEffect;
        }

        public void CancelUIBlurEffect()
        {
            rtb1.Effect = null;
            MenuBar.Effect = null;
            MediaStatusBar.Effect = null;
            WindowSettingsStatusBar.Effect = null;
        }

        private void About_Click(object sender, EventArgs e)
        {
            UIBlurEffect();

            VersionWindow versionWindow = new VersionWindow();
            versionWindow.Owner = this;
            versionWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            versionWindow.ShowDialog();

            CancelUIBlurEffect();

        }

        private void ReadTextFile_Click(object sender, EventArgs e)
        {
            UIBlurEffect();

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "テキストファイルを選択...";
            openFileDialog.Filter = "書式なしテキストファイル(*.txt)|*.txt|リッチテキストファイル(*.rtf)|*.rtf";
            if (openFileDialog.ShowDialog() == true)
            {
                StreamReader streamReader = new StreamReader(openFileDialog.FileName);
                Text1.Text = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            CancelUIBlurEffect();
        }

        private void ReadWebsite_Click(object sender,EventArgs e)
        {
            UIBlurEffect();

            SelectWebSiteDialog selectWebSiteDialog = new SelectWebSiteDialog();
            selectWebSiteDialog.Owner = this;
            selectWebSiteDialog.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            selectWebSiteDialog.ShowDialog();
            if(selectWebSiteDialog.DialogResult == true)
            {
                Text1.Text = selectWebSiteDialog.WebSiteText;
            }

            CancelUIBlurEffect();
        }

        private void SaveWAVFile_Click(object sender, EventArgs e)
        {
            UIBlurEffect();

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "WAVファイル(*.wav)|*.wav";
            if (saveFileDialog.ShowDialog() == true)
            {
                // 再生中の非同期合成はキャンセルするが、メインの synthesizer を Pause しない（Paused 状態では出力先を変更できない）
                try
                {
                    voice.SpeakAsyncCancelAll();
                    PlayAndPauseButton.Content = "\uE769";

                    TextRange textRange = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);

                    // ファイル保存用に別インスタンスを使う（UI 合成と競合させない）
                    using (var fileSynth = new SpeechSynthesizer())
                    {
                        // 設定をコピー
                        fileSynth.Volume = voice.Volume;
                        fileSynth.Rate = voice.Rate;

                        if (SelectVoiceComboBox.SelectedIndex == 0)
                            fileSynth.SelectVoiceByHints(VoiceGender.NotSet);
                        else if (SelectVoiceComboBox.SelectedIndex == 1)
                            fileSynth.SelectVoiceByHints(VoiceGender.Male);
                        else if (SelectVoiceComboBox.SelectedIndex == 2)
                            fileSynth.SelectVoiceByHints(VoiceGender.Female);
                        else if (SelectVoiceComboBox.SelectedIndex == 3)
                            fileSynth.SelectVoiceByHints(VoiceGender.Neutral);

                        fileSynth.SetOutputToWaveFile(saveFileDialog.FileName);
                        fileSynth.Speak(textRange.Text); // 同期的にファイルに書き出す
                        fileSynth.SetOutputToDefaultAudioDevice();
                    }

                    System.Windows.MessageBox.Show("WAVファイルを保存しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                    System.Windows.Clipboard.SetText(ex.Message);
                }
            }
            CancelUIBlurEffect();



        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void VoiceResetButton_Click(object sender, RoutedEventArgs e)
        {
            voice.Pause();
            voice.SpeakAsyncCancelAll();
        }

        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("ja-JP"));
        
        private void VoiceRecodingButton_Click(object sender, RoutedEventArgs e)
        {
            if((string)VoiceRecodingButton.Content ==  "\uE7C8")
            {

                speechRecognitionEngine.SpeechRecognized += speechRecognitionEngine_SpeechRecognized;
                speechRecognitionEngine.LoadGrammar(new DictationGrammar());
                speechRecognitionEngine.SetInputToDefaultAudioDevice();
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

                VoiceResetButton.IsEnabled = false;
                PlayAndPauseButton.IsEnabled = false;
                MuteButton.IsEnabled = false;
                VolumeLabel.IsEnabled = false;
                VoiceSpeekSpeedSlider.IsEnabled = false;
                VoiceSpeedLabel.IsEnabled = false;
                VoiceSpeedIcon.IsEnabled = false;
                SelectVoiceComboBox.IsEnabled = false;
                SelectVoiceIcon.IsEnabled = false;

                VoiceRecodingButton.Content = "\uE978";

                if(Text1.Text == "(話したいことを入力してください)")
                {
                    Text1.Text = string.Empty;
                }
            }
            else if((string)VoiceRecodingButton.Content == "\uE978")
            {
                speechRecognitionEngine.RecognizeAsyncCancel();

                VoiceResetButton.IsEnabled = true;
                PlayAndPauseButton.IsEnabled = true;
                MuteButton.IsEnabled = true;
                VolumeLabel.IsEnabled = true;
                VoiceSpeekSpeedSlider.IsEnabled = true;
                VoiceSpeedLabel.IsEnabled = true;
                VoiceSpeedIcon.IsEnabled = true;
                SelectVoiceComboBox.IsEnabled = true;
                SelectVoiceIcon.IsEnabled = true;

                VoiceRecodingButton.Content = "\uE7C8";
            }

        }

        public void speechRecognitionEngine_SpeechRecognized(object sender, RecognitionEventArgs e)
        {
            string tx = Text1.Text + e.Result.Text.ToString();
            Text1.Text = tx;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(rtb1.Focusable == false) return;
            {
                if(e.Key == Key.Space)
                {
                    //一時停止の場合は再生する
                    if ((string)PlayAndPauseButton.Content == "\uE769")
                    {
                        TextRange textRange = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);
                        if (SelectVoiceComboBox.SelectedIndex == 0)
                        {
                            voice.SelectVoiceByHints(VoiceGender.NotSet);
                        }
                        else if (SelectVoiceComboBox.SelectedIndex == 1)
                        {
                            voice.SelectVoiceByHints(VoiceGender.Male);
                        }
                        else if (SelectVoiceComboBox.SelectedIndex == 2)
                        {
                            voice.SelectVoiceByHints(VoiceGender.Female);
                        }
                        else if (SelectVoiceComboBox.SelectedIndex == 3)
                        {
                            voice.SelectVoiceByHints(VoiceGender.Neutral);
                        }
                        voice.Resume();
                        voice.SpeakAsync(textRange.Text);
                        PlayAndPauseButton.Content = "\uE768";
                    }
                    else if ((string)PlayAndPauseButton.Content == "\uE768")
                    {
                        voice.Pause();
                        PlayAndPauseButton.Content = "\uE769";
                    }
                }
                else if (e.Key == Key.B)
                {
                    voice.Pause();
                    voice.SpeakAsyncCancelAll();
                }
                else if(e.Key == Key.R)
                {
                    if ((string)VoiceRecodingButton.Content == "\uE7C8")
                    {

                        speechRecognitionEngine.SpeechRecognized += speechRecognitionEngine_SpeechRecognized;
                        speechRecognitionEngine.LoadGrammar(new DictationGrammar());
                        speechRecognitionEngine.SetInputToDefaultAudioDevice();
                        speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

                        VoiceResetButton.IsEnabled = false;
                        PlayAndPauseButton.IsEnabled = false;
                        MuteButton.IsEnabled = false;
                        VolumeLabel.IsEnabled = false;
                        VoiceSpeekSpeedSlider.IsEnabled = false;
                        VoiceSpeedLabel.IsEnabled = false;
                        VoiceSpeedIcon.IsEnabled = false;
                        SelectVoiceComboBox.IsEnabled = false;
                        SelectVoiceIcon.IsEnabled = false;

                        VoiceRecodingButton.Content = "\uE978";

                        if (Text1.Text == "(話したいことを入力してください)")
                        {
                            Text1.Text = string.Empty;
                        }
                    }
                    else if ((string)VoiceRecodingButton.Content == "\uE978")
                    {
                        speechRecognitionEngine.RecognizeAsyncCancel();

                        VoiceResetButton.IsEnabled = true;
                        PlayAndPauseButton.IsEnabled = true;
                        MuteButton.IsEnabled = true;
                        VolumeLabel.IsEnabled = true;
                        VoiceSpeekSpeedSlider.IsEnabled = true;
                        VoiceSpeedLabel.IsEnabled = true;
                        VoiceSpeedIcon.IsEnabled = true;
                        SelectVoiceComboBox.IsEnabled = true;
                        SelectVoiceIcon.IsEnabled = true;

                        VoiceRecodingButton.Content = "\uE7C8";
                    }

                }

            }


        }

        private void FontMenu_Click(object sender, RoutedEventArgs e)
        {
            UIBlurEffect();

            FluentFontDialog dialog = new FluentFontDialog();
            dialog.Owner = this;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ShowDialog();
            if(dialog.DialogResult == true)
            {
                rtb1.FontFamily = dialog.fontFamily1;
                rtb1.FontSize = dialog.fontSize1;
                Text1.FontFamily = dialog.fontFamily1;
                Text1.FontSize = dialog.fontSize1;
            }

            CancelUIBlurEffect();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rtb1.FontFamily = new System.Windows.Media.FontFamily(Properties.Settings.Default.FontName);
            rtb1.FontSize = Properties.Settings.Default.Float_FontSize;
            Text1.FontFamily = new System.Windows.Media.FontFamily(Properties.Settings.Default.FontName);
            Text1.FontSize = Properties.Settings.Default.Float_FontSize;


            if (Properties.Settings.Default.Settings_Voice == "None")
            {
                SelectVoiceComboBox.SelectedIndex = 0;
            }
            else if (Properties.Settings.Default.Settings_Voice == "Male")
            {
                SelectVoiceComboBox.SelectedIndex = 1;
            }
            else if (Properties.Settings.Default.Settings_Voice == "Female")
            {
                SelectVoiceComboBox.SelectedIndex = 2;
            }
            else if (Properties.Settings.Default.Settings_Voice == "Nutural")
            {
                SelectVoiceComboBox.SelectedIndex = 3;
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UIBlurEffect();   
            
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog() { Filter ="書式なしテキストファイル(*.txt)|*.txt|リッチテキストファイル(*.rtf)|*.rtf"};
            if(saveFileDialog.ShowDialog() == true)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                TextRange textRange = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);
                streamWriter.WriteLine(textRange.Text);
                streamWriter.Close();
                streamWriter.Dispose();
            }

            CancelUIBlurEffect();
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            UIBlurEffect();
            SettingsDialog dialog = new SettingsDialog();
            dialog.Owner = this;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ShowDialog();
            CancelUIBlurEffect();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.IsVisible == true)
            {
                if(Properties.Settings.Default.Setting_StartRecord == true)
                {
                    speechRecognitionEngine.SpeechRecognized += speechRecognitionEngine_SpeechRecognized;
                    speechRecognitionEngine.LoadGrammar(new DictationGrammar());
                    speechRecognitionEngine.SetInputToDefaultAudioDevice();
                    speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

                    VoiceResetButton.IsEnabled = false;
                    PlayAndPauseButton.IsEnabled = false;
                    MuteButton.IsEnabled = false;
                    VolumeLabel.IsEnabled = false;
                    VoiceSpeekSpeedSlider.IsEnabled = false;
                    VoiceSpeedLabel.IsEnabled = false;
                    VoiceSpeedIcon.IsEnabled = false;
                    SelectVoiceComboBox.IsEnabled = false;
                    SelectVoiceIcon.IsEnabled = false;

                    VoiceRecodingButton.Content = "\uE978";

                    if (Text1.Text == "(話したいことを入力してください)")
                    {
                        Text1.Text = string.Empty;
                    }
                }
            }
        }
    }
}