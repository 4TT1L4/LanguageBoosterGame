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
        private Color Level1Color(int i, int j)
        {
            if (j == 1)
            {
                if (i % 2 == 0)
                    return Colors.Blue;
                else
                    return Colors.White;
            }

            if (j == 3)
            {
                if (i % 2 == 0)
                    return Colors.White;
                else
                    return Colors.Blue;
            }

            return Colors.White;
        }

        public void LoadLevel1()
        {
            for (int i = 4; i < 7; i++)
            {
                String type = EnemyType.Enemy1;
                Color color = Level1Color(i, 1);

                var EnemyShip = CreateEnemy(type,
                                            color,
                                            8 - (1.5 * i), 10, 0);
                view.Children.Add(EnemyShip);
            }

            for (int i = 1; i < 6; i++)
            {
                String type = EnemyType.Enemy2;
                Color color = Level1Color(i, 2);

                var EnemyShip = CreateEnemy(type,
                                            color,
                                            9.5 - (3 * i), 8 - Math.Abs(i - 3) * 1.5, 0);
                view.Children.Add(EnemyShip);
            }

            for (int i = 4; i < 7; i++)
            {
                String type = EnemyType.Enemy2;
                Color color = Level1Color(i, 3);

                var EnemyShip = CreateEnemy(type,
                                            color,
                                            8 - (1.5 * i), 2, 0);
                view.Children.Add(EnemyShip);
            }
        }
    }
}