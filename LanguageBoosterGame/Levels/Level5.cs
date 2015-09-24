using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LanguageBoosterGame
{
    partial class LevelManager
    {
        public void LoadLevel5()
        {
            for (int i = 0; i < 5; i += 2)
            {
                for (int j = 0; j < 5; j += 2)
                {
                    double YOffset = i;
                    double XOffset = j;

                    String type = EnemyType.Enemy1;
                    var EnemyShip = CreateEnemy(type,
                                                RandomColor(),
                                                -2 + XOffset, 0 + YOffset, 0, 1.3);
                    view.Children.Add(EnemyShip);
                }
            }
             
        }
    }
}