using System;
using System.Collections.Generic;
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
    /// Interaction logic for EnterNameForm.xaml
    /// </summary>
    public partial class EnterNameForm : UserControl
    {
        public EnterNameForm()
        {
            InitializeComponent();
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Focus();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                DataManager.PlayerName = this.NameTextBox.Text.Trim();
                this.NameTextBox.IsEnabled = false;

                Storyboard storyboard = new Storyboard();
                storyboard.Completed += new EventHandler(Story_Completed);
                TimeSpan duration = TimeSpan.FromMilliseconds(500);
                DoubleAnimation fadeOutAnimation = new DoubleAnimation() { From = 1.0, To = 0.0, Duration = new Duration(duration) };

                Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity", 1));
                storyboard.Children.Add(fadeOutAnimation);
                System.Diagnostics.Trace.WriteLine("Hiding started.");
                storyboard.Begin(this);
            }
        }

        private void Story_Completed(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Hiding completed.");
            MainWindow.Instance.StartGame();
        }
    }
}
