using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBoosterGame
{
    public class MyListener : TraceListener
    {
        static System.Diagnostics.EventLog EventLog = new EventLog("LanguageBooster - MyListener Log");

        static MyListener()
        {
            EventLog.Source = "LanguageBooster - Log - WindowsEvent";
        }

        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            EventLog.WriteEntry(message);
        }
    }
}
