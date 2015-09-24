using LanguageBoosterEditor.LanguageBoosterService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LanguageBoosterEditor
{
    public class WordManager
    {
        private static LanguageBoosterService.LanguageBoosterClient Client = new LanguageBoosterService.LanguageBoosterClient();

        public static void AddWord(string Question, string Answer)
        {
            Word NewWord = new Word()
            {
                Answer = Answer,
                Question = Question
            };

            Client.SubmitWord(NewWord);
            getWords();
        }
        public static void RemoveWord(Word word)
        {
            Client.RemoveWord(word);
            getWords();
        }

        private static ObservableCollection<Word> WordList = new ObservableCollection<Word>();
        public static ObservableCollection<Word> Words
        {
            get
            {
                System.Diagnostics.Trace.WriteLine("Getting words...");
                WordList.Clear();
                getWords();
                return WordList;
            }
        }

        private static async void getWords()
        {
            Word[] words = await Client.GetWordsAsync(0, 10000);
            var list = words.ToList<Word>();
            list = list.OrderBy(w => -w.Id).ToList();
            
                System.Windows.Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate()
                {
                    WordList.Clear();
                   foreach(Word w in list)
                   {
                      WordList.Add(w);
                    }                   
               });

                ((ListView)MainWindow.Instance.FindName("Words")).Visibility = System.Windows.Visibility.Visible;
                ((StackPanel)MainWindow.Instance.FindName("Loader")).Visibility = System.Windows.Visibility.Hidden;
                ((Border)MainWindow.Instance.FindName("LoaderBorder")).Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
