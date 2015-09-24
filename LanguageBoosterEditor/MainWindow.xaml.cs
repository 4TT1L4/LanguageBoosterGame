using LanguageBoosterEditor.LanguageBoosterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LanguageBoosterEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance = null;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            Original = this.QuestionTextBox.Background;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!AnswerTextBox.IsFocused)
            {
                QuestionTextBox.Focus();
            }
        }

        private void DeleteWord()
        {
            System.Diagnostics.Trace.WriteLine("Deleted some element");
        }

        Brush Original = null;
        private void AddNewWord()
        {
            string Question = QuestionTextBox.Text.Trim();
            string Answer = AnswerTextBox.Text.Trim();

            if (String.IsNullOrWhiteSpace(Question))
            {
                QuestionTextBox.Background = new LinearGradientBrush(Color.FromRgb(255, 200, 200), Color.FromRgb(255, 150, 150), 45);
                QuestionTextBox.Focus();
            }
            else
                if (String.IsNullOrWhiteSpace(Answer))
                {
                    AnswerTextBox.Background = new LinearGradientBrush(Color.FromRgb(255, 200, 200), Color.FromRgb(255, 150, 150), 45);
                    AnswerTextBox.Focus();
                }
                else
                {
                    WordManager.AddWord(Question, Answer);

                    QuestionTextBox.Text = "";
                    AnswerTextBox.Text = "";
                    QuestionTextBox.Focus();
                }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewWord();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(QuestionTextBox.Text.Trim()))
            {
                QuestionTextBox.Background = Original;
            }

            if(!String.IsNullOrWhiteSpace(AnswerTextBox.Text.Trim()))
            {
                AnswerTextBox.Background = Original;
            }

            if(e.Key == Key.Enter)
            {
                AddNewWord();
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            int selected = this.Words.SelectedIndex;
            if (selected > -1)
            {
                var SelectedItem = this.Words.Items[selected];
                if (SelectedItem is Word)
                {
                    Word CurrentWord = (Word)SelectedItem;
                    System.Diagnostics.Trace.WriteLine("Deleting item (" + CurrentWord.Id + ")");
                    WordManager.RemoveWord(CurrentWord);
                }
            }
        }

    }
}
