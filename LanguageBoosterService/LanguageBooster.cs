using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LanguageBoosterService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface LanguageBooster
    {
        [OperationContract]
        List<Score> GetHighscores();

        [OperationContract]
        void SubmitScore(Score NewScore);

        [OperationContract]
        void CreatePlayer(Player NewPlayer);

        [OperationContract]
        Score GetPlayerHighScore(Player NewPlayer);

        [OperationContract]
        Word GetNextWord(Player NewPlayer);

        [OperationContract]
        void SubmitWord(Word NewWord);

        [OperationContract]
        void RemoveWord(Word NewWord);

        [OperationContract]
        List<Word> GetWords(int Page, int PageSize);

        [OperationContract]
        int GetWordCount();
    }
    
}
