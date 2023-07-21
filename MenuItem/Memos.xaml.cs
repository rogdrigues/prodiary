using Microsoft.Win32;
using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace ProDiaryApplication.MenuItem
{
    /// <summary>
    /// Interaction logic for Memos.xaml
    /// </summary>
    public partial class Memos : Window
    {
        private RichTextBox focusedRichTextBox = null;

        public Memos()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                var tags = context.Tags.ToList();
                tagLists.ItemsSource = tags;

                var memos = context.Memos.ToList();
                foreach (var memo in memos)
                {
                    if (memo.MemoAttachments != null)
                    {
                        var image = new Image();
                        image.Source = LoadImageFromByteArray(memo.MemoAttachments);
                        image.Width = 100; 
                        image.Height = 100;
                    }
                }
                memoList.ItemsSource = memos;
            }
        }

        private BitmapImage LoadImageFromByteArray(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
            }

            return image;
        }

        private void btnWordAction(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if(btn.Name == "btnUndo")
            {
                FormatText("Undo");
            }
            else if(btn.Name == "btnRedo")
            {
                FormatText("Redo");
            }
            else if (btn.Name == "btnImportImage")
            {
                FormatText("Image");
            }
            else if (btn.Name == "btnAlignLeft")
            {
                FormatText("AlignLeft");
            }
            else if (btn.Name == "btnAlignJustify")
            {
                FormatText("AlignJustify");
            }
            else if (btn.Name == "btnAlignRight")
            {
                FormatText("AlignRight");
            }
            else if (btn.Name == "btnAlignFormat")
            {
                FormatText("AlignCenter");
            }
            else if (btn.Name == "btnSizeIncrease")
            {
                FormatText("SizeIncrease");
            }
            else if (btn.Name == "btnSizeDecrease")
            {
                FormatText("SizeDecrease");
            }
            else if (btn.Name == "btnBold")
            {
                FormatText("Bold");
            }
            else if (btn.Name == "btnItalic")
            {
                FormatText("Italic");
            }
            else if (btn.Name == "btnUnderline")
            {
                FormatText("Underline");
            }
            else if (btn.Name == "btnCut")
            {
                FormatText("Cut");
            }
            else if (btn.Name == "btnCopy")
            {
                FormatText("Copy");
            }
            else if (btn.Name == "btnPaste")
            {
                FormatText("Paste");
            }else if(btn.Name == "btnImportImage")
            {
                FormatText("Image");
            }
        }
        private void FormatText(string action)
        {
            RichTextBox richTextBox = focusedRichTextBox;
            TextSelection selectedText = richTextBox?.Selection;

            if (richTextBox != null && selectedText != null)
            {
                switch (action)
                {
                    case "Undo":
                        if (richTextBox.CanUndo)
                            richTextBox.Undo();
                        break;
                    case "Redo":
                        if (richTextBox.CanRedo)
                            richTextBox.Redo();
                        break;
                    case "Cut":
                        if (!selectedText.IsEmpty)
                            richTextBox.Cut();
                        break;
                    case "Copy":
                        if (!selectedText.IsEmpty)
                            richTextBox.Copy();
                        break;
                    case "Paste":
                        if (Clipboard.ContainsText())
                        {
                            int caretPosition = richTextBox.CaretPosition.GetOffsetToPosition(richTextBox.Document.ContentStart);
                            richTextBox.CaretPosition.InsertTextInRun(Clipboard.GetText());

                            int newPositionOffset = caretPosition + Clipboard.GetText().Length;
                            TextPointer newPosition = richTextBox.Document.ContentStart.GetPositionAtOffset(newPositionOffset, LogicalDirection.Forward);
                            if (newPosition != null)
                            {
                                richTextBox.CaretPosition = newPosition;
                            }
                        }
                        break;
                    case "AlignLeft":
                        richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
                        break;
                    case "AlignJustify":
                        richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Justify);
                        break;
                    case "AlignRight":
                        richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
                        break;
                    case "AlignCenter":
                        richTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
                        break;
                    case "SizeIncrease":
                        richTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, richTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty));
                        break;
                    case "SizeDecrease":
                        richTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, richTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty));
                        break;
                    case "Bold":
                        if (!selectedText.IsEmpty)
                        {
                            TextRange tr = new TextRange(selectedText.Start, selectedText.End);
                            object fontWeight = tr.GetPropertyValue(TextElement.FontWeightProperty);
                            FontWeight boldWeight = fontWeight is FontWeight weight && weight == FontWeights.Bold ? FontWeights.Normal : FontWeights.Bold;
                            tr.ApplyPropertyValue(TextElement.FontWeightProperty, boldWeight);
                        }
                        break;
                    case "Italic":
                        if (!selectedText.IsEmpty)
                        {
                            TextRange tr = new TextRange(selectedText.Start, selectedText.End);
                            object fontStyle = tr.GetPropertyValue(TextElement.FontStyleProperty);
                            FontStyle italicStyle = fontStyle is FontStyle style && style == FontStyles.Italic ? FontStyles.Normal : FontStyles.Italic;
                            tr.ApplyPropertyValue(TextElement.FontStyleProperty, italicStyle);
                        }
                        break;
                    case "Underline":
                        if (!selectedText.IsEmpty)
                        {
                            TextRange tr = new TextRange(selectedText.Start, selectedText.End);
                            object textDecorations = tr.GetPropertyValue(Inline.TextDecorationsProperty);
                            TextDecorationCollection underlineCollection = textDecorations is TextDecorationCollection decorations && decorations.Count > 0 ? null : TextDecorations.Underline;
                            tr.ApplyPropertyValue(Inline.TextDecorationsProperty, underlineCollection);
                        }
                        break;
                    case "Image":
                        InsertImage_Click();
                        break;
                }
            }
        }
        private void InsertImage_Click()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                CreateImageControl(imagePath);
            }
        }
        private void CreateImageControl(string imagePath)
        {
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            image.Source = bitmapImage;


            // Đặt kích thước cho hình ảnh (tùy chọn)
            image.Width = 200;
            image.Height = 150;

            // Thêm hình ảnh vào StackPanel diaryBox
            diaryBox.Children.Add(image);
        }
        [DebuggerHidden]
        private void rtbDescription_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            focusedRichTextBox = sender as RichTextBox;
        }

        [DebuggerHidden]
        private void rtbDescription_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //focusedRichTextBox = null;
        }
    }
}
