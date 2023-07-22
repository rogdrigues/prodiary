using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProDiaryApplication.MenuItem
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] imageData && imageData.Length > 0)
            {
                using (var memoryStream = new MemoryStream(imageData))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = memoryStream;
                    image.EndInit();
                    return image;
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class Memos : Window
    {
        private RichTextBox focusedRichTextBox = null;
        private bool isEmptyDiaryShow, isNewDiaryCreated, isDateCreatedSort, isTitleSort = false;
        private bool isNewDiary, isDateUpdatedSort = true;

        public Account? CurrentUser { get; set; }
        private Memo currentMemoSelected { get; set; }
        public Memos()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            loadDataUser();
        }

        private void loadDataUser()
        {
            if (CurrentUser != null)
            {
                byte[]? avatarData = CurrentUser.Avatar;
                if (avatarData != null && avatarData.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream(avatarData))
                    {
                        BitmapImage avatarImage = new BitmapImage();
                        avatarImage.BeginInit();
                        avatarImage.StreamSource = stream;
                        avatarImage.CacheOption = BitmapCacheOption.OnLoad;
                        avatarImage.EndInit();

                        imgAvatar.Source = avatarImage;
                    }
                }
            }
        }

        private void LoadData()
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                var tags = context.Tags.ToList();
                tagLists.ItemsSource = tags;

                var memos = context.Memos.Where(i => i.MemoAuthor == CurrentUser.FullName).OrderByDescending(i => i.MemoUpdated).ToList();
                memoList.ItemsSource = memos;
            }
        }

        private void LoadDataWithKeyWord(string keyword)
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                var memos = context.Memos.Where(i => i.MemoAuthor == CurrentUser.FullName)
                     .Where(i => i.MemoTitle.Contains(keyword) || i.MemoContent.Contains(keyword))
                     .OrderByDescending(i => i.MemoUpdated).ToList();
                memoList.ItemsSource = memos;
            }
        }

        private void LoadDataWithFilterKeyWord()
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                var memosQuery = context.Memos.Where(i => i.MemoAuthor == CurrentUser.FullName);

                if (isNewDiary)
                {
                    if (isDateUpdatedSort)
                        memosQuery = memosQuery.OrderByDescending(i => i.MemoUpdated);
                    else if (isDateCreatedSort)
                        memosQuery = memosQuery.OrderByDescending(i => i.MemoCreated);
                    else if (isTitleSort)
                        memosQuery = memosQuery.OrderByDescending(i => i.MemoTitle);
                }
                else
                {
                    if (isDateUpdatedSort)
                        memosQuery = memosQuery.OrderBy(i => i.MemoUpdated);
                    else if (isDateCreatedSort)
                        memosQuery = memosQuery.OrderBy(i => i.MemoCreated);
                    else if (isTitleSort)
                        memosQuery = memosQuery.OrderBy(i => i.MemoTitle);
                }

                var memos = memosQuery.ToList();
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

            if (btn.Name == "btnUndo")
            {
                FormatText("Undo");
            }
            else if (btn.Name == "btnRedo")
            {
                FormatText("Redo");
            }
            else if (btn.Name == "btnImportImage")
            {
                InsertImage_Click();
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
            StackPanel stackPanel = new StackPanel();

            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            image.Source = bitmapImage;

            stackPanel.Children.Add(image);
            string rtbXaml = XamlWriter.Save(rtbDescription);
            RichTextBox clonedRtb = (RichTextBox)XamlReader.Parse(rtbXaml);

            stackPanel.Children.Add(clonedRtb);
            diaryBox.Children.Add(stackPanel);
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

        private void MemoBorder_PreviewMouseDown(object sender, MouseButtonEventArgs? e)
        {
            if (sender is Border border)
            {
                if (border.DataContext is Memo memo)
                {
                    currentMemoSelected = memo;
                    showExistDiary();
                }
            }
        }

        private void showExistDiary()
        {
            defaultValueSet();
            isNewDiaryCreated = true;
            checkCurrentWindow();

            using (DiaryNoteContext noteContext = new DiaryNoteContext())
            {
                ClearDiaryBoxExceptRichTextBox();
                rtbTitleRun.Text = currentMemoSelected.MemoTitle.Trim();
                rtbDescriptionRun.Text = currentMemoSelected.MemoContent.Trim();

                List<MemoAddition> memoAddition = noteContext.MemoAdditions.Where(i => i.MemoId == currentMemoSelected.MemoId).ToList();

                foreach (MemoAddition memo in memoAddition)
                {
                    StackPanel stackPanel = new StackPanel();

                    Image image = new Image();
                    image.Source = LoadImageFromByteArray(memo.MemoAttachments);

                    stackPanel.Children.Add(image);

                    RichTextBox clonedRtb = new RichTextBox();
                    FlowDocument doc = new FlowDocument();
                    Paragraph paragraph = new Paragraph();

                    Run run = new Run(memo.MemoContentAddition)
                    {
                        FontFamily = new FontFamily("Script MT Bold"),
                        FontSize = 16,
                        Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xB9, 0xB6, 0xB6))
                    };
                    paragraph.Inlines.Add(run);
                    doc.Blocks.Add(paragraph);
                    clonedRtb.Document = doc;
                    clonedRtb.Background = null;
                    clonedRtb.BorderBrush = null;

                    stackPanel.Children.Add(clonedRtb);
                    diaryBox.Children.Add(stackPanel);
                }
            }
        }
        private void ClearDiaryBoxExceptRichTextBox()
        {
            List<UIElement> elementsToRemove = new List<UIElement>();

            foreach (var child in diaryBox.Children)
            {
                if ((child is StackPanel))
                {
                    elementsToRemove.Add((UIElement)child);
                }
            }

            foreach (var element in elementsToRemove)
            {
                diaryBox.Children.Remove(element);
            }
        }

        private void btnAddMemo_Click(object sender, RoutedEventArgs e)
        {
            defaultValueSet();
            isNewDiaryCreated = true;
            checkCurrentWindow();

            try
            {
                using (DiaryNoteContext noteContext = new DiaryNoteContext())
                {
                    string richTextBoxContent = new TextRange(rtbTitle.Document.ContentStart, rtbTitle.Document.ContentEnd).Text;
                    string richTextBoxDescription = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd).Text;
                    string? authorName = noteContext.Accounts.FirstOrDefault(i => i.Id == CurrentUser.Id)?.FullName;
                    Memo memo = new Memo();

                    memo.MemoTitle = richTextBoxContent;
                    memo.MemoContent = richTextBoxDescription;
                    memo.MemoAuthor = authorName;

                    noteContext.Memos.Add(memo);
                    noteContext.SaveChanges();
                    currentMemoSelected = memo;
                    txtSearch.Text = "";
                    LoadData();
                    showExistDiary();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the Memo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnSaveMemo_Click(object sender, RoutedEventArgs e)
        {
            bool isFirstImage = true;
            try
            {
                using (DiaryNoteContext context = new DiaryNoteContext())
                {
                    List<MemoAddition> memoAdditionsNew = new List<MemoAddition>();
                    List<MemoAddition> existingMemoAdditions = context.MemoAdditions.Where(m => m.MemoId == currentMemoSelected.MemoId).ToList();
                    context.MemoAdditions.RemoveRange(existingMemoAdditions);

                    foreach (var child in diaryBox.Children)
                    {
                        if (child is StackPanel stackPanel)
                        {
                            if (stackPanel.Children.Count == 2 &&
                                stackPanel.Children[0] is Image image &&
                                stackPanel.Children[1] is RichTextBox richTextBox)
                            {
                                byte[] imageData = GetImageDataAsByteArray(image);

                                string richTextBoxContent = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                                MemoAddition memoAddition = new MemoAddition
                                {
                                    MemoId = currentMemoSelected.MemoId,
                                    MemoAttachments = imageData,
                                    MemoContentAddition = richTextBoxContent
                                };
                                memoAdditionsNew.Add(memoAddition);

                                if (isFirstImage)
                                {
                                    Memo? memo = context.Memos.FirstOrDefault(i => i.MemoId == currentMemoSelected.MemoId);
                                    if (memo != null)
                                    {
                                        string richTextBoxContentUpdate = new TextRange(rtbTitle.Document.ContentStart, rtbTitle.Document.ContentEnd).Text;
                                        string richTextBoxDescriptionUpdate = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd).Text;

                                        memo.MemoTitle = richTextBoxContentUpdate;
                                        memo.MemoContent = richTextBoxDescriptionUpdate;
                                        memo.MemoAttachments = imageData;
                                        memo.MemoUpdated = DateTime.Now;
                                        context.Memos.Update(memo);
                                    }
                                    isFirstImage = false;
                                }
                            }
                        }
                    }

                    context.MemoAdditions.AddRange(memoAdditionsNew);
                    int result = context.SaveChanges();
                    if (result != 0)
                    {
                        checkSearchBarStatus();
                        MessageBox.Show("Diary successfully update!");
                    }
                    else
                    {
                        MessageBox.Show("Diary failed to update!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the Diary: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private byte[] GetImageDataAsByteArray(Image image)
        {
            BitmapImage bitmapImage = image.Source as BitmapImage;
            if (bitmapImage != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        private void ShowDiaryControl(Border controlToShow)
        {
            List<Border> diaryControls = new List<Border> { emptyDiary, trueDiary };

            foreach (var control in diaryControls)
            {
                control.Visibility = Visibility.Hidden;
            }

            controlToShow.Visibility = Visibility.Visible;
        }

        private void checkCurrentWindow()
        {
            if (isEmptyDiaryShow)
            {
                ShowDiaryControl(emptyDiary);
            }
            else if (isNewDiaryCreated)
            {
                ShowDiaryControl(trueDiary);
            }
        }

        private void defaultValueSet()
        {
            isEmptyDiaryShow = false;
            isNewDiaryCreated = false;
        }

        private void defaultValueSetForFilter()
        {
            isDateUpdatedSort = false;
            isDateCreatedSort = false;
            isTitleSort = false;

        }

        private void btnDeleteMemo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DiaryNoteContext noteContext = new DiaryNoteContext())
                {
                    var memoSelected = noteContext.Memos
                        .Include(i => i.MemoAdditions)
                        .SingleOrDefault(i => i.MemoId == currentMemoSelected.MemoId);

                    if (memoSelected != null)
                    {
                        noteContext.MemoAdditions.RemoveRange(memoSelected.MemoAdditions);
                        noteContext.Memos.Remove(memoSelected);

                        int result = noteContext.SaveChanges();

                        if (result != 0)
                        {
                            checkSearchBarStatus();
                            defaultValueSet();
                            isEmptyDiaryShow = true;
                            checkCurrentWindow();
                            MessageBox.Show("Diary successfully deleted!");
                        }
                        else
                        {
                            MessageBox.Show("Diary failed to delete!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the Diary: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void checkSearchBarStatus()
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadData();
            }
            else
            {
                LoadDataWithKeyWord(keyword);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkSearchBarStatus();
        }

        private void btnFilter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            defaultValueSetForFilter();
            Border? border = sender as Border;

            if(border.Name == "btnFilterNewDiary" || border.Name == "btnFilterOldDiary")
            {
                if (border.Name == "btnFilterNewDiary")
                {
                    isNewDiary = true;
                }
                else if (border.Name == "btnFilterOldDiary")
                {
                    isNewDiary = false;
                }
                SetLabelBlockDefaultContentOrder();
            }
            else
            {
                ResetFilterText();
                if (border.Name == "btnFilterDateUpdated")
                {
                    isDateUpdatedSort = true;
                    SetTextBlockDefaultContent(tbDateUpdated);
                }
                else if (border.Name == "btnFilterDateCreated")
                {
                    isDateCreatedSort = true;
                    SetTextBlockDefaultContent(tbDateCreated);
                }
                else if (border.Name == "btnFilterTitle")
                {
                    isTitleSort = true;
                    SetTextBlockDefaultContent(tbFilterTitle);
                }
            }
           
            LoadDataWithFilterKeyWord();
        }

        private void ResetFilterText()
        {
            tbDateUpdated.Content = "Date Updated";
            tbDateCreated.Content = "Date Created";
            tbFilterTitle.Content = "Title";
        }

        private void SetTextBlockDefaultContent(Label labelBlock)
        {
            labelBlock.Content = labelBlock.Content.ToString() + " (Default)";
        }
        private void SetLabelBlockDefaultContentOrder()
        {
            tbNewDiaryFilter.Content = "New Diary";
            tbOldDiaryFilter.Content = "Old Diary";
            Label labelBlock = new Label();
            if (isNewDiary)
            {
                labelBlock = tbNewDiaryFilter;
            }
            else
            {
                labelBlock = tbOldDiaryFilter;
            }
            labelBlock.Content = labelBlock.Content.ToString() + " (Default)";
        }
    }
}
