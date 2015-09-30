using HelixToolkit.Wpf;
using LanguageBoosterEditor.LanguageBoosterService;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LanguageBoosterGame
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        private int Score = 0;
        private int DisplayedScore = 0;
        private int LaserAmmoCount = 10;

        private Word CurrentWord;
        private LevelManager LevelLoader = null;

        public Game()
        {
            InitializeComponent();
            this.LevelLoader = new LevelManager(this.view1);
            this.PlayerName.Text = DataManager.PlayerName;
            NextWord();
        }

        private void NextWord()
        {
            CurrentWord = DataManager.GetNextWord();
        }

        public void RandomizeBackground()
        {
            Uri src = new Uri(RandomFile("Backgrounds"), UriKind.Relative);
            BitmapImage img = new BitmapImage(src);
            ImageBrush brush = new ImageBrush(img);
            this.GameGrid.Background = brush;
        }      

        private string RandomFile(string Folder)
        {
            var rand = new Random();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + Folder, "*.jpg");
            return files[rand.Next(files.Length)];
        }


        private HelixToolkit.Wpf.ModelImporter importer = new HelixToolkit.Wpf.ModelImporter();
        
        private List<Model3DGroup> Enemies = new List<Model3DGroup>();

        private Model3DGroup CreateEnemyModel(string EnemyType, Color color)
        {
            Model3DGroup EnemyModel = importer.Load(EnemyType.ToString(), null, false);
            foreach (var Child in EnemyModel.Children)
            {
                var Model = ((GeometryModel3D)Child);
                Model.Material = MaterialHelper.CreateMaterial(color);
            }

            return EnemyModel;
        }

        private ModelVisual3D CreateEnemy(String EnemyType, Color EnemyColor, double x, double y, double z)
        {
            ModelVisual3D Enemy = new ModelVisual3D();

            Enemy.SetName("Enemy");
            
            Enemy.Content = CreateEnemyModel(EnemyType, EnemyColor);

            Transform3DGroup Transforms = new Transform3DGroup();
            Transforms.Children.Add(new ScaleTransform3D(0.01, 0.01, 0.01));
            Transforms.Children.Add(new TranslateTransform3D(x, y, z));
            Enemy.Transform = Transforms;

            return Enemy;
        }


        private void Animate(object d)
        {
            var dispatcher = (Dispatcher)d;

            try
            {
                while (!this.source.IsCancellationRequested)
                {
                    System.Threading.Thread.Sleep(50);
                    System.Threading.Thread.Yield();
                    dispatcher.Invoke((Action)(this.DoAnimation));
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception..." + ex.Message);
            }
           
        }

        object Lock = new object();
        DateTime LastAnimation = DateTime.Now;
        DateTime EndOfLevel = DateTime.Now;

        AudioPlayback ExplosionSound = null;
        Dictionary<ModelVisual3D, double> OpacityDictionary = new Dictionary<ModelVisual3D, double>();
        

        private void DoAnimation()
        {
            List<ModelVisual3D> Enemies = new List<ModelVisual3D>();
            List<ModelVisual3D> Lasers = new List<ModelVisual3D>();
            double LowestEnemy = 10000;

            DisplayScore();
            DisplayAmmo();
            this.Question.Text = this.CurrentWord.Question.Replace("ő","ö").Replace("ű","ü");
            
            if ((DateTime.Now - LevelStartedTime).Seconds > 4 && !this.GameOver)
            {
                this.BigTextPanel.Visibility = System.Windows.Visibility.Hidden;
                this.QAPanel.Visibility = System.Windows.Visibility.Visible;
            }

            Answer.Text = AnswerTextBox.Text.Replace(" ", "_").Replace("ő","ö").Replace("ű","ű");
            AnswerTextBox.Focus();

            lock (this.view1.Children)
            {
                TimeSpan DeltaTime = DateTime.Now - LastAnimation;
                LastAnimation = DateTime.Now;

                List<ModelVisual3D> ToRemove = new List<ModelVisual3D>();

                foreach (ModelVisual3D Model in this.view1.Children.ToList())
                {

                    if (Model.GetName() != null && Model.GetName().Equals("DeadEnemy"))
                    {
                        if (OpacityDictionary.ContainsKey(Model))
                        {
                            if ((OpacityDictionary[Model]) > 0)
                            {
                                OpacityDictionary[Model] -= DeltaTime.TotalMilliseconds * 0.003;
                            }
                            else
                            {
                                OpacityDictionary[Model] = 0;
                                ToRemove.Add(Model);
                            }
                        }
                        else
                        {
                            OpacityDictionary.Add(Model, 1);
                        }

                        Model3DGroup DeadEnemy = (Model3DGroup)Model.Content;
                        foreach (var Child in DeadEnemy.Children)
                        {
                            var DeadEnemyModel = ((GeometryModel3D)Child);
                            DeadEnemyModel.BackMaterial = MaterialHelper.CreateMaterial(Colors.Yellow, OpacityDictionary[Model]);
                            DeadEnemyModel.Material = MaterialHelper.CreateMaterial(Colors.Red, OpacityDictionary[Model] / 2.0);
                        }
                    }

                    if (Model.GetName() != null && Model.GetName().Equals("Enemy"))
                    {
                        var Enemy = Model;
                        Enemies.Add(Enemy);
                        ((TranslateTransform3D)((Transform3DGroup)Enemy.Transform).Children[1]).OffsetY -= DeltaTime.Milliseconds * 0.00007 * (1 + Level / 3.2);

                        double EnemyPosition = ((TranslateTransform3D)((Transform3DGroup)Enemy.Transform).Children[1]).OffsetY;

                        if(EnemyPosition < LowestEnemy)
                        {
                            LowestEnemy = EnemyPosition;
                        }
                    }

                    if (Model.GetName() != null && Model.GetName().Equals("Laser"))
                    {
                        var Laser = Model;
                        Lasers.Add(Laser);

                        ((TranslateTransform3D)((Transform3DGroup)Laser.Transform).Children[1]).OffsetY += DeltaTime.Milliseconds * 0.005;

                        if (((TranslateTransform3D)((Transform3DGroup)Laser.Transform).Children[1]).OffsetY > 25)
                        {
                            ToRemove.Add(Laser);
                        }
                    }                    
                }

                if(LowestEnemy < -5.2)
                {
                    if (GameOver == false)
                    {
                        this.GameOver = true;
                        this.GameOverTime = DateTime.Now;
                        this.ship.SetName("DeadEnemy");
                        this.BigTextPanel.Visibility = System.Windows.Visibility.Visible;
                        this.QAPanel.Visibility = System.Windows.Visibility.Hidden;
                        this.BigText.Text = "GAME OVER";
                        if (GameOverSound != null)
                        {
                            GameOverSound.Stop();
                            GameOverSound.Dispose();
                        }

                        GameOverSound = new AudioPlayback();
                        GameOverSound.Load("Sounds/Gameover.mp3");
                        GameOverSound.Play();

                        Task.Factory.StartNew(this.SubmitHighscore, this.Dispatcher, source.Token); 
                    }
                }

                foreach (var laser in Lasers)
                {
                    var LaserLocation = laser.FindBounds(laser.Transform).Location;

                    foreach (var enemy in Enemies)
                    {
                        
                        var EnemyBound = enemy.FindBounds(enemy.Transform);
                        double Scale = ((ScaleTransform3D)((Transform3DGroup)enemy.Transform).Children[0]).ScaleX;


                        if (EnemyBound.Location.DistanceTo(LaserLocation) < 42 * Scale)
                        {
                            enemy.SetName("DeadEnemy");
                            ToRemove.Add(laser);

                            if (ExplosionSound != null)
                            {
                                ExplosionSound.Stop();
                                ExplosionSound.Dispose();
                            }

                            ExplosionSound = new AudioPlayback();
                            ExplosionSound.Load("Sounds/Explosions/Explosion01.mp3");
                            ExplosionSound.Play();

                            this.Score += 1000;

                        }
                    }
                }

                // Remove far lasers & Dead enemies
                foreach (var item in ToRemove)
                {
                    this.view1.Children.Remove(item);
                }

                if (Math.Abs(ShipPosition.OffsetX - _ShipXCoord) > 0.05)
                {
                    ShipPosition.OffsetX -= (ShipPosition.OffsetX - _ShipXCoord) / (2000.0 / (double)DeltaTime.Milliseconds);
                }

                if (Enemies.Count == 0)
                {

                    if (IsEndOfLevel == false)
                    {
                        IsEndOfLevel = true;
                        EndOfLevel = DateTime.Now;
                        // this.QAPanel.Visibility = System.Windows.Visibility.Hidden;
                    }

                    var TimeSinceEndOfLevel = DateTime.Now - EndOfLevel;

                    if (IsEndOfLevel && TimeSinceEndOfLevel.Seconds > 3)
                    {
                        NextLevel();
                    }
                }
            }
        }

        private void SubmitHighscore(object arg)
        {
            DataManager.SubmitHighscore(this.Score, DataManager.PlayerName);
        }

        private void DisplayAmmo()
        {
            this.Ammo.Text = "x " + this.LaserAmmoCount;
        }

        private void DisplayScore()
        {
            if(Score > DisplayedScore)
            {
                DisplayedScore = (int)Math.Ceiling(DisplayedScore + ((Score - DisplayedScore) / 10.0));
            }

            this.PlayerScore.Text = DisplayedScore.ToString();
        }

        private DateTime LevelStartedTime;
        private void NextLevel()
        {
            Level++;
            this.BigTextPanel.Visibility = System.Windows.Visibility.Visible;
            //this.QAPanel.Visibility = System.Windows.Visibility.Hidden;
            this.BigText.Text = "LEVEL" + Level;
            LevelStartedTime = DateTime.Now;
            OpacityDictionary.Clear();
            LevelLoader.LoadLevel(Level);          
        }

        private System.Threading.CancellationTokenSource source;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lock (this.view1.Children)
            {
                NextLevel();

                this.ship.Content = importer.Load("Models/Tie.3ds");

                this.source = new System.Threading.CancellationTokenSource();
                Task.Factory.StartNew(this.Animate, this.Dispatcher, source.Token);
            }
        }

        private int _ShipXCoord = 0;
        private int ShipXCoord
        {
            get
            {
                return _ShipXCoord;
            }

            set
            {
                _ShipXCoord = value;
            }
        }

        public void ShipLeft()
        {
            if (ShipXCoord > -8)
            {
                ShipXCoord -= 1;
            }
        }
        public void ShipRight()
        {
            if (ShipXCoord < 8)
            {
                ShipXCoord += 1;
            }
        }


        private ModelVisual3D CreateLaser(double x, double y, double z)
        {
            MeshBuilder b = new MeshBuilder();
            b.AddBox(new Point3D(1, 1, 1), 5, 50, 5);
            var Mesh = b.ToMesh();
            Material MaterialForLaser = MaterialHelper.CreateMaterial(Colors.Red);

            ModelVisual3D Laser = new ModelVisual3D();

            var LaserModel = new GeometryModel3D { Geometry = Mesh, Material = MaterialForLaser };
            
            Laser.SetName("Laser");
            Laser.Content = LaserModel;

            Transform3DGroup Transforms = new Transform3DGroup();
            Transforms.Children.Add(new ScaleTransform3D(0.01, 0.01, 0.01));
            Transforms.Children.Add(new TranslateTransform3D(x, y, z));
            Laser.Transform = Transforms;
            
            return Laser;
        }

        List<ModelVisual3D> NewModels = new List<ModelVisual3D>();
        AudioPlayback LaserSound = null;
        AudioPlayback GameOverSound = null;
        DateTime LastShootTime = DateTime.Now;
        private bool IsEndOfLevel = false;
        private int Level = 0;
        private bool GameOver = false;

        public bool IsGameOver
        {
            get
            {
                return GameOver;
            }

        }

        public void addAmmo(int ammoCount)
        {
            this.LaserAmmoCount += ammoCount;
        }

        internal void ShootLaser()
        {
            var DeltaTime = DateTime.Now - LastShootTime;

            if ((DeltaTime.Milliseconds > 250) && (!this.GameOver) && (LaserAmmoCount>0))
            {
                LastShootTime = DateTime.Now;
                LaserAmmoCount--;

                if(LaserSound != null)
                {
                    LaserSound.Stop();
                    LaserSound.Dispose();                    
                }

                LaserSound = new AudioPlayback();
                LaserSound.Load("Sounds/Laser.mp3");
                LaserSound.Play();

                // Create Laser
                var LaserModel = CreateLaser(ShipPosition.OffsetX, ShipPosition.OffsetY, ShipPosition.OffsetZ);
                LaserModel.SetName("Laser");

                lock (this.view1.Children)
                {
                    this.view1.Children.Add(LaserModel);
                }
            }            
            else
                if ((DeltaTime.Milliseconds > 250) && (!this.GameOver) && (LaserAmmoCount == 0))
                {
                    LastShootTime = DateTime.Now;
                    if (LaserSound != null)
                    {
                        LaserSound.Stop();
                        LaserSound.Dispose();
                    }

                    LaserSound = new AudioPlayback();
                    LaserSound.Load("Sounds/LaserError.mp3");
                    LaserSound.Play();
                }
        }

        private void AnswerTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = true; // Don't move the cursor in TextBox.
            }

            MainWindow.Instance.Window_KeyDown(sender, e);

            if(e.Key == Key.Enter)
            {
                ProcessWord();
            }
        }

        private bool Mistaken = false;
        private void ProcessWord()
        {
            string PlayerAnswer = this.AnswerTextBox.Text;
            PlayerAnswer = PlayerAnswer.Trim().ToLowerInvariant();
            string CorrectAnswer = this.CurrentWord.Answer.Trim().ToLowerInvariant();

            if(PlayerAnswer.Equals(CorrectAnswer))
            {
                if (!this.Mistaken)
                {
                    this.LaserAmmoCount += 3;
                    this.Score += 1500;
                }

                this.AnswerHelper.Text = "";
                this.AnswerHelper.Visibility = System.Windows.Visibility.Hidden;
                this.NextWord();
                this.Mistaken = false;
            }
            else
            {
                Mistaken = true;
                this.AnswerHelper.Text = CurrentWord.Answer;
                this.AnswerHelper.Visibility = System.Windows.Visibility.Visible;
            }


            this.AnswerTextBox.Text = "";
        }

        private bool _Highscores = false;
        private DateTime GameOverTime;
        
        public bool Highscores
        {
            get
            {
                return _Highscores;
            }
        }

        internal void ShowHighScores()
        {
            if ((DateTime.Now - GameOverTime).Seconds > 2)
            {
                var Highscores = DataManager.GetHighScores();

                this.BigText.FontSize = 65;
                this.BigText.FontFamily = new FontFamily("Courier New");
                this.FontWeight = FontWeights.Bold;
                this.Foreground = new SolidColorBrush(Colors.Orange);

                this.BigText.Text = "     HIGHSCORES\n\n";
                foreach (var score in Highscores)
                {
                    string PlayerName = score.PlayerName;
                    if(PlayerName.Length > 8)
                    {
                        PlayerName = PlayerName.Substring(0, 8);
                    }

                    this.BigText.Text += PlayerName.ToUpper().PadRight(9, ' ') + score.Value.ToString().ToUpper().PadLeft(10, ' ') + "\n";
                }
                _Highscores = true;
            }
        }
    }
}
