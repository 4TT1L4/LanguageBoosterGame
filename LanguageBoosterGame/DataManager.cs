using LanguageBoosterEditor.LanguageBoosterService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LanguageBoosterGame
{
    class DataManager
    {
        private static ObservableCollection<Word> WordList = new ObservableCollection<Word>();

        private static LanguageBoosterService.LanguageBoosterClient Client = new LanguageBoosterService.LanguageBoosterClient();
        
        public static string PlayerName = "";

        public static int Points = 0;

        public static void LoadData()
        {
            getWords();
        }

        static Random r = new Random();
        static private int LastWordIndex = -1;
        public static Word GetNextWord()
        {
            int id = 0;
            do
            {
                id = r.Next(WordList.Count);
            } while (id == LastWordIndex);

            LastWordIndex = id;
            return WordList[id];
        }

        static public List<Score> GetHighScores()
        {
            return Client.GetHighscores().ToList();
        }

        static public void SubmitHighscore(int Score, string Player)
        {

            Player pl = new Player();
            pl.Name = Player;
            Client.CreatePlayer(pl);

            Score sc = new Score();
            sc.Value = Score;
            sc.PlayerName = pl.Name;
            Client.SubmitScore(sc);
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
                foreach (Word w in list)
                {
                    WordList.Add(w);
                }
            });

            new Thread(delegate()
            {
                    var GoogleSheetsData = new GetGoogleSheetData();
                    foreach (Word word in GoogleSheetsData.GetWords())
                    {
                        WordList.Add(word);
                    }
            }).Start();
            
            
            ((Border)MainWindow.Instance.FindName("LoaderBorder")).Visibility = System.Windows.Visibility.Hidden;
            MainWindow.Instance.DisplayEnterNameForm();
        }
    }
}
