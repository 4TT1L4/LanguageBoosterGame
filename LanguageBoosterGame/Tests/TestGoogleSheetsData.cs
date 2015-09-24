using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBoosterGame.Tests
{
    [TestFixture]
    class TestGoogleSheetsData
    {
        [Test]
        public void Get10Words_AllWordsAreCorrect()
        {
            GetGoogleSheetData gsd = new GetGoogleSheetData();
            var enumerable = gsd.GetWords();
            var TopWords = enumerable.Take(10);

            foreach(var w in TopWords)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(w.Answer));
                Assert.IsFalse(string.IsNullOrWhiteSpace(w.Question));
            }
        }
    }
}
