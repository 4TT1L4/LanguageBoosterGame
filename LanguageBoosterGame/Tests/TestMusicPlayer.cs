using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HelixToolkit.Wpf;

namespace LanguageBoosterGame.Tests
{   
    [TestFixture]
    class TestMusicPlayer
    {
        [Test]
        [STAThread]
        public void MusicPlayer_PlaysMusic()
        {
            System.Diagnostics.Trace.WriteLine("Started");
            MusicPlayer p = new MusicPlayer();
            var ts = new System.Threading.CancellationTokenSource();
            p.MuteUnMute();
            Task.Factory.StartNew(p.PlayMusic, null, ts.Token);

            System.Threading.Thread.Sleep(200);
            string track1 = p.CurrentlyPlaying;
            System.Diagnostics.Trace.WriteLine("Playing 1:" + track1.ToString());
            p.PlayNext();

            Assert.IsFalse(string.IsNullOrWhiteSpace(track1));

            ts.Cancel(true);
            System.Diagnostics.Trace.WriteLine("Stopped");
        }
    }
}
