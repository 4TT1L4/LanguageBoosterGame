using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace LanguageBoosterGame
{
    class AudioPlayback : IDisposable
    {
        private IWavePlayer playbackDevice;
        private WaveChannel32 inputStream;

        public event EventHandler<SampleEventArgs> OnSample;

        public float Volume
        {
            get
            {
                if (this.inputStream != null)
                {
                    return inputStream.Volume;
                }

                return 0;
            }
            set
            {
                if (this.inputStream != null)
                {
                    inputStream.Volume = value;
                }
            }
        }

        public event EventHandler<StoppedEventArgs> StoppedEvent
        {
            add { playbackDevice.PlaybackStopped += value; }
            remove { playbackDevice.PlaybackStopped -= value; }
        }


        public AudioPlayback()
        {
        }

        public void Load(string fileName)
        {
            Stop();
            EnsureDeviceCreated();
            CloseFile();
            OpenFile(fileName);
        }

        private void CloseFile()
        {
            if (inputStream != null)
            {
                inputStream.Dispose();
                inputStream = null;
            }
        }

        private void OpenFile(string fileName)
        {
            CreateInputStream(fileName);
            playbackDevice.Init(inputStream);
        }

        private void CreateInputStream(string fileName)
        {
            if (fileName.EndsWith(".wav"))
            {
                inputStream = OpenWavStream(fileName);
            }
            else if (fileName.EndsWith(".mp3"))
            {
                inputStream = OpenMp3Stream(fileName);
            }
            else
            {
                throw new InvalidOperationException("Unsupported extension");
            }
            inputStream.Sample += new EventHandler<SampleEventArgs>(inputStream_Sample);
        }

        void inputStream_Sample(object sender, SampleEventArgs e)
        {
            if (OnSample != null)
            {
                OnSample(this, e);
            }
        }

        private static WaveChannel32 OpenMp3Stream(string fileName)
        {
            WaveChannel32 inputStream;
            WaveStream mp3Reader = new Mp3FileReader(fileName);
            WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader);
            WaveStream blockAlignedStream = new BlockAlignReductionStream(pcmStream);
            inputStream = new WaveChannel32(blockAlignedStream);
            return inputStream;
        }

        private static WaveChannel32 OpenWavStream(string fileName)
        {
            WaveChannel32 inputStream;
            WaveStream readerStream = new WaveFileReader(fileName);
            if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
            {
                readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                readerStream = new BlockAlignReductionStream(readerStream);
            }
            if (readerStream.WaveFormat.BitsPerSample != 16)
            {
                var format = new WaveFormat(readerStream.WaveFormat.SampleRate,
                    16, readerStream.WaveFormat.Channels);
                readerStream = new WaveFormatConversionStream(format, readerStream);
            }
            inputStream = new WaveChannel32(readerStream);
            return inputStream;
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
            {
                CreateDevice();
            }
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut(WaveCallbackInfo.FunctionCallback());           
        }

        public void Play()
        {
            try
            {
                if (playbackDevice != null && inputStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
                {
                    playbackDevice.Play();                    
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("AudioPlayback Exception. Play() method:" + ex.Message);
            }
        }

        private void Stopped(object sender, StoppedEventArgs e)
        {
            this.Dispose();            
        }


        public PlaybackState GetPlaybackState()
        {
            return playbackDevice.PlaybackState;
        }

        public void Stop()
        {
            if (playbackDevice != null)
            {
                playbackDevice.Stop();
            }
            CloseFile();
        }

        public void Dispose()
        {
            Stop();
            if (playbackDevice != null)
            {
                playbackDevice.Dispose();
                playbackDevice = null;
            }
        }

        public bool IsTrackOver()
        {
            if (playbackDevice != null)
            {
                return (this.inputStream.Position > this.inputStream.Length);
            }
            else
            {
                return false;
            }
        }
    }
}