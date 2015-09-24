using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageBoosterGame
{
    class MusicPlayer
    {

        public MusicPlayer()
        {

        }

        private string RandomFile(string Folder)
        {
            var rand = new Random();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + Folder, "*.mp3");
            return files[rand.Next(files.Length)];
        }

        private string _CurrentlyPlaying = null;
        public string CurrentlyPlaying
        {
            get
            {
                return _CurrentlyPlaying;
            }
        }

        private bool _PlayNext = false;
        public void PlayNext()
        {
            this._PlayNext = true;
        }

        private bool _Mute = false;

        public void MuteUnMute()
        {
            this._Mute = !this._Mute;
        }


        public void PlayMusic(object d)
        {
            bool play = true;
            while (play)
            {
                System.Diagnostics.Trace.WriteLine("Creating new AudioPlayback object.");
                using (AudioPlayback Playback = new AudioPlayback())
                {
                    if (_Mute)
                    {
                        Playback.Volume = 0;
                    }
                    else
                    {
                        Playback.Volume = 1;
                    }

                    String File = RandomFile("Music");
                    System.Diagnostics.Trace.WriteLine("Loading music file: " + File);
                    _CurrentlyPlaying = File;

                    Playback.Load(File);
                    Playback.Play();

                    while (Playback.GetPlaybackState() == NAudio.Wave.PlaybackState.Playing)
                    {

                        if(Playback.IsTrackOver())
                        {
                            System.Diagnostics.Trace.WriteLine("Track is over. Playing next song.");
                            break;
                        }


                        if (_Mute)
                        {
                            Playback.Volume = 0;
                        }
                        else
                        {
                            Playback.Volume = 1;
                        }

                        if (_PlayNext)
                        {
                            System.Diagnostics.Trace.WriteLine("Play next requested. Playing next song.");
                            this._PlayNext = false;
                            break;
                        }                        
                    }
                }    
            }
        }


    }
}
