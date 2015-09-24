using LanguageBoosterBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LanguageBoosterService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class LanguageBoosterImplementation : LanguageBooster
    {        
        public List<Score> GetHighscores()
        {
            List<Score> HighScores = new List<Score>();

            using (var db = new LanguageBoosterContext())
            {
                var ActualHighScores = db.Scores.OrderBy(score => -score.Value).Take(10);
                HighScores.AddRange(ActualHighScores);
            }

            return HighScores;
        }

        public void SubmitScore(Score NewScore)
        {
            using (var db = new LanguageBoosterContext())
            {
                db.Scores.Attach(NewScore);
                db.Scores.Add(NewScore);
                db.SaveChanges();
            }
        }

        public void CreatePlayer(Player NewPlayer)
        {
            using (var db = new LanguageBoosterContext())
            {
                db.Players.Attach(NewPlayer);
                db.Players.Add(NewPlayer);
                db.SaveChanges();
            }
        }

        public Score GetPlayerHighScore(Player Player)
        {
            Score HighScore = null;

            return HighScore;
        }

        static Random random = new Random();

        public Word GetNextWord(Player NewPlayer)
        {
            using (var db = new LanguageBoosterContext())
            {
                int Count = db.Words.Count();

                if (Count == 0)
                {
                    throw new Exception("No words set up!");
                }

                int RandomIndex = random.Next(Count - 1);

                return db.Words.Skip(RandomIndex).Single();
            }

        }

        public void SubmitWord(Word NewWord)
        {
            using (var db = new LanguageBoosterContext())
            {
                db.Words.Add(NewWord);
                db.SaveChanges();
            }
        }

        public List<Word> GetWords(int Page, int PageSize)
        {
            using (var db = new LanguageBoosterContext())
            {
                return db.Words.OrderBy(w => w.Id).Skip(Page * PageSize).Take(PageSize).ToList();
            }
        }

        public int GetWordCount()
        {
            using (var db = new LanguageBoosterContext())
            {
                return db.Words.Count();
            }
        }

        public void RemoveWord(Word word)
        {
            using (var db = new LanguageBoosterContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM words WHERE id = " + word.Id);

                /*
                db.Words.Attach(word);
                db.Words.Remove(word);
                db.SaveChanges();
                */
            }
        }
    }
}
