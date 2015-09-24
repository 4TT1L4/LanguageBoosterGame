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
        private Color Level2Color(int i, int j)
        {
           if((i + j) % 2 == 1)
           {
               return Colors.Yellow;
           }

           return Colors.White;
        }

        public void LoadLevel2()
        {
            for (int j = 1; j < 4; j++)
            {
                for (int i = 1; i < 10; i++)
                {
                    if( (i + j) % 2 == 0)
                    {
                        continue;
                    }

                    String type = EnemyType.Enemy1;

                    if(j % 2 == 1)
                    {
                        type = EnemyType.Enemy2;
                    }



                    Color color = Level2Color(i, 1);

                    var EnemyShip = CreateEnemy(type,
                                                color,
                                                8 - (1.5 * i), 10 - j, 0, 0.7);
                    view.Children.Add(EnemyShip);
                }
            }
        }
    }
}