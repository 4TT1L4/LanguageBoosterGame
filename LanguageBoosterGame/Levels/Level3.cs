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
        private Color Level3Color(int i, int j)
        {
           if((i + j) % 2 == 1)
           {
               return Colors.SteelBlue;
           }

           return Colors.White;
        }

        public void LoadLevel3()
        {
            for (int i = -1; i < 2; i++)
            {
                double YOffset = 0;
                if(i == 0) 
                {
                    YOffset = 2;
                }

                String type = EnemyType.Enemy1;
                var EnemyShip = CreateEnemy(type,
                                            Colors.White,
                                            5 * i, YOffset, 0, 3);
                view.Children.Add(EnemyShip);                            
            }
             
        }
    }
}