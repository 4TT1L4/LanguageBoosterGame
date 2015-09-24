using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanguageBoosterGame
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EnterNameForm enterNameForm = new EnterNameForm();
        public static MainWindow Instance;

        MusicPlayer player = new MusicPlayer();

        private System.Threading.CancellationTokenSource source;
        public MainWindow()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("LanguageBooster.log", "myListener"));
            Trace.Listeners.Add(new MyListener());

            Trace.AutoFlush = true;
            System.Diagnostics.Trace.WriteLine("Started");

            Instance = this;
            InitializeComponent();
            enterNameForm.Name = "EnterNameFormInstance";

            this.source = new System.Threading.CancellationTokenSource();
            Task.Factory.StartNew(player.PlayMusic, this.Dispatcher, source.Token); 

            this.Cursor = Cursors.None;

            System.Diagnostics.Trace.WriteLine("Loading data.");
            DataManager.LoadData();
            System.Diagnostics.Trace.WriteLine("Loading data. DONE");
        }

        public void DisplayEnterNameForm()
        {
            this.Content.Children.Clear();
            this.Content.Children.Add(enterNameForm);
        }

        Game GameInstance = null;
        public void StartGame()
        {
            GameInstance = new Game();
            GameInstance.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            GameInstance.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            GameInstance.RandomizeBackground();

            Storyboard storyboard = new Storyboard();
            TimeSpan duration = TimeSpan.FromMilliseconds(4000);
            DoubleAnimation fadeInAnimation = new DoubleAnimation() { From = 0.0, To = 1.0, Duration = new Duration(duration) };

            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity", 1));
            storyboard.Children.Add(fadeInAnimation);
            System.Diagnostics.Trace.WriteLine("Hiding started.");
            storyboard.Begin(GameInstance);

            this.Main.Children.Add(GameInstance);
        }

        private DateTime HighscoreTime;
        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            System.Diagnostics.Trace.WriteLine(e.Key.ToString());

            if (GameInstance != null)
            {
                if (e.Key == Key.Left)
                {
                    GameInstance.ShipLeft();
                    e.Handled = true;
                }
                if (e.Key == Key.Right)
                {
                    GameInstance.ShipRight();
                    e.Handled = true;
                }
                if (e.Key == Key.Tab)
                {
                    GameInstance.ShootLaser();
                    e.Handled = true;
                }
                if (e.Key == Key.F1)
                {
                    player.PlayNext();
                    e.Handled = true;
                }
                if (e.Key == Key.F2)
                {
                    player.MuteUnMute();
                    e.Handled = true;
                }
                if (e.Key == Key.Escape)
                {
                    this.Close();
                    e.Handled = true;
                }

                if(GameInstance.IsGameOver)
                {
                    if (!GameInstance.Highscores)
                    {
                        GameInstance.ShowHighScores();
                        HighscoreTime = DateTime.Now;
                        e.Handled = true;
                    }
                    else
                    {
                        if ((DateTime.Now - HighscoreTime).Seconds > 2)
                        {
                            StartGame();
                            e.Handled = true;
                        }
                    }
                }
            }
        }
    }
}
