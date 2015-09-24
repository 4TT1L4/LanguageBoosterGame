using Google.GData.Client;
using Google.GData.Spreadsheets;
using Google.GData.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LanguageBoosterEditor.LanguageBoosterService;

namespace LanguageBoosterGame
{
    public class GetGoogleSheetData
    {
        // FIX ME: The originally used Google Authentication is not working anymore
        private const bool Enabled = false;
        
        public IEnumerable<Word> GetWords()
        {
            if (Enabled)
            {
                SpreadsheetsService GoogleService = new SpreadsheetsService("LanguageBooster");

                // FIX ME: Remove credentials from code.
                GoogleService.setUserCredentials("xxx", "xxx");

                SpreadsheetQuery SpreadsheetsQuery = new SpreadsheetQuery();
                SpreadsheetFeed Spreadsheets = GoogleService.Query(SpreadsheetsQuery);

                // Get list of spreadsheets
                foreach (SpreadsheetEntry WordsSheet in Spreadsheets.Entries)
                {
                    System.Diagnostics.Trace.WriteLine(WordsSheet.Title.Text);


                    System.Diagnostics.Trace.WriteLine(WordsSheet.Title.Text);

                    // Get list of worksheets from spreadsgett
                    WorksheetFeed Worksheets = WordsSheet.Worksheets;

                    WorksheetEntry CurrentWorksheet = null;
                    foreach (WorksheetEntry Worksheet in Worksheets.Entries)
                    {
                        CurrentWorksheet = Worksheet;
                        break;
                    }

                    AtomLink CellsLink = CurrentWorksheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);
                    CellQuery CellsQuery = new Google.GData.Spreadsheets.CellQuery(CellsLink.HRef.ToString());
                    CellFeed Cells = GoogleService.Query(CellsQuery);

                    Word word = null;
                    // Load actual table data
                    foreach (CellEntry CurrentCell in Cells.Entries)
                    {
                        if (CurrentCell.Column == 1)
                        {
                            word = new Word();
                            word.Question = CurrentCell.Value;
                        }

                        if (CurrentCell.Column == 2)
                        {
                            word.Answer = CurrentCell.Value;
                            System.Diagnostics.Trace.WriteLine(word.Question + " - " + word.Answer);
                            yield return word;
                        }
                    }
                }
            }
        }
    }
}