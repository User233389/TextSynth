using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextSynth
{
    /// <summary>
    /// FluentFontDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class FluentFontDialog : Window
    {
        public FluentFontDialog()
        {
            InitializeComponent();

            FontNameListBox.ItemsSource = GetFontFamilies();
        }

        private SortedDictionary<string, System.Windows.Media.FontFamily> GetFontFamilies()
        {
            //今のPCで使っている言語(日本語)のCulture取得
            //var language =
            //    System.Windows.Markup.XmlLanguage.GetLanguage(
            //    CultureInfo.CurrentCulture.IetfLanguageTag);
            CultureInfo culture = CultureInfo.CurrentCulture;//日本
            CultureInfo cultureUS = new("en-US");//英語？米国？

            List<string> uName = new();//フォント名の重複判定に使う
            Dictionary<string, System.Windows.Media.FontFamily> tempDictionary = new();
            foreach (var item in Fonts.SystemFontFamilies)
            {
                var typefaces = item.GetTypefaces();
                foreach (var typeface in typefaces)
                {
                    _ = typeface.TryGetGlyphTypeface(out GlyphTypeface gType);
                    if (gType != null)
                    {
                        //フォント名取得はFamilyNamesではなく、Win32FamilyNamesを使う
                        //FamilyNamesだと違うフォントなのに同じフォント名で取得されるものがあるので
                        //Win32FamilyNamesを使う
                        //日本語名がなければ英語名
                        string fontName = gType.Win32FamilyNames[culture] ?? gType.Win32FamilyNames[cultureUS];
                        //string fontName = gType.FamilyNames[culture] ?? gType.FamilyNames[cultureUS];

                        //フォント名で重複判定
                        var uri = gType.FontUri;
                        if (uName.Contains(fontName) == false)
                        {
                            uName.Add(fontName);
                            tempDictionary.Add(fontName, new(uri, fontName));
                        }
                    }
                }
            }
            SortedDictionary<string, System.Windows.Media.FontFamily> fontDictionary = new(tempDictionary);
            return fontDictionary;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int iFind = -1;
            string sStr = FontSarchTextBox.Text;

            // ListBox.ItemsSourceはSortedDictionary<string, FontFamily>
            // キー（フォント名）で完全一致検索
            var items = FontNameListBox.Items;
            for (int i = 0; i < items.Count; i++)
            {
                // ListBoxのItemsはKeyValuePair<string, FontFamily>
                if (items[i] is KeyValuePair<string, System.Windows.Media.FontFamily> kvp)
                {
                    if (kvp.Key == sStr)
                    {
                        iFind = i;
                        FontNameListBox.SelectedIndex = iFind;
                        break;
                    }
                }
                // ItemsSourceがDictionaryでない場合はstringとして比較
                else if (items[i]?.ToString() == sStr)
                {
                    iFind = i;
                    FontNameListBox.SelectedIndex = iFind;
                    break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FontSarchTextBox.Text = Properties.Settings.Default.FontName;
            FontSizeCombobox.Text = Properties.Settings.Default.String_FontSize;
        }

        private void FontNameListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ItemsSourceはSortedDictionary<string, FontFamily>なので、SelectedItemはKeyValuePair<string, FontFamily>型
            if (FontNameListBox.SelectedItem is KeyValuePair<string, System.Windows.Media.FontFamily> kvp)
            {
                FontSampleTextBlock.FontFamily = kvp.Value;
            }
            // ItemsSourceがDictionaryでない場合は直接キャスト
            else if (FontNameListBox.SelectedItem is System.Windows.Media.FontFamily fontFamily)
            {
                FontSampleTextBlock.FontFamily = fontFamily;
            }


        }


        private void FontSizeCombobox_ValueChanged(object sender, EventArgs e)
        {
            string PreviosFontSize = Properties.Settings.Default.String_FontSize;

            if (FontSizeCombobox.Text != "0")
            {

                string Str_FtSize = (string)FontSizeCombobox.Text;
                if (int.TryParse(Str_FtSize, out int result))
                {
                    FontSampleTextBlock.FontSize = result;
                }
            }
            else
            {
                
                FontSizeCombobox.Text = PreviosFontSize;
            }
        }

        private void FontSizeCombobox_ValueChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string PreviosFontSize = Properties.Settings.Default.String_FontSize;

            if (FontSizeCombobox.Text != "0")
            {

                string Str_FtSize = (string)FontSizeCombobox.Text;
                if (int.TryParse(Str_FtSize, out int result))
                {
                    FontSampleTextBlock.FontSize = result;
                }
            }
            else
            {

                FontSizeCombobox.Text = PreviosFontSize;
            }
        }

        private void FontSizeCombobox_ValueChanged(object sender, SelectionChangedEventArgs e)
        {
            string PreviosFontSize = Properties.Settings.Default.String_FontSize;

            if (FontSizeCombobox.Text != "0")
            {

                string Str_FtSize = (string)FontSizeCombobox.Text;
                if (int.TryParse(Str_FtSize, out int result))
                {
                    FontSampleTextBlock.FontSize = result;
                }
            }
            else
            {

                FontSizeCombobox.Text = PreviosFontSize;
            }
        }

        public System.Windows.Media.FontFamily fontFamily1 { get; set; }
        public float fontSize1 { get; set; }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            fontFamily1 = FontSampleTextBlock.FontFamily;
            fontSize1 = (float)FontSampleTextBlock.FontSize;
            this.DialogResult = true;

            Properties.Settings.Default.FontName = fontFamily1.ToString();
            Properties.Settings.Default.String_FontSize = fontSize1.ToString();
            Properties.Settings.Default.Float_FontSize = fontSize1;
            Properties.Settings.Default.Save();
        }

        private void CancellButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.ShowDialog();
        }
    }
}
