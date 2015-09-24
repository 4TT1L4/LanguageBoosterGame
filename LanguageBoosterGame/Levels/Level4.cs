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

        public void LoadLevel4()
        {
            for (int i = 0; i < 5; i++)
            {
                double YOffset = r.Next(10);
                double XOffset = r.Next(10);

                String type = EnemyType.Enemy1;
                var EnemyShip = CreateEnemy(type,
                                            RandomColor(),
                                            -8 + XOffset , -3 + YOffset, 0, 2);
                view.Children.Add(EnemyShip);
            }
             
        }
    }
}