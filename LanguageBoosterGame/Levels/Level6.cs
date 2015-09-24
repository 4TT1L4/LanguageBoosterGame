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
        public void LoadLevel6()
        {
            for (int i = 0; i < 4; i += 1)
            {
                for (int j = 0; j < 4; j += 1)
                {
                    double YOffset = i * 3;
                    double XOffset = j * 3;

                    String type;
                    if(i % 2 == 0)
                    {
                        type = "#box";
                    }
                    else
                    {
                        type = "#sphere";
                        
                    }
                    var EnemyShip = CreateEnemy(type,
                                                RandomColor(),
                                                -4 + XOffset, YOffset, 0, 1);
                    view.Children.Add(EnemyShip);
                }
            }
             
        }
    }
}